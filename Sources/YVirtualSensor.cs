/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindVirtualSensor(), the high-level API for VirtualSensor functions
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

//--- (YVirtualSensor return codes)
//--- (end of YVirtualSensor return codes)
//--- (YVirtualSensor class start)
/**
 * <summary>
 *   YVirtualSensor Class: virtual sensor control interface
 * <para>
 *   The <c>YVirtualSensor</c> class allows you to use Yoctopuce virtual sensors.
 *   These sensors make it possible to show external data collected by the user
 *   as a Yoctopuce Sensor. This class inherits from <c>YSensor</c> class the core
 *   functions to read measurements, to register callback functions, and to access
 *   the autonomous datalogger. It adds the ability to change the sensor value as
 *   needed, or to mark current value as invalid.
 * </para>
 * </summary>
 */
public class YVirtualSensor : YSensor
{
//--- (end of YVirtualSensor class start)
//--- (YVirtualSensor definitions)
    /**
     * <summary>
     *   invalid invalidValue value
     * </summary>
     */
    public const  double INVALIDVALUE_INVALID = YAPI.INVALID_DOUBLE;
    protected double _invalidValue = INVALIDVALUE_INVALID;
    protected ValueCallback _valueCallbackVirtualSensor = null;
    protected TimedReportCallback _timedReportCallbackVirtualSensor = null;

    public new delegate Task ValueCallback(YVirtualSensor func, string value);
    public new delegate Task TimedReportCallback(YVirtualSensor func, YMeasure measure);
    //--- (end of YVirtualSensor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YVirtualSensor(YAPIContext ctx, string func)
        : base(ctx, func, "VirtualSensor")
    {
        //--- (YVirtualSensor attributes initialization)
        //--- (end of YVirtualSensor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YVirtualSensor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YVirtualSensor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("invalidValue")) {
            _invalidValue = Math.Round(json_val.getDouble("invalidValue") / 65.536) / 1000.0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the measured value.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the measured value
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
     *   Changes the current value of the sensor (raw value, before calibration).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the current value of the sensor (raw value, before calibration)
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
    public async Task<int> set_currentRawValue(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("currentRawValue",rest_val);
        return YAPI.SUCCESS;
    }

    public async Task<int> set_sensorState(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("sensorState",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the invalid value of the sensor, returned if the sensor is read when in invalid state
     *   (for instance before having been set).
     * <para>
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the invalid value of the sensor, returned if the sensor is
     *   read when in invalid state
     *   (for instance before having been set)
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
    public async Task<int> set_invalidValue(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("invalidValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the invalid value of the sensor, returned if the sensor is read when in invalid state
     *   (for instance before having been set).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the invalid value of the sensor, returned if the sensor is
     *   read when in invalid state
     *   (for instance before having been set)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YVirtualSensor.INVALIDVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_invalidValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return INVALIDVALUE_INVALID;
            }
        }
        res = _invalidValue;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a virtual sensor for a given identifier.
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
     *   This function does not require that the virtual sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YVirtualSensor.isOnline()</c> to test if the virtual sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a virtual sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the virtual sensor, for instance
     *   <c>MyDevice.virtualSensor1</c>.
     * </param>
     * <returns>
     *   a <c>YVirtualSensor</c> object allowing you to drive the virtual sensor.
     * </returns>
     */
    public static YVirtualSensor FindVirtualSensor(string func)
    {
        YVirtualSensor obj;
        obj = (YVirtualSensor) YFunction._FindFromCache("VirtualSensor", func);
        if (obj == null) {
            obj = new YVirtualSensor(func);
            YFunction._AddToCache("VirtualSensor", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a virtual sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the virtual sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YVirtualSensor.isOnline()</c> to test if the virtual sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a virtual sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the virtual sensor, for instance
     *   <c>MyDevice.virtualSensor1</c>.
     * </param>
     * <returns>
     *   a <c>YVirtualSensor</c> object allowing you to drive the virtual sensor.
     * </returns>
     */
    public static YVirtualSensor FindVirtualSensorInContext(YAPIContext yctx,string func)
    {
        YVirtualSensor obj;
        obj = (YVirtualSensor) YFunction._FindFromCacheInContext(yctx, "VirtualSensor", func);
        if (obj == null) {
            obj = new YVirtualSensor(yctx, func);
            YFunction._AddToCache("VirtualSensor", func, obj);
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
        _valueCallbackVirtualSensor = callback;
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
        if (_valueCallbackVirtualSensor != null) {
            await _valueCallbackVirtualSensor(this, value);
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
        _timedReportCallbackVirtualSensor = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackVirtualSensor != null) {
            await _timedReportCallbackVirtualSensor(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Changes the current sensor state to invalid (as if no value would have been ever set).
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> set_sensorAsInvalid()
    {
        return await this.set_sensorState(1);
    }

    /**
     * <summary>
     *   Continues the enumeration of virtual sensors started using <c>yFirstVirtualSensor()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned virtual sensors order.
     *   If you want to find a specific a virtual sensor, use <c>VirtualSensor.findVirtualSensor()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YVirtualSensor</c> object, corresponding to
     *   a virtual sensor currently online, or a <c>null</c> pointer
     *   if there are no more virtual sensors to enumerate.
     * </returns>
     */
    public YVirtualSensor nextVirtualSensor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindVirtualSensorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of virtual sensors currently accessible.
     * <para>
     *   Use the method <c>YVirtualSensor.nextVirtualSensor()</c> to iterate on
     *   next virtual sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YVirtualSensor</c> object, corresponding to
     *   the first virtual sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YVirtualSensor FirstVirtualSensor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("VirtualSensor");
        if (next_hwid == null)  return null;
        return FindVirtualSensorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of virtual sensors currently accessible.
     * <para>
     *   Use the method <c>YVirtualSensor.nextVirtualSensor()</c> to iterate on
     *   next virtual sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YVirtualSensor</c> object, corresponding to
     *   the first virtual sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YVirtualSensor FirstVirtualSensorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("VirtualSensor");
        if (next_hwid == null)  return null;
        return FindVirtualSensorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YVirtualSensor implementation)
}
}

