/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindRealTimeClock(), the high-level API for RealTimeClock functions
 *
 *  - - - - - - - - - License information: - - - - - - - - -
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
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
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
using System.Threading.Tasks;
namespace com.yoctopuce.YoctoAPI
{

//--- (YRealTimeClock return codes)
//--- (end of YRealTimeClock return codes)
//--- (YRealTimeClock class start)
/**
 * <summary>
 *   YRealTimeClock Class: real-time clock control interface, available for instance in the
 *   YoctoHub-GSM-4G, the YoctoHub-Wireless-SR, the YoctoHub-Wireless-g or the YoctoHub-Wireless-n
 * <para>
 *   The <c>YRealTimeClock</c> class provide access to the embedded real-time clock available on some Yoctopuce
 *   devices. It can provide current date and time, even after a power outage
 *   lasting several days. It is the base for automated wake-up functions provided by the WakeUpScheduler.
 *   The current time may represent a local time as well as an UTC time, but no automatic time change
 *   will occur to account for daylight saving time.
 * </para>
 * </summary>
 */
public class YRealTimeClock : YFunction
{
//--- (end of YRealTimeClock class start)
//--- (YRealTimeClock definitions)
    /**
     * <summary>
     *   invalid unixTime value
     * </summary>
     */
    public const  long UNIXTIME_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid dateTime value
     * </summary>
     */
    public const  string DATETIME_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid utcOffset value
     * </summary>
     */
    public const  int UTCOFFSET_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid timeSet value
     * </summary>
     */
    public const int TIMESET_FALSE = 0;
    public const int TIMESET_TRUE = 1;
    public const int TIMESET_INVALID = -1;
    /**
     * <summary>
     *   invalid disableHostSync value
     * </summary>
     */
    public const int DISABLEHOSTSYNC_FALSE = 0;
    public const int DISABLEHOSTSYNC_TRUE = 1;
    public const int DISABLEHOSTSYNC_INVALID = -1;
    protected long _unixTime = UNIXTIME_INVALID;
    protected string _dateTime = DATETIME_INVALID;
    protected int _utcOffset = UTCOFFSET_INVALID;
    protected int _timeSet = TIMESET_INVALID;
    protected int _disableHostSync = DISABLEHOSTSYNC_INVALID;
    protected ValueCallback _valueCallbackRealTimeClock = null;

    public new delegate Task ValueCallback(YRealTimeClock func, string value);
    public new delegate Task TimedReportCallback(YRealTimeClock func, YMeasure measure);
    //--- (end of YRealTimeClock definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRealTimeClock(YAPIContext ctx, string func)
        : base(ctx, func, "RealTimeClock")
    {
        //--- (YRealTimeClock attributes initialization)
        //--- (end of YRealTimeClock attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRealTimeClock(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YRealTimeClock implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("unixTime")) {
            _unixTime = json_val.getLong("unixTime");
        }
        if (json_val.has("dateTime")) {
            _dateTime = json_val.getString("dateTime");
        }
        if (json_val.has("utcOffset")) {
            _utcOffset = json_val.getInt("utcOffset");
        }
        if (json_val.has("timeSet")) {
            _timeSet = json_val.getInt("timeSet") > 0 ? 1 : 0;
        }
        if (json_val.has("disableHostSync")) {
            _disableHostSync = json_val.getInt("disableHostSync") > 0 ? 1 : 0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current time in Unix format (number of elapsed seconds since Jan 1st, 1970).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current time in Unix format (number of elapsed seconds since Jan 1st, 1970)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRealTimeClock.UNIXTIME_INVALID</c>.
     * </para>
     */
    public async Task<long> get_unixTime()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return UNIXTIME_INVALID;
            }
        }
        res = _unixTime;
        return res;
    }


    /**
     * <summary>
     *   Changes the current time.
     * <para>
     *   Time is specifid in Unix format (number of elapsed seconds since Jan 1st, 1970).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the current time
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
    public async Task<int> set_unixTime(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("unixTime",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current time in the form "YYYY/MM/DD hh:mm:ss".
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the current time in the form "YYYY/MM/DD hh:mm:ss"
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRealTimeClock.DATETIME_INVALID</c>.
     * </para>
     */
    public async Task<string> get_dateTime()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DATETIME_INVALID;
            }
        }
        res = _dateTime;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of seconds between current time and UTC time (time zone).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of seconds between current time and UTC time (time zone)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRealTimeClock.UTCOFFSET_INVALID</c>.
     * </para>
     */
    public async Task<int> get_utcOffset()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return UTCOFFSET_INVALID;
            }
        }
        res = _utcOffset;
        return res;
    }


    /**
     * <summary>
     *   Changes the number of seconds between current time and UTC time (time zone).
     * <para>
     *   The timezone is automatically rounded to the nearest multiple of 15 minutes.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the number of seconds between current time and UTC time (time zone)
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
    public async Task<int> set_utcOffset(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("utcOffset",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns true if the clock has been set, and false otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YRealTimeClock.TIMESET_FALSE</c> or <c>YRealTimeClock.TIMESET_TRUE</c>, according to true
     *   if the clock has been set, and false otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRealTimeClock.TIMESET_INVALID</c>.
     * </para>
     */
    public async Task<int> get_timeSet()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return TIMESET_INVALID;
            }
        }
        res = _timeSet;
        return res;
    }


    /**
     * <summary>
     *   Returns true if the automatic clock synchronization with host has been disabled,
     *   and false otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YRealTimeClock.DISABLEHOSTSYNC_FALSE</c> or <c>YRealTimeClock.DISABLEHOSTSYNC_TRUE</c>,
     *   according to true if the automatic clock synchronization with host has been disabled,
     *   and false otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRealTimeClock.DISABLEHOSTSYNC_INVALID</c>.
     * </para>
     */
    public async Task<int> get_disableHostSync()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DISABLEHOSTSYNC_INVALID;
            }
        }
        res = _disableHostSync;
        return res;
    }


    /**
     * <summary>
     *   Changes the automatic clock synchronization with host working state.
     * <para>
     *   To disable automatic synchronization, set the value to true.
     *   To enable automatic synchronization (default), set the value to false.
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YRealTimeClock.DISABLEHOSTSYNC_FALSE</c> or <c>YRealTimeClock.DISABLEHOSTSYNC_TRUE</c>,
     *   according to the automatic clock synchronization with host working state
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
    public async Task<int> set_disableHostSync(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("disableHostSync",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a real-time clock for a given identifier.
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
     *   This function does not require that the real-time clock is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRealTimeClock.isOnline()</c> to test if the real-time clock is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a real-time clock by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the real-time clock, for instance
     *   <c>YHUBGSM5.realTimeClock</c>.
     * </param>
     * <returns>
     *   a <c>YRealTimeClock</c> object allowing you to drive the real-time clock.
     * </returns>
     */
    public static YRealTimeClock FindRealTimeClock(string func)
    {
        YRealTimeClock obj;
        obj = (YRealTimeClock) YFunction._FindFromCache("RealTimeClock", func);
        if (obj == null) {
            obj = new YRealTimeClock(func);
            YFunction._AddToCache("RealTimeClock", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a real-time clock for a given identifier in a YAPI context.
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
     *   This function does not require that the real-time clock is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRealTimeClock.isOnline()</c> to test if the real-time clock is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a real-time clock by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the real-time clock, for instance
     *   <c>YHUBGSM5.realTimeClock</c>.
     * </param>
     * <returns>
     *   a <c>YRealTimeClock</c> object allowing you to drive the real-time clock.
     * </returns>
     */
    public static YRealTimeClock FindRealTimeClockInContext(YAPIContext yctx,string func)
    {
        YRealTimeClock obj;
        obj = (YRealTimeClock) YFunction._FindFromCacheInContext(yctx, "RealTimeClock", func);
        if (obj == null) {
            obj = new YRealTimeClock(yctx, func);
            YFunction._AddToCache("RealTimeClock", func, obj);
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
        _valueCallbackRealTimeClock = callback;
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
        if (_valueCallbackRealTimeClock != null) {
            await _valueCallbackRealTimeClock(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of real-time clocks started using <c>yFirstRealTimeClock()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned real-time clocks order.
     *   If you want to find a specific a real-time clock, use <c>RealTimeClock.findRealTimeClock()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRealTimeClock</c> object, corresponding to
     *   a real-time clock currently online, or a <c>null</c> pointer
     *   if there are no more real-time clocks to enumerate.
     * </returns>
     */
    public YRealTimeClock nextRealTimeClock()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindRealTimeClockInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of real-time clocks currently accessible.
     * <para>
     *   Use the method <c>YRealTimeClock.nextRealTimeClock()</c> to iterate on
     *   next real-time clocks.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRealTimeClock</c> object, corresponding to
     *   the first real-time clock currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRealTimeClock FirstRealTimeClock()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RealTimeClock");
        if (next_hwid == null)  return null;
        return FindRealTimeClockInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of real-time clocks currently accessible.
     * <para>
     *   Use the method <c>YRealTimeClock.nextRealTimeClock()</c> to iterate on
     *   next real-time clocks.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YRealTimeClock</c> object, corresponding to
     *   the first real-time clock currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRealTimeClock FirstRealTimeClockInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RealTimeClock");
        if (next_hwid == null)  return null;
        return FindRealTimeClockInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YRealTimeClock implementation)
}
}

