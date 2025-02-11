/*********************************************************************
 *
 * $Id: YDataLogger.cs 64034 2025-01-06 15:37:18Z seb $
 *
 * Implements yFindDataLogger(), the high-level API for DataLogger functions
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
using System.Collections.Generic;
using System.Threading.Tasks;


namespace com.yoctopuce.YoctoAPI { 


    //--- (generated code: YDataLogger class start)
/**
 * <summary>
 *   YDataLogger Class: DataLogger control interface, available on most Yoctopuce sensors.
 * <para>
 * </para>
 * <para>
 *   A non-volatile memory for storing ongoing measured data is available on most Yoctopuce
 *   sensors. Recording can happen automatically, without requiring a permanent
 *   connection to a computer.
 *   The <c>YDataLogger</c> class controls the global parameters of the internal data
 *   logger. Recording control (start/stop) as well as data retrieval is done at
 *   sensor objects level.
 * </para>
 * </summary>
 */
public class YDataLogger : YFunction
{
//--- (end of generated code: YDataLogger class start)
        //--- (generated code: YDataLogger definitions)
    /**
     * <summary>
     *   invalid currentRunIndex value
     * </summary>
     */
    public const  int CURRENTRUNINDEX_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid timeUTC value
     * </summary>
     */
    public const  long TIMEUTC_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid recording value
     * </summary>
     */
    public const int RECORDING_OFF = 0;
    public const int RECORDING_ON = 1;
    public const int RECORDING_PENDING = 2;
    public const int RECORDING_INVALID = -1;
    /**
     * <summary>
     *   invalid autoStart value
     * </summary>
     */
    public const int AUTOSTART_OFF = 0;
    public const int AUTOSTART_ON = 1;
    public const int AUTOSTART_INVALID = -1;
    /**
     * <summary>
     *   invalid beaconDriven value
     * </summary>
     */
    public const int BEACONDRIVEN_OFF = 0;
    public const int BEACONDRIVEN_ON = 1;
    public const int BEACONDRIVEN_INVALID = -1;
    /**
     * <summary>
     *   invalid usage value
     * </summary>
     */
    public const  int USAGE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid clearHistory value
     * </summary>
     */
    public const int CLEARHISTORY_FALSE = 0;
    public const int CLEARHISTORY_TRUE = 1;
    public const int CLEARHISTORY_INVALID = -1;
    protected int _currentRunIndex = CURRENTRUNINDEX_INVALID;
    protected long _timeUTC = TIMEUTC_INVALID;
    protected int _recording = RECORDING_INVALID;
    protected int _autoStart = AUTOSTART_INVALID;
    protected int _beaconDriven = BEACONDRIVEN_INVALID;
    protected int _usage = USAGE_INVALID;
    protected int _clearHistory = CLEARHISTORY_INVALID;
    protected ValueCallback _valueCallbackDataLogger = null;

    public new delegate Task ValueCallback(YDataLogger func, string value);
    public new delegate Task TimedReportCallback(YDataLogger func, YMeasure measure);
    //--- (end of generated code: YDataLogger definitions)

        protected internal string _dataLoggerURL;

        /// <summary>
        /// Internal function to retrieve datalogger memory
        /// </summary>
        internal virtual async Task<byte[]> getData(int? runIdx, int? timeIdx) {
            if (_dataLoggerURL == null) {
                _dataLoggerURL = "/logger.json";
            }

            // get the device serial number
            YModule mod = await module();
            string devid = await mod.get_serialNumber();

            string httpreq = "GET " + _dataLoggerURL;
            if (timeIdx != null) {
                httpreq += string.Format("?run={0:D}&time={1:D}", runIdx, timeIdx);
            }
            byte[] result;
            YDevice dev = _yapi._yHash.imm_getDevice(devid);
            try {
                result = await dev.requestHTTPSync(httpreq, null);
            }
            catch (YAPI_Exception ex) {
                if (!_dataLoggerURL.Equals("/dataLogger.json")) {
                    _dataLoggerURL = "/dataLogger.json";
                    return await getData(runIdx, timeIdx);
                }
                throw ex;
            }
            return result;
        }


        /// <param name="func"> : functionid </param>
        protected internal YDataLogger(YAPIContext yctx, string func) : base(yctx, func, "DataLogger") {
            //--- (generated code: YDataLogger attributes initialization)
        //--- (end of generated code: YDataLogger attributes initialization)
        }

        protected internal YDataLogger(string func) : this(YAPI.imm_GetYCtx(), func) {
        }

        /// <summary>
        /// Builds a list of all data streams hold by the data logger (legacy method).
        /// The caller must pass by reference an empty array to hold YDataStream
        /// objects, and the function fills it with objects describing available
        /// data sequences.
        /// 
        /// This is the old way to retrieve data from the DataLogger.
        /// For new applications, you should rather use get_dataSets()
        /// method, or call directly get_recordedData() on the
        /// sensor object.
        /// </summary>
        /// <param name="v"> : an array of YDataStream objects to be filled in
        /// </param>
        /// <returns> YAPI.SUCCESS if the call succeeds.
        /// </returns>
        /// <exception cref="YAPI_Exception"> on error </exception>
        public virtual async Task<int> get_dataStreams(List<YDataStream> v) {

            byte[] loadval = await getData(null, null);
            YJSONArray jsonAllStreams = new YJSONArray(YAPI.DefaultEncoding.GetString(loadval));
            jsonAllStreams.parse();
            if (jsonAllStreams.Length == 0) {
                return YAPI.SUCCESS;
            }
            if (jsonAllStreams.get(0) is YJSONArray) {
                // old datalogger format: [runIdx, timerel, utc, interval]
                _throw(YAPI.NOT_SUPPORTED, "Old datalogger is no more supported. Please upgrade your device.");
                return YAPI.NOT_SUPPORTED;                
            } else {
                // new datalogger format: {"id":"...","unit":"...","streams":["...",...]}
                List<YDataSet> sets = await this.parse_dataSets(YAPI.DefaultEncoding.GetBytes(jsonAllStreams.ToString()));
                for (int j = 0; j < sets.Count; j++) {
                    List<YDataStream> ds = await sets[j].get_privateDataStreams();
                    for (int si = 0; si < ds.Count; si++) {
                        v.Add(ds[si]);
                    }
                }
                return YAPI.SUCCESS;
            }
        }


        //--- (generated code: YDataLogger implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("currentRunIndex")) {
            _currentRunIndex = json_val.getInt("currentRunIndex");
        }
        if (json_val.has("timeUTC")) {
            _timeUTC = json_val.getLong("timeUTC");
        }
        if (json_val.has("recording")) {
            _recording = json_val.getInt("recording");
        }
        if (json_val.has("autoStart")) {
            _autoStart = json_val.getInt("autoStart") > 0 ? 1 : 0;
        }
        if (json_val.has("beaconDriven")) {
            _beaconDriven = json_val.getInt("beaconDriven") > 0 ? 1 : 0;
        }
        if (json_val.has("usage")) {
            _usage = json_val.getInt("usage");
        }
        if (json_val.has("clearHistory")) {
            _clearHistory = json_val.getInt("clearHistory") > 0 ? 1 : 0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current run number, corresponding to the number of times the module was
     *   powered on with the dataLogger enabled at some point.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current run number, corresponding to the number of times the module was
     *   powered on with the dataLogger enabled at some point
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDataLogger.CURRENTRUNINDEX_INVALID</c>.
     * </para>
     */
    public async Task<int> get_currentRunIndex()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CURRENTRUNINDEX_INVALID;
            }
        }
        res = _currentRunIndex;
        return res;
    }


    /**
     * <summary>
     *   Returns the Unix timestamp for current UTC time, if known.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Unix timestamp for current UTC time, if known
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDataLogger.TIMEUTC_INVALID</c>.
     * </para>
     */
    public async Task<long> get_timeUTC()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return TIMEUTC_INVALID;
            }
        }
        res = _timeUTC;
        return res;
    }


    /**
     * <summary>
     *   Changes the current UTC time reference used for recorded data.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current UTC time reference used for recorded data
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_timeUTC(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("timeUTC",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current activation state of the data logger.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YDataLogger.RECORDING_OFF</c>, <c>YDataLogger.RECORDING_ON</c> and
     *   <c>YDataLogger.RECORDING_PENDING</c> corresponding to the current activation state of the data logger
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDataLogger.RECORDING_INVALID</c>.
     * </para>
     */
    public async Task<int> get_recording()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return RECORDING_INVALID;
            }
        }
        res = _recording;
        return res;
    }


    /**
     * <summary>
     *   Changes the activation state of the data logger to start/stop recording data.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YDataLogger.RECORDING_OFF</c>, <c>YDataLogger.RECORDING_ON</c> and
     *   <c>YDataLogger.RECORDING_PENDING</c> corresponding to the activation state of the data logger to
     *   start/stop recording data
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_recording(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("recording",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the default activation state of the data logger on power up.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YDataLogger.AUTOSTART_OFF</c> or <c>YDataLogger.AUTOSTART_ON</c>, according to the
     *   default activation state of the data logger on power up
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDataLogger.AUTOSTART_INVALID</c>.
     * </para>
     */
    public async Task<int> get_autoStart()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return AUTOSTART_INVALID;
            }
        }
        res = _autoStart;
        return res;
    }


    /**
     * <summary>
     *   Changes the default activation state of the data logger on power up.
     * <para>
     *   Do not forget to call the <c>saveToFlash()</c> method of the module to save the
     *   configuration change.  Note: if the device doesn't have any time source at his disposal when
     *   starting up, it will wait for ~8 seconds before automatically starting to record  with
     *   an arbitrary timestamp
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YDataLogger.AUTOSTART_OFF</c> or <c>YDataLogger.AUTOSTART_ON</c>, according to the
     *   default activation state of the data logger on power up
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_autoStart(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("autoStart",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns true if the data logger is synchronised with the localization beacon.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YDataLogger.BEACONDRIVEN_OFF</c> or <c>YDataLogger.BEACONDRIVEN_ON</c>, according to true
     *   if the data logger is synchronised with the localization beacon
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDataLogger.BEACONDRIVEN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_beaconDriven()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return BEACONDRIVEN_INVALID;
            }
        }
        res = _beaconDriven;
        return res;
    }


    /**
     * <summary>
     *   Changes the type of synchronisation of the data logger.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YDataLogger.BEACONDRIVEN_OFF</c> or <c>YDataLogger.BEACONDRIVEN_ON</c>, according to the
     *   type of synchronisation of the data logger
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_beaconDriven(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("beaconDriven",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the percentage of datalogger memory in use.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the percentage of datalogger memory in use
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YDataLogger.USAGE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_usage()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return USAGE_INVALID;
            }
        }
        res = _usage;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_clearHistory()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CLEARHISTORY_INVALID;
            }
        }
        res = _clearHistory;
        return res;
    }


    public async Task<int> set_clearHistory(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("clearHistory",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a data logger for a given identifier.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the data logger is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDataLogger.isOnline()</c> to test if the data logger is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a data logger by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the matching device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the data logger, for instance
     *   <c>LIGHTMK4.dataLogger</c>.
     * </param>
     * <returns>
     *   a <c>YDataLogger</c> object allowing you to drive the data logger.
     * </returns>
     */
    public static YDataLogger FindDataLogger(string func)
    {
        YDataLogger obj;
        obj = (YDataLogger) YFunction._FindFromCache("DataLogger", func);
        if (obj == null) {
            obj = new YDataLogger(func);
            YFunction._AddToCache("DataLogger", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a data logger for a given identifier in a YAPI context.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the data logger is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YDataLogger.isOnline()</c> to test if the data logger is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a data logger by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the data logger, for instance
     *   <c>LIGHTMK4.dataLogger</c>.
     * </param>
     * <returns>
     *   a <c>YDataLogger</c> object allowing you to drive the data logger.
     * </returns>
     */
    public static YDataLogger FindDataLoggerInContext(YAPIContext yctx,string func)
    {
        YDataLogger obj;
        obj = (YDataLogger) YFunction._FindFromCacheInContext(yctx, "DataLogger", func);
        if (obj == null) {
            obj = new YDataLogger(yctx, func);
            YFunction._AddToCache("DataLogger", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and the character string describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public async Task<int> registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            await YFunction._UpdateValueCallbackList(this, true);
        } else {
            await YFunction._UpdateValueCallbackList(this, false);
        }
        _valueCallbackDataLogger = callback;
        // Immediately invoke value callback with current value
        if (callback != null && await this.isOnline()) {
            val = _advertisedValue;
            if (!(val == "")) {
                await this._invokeValueCallback(val);
            }
        }
        return 0;
    }

    public override async Task<int> _invokeValueCallback(string value)
    {
        if (_valueCallbackDataLogger != null) {
            await _valueCallbackDataLogger(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Clears the data logger memory and discards all recorded data streams.
     * <para>
     *   This method also resets the current run index to zero.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> forgetAllDataStreams()
    {
        return await this.set_clearHistory(CLEARHISTORY_TRUE);
    }

    /**
     * <summary>
     *   Returns a list of <c>YDataSet</c> objects that can be used to retrieve
     *   all measures stored by the data logger.
     * <para>
     * </para>
     * <para>
     *   This function only works if the device uses a recent firmware,
     *   as <c>YDataSet</c> objects are not supported by firmwares older than
     *   version 13000.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of <c>YDataSet</c> object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list.
     * </para>
     */
    public virtual async Task<List<YDataSet>> get_dataSets()
    {
        return await this.parse_dataSets(await this._download("logger.json"));
    }

    public virtual async Task<List<YDataSet>> parse_dataSets(byte[] json)
    {
        List<string> dslist = new List<string>();
        YDataSet dataset;
        List<YDataSet> res = new List<YDataSet>();

        dslist = this.imm_json_get_array(json);
        res.Clear();
        for (int ii_0 = 0; ii_0 < dslist.Count; ii_0++) {
            dataset = new YDataSet(this);
            await dataset._parse(dslist[ii_0]);
            res.Add(dataset);
        }
        return res;
    }

    /**
     * <summary>
     *   Continues the enumeration of data loggers started using <c>yFirstDataLogger()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned data loggers order.
     *   If you want to find a specific a data logger, use <c>DataLogger.findDataLogger()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDataLogger</c> object, corresponding to
     *   a data logger currently online, or a <c>null</c> pointer
     *   if there are no more data loggers to enumerate.
     * </returns>
     */
    public YDataLogger nextDataLogger()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindDataLoggerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of data loggers currently accessible.
     * <para>
     *   Use the method <c>YDataLogger.nextDataLogger()</c> to iterate on
     *   next data loggers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YDataLogger</c> object, corresponding to
     *   the first data logger currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDataLogger FirstDataLogger()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("DataLogger");
        if (next_hwid == null)  return null;
        return FindDataLoggerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of data loggers currently accessible.
     * <para>
     *   Use the method <c>YDataLogger.nextDataLogger()</c> to iterate on
     *   next data loggers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YDataLogger</c> object, corresponding to
     *   the first data logger currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YDataLogger FirstDataLoggerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("DataLogger");
        if (next_hwid == null)  return null;
        return FindDataLoggerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YDataLogger implementation)
     }

}