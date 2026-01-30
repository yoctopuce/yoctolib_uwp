/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindAngularSpeed(), the high-level API for AngularSpeed functions
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

//--- (YAngularSpeed return codes)
//--- (end of YAngularSpeed return codes)
//--- (YAngularSpeed class start)
/**
 * <summary>
 *   YAngularSpeed Class: tachometer control interface
 * <para>
 *   The <c>YAngularSpeed</c> class allows you to read and configure Yoctopuce tachometers.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YAngularSpeed : YSensor
{
//--- (end of YAngularSpeed class start)
//--- (YAngularSpeed definitions)
    protected ValueCallback _valueCallbackAngularSpeed = null;
    protected TimedReportCallback _timedReportCallbackAngularSpeed = null;

    public new delegate Task ValueCallback(YAngularSpeed func, string value);
    public new delegate Task TimedReportCallback(YAngularSpeed func, YMeasure measure);
    //--- (end of YAngularSpeed definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAngularSpeed(YAPIContext ctx, string func)
        : base(ctx, func, "AngularSpeed")
    {
        //--- (YAngularSpeed attributes initialization)
        //--- (end of YAngularSpeed attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAngularSpeed(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YAngularSpeed implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Retrieves a tachometer for a given identifier.
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
     *   This function does not require that the rtachometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAngularSpeed.isOnline()</c> to test if the rtachometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a tachometer by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the rtachometer, for instance
     *   <c>MyDevice.angularSpeed</c>.
     * </param>
     * <returns>
     *   a <c>YAngularSpeed</c> object allowing you to drive the rtachometer.
     * </returns>
     */
    public static YAngularSpeed FindAngularSpeed(string func)
    {
        YAngularSpeed obj;
        obj = (YAngularSpeed) YFunction._FindFromCache("AngularSpeed", func);
        if (obj == null) {
            obj = new YAngularSpeed(func);
            YFunction._AddToCache("AngularSpeed", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a tachometer for a given identifier in a YAPI context.
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
     *   This function does not require that the rtachometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAngularSpeed.isOnline()</c> to test if the rtachometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a tachometer by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the rtachometer, for instance
     *   <c>MyDevice.angularSpeed</c>.
     * </param>
     * <returns>
     *   a <c>YAngularSpeed</c> object allowing you to drive the rtachometer.
     * </returns>
     */
    public static YAngularSpeed FindAngularSpeedInContext(YAPIContext yctx,string func)
    {
        YAngularSpeed obj;
        obj = (YAngularSpeed) YFunction._FindFromCacheInContext(yctx, "AngularSpeed", func);
        if (obj == null) {
            obj = new YAngularSpeed(yctx, func);
            YFunction._AddToCache("AngularSpeed", func, obj);
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
        _valueCallbackAngularSpeed = callback;
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
        if (_valueCallbackAngularSpeed != null) {
            await _valueCallbackAngularSpeed(this, value);
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
        _timedReportCallbackAngularSpeed = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackAngularSpeed != null) {
            await _timedReportCallbackAngularSpeed(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of tachometers started using <c>yFirstAngularSpeed()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned tachometers order.
     *   If you want to find a specific a tachometer, use <c>AngularSpeed.findAngularSpeed()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAngularSpeed</c> object, corresponding to
     *   a tachometer currently online, or a <c>null</c> pointer
     *   if there are no more tachometers to enumerate.
     * </returns>
     */
    public YAngularSpeed nextAngularSpeed()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindAngularSpeedInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of tachometers currently accessible.
     * <para>
     *   Use the method <c>YAngularSpeed.nextAngularSpeed()</c> to iterate on
     *   next tachometers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAngularSpeed</c> object, corresponding to
     *   the first tachometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAngularSpeed FirstAngularSpeed()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AngularSpeed");
        if (next_hwid == null)  return null;
        return FindAngularSpeedInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of tachometers currently accessible.
     * <para>
     *   Use the method <c>YAngularSpeed.nextAngularSpeed()</c> to iterate on
     *   next tachometers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YAngularSpeed</c> object, corresponding to
     *   the first tachometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAngularSpeed FirstAngularSpeedInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("AngularSpeed");
        if (next_hwid == null)  return null;
        return FindAngularSpeedInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YAngularSpeed implementation)
}
}

