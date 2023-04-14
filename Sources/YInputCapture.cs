/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindInputCapture(), the high-level API for InputCapture functions
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

    //--- (generated code: YInputCapture return codes)
//--- (end of generated code: YInputCapture return codes)
    //--- (generated code: YInputCapture class start)
/**
 * <summary>
 *   YInputCapture Class: instant snapshot trigger control interface
 * <para>
 *   The <c>YInputCapture</c> class allows you to access data samples
 *   measured by a Yoctopuce electrical sensor. The data capture can be
 *   triggered manually, or be configured to detect specific events.
 * </para>
 * </summary>
 */
public class YInputCapture : YFunction
{
//--- (end of generated code: YInputCapture class start)
        //--- (generated code: YInputCapture definitions)
    /**
     * <summary>
     *   invalid lastCaptureTime value
     * </summary>
     */
    public const  long LASTCAPTURETIME_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid nSamples value
     * </summary>
     */
    public const  int NSAMPLES_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid samplingRate value
     * </summary>
     */
    public const  int SAMPLINGRATE_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid captureType value
     * </summary>
     */
    public const int CAPTURETYPE_NONE = 0;
    public const int CAPTURETYPE_TIMED = 1;
    public const int CAPTURETYPE_V_MAX = 2;
    public const int CAPTURETYPE_V_MIN = 3;
    public const int CAPTURETYPE_I_MAX = 4;
    public const int CAPTURETYPE_I_MIN = 5;
    public const int CAPTURETYPE_P_MAX = 6;
    public const int CAPTURETYPE_P_MIN = 7;
    public const int CAPTURETYPE_V_AVG_MAX = 8;
    public const int CAPTURETYPE_V_AVG_MIN = 9;
    public const int CAPTURETYPE_V_RMS_MAX = 10;
    public const int CAPTURETYPE_V_RMS_MIN = 11;
    public const int CAPTURETYPE_I_AVG_MAX = 12;
    public const int CAPTURETYPE_I_AVG_MIN = 13;
    public const int CAPTURETYPE_I_RMS_MAX = 14;
    public const int CAPTURETYPE_I_RMS_MIN = 15;
    public const int CAPTURETYPE_P_AVG_MAX = 16;
    public const int CAPTURETYPE_P_AVG_MIN = 17;
    public const int CAPTURETYPE_PF_MIN = 18;
    public const int CAPTURETYPE_DPF_MIN = 19;
    public const int CAPTURETYPE_INVALID = -1;
    /**
     * <summary>
     *   invalid condValue value
     * </summary>
     */
    public const  double CONDVALUE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid condAlign value
     * </summary>
     */
    public const  int CONDALIGN_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid captureTypeAtStartup value
     * </summary>
     */
    public const int CAPTURETYPEATSTARTUP_NONE = 0;
    public const int CAPTURETYPEATSTARTUP_TIMED = 1;
    public const int CAPTURETYPEATSTARTUP_V_MAX = 2;
    public const int CAPTURETYPEATSTARTUP_V_MIN = 3;
    public const int CAPTURETYPEATSTARTUP_I_MAX = 4;
    public const int CAPTURETYPEATSTARTUP_I_MIN = 5;
    public const int CAPTURETYPEATSTARTUP_P_MAX = 6;
    public const int CAPTURETYPEATSTARTUP_P_MIN = 7;
    public const int CAPTURETYPEATSTARTUP_V_AVG_MAX = 8;
    public const int CAPTURETYPEATSTARTUP_V_AVG_MIN = 9;
    public const int CAPTURETYPEATSTARTUP_V_RMS_MAX = 10;
    public const int CAPTURETYPEATSTARTUP_V_RMS_MIN = 11;
    public const int CAPTURETYPEATSTARTUP_I_AVG_MAX = 12;
    public const int CAPTURETYPEATSTARTUP_I_AVG_MIN = 13;
    public const int CAPTURETYPEATSTARTUP_I_RMS_MAX = 14;
    public const int CAPTURETYPEATSTARTUP_I_RMS_MIN = 15;
    public const int CAPTURETYPEATSTARTUP_P_AVG_MAX = 16;
    public const int CAPTURETYPEATSTARTUP_P_AVG_MIN = 17;
    public const int CAPTURETYPEATSTARTUP_PF_MIN = 18;
    public const int CAPTURETYPEATSTARTUP_DPF_MIN = 19;
    public const int CAPTURETYPEATSTARTUP_INVALID = -1;
    /**
     * <summary>
     *   invalid condValueAtStartup value
     * </summary>
     */
    public const  double CONDVALUEATSTARTUP_INVALID = YAPI.INVALID_DOUBLE;
    protected long _lastCaptureTime = LASTCAPTURETIME_INVALID;
    protected int _nSamples = NSAMPLES_INVALID;
    protected int _samplingRate = SAMPLINGRATE_INVALID;
    protected int _captureType = CAPTURETYPE_INVALID;
    protected double _condValue = CONDVALUE_INVALID;
    protected int _condAlign = CONDALIGN_INVALID;
    protected int _captureTypeAtStartup = CAPTURETYPEATSTARTUP_INVALID;
    protected double _condValueAtStartup = CONDVALUEATSTARTUP_INVALID;
    protected ValueCallback _valueCallbackInputCapture = null;

    public new delegate Task ValueCallback(YInputCapture func, string value);
    public new delegate Task TimedReportCallback(YInputCapture func, YMeasure measure);
    //--- (end of generated code: YInputCapture definitions)


        /**
         * <summary>
         * </summary>
         * <param name="func">
         *   functionid
         * </param>
         */
        protected YInputCapture(YAPIContext ctx, string func)
            : base(ctx, func, "InputCapture")
        {
            //--- (generated code: YInputCapture attributes initialization)
        //--- (end of generated code: YInputCapture attributes initialization)
        }

        /**
         * <summary>
         * </summary>
         * <param name="func">
         *   functionid
         * </param>
         */
        protected YInputCapture(string func)
            : this(YAPI.imm_GetYCtx(), func)
        {
        }

        //--- (generated code: YInputCapture implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("lastCaptureTime")) {
            _lastCaptureTime = json_val.getLong("lastCaptureTime");
        }
        if (json_val.has("nSamples")) {
            _nSamples = json_val.getInt("nSamples");
        }
        if (json_val.has("samplingRate")) {
            _samplingRate = json_val.getInt("samplingRate");
        }
        if (json_val.has("captureType")) {
            _captureType = json_val.getInt("captureType");
        }
        if (json_val.has("condValue")) {
            _condValue = Math.Round(json_val.getDouble("condValue") / 65.536) / 1000.0;
        }
        if (json_val.has("condAlign")) {
            _condAlign = json_val.getInt("condAlign");
        }
        if (json_val.has("captureTypeAtStartup")) {
            _captureTypeAtStartup = json_val.getInt("captureTypeAtStartup");
        }
        if (json_val.has("condValueAtStartup")) {
            _condValueAtStartup = Math.Round(json_val.getDouble("condValueAtStartup") / 65.536) / 1000.0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the number of elapsed milliseconds between the module power on
     *   and the last capture (time of trigger), or zero if no capture has been done.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of elapsed milliseconds between the module power on
     *   and the last capture (time of trigger), or zero if no capture has been done
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.LASTCAPTURETIME_INVALID</c>.
     * </para>
     */
    public async Task<long> get_lastCaptureTime()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return LASTCAPTURETIME_INVALID;
            }
        }
        res = _lastCaptureTime;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of samples that will be captured.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of samples that will be captured
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.NSAMPLES_INVALID</c>.
     * </para>
     */
    public async Task<int> get_nSamples()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return NSAMPLES_INVALID;
            }
        }
        res = _nSamples;
        return res;
    }


    /**
     * <summary>
     *   Changes the type of automatic conditional capture.
     * <para>
     *   The maximum number of samples depends on the device memory.
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the type of automatic conditional capture
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
    public async Task<int> set_nSamples(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("nSamples",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the sampling frequency, in Hz.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the sampling frequency, in Hz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.SAMPLINGRATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_samplingRate()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return SAMPLINGRATE_INVALID;
            }
        }
        res = _samplingRate;
        return res;
    }


    /**
     * <summary>
     *   Returns the type of automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YInputCapture.CAPTURETYPE_NONE</c>, <c>YInputCapture.CAPTURETYPE_TIMED</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_PF_MIN</c> and <c>YInputCapture.CAPTURETYPE_DPF_MIN</c> corresponding
     *   to the type of automatic conditional capture
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CAPTURETYPE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_captureType()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CAPTURETYPE_INVALID;
            }
        }
        res = _captureType;
        return res;
    }


    /**
     * <summary>
     *   Changes the type of automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YInputCapture.CAPTURETYPE_NONE</c>, <c>YInputCapture.CAPTURETYPE_TIMED</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_V_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_V_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_I_RMS_MAX</c>, <c>YInputCapture.CAPTURETYPE_I_RMS_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_P_AVG_MAX</c>, <c>YInputCapture.CAPTURETYPE_P_AVG_MIN</c>,
     *   <c>YInputCapture.CAPTURETYPE_PF_MIN</c> and <c>YInputCapture.CAPTURETYPE_DPF_MIN</c> corresponding
     *   to the type of automatic conditional capture
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
    public async Task<int> set_captureType(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("captureType",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes current threshold value for automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to current threshold value for automatic conditional capture
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
    public async Task<int> set_condValue(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("condValue",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns current threshold value for automatic conditional capture.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to current threshold value for automatic conditional capture
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CONDVALUE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_condValue()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CONDVALUE_INVALID;
            }
        }
        res = _condValue;
        return res;
    }


    /**
     * <summary>
     *   Returns the relative position of the trigger event within the capture window.
     * <para>
     *   When the value is 50%, the capture is centered on the event.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the relative position of the trigger event within the capture window
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CONDALIGN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_condAlign()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CONDALIGN_INVALID;
            }
        }
        res = _condAlign;
        return res;
    }


    /**
     * <summary>
     *   Changes the relative position of the trigger event within the capture window.
     * <para>
     *   The new value must be between 10% (on the left) and 90% (on the right).
     *   When the value is 50%, the capture is centered on the event.
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the relative position of the trigger event within the capture window
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
    public async Task<int> set_condAlign(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("condAlign",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the type of automatic conditional capture
     *   applied at device power on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YInputCapture.CAPTURETYPEATSTARTUP_NONE</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_TIMED</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_PF_MIN</c>
     *   and <c>YInputCapture.CAPTURETYPEATSTARTUP_DPF_MIN</c> corresponding to the type of automatic conditional capture
     *   applied at device power on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CAPTURETYPEATSTARTUP_INVALID</c>.
     * </para>
     */
    public async Task<int> get_captureTypeAtStartup()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CAPTURETYPEATSTARTUP_INVALID;
            }
        }
        res = _captureTypeAtStartup;
        return res;
    }


    /**
     * <summary>
     *   Changes the type of automatic conditional capture
     *   applied at device power on.
     * <para>
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YInputCapture.CAPTURETYPEATSTARTUP_NONE</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_TIMED</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_V_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_I_RMS_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MAX</c>,
     *   <c>YInputCapture.CAPTURETYPEATSTARTUP_P_AVG_MIN</c>, <c>YInputCapture.CAPTURETYPEATSTARTUP_PF_MIN</c>
     *   and <c>YInputCapture.CAPTURETYPEATSTARTUP_DPF_MIN</c> corresponding to the type of automatic conditional capture
     *   applied at device power on
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
    public async Task<int> set_captureTypeAtStartup(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("captureTypeAtStartup",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes current threshold value for automatic conditional
     *   capture applied at device power on.
     * <para>
     * </para>
     * <para>
     *   If you want the change to be kept after a device reboot,
     *   make sure  to call the matching module <c>saveToFlash()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to current threshold value for automatic conditional
     *   capture applied at device power on
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
    public async Task<int> set_condValueAtStartup(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("condValueAtStartup",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the threshold value for automatic conditional
     *   capture applied at device power on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the threshold value for automatic conditional
     *   capture applied at device power on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YInputCapture.CONDVALUEATSTARTUP_INVALID</c>.
     * </para>
     */
    public async Task<double> get_condValueAtStartup()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CONDVALUEATSTARTUP_INVALID;
            }
        }
        res = _condValueAtStartup;
        return res;
    }


    /**
     * <summary>
     *   Retrieves an instant snapshot trigger for a given identifier.
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
     *   This function does not require that the instant snapshot trigger is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YInputCapture.isOnline()</c> to test if the instant snapshot trigger is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an instant snapshot trigger by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the instant snapshot trigger, for instance
     *   <c>MyDevice.inputCapture</c>.
     * </param>
     * <returns>
     *   a <c>YInputCapture</c> object allowing you to drive the instant snapshot trigger.
     * </returns>
     */
    public static YInputCapture FindInputCapture(string func)
    {
        YInputCapture obj;
        obj = (YInputCapture) YFunction._FindFromCache("InputCapture", func);
        if (obj == null) {
            obj = new YInputCapture(func);
            YFunction._AddToCache("InputCapture",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an instant snapshot trigger for a given identifier in a YAPI context.
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
     *   This function does not require that the instant snapshot trigger is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YInputCapture.isOnline()</c> to test if the instant snapshot trigger is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an instant snapshot trigger by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the instant snapshot trigger, for instance
     *   <c>MyDevice.inputCapture</c>.
     * </param>
     * <returns>
     *   a <c>YInputCapture</c> object allowing you to drive the instant snapshot trigger.
     * </returns>
     */
    public static YInputCapture FindInputCaptureInContext(YAPIContext yctx,string func)
    {
        YInputCapture obj;
        obj = (YInputCapture) YFunction._FindFromCacheInContext(yctx,  "InputCapture", func);
        if (obj == null) {
            obj = new YInputCapture(yctx, func);
            YFunction._AddToCache("InputCapture",  func, obj);
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
        _valueCallbackInputCapture = callback;
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
        if (_valueCallbackInputCapture != null) {
            await _valueCallbackInputCapture(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Returns all details about the last automatic input capture.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an <c>YInputCaptureData</c> object including
     *   data series and all related meta-information.
     *   On failure, throws an exception or returns an capture object.
     * </returns>
     */
    public virtual async Task<YInputCaptureData> get_lastCapture()
    {
        byte[] snapData = new byte[0];

        snapData = await this._download("snap.bin");
        return new YInputCaptureData(this, snapData);
    }

    /**
     * <summary>
     *   Returns a new immediate capture of the device inputs.
     * <para>
     * </para>
     * </summary>
     * <param name="msDuration">
     *   duration of the capture window,
     *   in milliseconds (eg. between 20 and 1000).
     * </param>
     * <returns>
     *   an <c>YInputCaptureData</c> object including
     *   data series for the specified duration.
     *   On failure, throws an exception or returns an capture object.
     * </returns>
     */
    public virtual async Task<YInputCaptureData> get_immediateCapture(int msDuration)
    {
        string snapUrl;
        byte[] snapData = new byte[0];
        int snapStart;
        if (msDuration < 1) {
            msDuration = 20;
        }
        if (msDuration > 1000) {
            msDuration = 1000;
        }
        snapStart = ((-msDuration) / (2));
        snapUrl = "snap.bin?t="+Convert.ToString( snapStart)+"&d="+Convert.ToString(msDuration);

        snapData = await this._download(snapUrl);
        return new YInputCaptureData(this, snapData);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public YInputCapture nextInputCapture()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindInputCaptureInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YInputCapture FirstInputCapture()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("InputCapture");
        if (next_hwid == null)  return null;
        return FindInputCaptureInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   c
     * <para>
     *   omment from .yc definition
     * </para>
     * </summary>
     */
    public static YInputCapture FirstInputCaptureInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("InputCapture");
        if (next_hwid == null)  return null;
        return FindInputCaptureInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YInputCapture implementation)
    }
}

