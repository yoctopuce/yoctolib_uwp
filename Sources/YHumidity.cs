/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindHumidity(), the high-level API for Humidity functions
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

//--- (YHumidity return codes)
//--- (end of YHumidity return codes)
//--- (YHumidity class start)
/**
 * <summary>
 *   YHumidity Class: humidity sensor control interface, available for instance in the Yocto-CO2-V2, the
 *   Yocto-Meteo-V2 or the Yocto-VOC-V3
 * <para>
 *   The <c>YHumidity</c> class allows you to read and configure Yoctopuce humidity sensors.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YHumidity : YSensor
{
//--- (end of YHumidity class start)
//--- (YHumidity definitions)
    /**
     * <summary>
     *   invalid relHum value
     * </summary>
     */
    public const  double RELHUM_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid absHum value
     * </summary>
     */
    public const  double ABSHUM_INVALID = YAPI.INVALID_DOUBLE;
    protected double _relHum = RELHUM_INVALID;
    protected double _absHum = ABSHUM_INVALID;
    protected ValueCallback _valueCallbackHumidity = null;
    protected TimedReportCallback _timedReportCallbackHumidity = null;

    public new delegate Task ValueCallback(YHumidity func, string value);
    public new delegate Task TimedReportCallback(YHumidity func, YMeasure measure);
    //--- (end of YHumidity definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YHumidity(YAPIContext ctx, string func)
        : base(ctx, func, "Humidity")
    {
        //--- (YHumidity attributes initialization)
        //--- (end of YHumidity attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YHumidity(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YHumidity implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("relHum")) {
            _relHum = Math.Round(json_val.getDouble("relHum") / 65.536) / 1000.0;
        }
        if (json_val.has("absHum")) {
            _absHum = Math.Round(json_val.getDouble("absHum") / 65.536) / 1000.0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the primary unit for measuring humidity.
     * <para>
     *   That unit is a string.
     *   If that strings starts with the letter 'g', the primary measured value is the absolute
     *   humidity, in g/m3. Otherwise, the primary measured value will be the relative humidity
     *   (RH), in per cents.
     * </para>
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification
     *   must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the primary unit for measuring humidity
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
    public async Task<int> set_unit(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("unit",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current relative humidity, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current relative humidity, in per cents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YHumidity.RELHUM_INVALID</c>.
     * </para>
     */
    public async Task<double> get_relHum()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return RELHUM_INVALID;
            }
        }
        res = _relHum;
        return res;
    }


    /**
     * <summary>
     *   Returns the current absolute humidity, in grams per cubic meter of air.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current absolute humidity, in grams per cubic meter of air
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YHumidity.ABSHUM_INVALID</c>.
     * </para>
     */
    public async Task<double> get_absHum()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ABSHUM_INVALID;
            }
        }
        res = _absHum;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a humidity sensor for a given identifier.
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
     *   This function does not require that the humidity sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YHumidity.isOnline()</c> to test if the humidity sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a humidity sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the humidity sensor, for instance
     *   <c>YCO2MK02.humidity</c>.
     * </param>
     * <returns>
     *   a <c>YHumidity</c> object allowing you to drive the humidity sensor.
     * </returns>
     */
    public static YHumidity FindHumidity(string func)
    {
        YHumidity obj;
        obj = (YHumidity) YFunction._FindFromCache("Humidity", func);
        if (obj == null) {
            obj = new YHumidity(func);
            YFunction._AddToCache("Humidity", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a humidity sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the humidity sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YHumidity.isOnline()</c> to test if the humidity sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a humidity sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the humidity sensor, for instance
     *   <c>YCO2MK02.humidity</c>.
     * </param>
     * <returns>
     *   a <c>YHumidity</c> object allowing you to drive the humidity sensor.
     * </returns>
     */
    public static YHumidity FindHumidityInContext(YAPIContext yctx,string func)
    {
        YHumidity obj;
        obj = (YHumidity) YFunction._FindFromCacheInContext(yctx, "Humidity", func);
        if (obj == null) {
            obj = new YHumidity(yctx, func);
            YFunction._AddToCache("Humidity", func, obj);
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
        _valueCallbackHumidity = callback;
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
        if (_valueCallbackHumidity != null) {
            await _valueCallbackHumidity(this, value);
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
        _timedReportCallbackHumidity = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackHumidity != null) {
            await _timedReportCallbackHumidity(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of humidity sensors started using <c>yFirstHumidity()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned humidity sensors order.
     *   If you want to find a specific a humidity sensor, use <c>Humidity.findHumidity()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YHumidity</c> object, corresponding to
     *   a humidity sensor currently online, or a <c>null</c> pointer
     *   if there are no more humidity sensors to enumerate.
     * </returns>
     */
    public YHumidity nextHumidity()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindHumidityInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of humidity sensors currently accessible.
     * <para>
     *   Use the method <c>YHumidity.nextHumidity()</c> to iterate on
     *   next humidity sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YHumidity</c> object, corresponding to
     *   the first humidity sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YHumidity FirstHumidity()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Humidity");
        if (next_hwid == null)  return null;
        return FindHumidityInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of humidity sensors currently accessible.
     * <para>
     *   Use the method <c>YHumidity.nextHumidity()</c> to iterate on
     *   next humidity sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YHumidity</c> object, corresponding to
     *   the first humidity sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YHumidity FirstHumidityInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Humidity");
        if (next_hwid == null)  return null;
        return FindHumidityInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YHumidity implementation)
}
}

