/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindOrientation(), the high-level API for Orientation functions
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

//--- (YOrientation return codes)
//--- (end of YOrientation return codes)
//--- (YOrientation class start)
/**
 * <summary>
 *   YOrientation Class: orientation sensor control interface
 * <para>
 *   The <c>YOrientation</c> class allows you to read and configure Yoctopuce orientation sensors.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YOrientation : YSensor
{
//--- (end of YOrientation class start)
//--- (YOrientation definitions)
    protected ValueCallback _valueCallbackOrientation = null;
    protected TimedReportCallback _timedReportCallbackOrientation = null;

    public new delegate Task ValueCallback(YOrientation func, string value);
    public new delegate Task TimedReportCallback(YOrientation func, YMeasure measure);
    //--- (end of YOrientation definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YOrientation(YAPIContext ctx, string func)
        : base(ctx, func, "Orientation")
    {
        //--- (YOrientation attributes initialization)
        //--- (end of YOrientation attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YOrientation(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YOrientation implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Retrieves an orientation sensor for a given identifier.
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
     *   This function does not require that the orientation sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YOrientation.isOnline()</c> to test if the orientation sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an orientation sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the orientation sensor, for instance
     *   <c>MyDevice.orientation</c>.
     * </param>
     * <returns>
     *   a <c>YOrientation</c> object allowing you to drive the orientation sensor.
     * </returns>
     */
    public static YOrientation FindOrientation(string func)
    {
        YOrientation obj;
        obj = (YOrientation) YFunction._FindFromCache("Orientation", func);
        if (obj == null) {
            obj = new YOrientation(func);
            YFunction._AddToCache("Orientation", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an orientation sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the orientation sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YOrientation.isOnline()</c> to test if the orientation sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an orientation sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the orientation sensor, for instance
     *   <c>MyDevice.orientation</c>.
     * </param>
     * <returns>
     *   a <c>YOrientation</c> object allowing you to drive the orientation sensor.
     * </returns>
     */
    public static YOrientation FindOrientationInContext(YAPIContext yctx,string func)
    {
        YOrientation obj;
        obj = (YOrientation) YFunction._FindFromCacheInContext(yctx, "Orientation", func);
        if (obj == null) {
            obj = new YOrientation(yctx, func);
            YFunction._AddToCache("Orientation", func, obj);
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
        _valueCallbackOrientation = callback;
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
        if (_valueCallbackOrientation != null) {
            await _valueCallbackOrientation(this, value);
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
        _timedReportCallbackOrientation = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackOrientation != null) {
            await _timedReportCallbackOrientation(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of orientation sensors started using <c>yFirstOrientation()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned orientation sensors order.
     *   If you want to find a specific an orientation sensor, use <c>Orientation.findOrientation()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YOrientation</c> object, corresponding to
     *   an orientation sensor currently online, or a <c>null</c> pointer
     *   if there are no more orientation sensors to enumerate.
     * </returns>
     */
    public YOrientation nextOrientation()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindOrientationInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of orientation sensors currently accessible.
     * <para>
     *   Use the method <c>YOrientation.nextOrientation()</c> to iterate on
     *   next orientation sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YOrientation</c> object, corresponding to
     *   the first orientation sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YOrientation FirstOrientation()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Orientation");
        if (next_hwid == null)  return null;
        return FindOrientationInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of orientation sensors currently accessible.
     * <para>
     *   Use the method <c>YOrientation.nextOrientation()</c> to iterate on
     *   next orientation sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YOrientation</c> object, corresponding to
     *   the first orientation sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YOrientation FirstOrientationInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Orientation");
        if (next_hwid == null)  return null;
        return FindOrientationInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YOrientation implementation)
}
}

