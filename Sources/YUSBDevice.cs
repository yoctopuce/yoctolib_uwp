﻿/*********************************************************************
 *
 * $Id: YUSBDevice.cs 33880 2018-12-26 16:49:04Z seb $
 *
 * High-level programming interface, common to all modules
 *
 * - - - - - - - - - License information: - - - - - - - - -
 *
 *  Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 *  Yoctopuce Sarl (hereafter Licensor) grants to you a perpetual
 *  non-exclusive license to use, modify, copy and integrate this
 *  file into your software for the sole purpose of interfacing
 *  with Yoctopuce products.
 *
 *  You may reproduce and distribute copies of this file in
 *  source or object form, as long as the sole purpose of this
 *  code is to interface with Yoctopuce products. You must retain
 *  this notice in the distributed source file.
 *
 *  You should refer to Yoctopuce General Terms and Conditions
 *  for additional information regarding your rights and
 *  obligations.
 *
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT
 *  WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
 *  WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS
 *  FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *  EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *  INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA,
 *  COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR
 *  SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT
 *  LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *  CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *  BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *  WARRANTY, OR OTHERWISE.
 *
 *********************************************************************/


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Buffer = System.Buffer;

namespace com.yoctopuce.YoctoAPI
{
    internal class YRequest
    {
        private byte[] _response;
        private uint _response_size;
        private volatile bool _isDone;
        private YGenericHub.RequestAsyncResult _asyncResult;
        private object _asyncContext;
        private TaskCompletionSource<byte[]> _tcs;
        private static readonly byte[] rnrn = new byte[] {13, 10, 13, 10};
        private ulong _timeout;
        private ulong _tm_start;
        private string _debug_name;


        public YRequest(byte[] requestBytes, YGenericHub.RequestAsyncResult asyncResult, object asyncContext, ulong maxTime)
        {
            _response = new byte[1024];
            _response_size = 0;
            _isDone = false;
            _debug_name = YAPI.DefaultEncoding.GetString(requestBytes);
            int debug_pos = _debug_name.IndexOf('\r');
            if (debug_pos > 0) {
                _debug_name = _debug_name.Substring(0, debug_pos);
            }

            _asyncResult = asyncResult;
            _asyncContext = asyncContext;
            _tm_start = YAPI.GetTickCount();
            _timeout = _tm_start + maxTime;
            _tcs = new TaskCompletionSource<byte[]>();
        }

        public override string ToString()
        {
            string res = "Request " + _debug_name + "\n";

            res += _isDone ? "done" : "working";
            if (_asyncResult != null) {
                res += "async";
            }

            res += " res_len=" + _response_size;
            ulong now = YAPI.GetTickCount();
            res += " tmout=" + (_timeout - _tm_start);
            res += " start=" + (_tm_start - now);
            return res;
        }


        //NOTE: will bee called by another thread
        internal void imm_AddIncommingData(YPktStreamHead stream)
        {
            if (stream.Len + _response_size > _response.Length) {
                byte[] tmp = new byte[_response.Length * 2];
                Buffer.BlockCopy(_response, 0, tmp, 0, _response.Length);
                _response = tmp;
            }

            uint len = stream.imm_CopyData(_response, _response_size);
            _response_size += len;
        }

        //NOTE: will bee called by another thread
        internal void imm_Close()
        {
            _asyncResult?.Invoke(_asyncContext, imm_RemoveHeader(_response), YAPI.SUCCESS, "");
            //Debug.WriteLine(string.Format("{0}:End request {1} finished after {2}ms", Environment.CurrentManagedThreadId, _debug_name, YAPI.GetTickCount() - _tm_start));
            _tcs.SetResult(_response);
            _isDone = true;
        }


        internal async Task<byte[]> GetResponse()
        {
            Task<byte[]> task = _tcs.Task;
            ulong now = YAPI.GetTickCount();
            if (_timeout > now) {
                int msTimeout = (int) (_timeout - now);
                Task endedTask = await Task.WhenAny(task, Task.Delay(msTimeout));
                if (endedTask != task) {
                    _asyncResult?.Invoke(_asyncContext, imm_RemoveHeader(_response), YAPI.TIMEOUT, "USB request " + _debug_name + " did not finished in time");
                    throw new YAPI_Exception(YAPI.TIMEOUT, "USB request " + _debug_name + " did not finished in time");
                }
            } else {
                if (!task.IsCompleted) {
                    _asyncResult?.Invoke(_asyncContext, imm_RemoveHeader(_response), YAPI.TIMEOUT, "USB request " + _debug_name + " did not finished in time");
                    throw new YAPI_Exception(YAPI.TIMEOUT, "USB request " + _debug_name + " did not finished in time");
                }
            }

            byte[] response = task.Result;
            var res = imm_RemoveHeader(response);
            return res;
        }

        private byte[] imm_RemoveHeader(byte[] response)
        {
            int hpos = YAPIContext.imm_find_in_bytes(response, rnrn);
            int ofs = 0;
            int size = (int) _response_size;
            if (hpos >= 0) {
                ofs += hpos + 4;
                size -= hpos + 4;
            }

            byte[] res = new byte[size];
            Buffer.BlockCopy(_response, ofs, res, 0, size);
            return res;
        }
    }

    internal class YUSBDevice
    {
        //notifications type
        private const int NOTIFY_1STBYTE_MAXTINY = 63;

        private const int NOTIFY_1STBYTE_MINSMALL = 128;

        private const int NOTIFY_V2_FUNYDX_MASK = 0xF;
        private const int NOTIFY_V2_TYPE_MASK = 0X3;
        private const int NOTIFY_V2_TYPE_OFS = 4;
        private const int NOTIFY_V2_IS_SMALL_FLAG = 0x80;


        protected const int NOTIFY_PKT_NAME = 0;
        protected const int NOTIFY_PKT_PRODNAME = 1;
        protected const int NOTIFY_PKT_CHILD = 2;
        protected const int NOTIFY_PKT_FIRMWARE = 3;
        protected const int NOTIFY_PKT_FUNCNAME = 4;
        protected const int NOTIFY_PKT_FUNCVAL = 5;
        protected const int NOTIFY_PKT_STREAMREADY = 6;
        protected const int NOTIFY_PKT_LOG = 7;
        protected const int NOTIFY_PKT_FUNCNAMEYDX = 8;
        protected const int NOTIFY_PKT_PRODINFO = 9;
        protected const int NOTIFY_PKT_CONFCHANGE = 10;

        private const ulong META_UTC_DELAY = 60000;


        private enum DevState
        {
            Detected,
            ResetSend,
            StartSend,
            StartReceived,
            StreamReadyReceived,
            IOError
        }

        private DevState _devState;
        private YUSBWatcher _watcher;
        private YAPIContext _yctx;
        private byte _pktAckDelay;
        private uint _devVersion;
        private uint _lastpktno;
        private string _logicalname;
        private byte _beacon;
        private string _firmware;
        private string _product;
        private UInt16 _deviceid;
        private WPEntry _wp;
        private readonly Dictionary<String, YPEntry> _usbYP = new Dictionary<string, YPEntry>(2);
        private YRequest _currentRequest = null;
        private ulong _lastMetaUTC;
        private readonly YUSBHub _hub;
        private HidDevice _hid;
        internal DeviceInformation Info { get; }
        internal string SerialNumber { get; private set; }
        private TaskCompletionSource<bool> _currentTask;

        internal string Firmware {
            get { return _firmware; }
        }

        public bool MarkForUnplug { get; set; }

        public YUSBDevice(YUSBWatcher watcher, YUSBHub hub, HidDevice hid, DeviceInformation info)
        {
            _watcher = watcher;
            _hub = hub;
            _yctx = hub._yctx;
            _hid = hid;
            Info = info;
            hid.InputReportReceived += OnInputReportEvent;
            _devState = DevState.Detected;
            _pktAckDelay = 0;
        }


        public void imm_Stop()
        {
            _hid.InputReportReceived -= OnInputReportEvent;
            _hid.Dispose();
            _hid = null;
        }

        internal async Task Setup(uint pktVersion)
        {
            _currentTask = new TaskCompletionSource<bool>();

            // construct a HID output report to send to the device
            HidOutputReport outReport = _hid.CreateOutputReport();
            YUSBPkt.imm_FormatConfReset(outReport, pktVersion);
            // Send the output report asynchronously
            _devState = DevState.ResetSend;
            var u = await _hid.SendOutputReportAsync(outReport);
            if (u != 65) {
                _devState = DevState.IOError;
                throw new YAPI_Exception(YAPI.IO_ERROR, "Unable to send Reset PKT");
            }

            Task<bool> task = _currentTask.Task;
            Task taskDone = await Task.WhenAny(task, Task.Delay(1000));
            if (taskDone != task) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "Device does not respond to reset");
            }
        }

        internal async Task Start(byte pktAckDelay)
        {
            // construct a HID output report to send to the device
            HidOutputReport outReport = _hid.CreateOutputReport();
            //("Activate USB pkt ack (%dms)\n", dev->pktAckDelay);
            YUSBPkt.imm_FormatConfStart(outReport, 1, pktAckDelay);
            // Send the output report asynchronously
            _devState = DevState.StartSend;
            var u = await _hid.SendOutputReportAsync(outReport);
            if (u != 65) {
                _devState = DevState.IOError;
                throw new YAPI_Exception(YAPI.IO_ERROR, "Unable to send Start PKT");
            }
        }


        // check procol version compatibility
        // compatible without limitation -> return 1
        // compatible with limitations -> return 0;
        // incompatible -> return YAPI_IO_ERROR
        private int imm_CheckVersionCompatibility(uint version)
        {
            // fixme: throw exception instead of logging error
            if ((version & 0xff00) != (YUSBPkt.YPKT_USB_VERSION_BCD & 0xff00)) {
                // major version change
                if ((version & 0xff00) > (YUSBPkt.YPKT_USB_VERSION_BCD & 0xff00)) {
                    String error = String.Format("Yoctpuce library is too old (using 0x%x, need 0x%x) to handle device, please upgrade your Yoctopuce library", YUSBPkt.YPKT_USB_VERSION_BCD, version);
                    _yctx._Log(error + "\n");
                    throw new YAPI_Exception(YAPI.VERSION_MISMATCH, error);
                } else {
                    // implement backward compatibility when implementing a new protocol
                    return 1;
                }
            } else if (version != YUSBPkt.YPKT_USB_VERSION_BCD) {
                if (_devVersion == YUSBPkt.YPKT_USB_VERSION_NO_CONFCHG_BCD && YUSBPkt.YPKT_USB_VERSION_BCD == _devVersion + 1) {
                    return 0;
                }
                if (version > YUSBPkt.YPKT_USB_VERSION_BCD) {
                    _yctx._Log("Device is using a newer protocol, consider upgrading your Yoctopuce library\n");
                } else {
                    _yctx._Log("Device is using an older protocol, consider upgrading the device firmware\n");
                }

                return 0;
            } else {
                return 1;
            }
        }


        internal async void OnInputReportEvent(HidDevice sender, HidInputReportReceivedEventArgs args)
        {
            if (_devState == DevState.Detected || _devState == DevState.IOError) {
                // drop all packet until reset has been sent
                return;
            }

            try {
                byte[] bb = args.Report.Data.ToArray();
                long ofs = 1; //skip first byte that is not part of the packet
                List<YPktStreamHead> streams = new List<YPktStreamHead>();
                while (ofs < bb.Length) {
                    YPktStreamHead s = YPktStreamHead.imm_Decode(ofs, bb);
                    if (s == null) {
                        break;
                    }

                    //Debug.WriteLine(s.ToString());
                    streams.Add(s);
                    ofs += s.Len + 2;
                }

                YPktStreamHead streamHead = streams[0];
                switch (_devState) {
                    case DevState.ResetSend:
                        if (streamHead.PktType != YUSBPkt.YPKT_CONF || streamHead.StreamType != YUSBPkt.USB_CONF_RESET) {
                            return;
                        }

                        byte low = streamHead.imm_GetByte(0);
                        uint hig = streamHead.imm_GetByte(1);
                        uint devapi = (hig << 8) + low;
                        _devVersion = devapi;
                        if (imm_CheckVersionCompatibility(devapi) < 0) {
                            return;
                        }

                        await Start(_pktAckDelay);
                        break;
                    case DevState.StartSend:
                        if (streamHead.PktType != YUSBPkt.YPKT_CONF || streamHead.StreamType != YUSBPkt.USB_CONF_START) {
                            return;
                        }

                        if (_devVersion >= YUSBPkt.YPKT_USB_VERSION_BCD) {
                            _pktAckDelay = streamHead.imm_GetByte(1);
                        } else {
                            _pktAckDelay = 0;
                        }

                        _lastpktno = streamHead.PktNumber;
                        _devState = DevState.StartReceived;
                        break;
                    case DevState.StreamReadyReceived:
                    case DevState.StartReceived:
                        if (_devState == DevState.StreamReadyReceived || _devState == DevState.StartReceived) {
                            if (_pktAckDelay > 0 && _lastpktno == streamHead.PktNumber) {
                                //late retry : drop it since we already have the packet.
                                return;
                            }

                            uint expectedPktNo = (_lastpktno + 1) & 7;
                            if (streamHead.PktNumber != expectedPktNo) {
                                String message = "Missing packet (look of pkt " + expectedPktNo + " but get " + streamHead.PktNumber + ")";
                                _yctx._Log(message + "\n");
                                _yctx._Log("Set YAPI.RESEND_MISSING_PKT on YAPI.InitAPI()\n");
                                _devState = DevState.IOError;
                                _watcher.imm_removeUsableDevice(this);
                                return;
                            }

                            _lastpktno = streamHead.PktNumber;
                            await streamHandler(streams);
                            await checkMetaUTC();
                        }

                        break;
                    default:
                        return;
                }
            } catch (YAPI_Exception ex) {
                _yctx._Log(ex.Message + "\n");
                _yctx._Log("Set YAPI.RESEND_MISSING_PKT on YAPI.InitAPI()\n");
                _devState = DevState.IOError;
                if (_currentTask != null) {
                    _currentTask.TrySetException(ex);
                }
            }
        }


        private YPEntry imm_getYPEntryFromYdx(int funIdx)
        {
            foreach (YPEntry ypEntry in _usbYP.Values) {
                if (ypEntry.Index == funIdx) {
                    return ypEntry;
                }
            }

            return null;
        }

        private async Task checkMetaUTC()
        {
            if (_lastMetaUTC + META_UTC_DELAY < YAPI.GetTickCount()) {
                HidOutputReport outReport = _hid.CreateOutputReport();
                YUSBPkt.imm_FormatMetaUTC(outReport, true);
                var u = await _hid.SendOutputReportAsync(outReport);
                if (u != 65) {
                    _devState = DevState.IOError;
                    throw new YAPI_Exception(YAPI.IO_ERROR, "Unable to send Start PKT");
                }

                _lastMetaUTC = YAPI.GetTickCount();
            }
        }


        internal async Task streamHandler(List<YPktStreamHead> streams)
        {
            foreach (YPktStreamHead s in streams) {
                uint streamType = s.StreamType;
                switch (streamType) {
                    case YGenericHub.YSTREAM_NOTICE:
                    case YGenericHub.YSTREAM_NOTICE_V2:
                        imm_handleNotifcation(s);
                        break;
                    case YGenericHub.YSTREAM_TCP_CLOSE:
                    case YGenericHub.YSTREAM_TCP:
                        if (_devState != DevState.StreamReadyReceived || _currentRequest == null) {
                            continue;
                        }

                        _currentRequest.imm_AddIncommingData(s);
                        if (streamType == YGenericHub.YSTREAM_TCP_CLOSE) {
                            // construct a HID output report to send to the device
                            HidOutputReport outReport = _hid.CreateOutputReport();
                            YUSBPkt.imm_FormatTCP(outReport, null, 0, true);
                            // Send the output report asynchronously
                            var u = await _hid.SendOutputReportAsync(outReport);
                            if (u != 65) {
                                _devState = DevState.IOError;
                                _watcher.imm_removeUsableDevice(this);
                                return;
                            }

                            _currentRequest.imm_Close();
                        }

                        break;
                    case YGenericHub.YSTREAM_EMPTY:
                        break;
                    case YGenericHub.YSTREAM_REPORT:
                        if (_devState == DevState.StreamReadyReceived) {
                            imm_handleTimedNotification(s);
                        }

                        break;
                    case YGenericHub.YSTREAM_REPORT_V2:
                        if (_devState == DevState.StreamReadyReceived) {
                            handleTimedNotificationV2(s);
                        }

                        break;
                    default:
                        _yctx._Log("drop unknown ystream:" + s);
                        break;
                }
            }
        }

        private void imm_handleNotifcation(YPktStreamHead ystream)
        {
            string functionId;
            int firstByte = ystream.imm_GetByte(0);
            bool isV2 = ystream.StreamType == YGenericHub.YSTREAM_NOTICE_V2;

            if (isV2 || firstByte <= NOTIFY_1STBYTE_MAXTINY || firstByte >= NOTIFY_1STBYTE_MINSMALL) {
                int funcvalType = (firstByte >> NOTIFY_V2_TYPE_OFS) & NOTIFY_V2_TYPE_MASK;
                int funydx = firstByte & NOTIFY_V2_FUNYDX_MASK;
                YPEntry ypEntry = imm_getYPEntryFromYdx(funydx);
                if (ypEntry != null) {
                    if (ypEntry.Index == funydx) {
                        if (funcvalType == YGenericHub.NOTIFY_V2_FLUSHGROUP) {
                            // not yet used by devices
                        } else {
                            if ((firstByte & NOTIFY_V2_IS_SMALL_FLAG) != 0) {
                                // added on 2015-02-25, remove code below when confirmed dead code
                                throw new YAPI_Exception(YAPI.IO_ERROR, "Hub Should not fwd notification");
                            }

                            int len = (int) ystream.Len;
                            byte[] data = new byte[len];
                            ystream.imm_CopyData(data, 0);
                            string funcval = YGenericHub.imm_decodePubVal(funcvalType, data, 1, len - 1);
                            _hub.imm_handleValueNotification(SerialNumber, ypEntry.FuncId, funcval);
                        }
                    }
                }
            } else {
                string serial = ystream.imm_GetString(0, YAPI.YOCTO_SERIAL_LEN);
                if (SerialNumber == null) {
                    SerialNumber = serial;
                }

                uint p = YAPI.YOCTO_SERIAL_LEN;
                int type = ystream.imm_GetByte(p++);
                switch (type) {
                    case NOTIFY_PKT_NAME:
                        _logicalname = ystream.imm_GetString(p, YAPI.YOCTO_LOGICAL_LEN);
                        byte b = ystream.imm_GetByte(p + YAPI.YOCTO_LOGICAL_LEN);
                        if (_beacon != b) {
                            _hub.imm_handleBeaconNotification(SerialNumber, b);
                            _beacon = b;
                        }
                        break;
                    case NOTIFY_PKT_PRODNAME:
                        _product = ystream.imm_GetString(p, YAPI.YOCTO_PRODUCTNAME_LEN);
                        break;
                    case NOTIFY_PKT_CHILD:
                        break;
                    case NOTIFY_PKT_FIRMWARE:
                        _firmware = ystream.imm_GetString(p, YAPI.YOCTO_FIRMWARE_LEN);
                        p += YAPI.YOCTO_FIRMWARE_LEN;
                        p += 2;
                        _deviceid = (ushort) (ystream.imm_GetByte(p) + (ystream.imm_GetByte(p + 1) << 8));
                        break;
                    case NOTIFY_PKT_FUNCNAME:
                        functionId = ystream.imm_GetString(p, YAPI.YOCTO_FUNCTION_LEN);
                        p += YAPI.YOCTO_FUNCTION_LEN;
                        string funcname = ystream.imm_GetString(p, YAPI.YOCTO_LOGICAL_LEN);
                        if (!_usbYP.ContainsKey(functionId)) {
                            _usbYP[functionId] = new YPEntry(serial, functionId, YPEntry.BaseClass.Function);
                        }

                        _usbYP[functionId].LogicalName = funcname;
                        break;
                    case NOTIFY_PKT_FUNCVAL:
                        functionId = ystream.imm_GetString(p, YAPI.YOCTO_FUNCTION_LEN);
                        p += YAPI.YOCTO_FUNCTION_LEN;
                        string funcval = ystream.imm_GetString(p, YAPI.YOCTO_PUBVAL_SIZE);
                        _hub.imm_handleValueNotification(serial, functionId, funcval);
                        break;
                    case NOTIFY_PKT_STREAMREADY:
                        _devState = DevState.StreamReadyReceived;
                        _wp = new WPEntry(_logicalname, _product, _deviceid, "", _beacon, SerialNumber);
                        _yctx._Log("Device " + SerialNumber + " ready.\n");
                        _currentTask.SetResult(true);
                        break;
                    case NOTIFY_PKT_LOG:
                        //FIXME: handle log notification
                        break;
                    case NOTIFY_PKT_CONFCHANGE:
                        if (_devState == DevState.StreamReadyReceived) {
                            _hub.imm_handleConfigChangeNotification(SerialNumber);
                        }
                        break;
                    case NOTIFY_PKT_FUNCNAMEYDX:
                        functionId = ystream.imm_GetString(p, YAPI.YOCTO_FUNCTION_LEN - 1);
                        p += YAPI.YOCTO_FUNCTION_LEN - 1;
                        byte funclass = ystream.imm_GetByte(p++);
                        funcname = ystream.imm_GetString(p, YAPI.YOCTO_LOGICAL_LEN);
                        p += YAPI.YOCTO_LOGICAL_LEN;
                        byte funydx = ystream.imm_GetByte(p);
                        if (!_usbYP.ContainsKey(functionId)) {
                            _usbYP[functionId] = new YPEntry(serial, functionId, YPEntry.BaseClass.forByte(funclass));
                        }

                        // update ydx
                        _usbYP[functionId].Index = funydx;
                        _usbYP[functionId].LogicalName = funcname;
                        break;
                    case NOTIFY_PKT_PRODINFO:
                    default:
                        // silently ignore unknown notifications
                        break;
                }
            }
        }


        public void imm_handleTimedNotification(YPktStreamHead data)
        {
            uint pos = 0;
            YDevice ydev = _yctx._yHash.imm_getDevice(SerialNumber);
            if (ydev == null) {
                // device has not been registered;
                return;
            }

            while (pos < data.Len) {
                int funYdx = data.imm_GetByte(pos) & 0xf;
                bool isAvg = (data.imm_GetByte(pos) & 0x80) != 0;
                uint len = (uint) (1 + ((data.imm_GetByte(pos) >> 4) & 0x7));
                pos++;
                if (data.Len < pos + len) {
                    _yctx._Log("drop invalid timedNotification");
                    return;
                }

                if (funYdx == 0xf) {
                    byte[] intData = new byte[len];
                    for (uint i = 0; i < len; i++) {
                        intData[i] = data.imm_GetByte(pos + i);
                    }
                    ydev.imm_setLastTimeRef(intData);
                } else {
                    YPEntry yp = imm_getYPEntryFromYdx(funYdx);
                    if (yp != null) {
                        List<int> report = new List<int>((int) (len + 1));
                        report.Add(isAvg ? 1 : 0);
                        for (uint i = 0; i < len; i++) {
                            int b = data.imm_GetByte(pos + i) & 0xff;
                            report.Add(b);
                        }
                        _hub.imm_handleTimedNotification(yp.Serial, yp.FuncId, ydev.imm_getLastTimeRef(), 0, report);
                    }
                }

                pos += len;
            }
        }


        public void handleTimedNotificationV2(YPktStreamHead data)
        {
            uint pos = 0;
            YDevice ydev = _yctx._yHash.imm_getDevice(SerialNumber);
            if (ydev == null) {
                // device has not been registered;
                return;
            }

            while (pos < data.Len) {
                int funYdx = data.imm_GetByte(pos) & 0xf;
                uint extralen = (uint) ((data.imm_GetByte(pos) >> 4) & 0xf);
                uint len = extralen + 1;
                pos++; // consume generic header
                if (funYdx == 0xf) {
                    byte[] intData = new byte[len];
                    for (uint i = 0; i < len; i++) {
                        intData[i] = data.imm_GetByte(pos + i);
                    }

                    ydev.imm_setLastTimeRef(intData);
                } else {
                    YPEntry yp = imm_getYPEntryFromYdx(funYdx);
                    if (yp != null) {
                        List<int> report = new List<int>((int) (len + 1));
                        report.Add(2);
                        for (uint i = 0; i < len; i++) {
                            int b = data.imm_GetByte(pos + i) & 0xff;
                            report.Add(b);
                        }
                        _hub.imm_handleTimedNotification(yp.Serial, yp.FuncId, ydev.imm_getLastTimeRef(),  ydev.imm_getLastDuration(), report);
                    }
                }

                pos += len;
            }
        }


        public void imm_UpdateYellowPages(Dictionary<string, List<YPEntry>> publicYP)
        {
            foreach (YPEntry yp in _usbYP.Values) {
                String classname = yp.Classname;
                if (!publicYP.ContainsKey(classname))
                    publicYP.Add(classname, new List<YPEntry>(2));
                publicYP[classname].Add(yp);
            }
        }

        public WPEntry imm_GetWhitesPagesEntry()
        {
            return _wp;
        }


        private async Task sendRequest(byte[] request, YGenericHub.RequestAsyncResult asyncResult, object asyncContext)
        {
            int pos = 0;

            if (_currentRequest != null) {
                await _currentRequest.GetResponse();
            }

            //Debug.WriteLine(string.Format("{0}:Check last request is sent", Environment.CurrentManagedThreadId));
            _currentRequest = new YRequest(request, asyncResult, asyncContext, 10000);
            while (pos < request.Length) {
                if (_hid == null) {
                    _devState = DevState.IOError;
                    _currentRequest = null;
                    throw new YAPI_Exception(YAPI.IO_ERROR, "USB device has been stopped");
                }

                // construct a HID output report to send to the device

                HidOutputReport outReport;
                try {
                    outReport = _hid.CreateOutputReport();
                } catch (Exception ex) {
                    _devState = DevState.IOError;
                    _currentRequest = null;
                    throw new YAPI_Exception(YAPI.IO_ERROR, "Error during CreateOutputReport():" + ex.Message);
                }

                int size = YUSBPkt.imm_FormatTCP(outReport, request, pos, true);
                // Send the output report asynchronously
                uint u;
                try {
                    u = await _hid.SendOutputReportAsync(outReport);
                } catch (Exception ex) {
                    _devState = DevState.IOError;
                    _currentRequest = null;
                    throw new YAPI_Exception(YAPI.IO_ERROR, "Error during SendOutputReportAsync():" + ex.Message);
                }

                if (u != 65) {
                    _devState = DevState.IOError;
                    _watcher.imm_removeUsableDevice(this);
                    return;
                }

                pos += size;
            }

            //Debug.WriteLine(string.Format("{0}:sent", Environment.CurrentManagedThreadId));
        }


        public async Task<byte[]> DevRequestSync(string serial, byte[] request, YGenericHub.RequestProgress progress, object context)
        {
            if (_devState != DevState.StreamReadyReceived) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "Device not ready");
            }

            await sendRequest(request, null, null);
            byte[] res = await _currentRequest.GetResponse();
            _currentRequest = null;
            return res;
        }

        public async Task DevRequestAsync(string serial, byte[] request, YGenericHub.RequestAsyncResult asyncResult, object asyncContext)
        {
            if (_devState != DevState.StreamReadyReceived) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "Device not ready");
            }

            await sendRequest(request, asyncResult, asyncContext);
        }

        internal string dumpDebug()
        {
            string res = "";
            res += "Dump device " + _hid.VendorId + ":" + _hid.ProductId + ":" + SerialNumber + "\n";
            res += "  logicalname=\"" + _logicalname + "\" beacon=" + _beacon + "\n";
            res += "  state=" + _devState;
            res += "  pktAckDelay=" + _pktAckDelay + " lastpktno=" + _lastpktno + "\n";
            res += "  devVersion=" + _devVersion + " firmware=" + _firmware + "\n";
            res += "  wp=" + _wp.ToString() + "\n";
            if (_currentRequest != null) {
                res += "  curRequ=" + _currentRequest.ToString() + "\n";
            } else {
                res += "  curRequ=null\n";
            }

            res += "  lastMetaUTC=" + _lastMetaUTC + "\n";
            return res;
        }

        public bool imm_isWorking()
        {
            return _devState == DevState.StreamReadyReceived;
        }
    }
}