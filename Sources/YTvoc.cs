/*********************************************************************
 *
 *  $Id: YTvoc.cs 63510 2024-11-28 10:46:59Z seb $
 *
 *  Implements FindTvoc(), the high-level API for Tvoc functions
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

//--- (YTvoc return codes)
//--- (end of YTvoc return codes)
//--- (YTvoc class start)
/**
 * <summary>
 *   YTvoc Class: Total Volatile Organic Compound sensor control interface, available for instance in
 *   the Yocto-VOC-V3
 * <para>
 *   The <c>YTvoc</c> class allows you to read and configure Yoctopuce Total Volatile Organic Compound sensors.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YTvoc : YSensor
{
//--- (end of YTvoc class start)
//--- (YTvoc definitions)
    protected ValueCallback _valueCallbackTvoc = null;
    protected TimedReportCallback _timedReportCallbackTvoc = null;

    public new delegate Task ValueCallback(YTvoc func, string value);
    public new delegate Task TimedReportCallback(YTvoc func, YMeasure measure);
    //--- (end of YTvoc definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YTvoc(YAPIContext ctx, string func)
        : base(ctx, func, "Tvoc")
    {
        //--- (YTvoc attributes initialization)
        //--- (end of YTvoc attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YTvoc(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YTvoc implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Retrieves a Total  Volatile Organic Compound sensor for a given identifier.
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
     *   This function does not require that the Total  Volatile Organic Compound sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YTvoc.isOnline()</c> to test if the Total  Volatile Organic Compound sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a Total  Volatile Organic Compound sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the Total  Volatile Organic Compound sensor, for instance
     *   <c>YVOCMK03.tvoc</c>.
     * </param>
     * <returns>
     *   a <c>YTvoc</c> object allowing you to drive the Total  Volatile Organic Compound sensor.
     * </returns>
     */
    public static YTvoc FindTvoc(string func)
    {
        YTvoc obj;
        obj = (YTvoc) YFunction._FindFromCache("Tvoc", func);
        if (obj == null) {
            obj = new YTvoc(func);
            YFunction._AddToCache("Tvoc", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a Total  Volatile Organic Compound sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the Total  Volatile Organic Compound sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YTvoc.isOnline()</c> to test if the Total  Volatile Organic Compound sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a Total  Volatile Organic Compound sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the Total  Volatile Organic Compound sensor, for instance
     *   <c>YVOCMK03.tvoc</c>.
     * </param>
     * <returns>
     *   a <c>YTvoc</c> object allowing you to drive the Total  Volatile Organic Compound sensor.
     * </returns>
     */
    public static YTvoc FindTvocInContext(YAPIContext yctx,string func)
    {
        YTvoc obj;
        obj = (YTvoc) YFunction._FindFromCacheInContext(yctx, "Tvoc", func);
        if (obj == null) {
            obj = new YTvoc(yctx, func);
            YFunction._AddToCache("Tvoc", func, obj);
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
        _valueCallbackTvoc = callback;
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
        if (_valueCallbackTvoc != null) {
            await _valueCallbackTvoc(this, value);
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
        _timedReportCallbackTvoc = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackTvoc != null) {
            await _timedReportCallbackTvoc(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of Total Volatile Organic Compound sensors started using <c>yFirstTvoc()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned Total Volatile Organic Compound sensors order.
     *   If you want to find a specific a Total  Volatile Organic Compound sensor, use <c>Tvoc.findTvoc()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YTvoc</c> object, corresponding to
     *   a Total  Volatile Organic Compound sensor currently online, or a <c>null</c> pointer
     *   if there are no more Total Volatile Organic Compound sensors to enumerate.
     * </returns>
     */
    public YTvoc nextTvoc()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindTvocInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of Total Volatile Organic Compound sensors currently accessible.
     * <para>
     *   Use the method <c>YTvoc.nextTvoc()</c> to iterate on
     *   next Total Volatile Organic Compound sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YTvoc</c> object, corresponding to
     *   the first Total Volatile Organic Compound sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YTvoc FirstTvoc()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Tvoc");
        if (next_hwid == null)  return null;
        return FindTvocInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of Total Volatile Organic Compound sensors currently accessible.
     * <para>
     *   Use the method <c>YTvoc.nextTvoc()</c> to iterate on
     *   next Total Volatile Organic Compound sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YTvoc</c> object, corresponding to
     *   the first Total Volatile Organic Compound sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YTvoc FirstTvocInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Tvoc");
        if (next_hwid == null)  return null;
        return FindTvocInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YTvoc implementation)
}
}

