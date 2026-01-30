/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindSoundLevel(), the high-level API for SoundLevel functions
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

//--- (YSoundLevel return codes)
//--- (end of YSoundLevel return codes)
//--- (YSoundLevel class start)
/**
 * <summary>
 *   YSoundLevel Class: sound pressure level meter control interface
 * <para>
 *   The <c>YSoundLevel</c> class allows you to read and configure Yoctopuce sound pressure level meters.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YSoundLevel : YSensor
{
//--- (end of YSoundLevel class start)
//--- (YSoundLevel definitions)
    /**
     * <summary>
     *   invalid label value
     * </summary>
     */
    public const  string LABEL_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid integrationTime value
     * </summary>
     */
    public const  int INTEGRATIONTIME_INVALID = YAPI.INVALID_UINT;
    protected string _label = LABEL_INVALID;
    protected int _integrationTime = INTEGRATIONTIME_INVALID;
    protected ValueCallback _valueCallbackSoundLevel = null;
    protected TimedReportCallback _timedReportCallbackSoundLevel = null;

    public new delegate Task ValueCallback(YSoundLevel func, string value);
    public new delegate Task TimedReportCallback(YSoundLevel func, YMeasure measure);
    //--- (end of YSoundLevel definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSoundLevel(YAPIContext ctx, string func)
        : base(ctx, func, "SoundLevel")
    {
        //--- (YSoundLevel attributes initialization)
        //--- (end of YSoundLevel attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSoundLevel(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YSoundLevel implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("label")) {
            _label = json_val.getString("label");
        }
        if (json_val.has("integrationTime")) {
            _integrationTime = json_val.getInt("integrationTime");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the sound pressure level (dBA, dBC or dBZ).
     * <para>
     *   That unit will directly determine frequency weighting to be used to compute
     *   the measured value. Remember to call the <c>saveToFlash()</c> method of the
     *   module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the sound pressure level (dBA, dBC or dBZ)
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
     *   Returns the label for the sound pressure level measurement, as per
     *   IEC standard 61672-1:2013.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the label for the sound pressure level measurement, as per
     *   IEC standard 61672-1:2013
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSoundLevel.LABEL_INVALID</c>.
     * </para>
     */
    public async Task<string> get_label()
    {
        string res;
        if (_cacheExpiration == 0) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return LABEL_INVALID;
            }
        }
        res = _label;
        return res;
    }


    /**
     * <summary>
     *   Returns the integration time in milliseconds for measuring the sound pressure level.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the integration time in milliseconds for measuring the sound pressure level
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSoundLevel.INTEGRATIONTIME_INVALID</c>.
     * </para>
     */
    public async Task<int> get_integrationTime()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return INTEGRATIONTIME_INVALID;
            }
        }
        res = _integrationTime;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a sound pressure level meter for a given identifier.
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
     *   This function does not require that the sound pressure level meter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSoundLevel.isOnline()</c> to test if the sound pressure level meter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a sound pressure level meter by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the sound pressure level meter, for instance
     *   <c>MyDevice.soundLevel1</c>.
     * </param>
     * <returns>
     *   a <c>YSoundLevel</c> object allowing you to drive the sound pressure level meter.
     * </returns>
     */
    public static YSoundLevel FindSoundLevel(string func)
    {
        YSoundLevel obj;
        obj = (YSoundLevel) YFunction._FindFromCache("SoundLevel", func);
        if (obj == null) {
            obj = new YSoundLevel(func);
            YFunction._AddToCache("SoundLevel", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a sound pressure level meter for a given identifier in a YAPI context.
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
     *   This function does not require that the sound pressure level meter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSoundLevel.isOnline()</c> to test if the sound pressure level meter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a sound pressure level meter by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the sound pressure level meter, for instance
     *   <c>MyDevice.soundLevel1</c>.
     * </param>
     * <returns>
     *   a <c>YSoundLevel</c> object allowing you to drive the sound pressure level meter.
     * </returns>
     */
    public static YSoundLevel FindSoundLevelInContext(YAPIContext yctx,string func)
    {
        YSoundLevel obj;
        obj = (YSoundLevel) YFunction._FindFromCacheInContext(yctx, "SoundLevel", func);
        if (obj == null) {
            obj = new YSoundLevel(yctx, func);
            YFunction._AddToCache("SoundLevel", func, obj);
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
        _valueCallbackSoundLevel = callback;
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
        if (_valueCallbackSoundLevel != null) {
            await _valueCallbackSoundLevel(this, value);
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
        _timedReportCallbackSoundLevel = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackSoundLevel != null) {
            await _timedReportCallbackSoundLevel(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of sound pressure level meters started using <c>yFirstSoundLevel()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned sound pressure level meters order.
     *   If you want to find a specific a sound pressure level meter, use <c>SoundLevel.findSoundLevel()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSoundLevel</c> object, corresponding to
     *   a sound pressure level meter currently online, or a <c>null</c> pointer
     *   if there are no more sound pressure level meters to enumerate.
     * </returns>
     */
    public YSoundLevel nextSoundLevel()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindSoundLevelInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of sound pressure level meters currently accessible.
     * <para>
     *   Use the method <c>YSoundLevel.nextSoundLevel()</c> to iterate on
     *   next sound pressure level meters.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSoundLevel</c> object, corresponding to
     *   the first sound pressure level meter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSoundLevel FirstSoundLevel()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SoundLevel");
        if (next_hwid == null)  return null;
        return FindSoundLevelInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of sound pressure level meters currently accessible.
     * <para>
     *   Use the method <c>YSoundLevel.nextSoundLevel()</c> to iterate on
     *   next sound pressure level meters.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YSoundLevel</c> object, corresponding to
     *   the first sound pressure level meter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSoundLevel FirstSoundLevelInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SoundLevel");
        if (next_hwid == null)  return null;
        return FindSoundLevelInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YSoundLevel implementation)
}
}

