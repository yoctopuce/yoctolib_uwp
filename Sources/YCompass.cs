/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindCompass(), the high-level API for Compass functions
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

//--- (YCompass return codes)
//--- (end of YCompass return codes)
//--- (YCompass class start)
/**
 * <summary>
 *   YCompass Class: compass function control interface, available for instance in the Yocto-3D-V2
 * <para>
 *   The <c>YCompass</c> class allows you to read and configure Yoctopuce compass functions.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YCompass : YSensor
{
//--- (end of YCompass class start)
//--- (YCompass definitions)
    /**
     * <summary>
     *   invalid bandwidth value
     * </summary>
     */
    public const  int BANDWIDTH_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid axis value
     * </summary>
     */
    public const int AXIS_X = 0;
    public const int AXIS_Y = 1;
    public const int AXIS_Z = 2;
    public const int AXIS_INVALID = -1;
    /**
     * <summary>
     *   invalid magneticHeading value
     * </summary>
     */
    public const  double MAGNETICHEADING_INVALID = YAPI.INVALID_DOUBLE;
    protected int _bandwidth = BANDWIDTH_INVALID;
    protected int _axis = AXIS_INVALID;
    protected double _magneticHeading = MAGNETICHEADING_INVALID;
    protected ValueCallback _valueCallbackCompass = null;
    protected TimedReportCallback _timedReportCallbackCompass = null;

    public new delegate Task ValueCallback(YCompass func, string value);
    public new delegate Task TimedReportCallback(YCompass func, YMeasure measure);
    //--- (end of YCompass definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCompass(YAPIContext ctx, string func)
        : base(ctx, func, "Compass")
    {
        //--- (YCompass attributes initialization)
        //--- (end of YCompass attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCompass(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YCompass implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("bandwidth")) {
            _bandwidth = json_val.getInt("bandwidth");
        }
        if (json_val.has("axis")) {
            _axis = json_val.getInt("axis");
        }
        if (json_val.has("magneticHeading")) {
            _magneticHeading = Math.Round(json_val.getDouble("magneticHeading") / 65.536) / 1000.0;
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
     *   On failure, throws an exception or returns <c>YCompass.BANDWIDTH_INVALID</c>.
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
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_axis()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return AXIS_INVALID;
            }
        }
        res = _axis;
        return res;
    }


    /**
     * <summary>
     *   Returns the magnetic heading, regardless of the configured bearing.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the magnetic heading, regardless of the configured bearing
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCompass.MAGNETICHEADING_INVALID</c>.
     * </para>
     */
    public async Task<double> get_magneticHeading()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return MAGNETICHEADING_INVALID;
            }
        }
        res = _magneticHeading;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a compass function for a given identifier.
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
     *   This function does not require that the compass function is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCompass.isOnline()</c> to test if the compass function is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a compass function by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the compass function, for instance
     *   <c>Y3DMK002.compass</c>.
     * </param>
     * <returns>
     *   a <c>YCompass</c> object allowing you to drive the compass function.
     * </returns>
     */
    public static YCompass FindCompass(string func)
    {
        YCompass obj;
        obj = (YCompass) YFunction._FindFromCache("Compass", func);
        if (obj == null) {
            obj = new YCompass(func);
            YFunction._AddToCache("Compass", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a compass function for a given identifier in a YAPI context.
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
     *   This function does not require that the compass function is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCompass.isOnline()</c> to test if the compass function is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a compass function by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the compass function, for instance
     *   <c>Y3DMK002.compass</c>.
     * </param>
     * <returns>
     *   a <c>YCompass</c> object allowing you to drive the compass function.
     * </returns>
     */
    public static YCompass FindCompassInContext(YAPIContext yctx,string func)
    {
        YCompass obj;
        obj = (YCompass) YFunction._FindFromCacheInContext(yctx, "Compass", func);
        if (obj == null) {
            obj = new YCompass(yctx, func);
            YFunction._AddToCache("Compass", func, obj);
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
        _valueCallbackCompass = callback;
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
        if (_valueCallbackCompass != null) {
            await _valueCallbackCompass(this, value);
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
        _timedReportCallbackCompass = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackCompass != null) {
            await _timedReportCallbackCompass(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of compass functions started using <c>yFirstCompass()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned compass functions order.
     *   If you want to find a specific a compass function, use <c>Compass.findCompass()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCompass</c> object, corresponding to
     *   a compass function currently online, or a <c>null</c> pointer
     *   if there are no more compass functions to enumerate.
     * </returns>
     */
    public YCompass nextCompass()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindCompassInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of compass functions currently accessible.
     * <para>
     *   Use the method <c>YCompass.nextCompass()</c> to iterate on
     *   next compass functions.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCompass</c> object, corresponding to
     *   the first compass function currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCompass FirstCompass()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Compass");
        if (next_hwid == null)  return null;
        return FindCompassInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of compass functions currently accessible.
     * <para>
     *   Use the method <c>YCompass.nextCompass()</c> to iterate on
     *   next compass functions.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YCompass</c> object, corresponding to
     *   the first compass function currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCompass FirstCompassInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Compass");
        if (next_hwid == null)  return null;
        return FindCompassInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YCompass implementation)
}
}

