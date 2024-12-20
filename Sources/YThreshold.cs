/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindThreshold(), the high-level API for Threshold functions
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

//--- (YThreshold return codes)
//--- (end of YThreshold return codes)
//--- (YThreshold class start)
/**
 * <summary>
 *   YThreshold Class: Control interface to define a threshold
 * <para>
 *   The <c>Threshold</c> class allows you define a threshold on a Yoctopuce sensor
 *   to trigger a predefined action, on specific devices where this is implemented.
 * </para>
 * </summary>
 */
public class YThreshold : YFunction
{
//--- (end of YThreshold class start)
//--- (YThreshold definitions)
    /**
     * <summary>
     *   invalid thresholdState value
     * </summary>
     */
    public const int THRESHOLDSTATE_SAFE = 0;
    public const int THRESHOLDSTATE_ALERT = 1;
    public const int THRESHOLDSTATE_INVALID = -1;
    /**
     * <summary>
     *   invalid targetSensor value
     * </summary>
     */
    public const  string TARGETSENSOR_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid alertLevel value
     * </summary>
     */
    public const  double ALERTLEVEL_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid safeLevel value
     * </summary>
     */
    public const  double SAFELEVEL_INVALID = YAPI.INVALID_DOUBLE;
    protected int _thresholdState = THRESHOLDSTATE_INVALID;
    protected string _targetSensor = TARGETSENSOR_INVALID;
    protected double _alertLevel = ALERTLEVEL_INVALID;
    protected double _safeLevel = SAFELEVEL_INVALID;
    protected ValueCallback _valueCallbackThreshold = null;

    public new delegate Task ValueCallback(YThreshold func, string value);
    public new delegate Task TimedReportCallback(YThreshold func, YMeasure measure);
    //--- (end of YThreshold definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YThreshold(YAPIContext ctx, string func)
        : base(ctx, func, "Threshold")
    {
        //--- (YThreshold attributes initialization)
        //--- (end of YThreshold attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YThreshold(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YThreshold implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("thresholdState")) {
            _thresholdState = json_val.getInt("thresholdState");
        }
        if (json_val.has("targetSensor")) {
            _targetSensor = json_val.getString("targetSensor");
        }
        if (json_val.has("alertLevel")) {
            _alertLevel = Math.Round(json_val.getDouble("alertLevel") / 65.536) / 1000.0;
        }
        if (json_val.has("safeLevel")) {
            _safeLevel = Math.Round(json_val.getDouble("safeLevel") / 65.536) / 1000.0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns current state of the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YThreshold.THRESHOLDSTATE_SAFE</c> or <c>YThreshold.THRESHOLDSTATE_ALERT</c>, according
     *   to current state of the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.THRESHOLDSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_thresholdState()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return THRESHOLDSTATE_INVALID;
            }
        }
        res = _thresholdState;
        return res;
    }


    /**
     * <summary>
     *   Returns the name of the sensor monitored by the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the name of the sensor monitored by the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.TARGETSENSOR_INVALID</c>.
     * </para>
     */
    public async Task<string> get_targetSensor()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return TARGETSENSOR_INVALID;
            }
        }
        res = _targetSensor;
        return res;
    }


    /**
     * <summary>
     *   Changes the sensor alert level triggering the threshold function.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method if you want to preserve the setting after reboot.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the sensor alert level triggering the threshold function
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
    public async Task<int> set_alertLevel(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("alertLevel",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the sensor alert level, triggering the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the sensor alert level, triggering the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.ALERTLEVEL_INVALID</c>.
     * </para>
     */
    public async Task<double> get_alertLevel()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ALERTLEVEL_INVALID;
            }
        }
        res = _alertLevel;
        return res;
    }


    /**
     * <summary>
     *   Changes the sensor acceptable level for disabling the threshold function.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method if you want to preserve the setting after reboot.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the sensor acceptable level for disabling the threshold function
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
    public async Task<int> set_safeLevel(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("safeLevel",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the sensor acceptable level for disabling the threshold function.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the sensor acceptable level for disabling the threshold function
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YThreshold.SAFELEVEL_INVALID</c>.
     * </para>
     */
    public async Task<double> get_safeLevel()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return SAFELEVEL_INVALID;
            }
        }
        res = _safeLevel;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a threshold function for a given identifier.
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
     *   This function does not require that the threshold function is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YThreshold.isOnline()</c> to test if the threshold function is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a threshold function by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the threshold function, for instance
     *   <c>MyDevice.threshold1</c>.
     * </param>
     * <returns>
     *   a <c>YThreshold</c> object allowing you to drive the threshold function.
     * </returns>
     */
    public static YThreshold FindThreshold(string func)
    {
        YThreshold obj;
        obj = (YThreshold) YFunction._FindFromCache("Threshold", func);
        if (obj == null) {
            obj = new YThreshold(func);
            YFunction._AddToCache("Threshold", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a threshold function for a given identifier in a YAPI context.
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
     *   This function does not require that the threshold function is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YThreshold.isOnline()</c> to test if the threshold function is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a threshold function by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the threshold function, for instance
     *   <c>MyDevice.threshold1</c>.
     * </param>
     * <returns>
     *   a <c>YThreshold</c> object allowing you to drive the threshold function.
     * </returns>
     */
    public static YThreshold FindThresholdInContext(YAPIContext yctx,string func)
    {
        YThreshold obj;
        obj = (YThreshold) YFunction._FindFromCacheInContext(yctx, "Threshold", func);
        if (obj == null) {
            obj = new YThreshold(yctx, func);
            YFunction._AddToCache("Threshold", func, obj);
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
        _valueCallbackThreshold = callback;
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
        if (_valueCallbackThreshold != null) {
            await _valueCallbackThreshold(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of threshold functions started using <c>yFirstThreshold()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned threshold functions order.
     *   If you want to find a specific a threshold function, use <c>Threshold.findThreshold()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YThreshold</c> object, corresponding to
     *   a threshold function currently online, or a <c>null</c> pointer
     *   if there are no more threshold functions to enumerate.
     * </returns>
     */
    public YThreshold nextThreshold()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindThresholdInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of threshold functions currently accessible.
     * <para>
     *   Use the method <c>YThreshold.nextThreshold()</c> to iterate on
     *   next threshold functions.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YThreshold</c> object, corresponding to
     *   the first threshold function currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YThreshold FirstThreshold()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Threshold");
        if (next_hwid == null)  return null;
        return FindThresholdInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of threshold functions currently accessible.
     * <para>
     *   Use the method <c>YThreshold.nextThreshold()</c> to iterate on
     *   next threshold functions.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YThreshold</c> object, corresponding to
     *   the first threshold function currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YThreshold FirstThresholdInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Threshold");
        if (next_hwid == null)  return null;
        return FindThresholdInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YThreshold implementation)
}
}

