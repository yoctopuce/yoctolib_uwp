/*********************************************************************
 *
 *  $Id: YLatitude.cs 33718 2018-12-14 14:22:23Z seb $
 *
 *  Implements FindLatitude(), the high-level API for Latitude functions
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

//--- (YLatitude return codes)
//--- (end of YLatitude return codes)
//--- (YLatitude class start)
/**
 * <summary>
 *   YLatitude Class: Latitude function interface
 * <para>
 *   The Yoctopuce class YLatitude allows you to read the latitude from Yoctopuce
 *   geolocation sensors. It inherits from the YSensor class the core functions to
 *   read measurements, to register callback functions, to access the autonomous
 *   datalogger.
 * </para>
 * </summary>
 */
public class YLatitude : YSensor
{
//--- (end of YLatitude class start)
//--- (YLatitude definitions)
    protected ValueCallback _valueCallbackLatitude = null;
    protected TimedReportCallback _timedReportCallbackLatitude = null;

    public new delegate Task ValueCallback(YLatitude func, string value);
    public new delegate Task TimedReportCallback(YLatitude func, YMeasure measure);
    //--- (end of YLatitude definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLatitude(YAPIContext ctx, string func)
        : base(ctx, func, "Latitude")
    {
        //--- (YLatitude attributes initialization)
        //--- (end of YLatitude attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YLatitude(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YLatitude implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Retrieves a latitude sensor for a given identifier.
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
     *   This function does not require that the latitude sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLatitude.isOnline()</c> to test if the latitude sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a latitude sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the latitude sensor
     * </param>
     * <returns>
     *   a <c>YLatitude</c> object allowing you to drive the latitude sensor.
     * </returns>
     */
    public static YLatitude FindLatitude(string func)
    {
        YLatitude obj;
        obj = (YLatitude) YFunction._FindFromCache("Latitude", func);
        if (obj == null) {
            obj = new YLatitude(func);
            YFunction._AddToCache("Latitude",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a latitude sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the latitude sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YLatitude.isOnline()</c> to test if the latitude sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a latitude sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the latitude sensor
     * </param>
     * <returns>
     *   a <c>YLatitude</c> object allowing you to drive the latitude sensor.
     * </returns>
     */
    public static YLatitude FindLatitudeInContext(YAPIContext yctx,string func)
    {
        YLatitude obj;
        obj = (YLatitude) YFunction._FindFromCacheInContext(yctx,  "Latitude", func);
        if (obj == null) {
            obj = new YLatitude(yctx, func);
            YFunction._AddToCache("Latitude",  func, obj);
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
        _valueCallbackLatitude = callback;
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
        if (_valueCallbackLatitude != null) {
            await _valueCallbackLatitude(this, value);
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
     *   arguments: the function object of which the value has changed, and an YMeasure object describing
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
        _timedReportCallbackLatitude = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackLatitude != null) {
            await _timedReportCallbackLatitude(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of latitude sensors started using <c>yFirstLatitude()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned latitude sensors order.
     *   If you want to find a specific a latitude sensor, use <c>Latitude.findLatitude()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLatitude</c> object, corresponding to
     *   a latitude sensor currently online, or a <c>null</c> pointer
     *   if there are no more latitude sensors to enumerate.
     * </returns>
     */
    public YLatitude nextLatitude()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindLatitudeInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of latitude sensors currently accessible.
     * <para>
     *   Use the method <c>YLatitude.nextLatitude()</c> to iterate on
     *   next latitude sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YLatitude</c> object, corresponding to
     *   the first latitude sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLatitude FirstLatitude()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Latitude");
        if (next_hwid == null)  return null;
        return FindLatitudeInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of latitude sensors currently accessible.
     * <para>
     *   Use the method <c>YLatitude.nextLatitude()</c> to iterate on
     *   next latitude sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YLatitude</c> object, corresponding to
     *   the first latitude sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YLatitude FirstLatitudeInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Latitude");
        if (next_hwid == null)  return null;
        return FindLatitudeInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YLatitude implementation)
}
}

