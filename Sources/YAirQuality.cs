/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindAirQuality(), the high-level API for AirQuality functions
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

//--- (YAirQuality return codes)
//--- (end of YAirQuality return codes)
//--- (YAirQuality class start)
/**
 * <summary>
 *   YAirQuality Class: air quality sensor control interface
 * <para>
 *   The <c>YAirQuality</c> class allows you to read and configure Yoctopuce air quality sensors.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YAirQuality : YSensor
{
//--- (end of YAirQuality class start)
//--- (YAirQuality definitions)
    /**
     * <summary>
     *   invalid ubaIndex value
     * </summary>
     */
    public const  double UBAINDEX_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid relativeIndex value
     * </summary>
     */
    public const  double RELATIVEINDEX_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid aqiMode value
     * </summary>
     */
    public const int AQIMODE_RELATIVE = 0;
    public const int AQIMODE_UBA = 1;
    public const int AQIMODE_INVALID = -1;
    protected double _ubaIndex = UBAINDEX_INVALID;
    protected double _relativeIndex = RELATIVEINDEX_INVALID;
    protected int _aqiMode = AQIMODE_INVALID;
    protected ValueCallback _valueCallbackAirQuality = null;
    protected TimedReportCallback _timedReportCallbackAirQuality = null;

    public new delegate Task ValueCallback(YAirQuality func, string value);
    public new delegate Task TimedReportCallback(YAirQuality func, YMeasure measure);
    //--- (end of YAirQuality definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAirQuality(YAPIContext ctx, string func)
        : base(ctx, func, "AirQuality")
    {
        //--- (YAirQuality attributes initialization)
        //--- (end of YAirQuality attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAirQuality(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YAirQuality implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("ubaIndex")) {
            _ubaIndex = Math.Round(json_val.getDouble("ubaIndex") / 65.536) / 1000.0;
        }
        if (json_val.has("relativeIndex")) {
            _relativeIndex = Math.Round(json_val.getDouble("relativeIndex") / 65.536) / 1000.0;
        }
        if (json_val.has("aqiMode")) {
            _aqiMode = json_val.getInt("aqiMode");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current air quality index, according to UBA (from 1 to 5).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current air quality index, according to UBA (from 1 to 5)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAirQuality.UBAINDEX_INVALID</c>.
     * </para>
     */
    public async Task<double> get_ubaIndex()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return UBAINDEX_INVALID;
            }
        }
        res = _ubaIndex;
        return res;
    }


    /**
     * <summary>
     *   Returns the relative air quality index, according to ScioSense (from 0 to 500).
     * <para>
     *   A value below 100 indicates better-than-average air quality compared to the past 24 hours,
     *   while a value above 100 indicates poorer-than-average air quality compared to the past 24 hours.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the relative air quality index, according to ScioSense (from 0 to 500)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAirQuality.RELATIVEINDEX_INVALID</c>.
     * </para>
     */
    public async Task<double> get_relativeIndex()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return RELATIVEINDEX_INVALID;
            }
        }
        res = _relativeIndex;
        return res;
    }


    /**
     * <summary>
     *   Returns the type of index reported by the get_currentValue function and callbacks (UBA index or relative index).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YAirQuality.AQIMODE_RELATIVE</c> or <c>YAirQuality.AQIMODE_UBA</c>, according to the type
     *   of index reported by the get_currentValue function and callbacks (UBA index or relative index)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAirQuality.AQIMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_aqiMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return AQIMODE_INVALID;
            }
        }
        res = _aqiMode;
        return res;
    }


    /**
     * <summary>
     *   Changes the the type of index reported by the get_currentValue function and callbacks (UBA index or relative index).
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YAirQuality.AQIMODE_RELATIVE</c> or <c>YAirQuality.AQIMODE_UBA</c>, according to the the
     *   type of index reported by the get_currentValue function and callbacks (UBA index or relative index)
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
    public async Task<int> set_aqiMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("aqiMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a air quality sensor for a given identifier.
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
     *   This function does not require that the air quality sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAirQuality.isOnline()</c> to test if the air quality sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a air quality sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the air quality sensor, for instance
     *   <c>MyDevice.airQuality</c>.
     * </param>
     * <returns>
     *   a <c>YAirQuality</c> object allowing you to drive the air quality sensor.
     * </returns>
     */
    public static YAirQuality FindAirQuality(string func)
    {
        YAirQuality obj;
        obj = (YAirQuality) YFunction._FindFromCache("AirQuality", func);
        if (obj == null) {
            obj = new YAirQuality(func);
            YFunction._AddToCache("AirQuality", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a air quality sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the air quality sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAirQuality.isOnline()</c> to test if the air quality sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a air quality sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the air quality sensor, for instance
     *   <c>MyDevice.airQuality</c>.
     * </param>
     * <returns>
     *   a <c>YAirQuality</c> object allowing you to drive the air quality sensor.
     * </returns>
     */
    public static YAirQuality FindAirQualityInContext(YAPIContext yctx,string func)
    {
        YAirQuality obj;
        obj = (YAirQuality) YFunction._FindFromCacheInContext(yctx, "AirQuality", func);
        if (obj == null) {
            obj = new YAirQuality(yctx, func);
            YFunction._AddToCache("AirQuality", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is then invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness,
     *   remember to call one of these two functions periodically. The callback is called once juste after beeing
     *   registered, passing the current advertised value  of the function, provided that it is not an empty string.
     *   To unregister a callback, pass a null pointer as argument.
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
        _valueCallbackAirQuality = callback;
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
        if (_valueCallbackAirQuality != null) {
            await _valueCallbackAirQuality(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every periodic timed notification.
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
     *   arguments: the function object of which the value has changed, and an <c>YMeasure</c> object describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public async Task<int> registerTimedReportCallback(TimedReportCallback callback)
    {
        YSensor sensor;
        sensor = this;
        if (callback != null) {
            await YFunction._UpdateTimedReportCallbackList(sensor, true);
        } else {
            await YFunction._UpdateTimedReportCallbackList(sensor, false);
        }
        _timedReportCallbackAirQuality = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackAirQuality != null) {
            await _timedReportCallbackAirQuality(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of air quality sensors started using <c>yFirstAirQuality()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned air quality sensors order.
     *   If you want to find a specific a air quality sensor, use <c>AirQuality.findAirQuality()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAirQuality</c> object, corresponding to
     *   a air quality sensor currently online, or a <c>null</c> pointer
     *   if there are no more air quality sensors to enumerate.
     * </returns>
     */
    public YAirQuality nextAirQuality()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindAirQualityInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of air quality sensors currently accessible.
     * <para>
     *   Use the method <c>YAirQuality.nextAirQuality()</c> to iterate on
     *   next air quality sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAirQuality</c> object, corresponding to
     *   the first air quality sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAirQuality FirstAirQuality()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AirQuality");
        if (next_hwid == null)  return null;
        return FindAirQualityInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of air quality sensors currently accessible.
     * <para>
     *   Use the method <c>YAirQuality.nextAirQuality()</c> to iterate on
     *   next air quality sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YAirQuality</c> object, corresponding to
     *   the first air quality sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAirQuality FirstAirQualityInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AirQuality");
        if (next_hwid == null)  return null;
        return FindAirQualityInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YAirQuality implementation)
}
}

