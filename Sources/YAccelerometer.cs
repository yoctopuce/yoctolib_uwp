/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindAccelerometer(), the high-level API for Accelerometer functions
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

//--- (YAccelerometer return codes)
//--- (end of YAccelerometer return codes)
//--- (YAccelerometer class start)
/**
 * <summary>
 *   YAccelerometer Class: accelerometer control interface, available for instance in the Yocto-3D-V2 or
 *   the Yocto-Inclinometer
 * <para>
 *   The <c>YAccelerometer</c> class allows you to read and configure Yoctopuce accelerometers.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 *   This class adds the possibility to access x, y and z components of the acceleration
 *   vector separately.
 * </para>
 * </summary>
 */
public class YAccelerometer : YSensor
{
//--- (end of YAccelerometer class start)
//--- (YAccelerometer definitions)
    /**
     * <summary>
     *   invalid bandwidth value
     * </summary>
     */
    public const  int BANDWIDTH_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid xValue value
     * </summary>
     */
    public const  double XVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid yValue value
     * </summary>
     */
    public const  double YVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid zValue value
     * </summary>
     */
    public const  double ZVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid gravityCancellation value
     * </summary>
     */
    public const int GRAVITYCANCELLATION_OFF = 0;
    public const int GRAVITYCANCELLATION_ON = 1;
    public const int GRAVITYCANCELLATION_INVALID = -1;
    protected int _bandwidth = BANDWIDTH_INVALID;
    protected double _xValue = XVALUE_INVALID;
    protected double _yValue = YVALUE_INVALID;
    protected double _zValue = ZVALUE_INVALID;
    protected int _gravityCancellation = GRAVITYCANCELLATION_INVALID;
    protected ValueCallback _valueCallbackAccelerometer = null;
    protected TimedReportCallback _timedReportCallbackAccelerometer = null;

    public new delegate Task ValueCallback(YAccelerometer func, string value);
    public new delegate Task TimedReportCallback(YAccelerometer func, YMeasure measure);
    //--- (end of YAccelerometer definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAccelerometer(YAPIContext ctx, string func)
        : base(ctx, func, "Accelerometer")
    {
        //--- (YAccelerometer attributes initialization)
        //--- (end of YAccelerometer attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YAccelerometer(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YAccelerometer implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("bandwidth")) {
            _bandwidth = json_val.getInt("bandwidth");
        }
        if (json_val.has("xValue")) {
            _xValue = Math.Round(json_val.getDouble("xValue") / 65.536) / 1000.0;
        }
        if (json_val.has("yValue")) {
            _yValue = Math.Round(json_val.getDouble("yValue") / 65.536) / 1000.0;
        }
        if (json_val.has("zValue")) {
            _zValue = Math.Round(json_val.getDouble("zValue") / 65.536) / 1000.0;
        }
        if (json_val.has("gravityCancellation")) {
            _gravityCancellation = json_val.getInt("gravityCancellation") > 0 ? 1 : 0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the measure update frequency, measured in Hz.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the measure update frequency, measured in Hz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.BANDWIDTH_INVALID</c>.
     * </para>
     */
    public async Task<int> get_bandwidth()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return BANDWIDTH_INVALID;
            }
        }
        res = _bandwidth;
        return res;
    }


    /**
     * <summary>
     *   Changes the measure update frequency, measured in Hz.
     * <para>
     *   When the
     *   frequency is lower, the device performs averaging.
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the measure update frequency, measured in Hz
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
    public async Task<int> set_bandwidth(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("bandwidth",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the X component of the acceleration, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the X component of the acceleration, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.XVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_xValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return XVALUE_INVALID;
            }
        }
        res = _xValue;
        return res;
    }


    /**
     * <summary>
     *   Returns the Y component of the acceleration, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Y component of the acceleration, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.YVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_yValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return YVALUE_INVALID;
            }
        }
        res = _yValue;
        return res;
    }


    /**
     * <summary>
     *   Returns the Z component of the acceleration, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Z component of the acceleration, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YAccelerometer.ZVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_zValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ZVALUE_INVALID;
            }
        }
        res = _zValue;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_gravityCancellation()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return GRAVITYCANCELLATION_INVALID;
            }
        }
        res = _gravityCancellation;
        return res;
    }


    public async Task<int> set_gravityCancellation(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("gravityCancellation",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves an accelerometer for a given identifier.
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
     *   This function does not require that the accelerometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAccelerometer.isOnline()</c> to test if the accelerometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an accelerometer by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the accelerometer, for instance
     *   <c>Y3DMK002.accelerometer</c>.
     * </param>
     * <returns>
     *   a <c>YAccelerometer</c> object allowing you to drive the accelerometer.
     * </returns>
     */
    public static YAccelerometer FindAccelerometer(string func)
    {
        YAccelerometer obj;
        obj = (YAccelerometer) YFunction._FindFromCache("Accelerometer", func);
        if (obj == null) {
            obj = new YAccelerometer(func);
            YFunction._AddToCache("Accelerometer", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an accelerometer for a given identifier in a YAPI context.
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
     *   This function does not require that the accelerometer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YAccelerometer.isOnline()</c> to test if the accelerometer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an accelerometer by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the accelerometer, for instance
     *   <c>Y3DMK002.accelerometer</c>.
     * </param>
     * <returns>
     *   a <c>YAccelerometer</c> object allowing you to drive the accelerometer.
     * </returns>
     */
    public static YAccelerometer FindAccelerometerInContext(YAPIContext yctx,string func)
    {
        YAccelerometer obj;
        obj = (YAccelerometer) YFunction._FindFromCacheInContext(yctx, "Accelerometer", func);
        if (obj == null) {
            obj = new YAccelerometer(yctx, func);
            YFunction._AddToCache("Accelerometer", func, obj);
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
        _valueCallbackAccelerometer = callback;
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
        if (_valueCallbackAccelerometer != null) {
            await _valueCallbackAccelerometer(this, value);
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
        _timedReportCallbackAccelerometer = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackAccelerometer != null) {
            await _timedReportCallbackAccelerometer(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of accelerometers started using <c>yFirstAccelerometer()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned accelerometers order.
     *   If you want to find a specific an accelerometer, use <c>Accelerometer.findAccelerometer()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAccelerometer</c> object, corresponding to
     *   an accelerometer currently online, or a <c>null</c> pointer
     *   if there are no more accelerometers to enumerate.
     * </returns>
     */
    public YAccelerometer nextAccelerometer()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindAccelerometerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of accelerometers currently accessible.
     * <para>
     *   Use the method <c>YAccelerometer.nextAccelerometer()</c> to iterate on
     *   next accelerometers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YAccelerometer</c> object, corresponding to
     *   the first accelerometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAccelerometer FirstAccelerometer()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Accelerometer");
        if (next_hwid == null)  return null;
        return FindAccelerometerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of accelerometers currently accessible.
     * <para>
     *   Use the method <c>YAccelerometer.nextAccelerometer()</c> to iterate on
     *   next accelerometers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YAccelerometer</c> object, corresponding to
     *   the first accelerometer currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YAccelerometer FirstAccelerometerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Accelerometer");
        if (next_hwid == null)  return null;
        return FindAccelerometerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YAccelerometer implementation)
}
}

