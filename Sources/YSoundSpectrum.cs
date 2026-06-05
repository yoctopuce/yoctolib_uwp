/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindSoundSpectrum(), the high-level API for SoundSpectrum functions
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

//--- (YSoundSpectrum return codes)
//--- (end of YSoundSpectrum return codes)
//--- (YSoundSpectrum class start)
/**
 * <summary>
 *   YSoundSpectrum Class: sound spectrum analyzer control interface
 * <para>
 *   The <c>YSoundSpectrum</c> class allows you to read and configure Yoctopuce sound spectrum analyzers.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YSoundSpectrum : YFunction
{
//--- (end of YSoundSpectrum class start)
//--- (YSoundSpectrum definitions)
    /**
     * <summary>
     *   invalid integrationTime value
     * </summary>
     */
    public const  int INTEGRATIONTIME_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid spectrumData value
     * </summary>
     */
    public const  string SPECTRUMDATA_INVALID = YAPI.INVALID_STRING;
    protected int _integrationTime = INTEGRATIONTIME_INVALID;
    protected string _spectrumData = SPECTRUMDATA_INVALID;
    protected ValueCallback _valueCallbackSoundSpectrum = null;

    public new delegate Task ValueCallback(YSoundSpectrum func, string value);
    public new delegate Task TimedReportCallback(YSoundSpectrum func, YMeasure measure);
    //--- (end of YSoundSpectrum definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSoundSpectrum(YAPIContext ctx, string func)
        : base(ctx, func, "SoundSpectrum")
    {
        //--- (YSoundSpectrum attributes initialization)
        //--- (end of YSoundSpectrum attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSoundSpectrum(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YSoundSpectrum implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("integrationTime")) {
            _integrationTime = json_val.getInt("integrationTime");
        }
        if (json_val.has("spectrumData")) {
            _spectrumData = json_val.getString("spectrumData");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the integration time in milliseconds for calculating time
     *   weighted spectrum data.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the integration time in milliseconds for calculating time
     *   weighted spectrum data
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSoundSpectrum.INTEGRATIONTIME_INVALID</c>.
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
     *   Changes the integration time in milliseconds for computing time weighted
     *   spectrum data.
     * <para>
     *   Be aware that on some devices, changing the integration
     *   time for time-weighted spectrum data may also affect the integration
     *   period for one or more sound pressure level measurements.
     *   Remember to call the <c>saveToFlash()</c> method of the
     *   module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the integration time in milliseconds for computing time weighted
     *   spectrum data
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
    public async Task<int> set_integrationTime(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("integrationTime",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_spectrumData()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return SPECTRUMDATA_INVALID;
            }
        }
        res = _spectrumData;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a sound spectrum analyzer for a given identifier.
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
     *   This function does not require that the sound spectrum analyzer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSoundSpectrum.isOnline()</c> to test if the sound spectrum analyzer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a sound spectrum analyzer by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the sound spectrum analyzer, for instance
     *   <c>MyDevice.soundSpectrum</c>.
     * </param>
     * <returns>
     *   a <c>YSoundSpectrum</c> object allowing you to drive the sound spectrum analyzer.
     * </returns>
     */
    public static YSoundSpectrum FindSoundSpectrum(string func)
    {
        YSoundSpectrum obj;
        obj = (YSoundSpectrum) YFunction._FindFromCache("SoundSpectrum", func);
        if (obj == null) {
            obj = new YSoundSpectrum(func);
            YFunction._AddToCache("SoundSpectrum", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a sound spectrum analyzer for a given identifier in a YAPI context.
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
     *   This function does not require that the sound spectrum analyzer is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSoundSpectrum.isOnline()</c> to test if the sound spectrum analyzer is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a sound spectrum analyzer by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the sound spectrum analyzer, for instance
     *   <c>MyDevice.soundSpectrum</c>.
     * </param>
     * <returns>
     *   a <c>YSoundSpectrum</c> object allowing you to drive the sound spectrum analyzer.
     * </returns>
     */
    public static YSoundSpectrum FindSoundSpectrumInContext(YAPIContext yctx,string func)
    {
        YSoundSpectrum obj;
        obj = (YSoundSpectrum) YFunction._FindFromCacheInContext(yctx, "SoundSpectrum", func);
        if (obj == null) {
            obj = new YSoundSpectrum(yctx, func);
            YFunction._AddToCache("SoundSpectrum", func, obj);
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
        _valueCallbackSoundSpectrum = callback;
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
        if (_valueCallbackSoundSpectrum != null) {
            await _valueCallbackSoundSpectrum(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public YSoundSpectrum nextSoundSpectrum()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindSoundSpectrumInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YSoundSpectrum FirstSoundSpectrum()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SoundSpectrum");
        if (next_hwid == null)  return null;
        return FindSoundSpectrumInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YSoundSpectrum FirstSoundSpectrumInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SoundSpectrum");
        if (next_hwid == null)  return null;
        return FindSoundSpectrumInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YSoundSpectrum implementation)
}
}

