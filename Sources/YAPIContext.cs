/*********************************************************************
 *
 * $Id: YAPIContext.cs 44026 2021-02-25 09:48:41Z web $
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
using System.Threading;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{
//--- (generated code: YAPIContext return codes)
//--- (end of generated code: YAPIContext return codes)
//--- (generated code: YAPIContext class start)
/**
 * <summary>
 *   YAPIContext Class: Yoctopuce I/O context configuration.
 * <para>
 * </para>
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YAPIContext
{
//--- (end of generated code: YAPIContext class start)
        internal ulong _deviceListValidityMs = 10000;
        internal uint _networkTimeoutMs = YHTTPHub.YIO_DEFAULT_TCP_TIMEOUT;

        //--- (generated code: YAPIContext definitions)
    protected ulong _defaultCacheValidity = 5;

    //--- (end of generated code: YAPIContext definitions)

        internal class DataEvent
        {
            internal readonly YModule _module;
            internal readonly YFunction _fun;
            internal readonly string _value;
            internal readonly List<int> _report;
            internal readonly double _timestamp;
            internal readonly double _duration;
            internal readonly int _beacon;


            public DataEvent(YFunction fun, string value)
            {
                _fun = fun;
                _module = null;
                _value = value;
                _report = null;
                _timestamp = 0;
                _duration = 0;
                _beacon = -1;
            }

            public DataEvent(YModule module)
            {
                _fun = null;
                _module = module;
                _value = null;
                _report = null;
                _timestamp = 0;
                _duration = 0;
                _beacon = -1;
            }

            public DataEvent(YFunction fun, double timestamp, double duration, List<int> report)
            {
                _fun = fun;
                _module = null;
                _value = null;
                _timestamp = timestamp;
                _duration = duration;
                _report = report;
                _beacon = -1;
            }

            public DataEvent(YModule module, int beacon)
            {
                _fun = null;
                _module = module;
                _value = null;
                _report = null;
                _timestamp = 0;
                _duration = 0;
                _beacon = beacon;
            }

            public virtual async Task invoke()
            {
                if (_module != null) {
                    if (_beacon < 0) {
                        await _module._invokeConfigChangeCallback();
                    } else {
                        await _module._invokeBeaconCallback(_beacon);
                    }
                } else if (_value == null) {
                    YSensor sensor = (YSensor) _fun;
                    YMeasure mesure = await sensor._decodeTimedReport(_timestamp, _duration, _report);
                    await sensor._invokeTimedReportCallback(mesure);
                } else {
                    // new value
                    await _fun._invokeValueCallback(_value);
                }
            }
        }

        internal class PlugEvent
        {
            public enum Event
            {
                PLUG,
                UNPLUG,
                CHANGE
            }

            public Event ev;
            public string serial;

            public PlugEvent(YAPIContext yctx, Event ev, string serial)
            {
                this.ev = ev;
                this.serial = serial;
            }
        }


        private static double[] decExp = new double[] {1.0e-6, 1.0e-5, 1.0e-4, 1.0e-3, 1.0e-2, 1.0e-1, 1.0, 1.0e1, 1.0e2, 1.0e3, 1.0e4, 1.0e5, 1.0e6, 1.0e7, 1.0e8, 1.0e9};

        // Convert Yoctopuce 16-bit decimal floats to standard double-precision floats
        //
        internal static double imm_decimalToDouble(long val)
        {
            bool negate = false;
            double res;
            long mantis = val & 2047;
            if (mantis == 0)
                return 0.0;
            if (val > 32767) {
                negate = true;
                val = 65536 - val;
            } else if (val < 0) {
                negate = true;
                val = -val;
            }

            long exp = val >> 11;
            res = (double) (mantis) * decExp[exp];
            return (negate ? -res : res);
        }

        // Convert standard double-precision floats to Yoctopuce 16-bit decimal floats
        //
        internal static long imm_doubleToDecimal(double val)
        {
            int negate = 0;
            double comp, mant;
            int decpow;
            long res;

            if (val == 0.0) {
                return 0;
            }

            if (val < 0) {
                negate = 1;
                val = -val;
            }

            comp = val / 1999.0;
            decpow = 0;
            while (comp > decExp[decpow] && decpow < 15) {
                decpow++;
            }

            mant = val / decExp[decpow];
            if (decpow == 15 && mant > 2047.0) {
                res = (15 << 11) + 2047; // overflow
            } else {
                res = (decpow << 11) + Convert.ToInt32(mant);
            }

            return (negate != 0 ? -res : res);
        }


        internal static List<int> imm_decodeWords(string sdat)
        {
            List<int> udat = new List<int>();

            for (int p = 0; p < sdat.Length;) {
                uint val;
                uint c = sdat[p++];
                if (c == '*') {
                    val = 0;
                } else if (c == 'X') {
                    val = 0xffff;
                } else if (c == 'Y') {
                    val = 0x7fff;
                } else if (c >= 'a') {
                    int srcpos = (int) (udat.Count - 1 - (c - 'a'));
                    if (srcpos < 0) {
                        val = 0;
                    } else {
                        val = (uint) udat[srcpos];
                    }
                } else {
                    if (p + 2 > sdat.Length) {
                        return udat;
                    }

                    val = (c - '0');
                    c = sdat[p++];
                    val += (c - '0') << 5;
                    c = sdat[p++];
                    if (c == 'z')
                        c = '\\';
                    val += (c - '0') << 10;
                }

                udat.Add((int) val);
            }

            return udat;
        }

        internal static List<int> imm_decodeFloats(string sdat)
        {
            List<int> idat = new List<int>();

            for (int p = 0; p < sdat.Length;) {
                int val = 0;
                int sign = 1;
                int dec = 0;
                int decInc = 0;
                int c = sdat[p++];
                while (c != (int) '-' && (c < (int) '0' || c > (int) '9')) {
                    if (p >= sdat.Length) {
                        return idat;
                    }

                    c = sdat[p++];
                }

                if (c == '-') {
                    if (p >= sdat.Length) {
                        return idat;
                    }

                    sign = -sign;
                    c = sdat[p++];
                }

                while ((c >= '0' && c <= '9') || c == '.') {
                    if (c == '.') {
                        decInc = 1;
                    } else if (dec < 3) {
                        val = val * 10 + (c - '0');
                        dec += decInc;
                    }

                    if (p < sdat.Length) {
                        c = sdat[p++];
                    } else {
                        c = 0;
                    }
                }

                if (dec < 3) {
                    if (dec == 0)
                        val *= 1000;
                    else if (dec == 1)
                        val *= 100;
                    else
                        val *= 10;
                }

                idat.Add(sign * val);
            }

            return idat;
        }

        // helper function to find pattern in byte[]
        // todo: look if there is a more efficient c# rewrite
        internal static int imm_find_in_bytes(byte[] source, byte[] match)
        {
            // sanity checks
            if (source == null || match == null) {
                return -1;
            }

            if (source.Length == 0 || match.Length == 0) {
                return -1;
            }

            int ret = -1;
            int spos = 0;
            int mpos = 0;
            byte m = match[mpos];
            for (; spos < source.Length; spos++) {
                if (m == source[spos]) {
                    // starting match
                    if (mpos == 0) {
                        ret = spos;
                    } // finishing match
                    else if (mpos == match.Length - 1) {
                        return ret;
                    }

                    mpos++;
                    m = match[mpos];
                } else {
                    ret = -1;
                    mpos = 0;
                    m = match[mpos];
                }
            }

            return ret;
        }

        internal static string imm_floatToStr(double value)
        {
            int rounded = (int) Math.Round(value * 1000);
            string res = "";
            if (rounded < 0) {
                res += "-";
                rounded = -rounded;
            }

            res += Convert.ToString((int) (rounded / 1000));
            int decim = rounded % 1000;
            if (decim > 0) {
                res += ".";
                if (decim < 100)
                    res += "0";
                if (decim < 10)
                    res += "0";
                if ((decim % 10) == 0)
                    decim /= 10;
                if ((decim % 10) == 0)
                    decim /= 10;
                res += Convert.ToString(decim);
            }

            return res;
        }

        internal static int imm_atoi(string val)
        {
            int p = 0;
            while (p < val.Length && Char.IsWhiteSpace(val[p])) {
                p++;
            }

            int start = p;
            if (p < val.Length && (val[p] == '-' || val[p] == '+'))
                p++;
            while (p < val.Length && Char.IsDigit(val[p])) {
                p++;
            }

            if (start < p) {
                return int.Parse(val.Substring(start, p - start));
            }

            return 0;
        }

        protected const string _hexArray = "0123456789ABCDEF";

        internal static string imm_bytesToHexStr(byte[] bytes, int offset, int len)
        {
            char[] hexChars = new char[len * 2];
            for (int j = 0; j < len; j++) {
                int v = bytes[offset + j] & 0xFF;
                hexChars[j * 2] = _hexArray[v >> 4];
                hexChars[j * 2 + 1] = _hexArray[v & 0x0F];
            }

            return new string(hexChars);
        }

        internal static byte[] imm_hexStrToBin(string hex_str)
        {
            int len = hex_str.Length / 2;
            byte[] res = new byte[len];
            for (int i = 0; i < len; i++) {
                int val = 0;
                for (int n = 0; n < 2; n++) {
                    char c = hex_str[i * 2 + n];
                    val <<= 4;
                    if (c <= '9') {
                        val += c - '0';
                    } else if (c <= 'F') {
                        val += c - 'A' + 10;
                    } else {
                        val += c - 'a' + 10;
                    }
                }

                res[i] = (byte) val;
            }

            return res;
        }

        internal static byte[] imm_bytesMerge(byte[] array_a, byte[] array_b)
        {
            byte[] res = new byte[array_a.Length + array_b.Length];
            System.Buffer.BlockCopy(array_a, 0, res, 0, array_a.Length);
            System.Buffer.BlockCopy(array_b, 0, res, array_a.Length, array_b.Length);
            return res;
        }


        // Return the class name for a given function ID or full Hardware Id
        internal static string imm_functionClass(string funcid)
        {
            int dotpos = funcid.IndexOf('.');

            if (dotpos >= 0) {
                funcid = funcid.Substring(dotpos + 1);
            }

            int classlen = funcid.Length;

            while (funcid[classlen - 1] <= 57) {
                classlen--;
            }

            return funcid.Substring(0, 1).ToUpperInvariant() + funcid.Substring(1, classlen - 1);
        }


        internal static string imm_escapeAttr(string changeval)
        {
            string espcaped = "";
            int i = 0;
            char c = '\0';
            string h = null;
            for (i = 0; i < changeval.Length; i++) {
                c = changeval[i];
                if (c <= ' ' || (c > 'z' && c != '~') || c == '"' || c == '%' || c == '&' || c == '+' || c == '<' || c == '=' || c == '>' || c == '\\' || c == '^' || c == '`') {
                    int hh;
                    if ((c == 0xc2 || c == 0xc3) && (i + 1 < changeval.Length) && (changeval[i + 1] & 0xc0) == 0x80) {
                        // UTF8-encoded ISO-8859-1 character: translate to plain ISO-8859-1
                        hh = (c & 1) * 0x40;
                        i++;
                        hh += changeval[i];
                    } else {
                        hh = c;
                    }

                    h = hh.ToString("X");
                    if ((h.Length < 2))
                        h = "0" + h;
                    espcaped += "%" + h;
                } else {
                    espcaped += c;
                }
            }

            return espcaped;
        }


        //todo: Replace global encding to the YAPIContext one
        //internal string _defaultEncoding = YAPI.DefaultEncoding;
        private int _apiMode;

        internal bool _exceptionsDisabled = false;
        internal readonly List<YGenericHub> _hubs = new List<YGenericHub>(1); // array of root urls
        private bool _firstArrival;
        private readonly LinkedList<PlugEvent> _pendingCallbacks = new LinkedList<PlugEvent>();
        private readonly LinkedList<DataEvent> _data_events = new LinkedList<DataEvent>();

        public event YAPI.DeviceUpdateHandler _arrivalCallback;
        private event YAPI.DeviceUpdateHandler _namechgCallback;
        public event YAPI.DeviceUpdateHandler _removalCallback;
        public event YAPI.LogHandler _logCallback;
        private event YAPI.HubDiscoveryHandler _HubDiscoveryCallback;

        private readonly Dictionary<int, YAPI.CalibrationHandler> _calibHandlers = new Dictionary<int, YAPI.CalibrationHandler>();

        private readonly YSSDP _ssdp;
        internal readonly YHash _yHash;
        private readonly List<YFunction> _ValueCallbackList = new List<YFunction>();
        private readonly List<YFunction> _TimedReportCallbackList = new List<YFunction>();
        private readonly Dictionary<YModule, int> _moduleCallbackList = new Dictionary<YModule, int>();

        internal readonly Dictionary<string, YPEntry.BaseClass> _BaseType = new Dictionary<string, YPEntry.BaseClass>();


        // fixme: review SSDP code
        internal async void HubDiscoveryCallback(string serial, string urlToRegister, string urlToUnregister)
        {
            if (urlToRegister != null) {
                if (_HubDiscoveryCallback != null) {
                    await _HubDiscoveryCallback(serial, urlToRegister);
                }
            }

            if ((this._apiMode & YAPI.DETECT_NET) != 0) {
                if (urlToRegister != null) {
                    if (urlToUnregister != null) {
                        await this.UnregisterHub(urlToUnregister);
                    }

                    try {
                        await this.PreregisterHub(urlToRegister);
                    } catch (YAPI_Exception ex) {
                        this._Log("Unable to register hub " + urlToRegister + " detected by SSDP:" + ex.ToString());
                    }
                }
            }
        }


        internal double linearCalibrationHandler(double rawValue, int calibType, List<int> param, List<double> rawValues, List<double> refValues)
        {
            // calibration types n=1..10 and 11.20 are meant for linear calibration using n points
            int npt;
            double x = rawValues[0];
            double adj = refValues[0] - x;
            int i = 0;

            if (calibType < YAPI.YOCTO_CALIB_TYPE_OFS) {
                npt = calibType % 10;
                if (npt > rawValues.Count) {
                    npt = rawValues.Count;
                }

                if (npt > refValues.Count) {
                    npt = refValues.Count;
                }
            } else {
                npt = refValues.Count;
            }

            while (rawValue > rawValues[i] && ++i < npt) {
                double x2 = x;
                double adj2 = adj;

                x = rawValues[i];
                adj = refValues[i] - x;

                if (rawValue < x && x > x2) {
                    adj = adj2 + (adj - adj2) * (rawValue - x2) / (x - x2);
                }
            }

            return rawValue + adj;
        }

        //INTERNAL METHOD:

        public YAPIContext()
        {
            _yHash = new YHash(this);
            _ssdp = new YSSDP(this);
            imm_resetContext();
            //--- (generated code: YAPIContext attributes initialization)
        //--- (end of generated code: YAPIContext attributes initialization)
        }

        private void imm_resetContext()
        {
            _apiMode = 0;
            _firstArrival = true;
            _pendingCallbacks.Clear();
            _data_events.Clear();
            _arrivalCallback = null;
            _namechgCallback = null;
            _removalCallback = null;
            _logCallback = null;
            _HubDiscoveryCallback = null;
            _hubs.Clear();
            _calibHandlers.Clear();
            _ssdp.reset();
            _yHash.imm_reset();
            _ValueCallbackList.Clear();
            _TimedReportCallbackList.Clear();
            for (int i = 1; i <= 20; i++) {
                _calibHandlers[i] = linearCalibrationHandler;
            }

            _calibHandlers[YAPI.YOCTO_CALIB_TYPE_OFS] = linearCalibrationHandler;
            _BaseType.Clear();
            _BaseType["Function"] = YPEntry.BaseClass.Function;
            _BaseType["Sensor"] = YPEntry.BaseClass.Sensor;
        }

        internal void _pushPlugEvent(PlugEvent.Event ev, string serial)
        {
            _pendingCallbacks.AddLast(new PlugEvent(this, ev, serial));
        }


        // Queue a function data event (timed report of notification value)
        internal void _PushDataEvent(DataEvent ev)
        {
            _data_events.AddLast(ev);
        }

        /*
        * Return a the calibration handler for a given type
        */
        internal YAPI.CalibrationHandler imm_getCalibrationHandler(int calibType)
        {
            if (!_calibHandlers.ContainsKey(calibType)) {
                return null;
            }

            return _calibHandlers[calibType];
        }


        internal async Task<YDevice> funcGetDevice(string className, string func)
        {
            string resolved;
            try {
                resolved = _yHash.imm_resolveSerial(className, func);
            } catch (YAPI_Exception ex) {
                if (ex.errorType == YAPI.DEVICE_NOT_FOUND && _hubs.Count == 0) {
                    throw new YAPI_Exception(ex.errorType, "Impossible to contact any device because no hub has been registered");
                } else {
                    await _updateDeviceList_internal(true, false);
                    resolved = _yHash.imm_resolveSerial(className, func);
                }
            }

            YDevice dev = _yHash.imm_getDevice(resolved);
            if (dev == null) {
                // try to force a device list update to check if the device arrived
                // in between
                await _updateDeviceList_internal(true, false);
                dev = _yHash.imm_getDevice(resolved);
                if (dev == null) {
                    throw new YAPI_Exception(YAPI.DEVICE_NOT_FOUND, "Device [" + resolved + "] not online");
                }
            }

            return dev;
        }


        internal async Task _UpdateValueCallbackList(YFunction func, bool add)
        {
            if (add) {
                await func.isOnline();
                if (!_ValueCallbackList.Contains(func)) {
                    _ValueCallbackList.Add(func);
                }
            } else {
                _ValueCallbackList.Remove(func);
            }
        }

        internal YFunction _GetValueCallback(string hwid)
        {
            foreach (YFunction func in _ValueCallbackList) {
                try {
                    string fhwid = func.imm_get_hardwareId();
                    if (fhwid != null && fhwid.Equals(hwid)) {
                        return func;
                    }
                } catch (YAPI_Exception) { }
            }

            return null;
        }


        internal async Task _UpdateTimedReportCallbackList(YFunction func, bool add)
        {
            if (add) {
                await func.isOnline();
                if (!_TimedReportCallbackList.Contains(func)) {
                    _TimedReportCallbackList.Add(func);
                }
            } else {
                _TimedReportCallbackList.Remove(func);
            }
        }

        internal YFunction _GetTimedReportCallback(string hwid)
        {
            foreach (YFunction func in _TimedReportCallbackList) {
                try {
                    string fhwid = func.imm_get_hardwareId();
                    if (fhwid != null && fhwid.Equals(hwid)) {
                        return func;
                    }
                } catch (YAPI_Exception) { }
            }

            return null;
        }


        internal async Task _UpdateModuleCallbackList(YModule module, bool add)
        {
            if (add) {
                await module.isOnline();
                if (!_moduleCallbackList.ContainsKey(module)) {
                    _moduleCallbackList[module] = 1;
                } else {
                    _moduleCallbackList[module] += 1;
                }
            } else {
                if (_moduleCallbackList.ContainsKey(module) && _moduleCallbackList[module] > 1) {
                    _moduleCallbackList[module] -= 1;
                }
            }
        }

        internal YModule _GetModuleCallack(String serial)
        {
            YModule module = YModule.FindModuleInContext(this, serial + ".module");
            if (_moduleCallbackList.ContainsKey(module) && _moduleCallbackList[module] > 0) {
                return module;
            }

            return null;
        }


        private async Task<int> AddNewHub(string url, bool reportConnnectionLost, System.IO.Stream request, System.IO.Stream response, object session)
        {
            foreach (YGenericHub h in _hubs) {
                if (h.imm_isSameHub(url, request, response, session)) {
                    return YAPI.SUCCESS;
                }
            }

            YGenericHub newhub;
            YGenericHub.HTTPParams parsedurl;
            parsedurl = new YGenericHub.HTTPParams(url);
            // Add hub to known list
            if (url.Equals("usb")) {
                newhub = new YUSBHub(this, _hubs.Count, true);
            } else if (url.Equals("usb_silent")) {
                newhub = new YUSBHub(this, _hubs.Count, false);
            } else if (url.Equals("net")) {
                if ((_apiMode & YAPI.DETECT_NET) == 0) {
                    _apiMode |= YAPI.DETECT_NET;
                    // todo: review ssdp callback
                    //_ssdp.addCallback(_ssdpCallback);
                }

                return YAPI.SUCCESS;
            } else if (parsedurl.Host.Equals("callback")) {
                //todo: add SUPPORT FOR CALLBACK
                throw new YAPI_Exception(YAPI.NOT_SUPPORTED, "callback is not yet supported");
            } else {
                newhub = new YHTTPHub(this, _hubs.Count, parsedurl, reportConnnectionLost);
            }

            _hubs.Add(newhub);
            await newhub.startNotifications();
            return YAPI.SUCCESS;
        }


        private async Task _updateDeviceList_internal(bool forceupdate, bool invokecallbacks)
        {
            if (_firstArrival && invokecallbacks && _arrivalCallback != null) {
                forceupdate = true;
            }

            // Rescan all hubs and update list of online devices
            foreach (YGenericHub h in _hubs) {
                await h.updateDeviceListAsync(forceupdate);
            }

            // after processing all hubs, invoke pending callbacks if required
            if (invokecallbacks) {
                while (true) {
                    PlugEvent evt;
                    if (_pendingCallbacks.Count == 0) {
                        break;
                    }

                    evt = _pendingCallbacks.First.Value;
                    _pendingCallbacks.RemoveFirst();
                    switch (evt.ev) {
                        case com.yoctopuce.YoctoAPI.YAPIContext.PlugEvent.Event.PLUG:
                            if (_arrivalCallback != null) {
                                YModule module = YModule.FindModuleInContext(this, evt.serial + ".module");
                                await _arrivalCallback(module);
                            }

                            break;
                        case com.yoctopuce.YoctoAPI.YAPIContext.PlugEvent.Event.CHANGE:
                            if (_namechgCallback != null) {
                                YModule module = YModule.FindModuleInContext(this, evt.serial + ".module");
                                await _namechgCallback(module);
                            }

                            break;
                        case com.yoctopuce.YoctoAPI.YAPIContext.PlugEvent.Event.UNPLUG:
                            if (_removalCallback != null) {
                                YModule module = YModule.FindModuleInContext(this, evt.serial + ".module");
                                await _removalCallback(module);
                            }

                            _yHash.imm_forgetDevice(evt.serial);
                            break;
                    }
                }

                if (_arrivalCallback != null && _firstArrival) {
                    _firstArrival = false;
                }
            }
        }

        internal void _Log(string message)
        {
            if (_logCallback != null) {
                _logCallback(message);
            }
        }
#pragma warning disable 1998

        private async Task SetDeviceListValidity_internal(int deviceListValidity)
        {
            _deviceListValidityMs = (ulong) (deviceListValidity * 1000);
        }

        private async Task<int> GetDeviceListValidity_internal()
        {
            return (int) (_deviceListValidityMs / 1000);
        }

        private async Task SetNetworkTimeout_internal(int networkMsTimeout)
        {
            _networkTimeoutMs = (uint) networkMsTimeout;
        }

        private async Task<int> GetNetworkTimeout_internal()
        {
            return (int) _networkTimeoutMs;
        }

        private async Task<string> AddUdevRule_internal(bool force)
        {
            return "error: Not supported in UWP";
        }

#pragma warning restore 1998

        //PUBLIC METHOD:

        //--- (generated code: YAPIContext implementation)
#pragma warning disable 1998

    //cannot be generated for UWP:
    //public virtual async Task SetDeviceListValidity_internal(int deviceListValidity)
    /**
     * <summary>
     *   Modifies the delay between each forced enumeration of the used YoctoHubs.
     * <para>
     *   By default, the library performs a full enumeration every 10 seconds.
     *   To reduce network traffic, you can increase this delay.
     *   It's particularly useful when a YoctoHub is connected to the GSM network
     *   where traffic is billed. This parameter doesn't impact modules connected by USB,
     *   nor the working of module arrival/removal callbacks.
     *   Note: you must call this function after <c>yInitAPI</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="deviceListValidity">
     *   nubmer of seconds between each enumeration.
     * @noreturn
     * </param>
     */
    public virtual async Task SetDeviceListValidity(int deviceListValidity)
    {
        await SetDeviceListValidity_internal(deviceListValidity);
    }

    //cannot be generated for UWP:
    //public virtual async Task<int> GetDeviceListValidity_internal()
    /**
     * <summary>
     *   Returns the delay between each forced enumeration of the used YoctoHubs.
     * <para>
     *   Note: you must call this function after <c>yInitAPI</c>.
     * </para>
     * </summary>
     * <returns>
     *   the number of seconds between each enumeration.
     * </returns>
     */
    public virtual async Task<int> GetDeviceListValidity()
    {
        return await GetDeviceListValidity_internal();
    }

    //cannot be generated for UWP:
    //public virtual async Task<string> AddUdevRule_internal(bool force)
    /**
     * <summary>
     *   Adds a UDEV rule which authorizes all users to access Yoctopuce modules
     *   connected to the USB ports.
     * <para>
     *   This function works only under Linux. The process that
     *   calls this method must have root privileges because this method changes the Linux configuration.
     * </para>
     * </summary>
     * <param name="force">
     *   if true, overwrites any existing rule.
     * </param>
     * <returns>
     *   an empty string if the rule has been added.
     * </returns>
     * <para>
     *   On failure, returns a string that starts with "error:".
     * </para>
     */
    public virtual async Task<string> AddUdevRule(bool force)
    {
        return await AddUdevRule_internal(force);
    }

    //cannot be generated for UWP:
    //public virtual async Task SetNetworkTimeout_internal(int networkMsTimeout)
    /**
     * <summary>
     *   Modifies the network connection delay for <c>yRegisterHub()</c> and <c>yUpdateDeviceList()</c>.
     * <para>
     *   This delay impacts only the YoctoHubs and VirtualHub
     *   which are accessible through the network. By default, this delay is of 20000 milliseconds,
     *   but depending or you network you may want to change this delay,
     *   gor example if your network infrastructure is based on a GSM connection.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="networkMsTimeout">
     *   the network connection delay in milliseconds.
     * @noreturn
     * </param>
     */
    public virtual async Task SetNetworkTimeout(int networkMsTimeout)
    {
        await SetNetworkTimeout_internal(networkMsTimeout);
    }

    //cannot be generated for UWP:
    //public virtual async Task<int> GetNetworkTimeout_internal()
    /**
     * <summary>
     *   Returns the network connection delay for <c>yRegisterHub()</c> and <c>yUpdateDeviceList()</c>.
     * <para>
     *   This delay impacts only the YoctoHubs and VirtualHub
     *   which are accessible through the network. By default, this delay is of 20000 milliseconds,
     *   but depending or you network you may want to change this delay,
     *   for example if your network infrastructure is based on a GSM connection.
     * </para>
     * </summary>
     * <returns>
     *   the network connection delay in milliseconds.
     * </returns>
     */
    public virtual async Task<int> GetNetworkTimeout()
    {
        return await GetNetworkTimeout_internal();
    }

    /**
     * <summary>
     *   Change the validity period of the data loaded by the library.
     * <para>
     *   By default, when accessing a module, all the attributes of the
     *   module functions are automatically kept in cache for the standard
     *   duration (5 ms). This method can be used to change this standard duration,
     *   for example in order to reduce network or USB traffic. This parameter
     *   does not affect value change callbacks
     *   Note: This function must be called after <c>yInitAPI</c>.
     * </para>
     * </summary>
     * <param name="cacheValidityMs">
     *   an integer corresponding to the validity attributed to the
     *   loaded function parameters, in milliseconds.
     * @noreturn
     * </param>
     */
    public virtual async Task SetCacheValidity(ulong cacheValidityMs)
    {
        _defaultCacheValidity = cacheValidityMs;
    }

    /**
     * <summary>
     *   Returns the validity period of the data loaded by the library.
     * <para>
     *   This method returns the cache validity of all attributes
     *   module functions.
     *   Note: This function must be called after <c>yInitAPI </c>.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the validity attributed to the
     *   loaded function parameters, in milliseconds
     * </returns>
     */
    public virtual async Task<ulong> GetCacheValidity()
    {
        return _defaultCacheValidity;
    }

#pragma warning restore 1998
    //--- (end of generated code: YAPIContext implementation)

        /**
         * <summary>
         *   Disables the use of exceptions to report runtime errors.
         * <para>
         *   When exceptions are disabled, every function returns a specific
         *   error value which depends on its type and which is documented in
         *   this reference manual.
         * </para>
         * </summary>
         */
        public void DisableExceptions()
        {
            _exceptionsDisabled = true;
        }

        /**
         * <summary>
         *   Re-enables the use of exceptions for runtime error handling.
         * <para>
         *   Be aware than when exceptions are enabled, every function that fails
         *   triggers an exception. If the exception is not caught by the user code,
         *   it  either fires the debugger or aborts (i.e. crash) the program.
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         * </summary>
         */
        public void EnableExceptions()
        {
            _exceptionsDisabled = false;
        }


        /**
         * <summary>
         *   Returns the version identifier for the Yoctopuce library in use.
         * <para>
         *   The version is a string in the form <c>"Major.Minor.Build"</c>,
         *   for instance <c>"1.01.5535"</c>. For languages using an external
         *   DLL (for instance C#, VisualBasic or Delphi), the character string
         *   includes as well the DLL version, for instance
         *   <c>"1.01.5535 (1.01.5439)"</c>.
         * </para>
         * <para>
         *   If you want to verify in your code that the library version is
         *   compatible with the version that you have used during development,
         *   verify that the major number is strictly equal and that the minor
         *   number is greater or equal. The build number is not relevant
         *   with respect to the library compatibility.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <returns>
         *   a character string describing the library version.
         * </returns>
         */
        public static string GetAPIVersion()
        {
            return YAPI.GetAPIVersion();
        }


        /**
         * <summary>
         *   Initializes the Yoctopuce programming library explicitly.
         * <para>
         *   It is not strictly needed to call <c>yInitAPI()</c>, as the library is
         *   automatically  initialized when calling <c>yRegisterHub()</c> for the
         *   first time.
         * </para>
         * <para>
         *   When <c>YAPI.DETECT_NONE</c> is used as detection <c>mode</c>,
         *   you must explicitly use <c>yRegisterHub()</c> to point the API to the
         *   VirtualHub on which your devices are connected before trying to access them.
         * </para>
         * </summary>
         * <param name="mode">
         *   an integer corresponding to the type of automatic
         *   device detection to use. Possible values are
         *   <c>YAPI.DETECT_NONE</c>, <c>YAPI.DETECT_USB</c>, <c>YAPI.DETECT_NET</c>,
         *   and <c>YAPI.DETECT_ALL</c>.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public async Task<int> InitAPI(int mode)
        {
            int res = YAPI.SUCCESS;
            if ((mode & YAPI.DETECT_NET) != 0) {
                res = await RegisterHub("net");
                if (res != YAPI.SUCCESS) {
                    return res;
                }
            }

            if ((mode & YAPI.RESEND_MISSING_PKT) != 0) {
                YAPI.pktAckDelay = YAPI.DEFAULT_PKT_RESEND_DELAY;
            }

            if ((mode & YAPI.DETECT_USB) != 0) {
                res = await RegisterHub("usb");
            }

            return res;
        }

        /**
         * <summary>
         *   Waits for all pending communications with Yoctopuce devices to be
         *   completed then frees dynamically allocated resources used by
         *   the Yoctopuce library.
         * <para>
         * </para>
         * <para>
         *   From an operating system standpoint, it is generally not required to call
         *   this function since the OS will automatically free allocated resources
         *   once your program is completed. However there are two situations when
         *   you may really want to use that function:
         * </para>
         * <para>
         *   - Free all dynamically allocated memory blocks in order to
         *   track a memory leak.
         * </para>
         * <para>
         *   - Send commands to devices right before the end
         *   of the program. Since commands are sent in an asynchronous way
         *   the program could exit before all commands are effectively sent.
         * </para>
         * <para>
         *   You should not call any other library function after calling
         *   <c>yFreeAPI()</c>, or your program will crash.
         * </para>
         * </summary>
         */
        public void FreeAPI()
        {
            if ((_apiMode & YAPI.DETECT_NET) != 0) {
                _ssdp.Stop();
            }

            foreach (YGenericHub h in _hubs) {
                h.stopNotifications();
                h.imm_release();
            }

            imm_resetContext();
        }


        /**
         * <summary>
         *   Setup the Yoctopuce library to use modules connected on a given machine.
         * <para>
         *   The
         *   parameter will determine how the API will work. Use the following values:
         * </para>
         * <para>
         *   <b>usb</b>: When the <c>usb</c> keyword is used, the API will work with
         *   devices connected directly to the USB bus. Some programming languages such a JavaScript,
         *   PHP, and Java don't provide direct access to USB hardware, so <c>usb</c> will
         *   not work with these. In this case, use a VirtualHub or a networked YoctoHub (see below).
         * </para>
         * <para>
         *   <b><i>x.x.x.x</i></b> or <b><i>hostname</i></b>: The API will use the devices connected to the
         *   host with the given IP address or hostname. That host can be a regular computer
         *   running a VirtualHub, or a networked YoctoHub such as YoctoHub-Ethernet or
         *   YoctoHub-Wireless. If you want to use the VirtualHub running on you local
         *   computer, use the IP address 127.0.0.1.
         * </para>
         * <para>
         *   <b>callback</b>: that keyword make the API run in "<i>HTTP Callback</i>" mode.
         *   This a special mode allowing to take control of Yoctopuce devices
         *   through a NAT filter when using a VirtualHub or a networked YoctoHub. You only
         *   need to configure your hub to call your server script on a regular basis.
         *   This mode is currently available for PHP and Node.JS only.
         * </para>
         * <para>
         *   Be aware that only one application can use direct USB access at a
         *   given time on a machine. Multiple access would cause conflicts
         *   while trying to access the USB modules. In particular, this means
         *   that you must stop the VirtualHub software before starting
         *   an application that uses direct USB access. The workaround
         *   for this limitation is to setup the library to use the VirtualHub
         *   rather than direct USB access.
         * </para>
         * <para>
         *   If access control has been activated on the hub, virtual or not, you want to
         *   reach, the URL parameter should look like:
         * </para>
         * <para>
         *   <c>http://username:password@address:port</c>
         * </para>
         * <para>
         *   You can call <i>RegisterHub</i> several times to connect to several machines.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public async Task<int> RegisterHub(string url)
        {
            await AddNewHub(url, true, null, null, null);
            // Register device list
            await _updateDeviceList_internal(true, false);
            return YAPI.SUCCESS;
        }


        /**
         * <summary>
         *   Fault-tolerant alternative to <c>yRegisterHub()</c>.
         * <para>
         *   This function has the same
         *   purpose and same arguments as <c>yRegisterHub()</c>, but does not trigger
         *   an error when the selected hub is not available at the time of the function call.
         *   This makes it possible to register a network hub independently of the current
         *   connectivity, and to try to contact it only when a device is actively needed.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public async Task<int> PreregisterHub(string url)
        {
            try {
                await AddNewHub(url, false, null, null, null);
            } catch (YAPI_Exception ex) {
                if (_exceptionsDisabled) {
                    return ex.errorType;
                } else {
                    throw ex;
                }
            }

            return YAPI.SUCCESS;
        }

        /**
         * <summary>
         *   Setup the Yoctopuce library to no more use modules connected on a previously
         *   registered machine with RegisterHub.
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c> or the
         *   root URL of the hub to monitor
         * </param>
         */
        public async Task UnregisterHub(string url)
        {
            if (url.Equals("net")) {
                _apiMode &= ~YAPI.DETECT_NET;
                return;
            }

            await unregisterHubEx(url, null, null, null);
        }

        private async Task unregisterHubEx(string url, System.IO.Stream request, System.IO.Stream response, object session)
        {
            foreach (YGenericHub h in _hubs) {
                if (h.imm_isSameHub(url, request, response, session)) {
                    await h.stopNotifications();
                    foreach (string serial in h._serialByYdx.Values) {
                        _yHash.imm_forgetDevice(serial);
                    }

                    h.imm_release();
                    _hubs.Remove(h);
                    return;
                }
            }
        }


        /**
         * <summary>
         *   Test if the hub is reachable.
         * <para>
         *   This method do not register the hub, it only test if the
         *   hub is usable. The url parameter follow the same convention as the <c>yRegisterHub</c>
         *   method. This method is useful to verify the authentication parameters for a hub. It
         *   is possible to force this method to return after mstimeout milliseconds.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="mstimeout">
         *   the number of millisecond available to test the connection.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public async Task<int> TestHub(string url, uint mstimeout)
        {
            YGenericHub newhub;
            YGenericHub.HTTPParams parsedurl = new YGenericHub.HTTPParams(url);
            // Add hub to known list
            if (url.Equals("usb")) {
                newhub = new YUSBHub(this, 0, true);
            } else if (url.Equals("net")) {
                return YAPI.SUCCESS;
            } else if (parsedurl.Host.Equals("callback")) {
                throw new YAPI_Exception(YAPI.NOT_SUPPORTED, "Not yet supported");
            } else {
                newhub = new YHTTPHub(this, 0, parsedurl, true);
            }

            return await newhub.ping(mstimeout);
        }


        /**
         * <summary>
         *   Triggers a (re)detection of connected Yoctopuce modules.
         * <para>
         *   The library searches the machines or USB ports previously registered using
         *   <c>yRegisterHub()</c>, and invokes any user-defined callback function
         *   in case a change in the list of connected devices is detected.
         * </para>
         * <para>
         *   This function can be called as frequently as desired to refresh the device list
         *   and to make the application aware of hot-plug events. However, since device
         *   detection is quite a heavy process, UpdateDeviceList shouldn't be called more
         *   than once every two seconds.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public async Task<int> UpdateDeviceList()
        {
            await _updateDeviceList_internal(false, true);
            return YAPI.SUCCESS;
        }

        /**
         * <summary>
         *   Maintains the device-to-library communication channel.
         * <para>
         *   If your program includes significant loops, you may want to include
         *   a call to this function to make sure that the library takes care of
         *   the information pushed by the modules on the communication channels.
         *   This is not strictly necessary, but it may improve the reactivity
         *   of the library for the following commands.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public async Task<int> HandleEvents()
        {
            try {
                // handle pending events
                while (true) {
                    DataEvent pv;
                    if (_data_events.Count == 0) {
                        break;
                    }

                    pv = _data_events.First.Value;
                    _data_events.RemoveFirst();
                    if (pv != null) {
                        await pv.invoke();
                    }
                }
            } catch (YAPI_Exception ex) {
                if (_exceptionsDisabled) {
                    return ex.errorType;
                } else {
                    throw ex;
                }
            }

            return YAPI.SUCCESS;
        }

        /**
         * <summary>
         *   Pauses the execution flow for a specified duration.
         * <para>
         *   This function implements a passive waiting loop, meaning that it does not
         *   consume CPU cycles significantly. The processor is left available for
         *   other threads and processes. During the pause, the library nevertheless
         *   reads from time to time information from the Yoctopuce modules by
         *   calling <c>yHandleEvents()</c>, in order to stay up-to-date.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="ms_duration">
         *   an integer corresponding to the duration of the pause,
         *   in milliseconds.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure, throws an exception or returns a negative error code.
         * </para>
         */
        public async Task<int> Sleep(ulong ms_duration)
        {
            try {
                ulong end = GetTickCount() + ms_duration;

                do {
                    await HandleEvents();
                    if (end > GetTickCount()) {
                        await Task.Delay(new TimeSpan(0, 0, 0, 0, 2));
                    }
                } while (end > GetTickCount());

                return YAPI.SUCCESS;
            } catch (YAPI_Exception ex) {
                if (_exceptionsDisabled) {
                    return ex.errorType;
                } else {
                    throw ex;
                }
            }
        }


        /**
         * <summary>
         *   Force a hub discovery, if a callback as been registered with <c>yRegisterHubDiscoveryCallback</c> it
         *   will be called for each net work hub that will respond to the discovery.
         * <para>
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         *   On failure, throws an exception or returns a negative error code.
         * </returns>
         */
        public Task<int> TriggerHubDiscovery()
        {
            // Register device list
            //todo: add ssd support
            //_ssdp.addCallback(_ssdpCallback);
            return Task.FromResult<int>(YAPI.SUCCESS);
        }

        /**
         * <summary>
         *   Returns the current value of a monotone millisecond-based time counter.
         * <para>
         *   This counter can be used to compute delays in relation with
         *   Yoctopuce devices, which also uses the millisecond as timebase.
         * </para>
         * </summary>
         * <returns>
         *   a long integer corresponding to the millisecond counter.
         * </returns>
         */
        public static ulong GetTickCount()
        {
            return (ulong) DateTime.Now.Ticks / 10000;
        }

        /**
         * <summary>
         *   Checks if a given string is valid as logical name for a module or a function.
         * <para>
         *   A valid logical name has a maximum of 19 characters, all among
         *   <c>A..Z</c>, <c>a..z</c>, <c>0..9</c>, <c>_</c>, and <c>-</c>.
         *   If you try to configure a logical name with an incorrect string,
         *   the invalid characters are ignored.
         * </para>
         * </summary>
         * <param name="name">
         *   a string containing the name to check.
         * </param>
         * <returns>
         *   <c>true</c> if the name is valid, <c>false</c> otherwise.
         * </returns>
         */
        public bool CheckLogicalName(string name)
        {
            return YAPI.CheckLogicalName(name);
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time
         *   a device is plugged.
         * <para>
         *   This callback will be invoked while <c>yUpdateDeviceList</c>
         *   is running. You will have to call this function on a regular basis.
         * </para>
         * </summary>
         * <param name="arrivalCallback">
         *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public void RegisterDeviceArrivalCallback(YAPI.DeviceUpdateHandler arrivalCallback)
        {
            _arrivalCallback = arrivalCallback;
        }

        public void RegisterDeviceChangeCallback(YAPI.DeviceUpdateHandler changeCallback)
        {
            _namechgCallback = changeCallback;
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time
         *   a device is unplugged.
         * <para>
         *   This callback will be invoked while <c>yUpdateDeviceList</c>
         *   is running. You will have to call this function on a regular basis.
         * </para>
         * </summary>
         * <param name="removalCallback">
         *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public void RegisterDeviceRemovalCallback(YAPI.DeviceUpdateHandler removalCallback)
        {
            _removalCallback += removalCallback;
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time an Network Hub send
         *   an SSDP message.
         * <para>
         *   The callback has two string parameter, the first one
         *   contain the serial number of the hub and the second contain the URL of the
         *   network hub (this URL can be passed to RegisterHub). This callback will be invoked
         *   while yUpdateDeviceList is running. You will have to call this function on a regular basis.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="hubDiscoveryCallback">
         *   a procedure taking two string parameter, the serial
         *   number and the hub URL. Use <c>null</c> to unregister a previously registered  callback.
         * </param>
         */
        public async Task RegisterHubDiscoveryCallback(YAPI.HubDiscoveryHandler hubDiscoveryCallback)
        {
            _HubDiscoveryCallback = hubDiscoveryCallback;
            try {
                await TriggerHubDiscovery();
            } catch (YAPI_Exception) { }
        }

        /**
         * <summary>
         *   Registers a log callback function.
         * <para>
         *   This callback will be called each time
         *   the API have something to say. Quite useful to debug the API.
         * </para>
         * </summary>
         * <param name="logfun">
         *   a procedure taking a string parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public void RegisterLogFunction(YAPI.LogHandler logfun)
        {
            _logCallback = logfun;
        }


        public string get_debugMsg(string serial)
        {
            string res = "";
            foreach (YGenericHub h in _hubs) {
                res += h.get_debugMsg(serial);
            }

            return res;
        }
    }
}