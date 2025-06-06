/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindPwmOutput(), the high-level API for PwmOutput functions
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

//--- (YPwmOutput return codes)
//--- (end of YPwmOutput return codes)
//--- (YPwmOutput class start)
/**
 * <summary>
 *   YPwmOutput Class: PWM generator control interface, available for instance in the Yocto-PWM-Tx
 * <para>
 *   The <c>YPwmOutput</c> class allows you to drive a pulse-width modulated output (PWM).
 *   You can configure the frequency as well as the duty cycle, and set up progressive
 *   transitions.
 * </para>
 * </summary>
 */
public class YPwmOutput : YFunction
{
//--- (end of YPwmOutput class start)
//--- (YPwmOutput definitions)
    /**
     * <summary>
     *   invalid enabled value
     * </summary>
     */
    public const int ENABLED_FALSE = 0;
    public const int ENABLED_TRUE = 1;
    public const int ENABLED_INVALID = -1;
    /**
     * <summary>
     *   invalid frequency value
     * </summary>
     */
    public const  double FREQUENCY_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid period value
     * </summary>
     */
    public const  double PERIOD_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid dutyCycle value
     * </summary>
     */
    public const  double DUTYCYCLE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid pulseDuration value
     * </summary>
     */
    public const  double PULSEDURATION_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid pwmTransition value
     * </summary>
     */
    public const  string PWMTRANSITION_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid invertedOutput value
     * </summary>
     */
    public const int INVERTEDOUTPUT_FALSE = 0;
    public const int INVERTEDOUTPUT_TRUE = 1;
    public const int INVERTEDOUTPUT_INVALID = -1;
    /**
     * <summary>
     *   invalid enabledAtPowerOn value
     * </summary>
     */
    public const int ENABLEDATPOWERON_FALSE = 0;
    public const int ENABLEDATPOWERON_TRUE = 1;
    public const int ENABLEDATPOWERON_INVALID = -1;
    /**
     * <summary>
     *   invalid dutyCycleAtPowerOn value
     * </summary>
     */
    public const  double DUTYCYCLEATPOWERON_INVALID = YAPI.INVALID_DOUBLE;
    protected int _enabled = ENABLED_INVALID;
    protected double _frequency = FREQUENCY_INVALID;
    protected double _period = PERIOD_INVALID;
    protected double _dutyCycle = DUTYCYCLE_INVALID;
    protected double _pulseDuration = PULSEDURATION_INVALID;
    protected string _pwmTransition = PWMTRANSITION_INVALID;
    protected int _invertedOutput = INVERTEDOUTPUT_INVALID;
    protected int _enabledAtPowerOn = ENABLEDATPOWERON_INVALID;
    protected double _dutyCycleAtPowerOn = DUTYCYCLEATPOWERON_INVALID;
    protected ValueCallback _valueCallbackPwmOutput = null;

    public new delegate Task ValueCallback(YPwmOutput func, string value);
    public new delegate Task TimedReportCallback(YPwmOutput func, YMeasure measure);
    //--- (end of YPwmOutput definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmOutput(YAPIContext ctx, string func)
        : base(ctx, func, "PwmOutput")
    {
        //--- (YPwmOutput attributes initialization)
        //--- (end of YPwmOutput attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmOutput(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPwmOutput implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("enabled")) {
            _enabled = json_val.getInt("enabled") > 0 ? 1 : 0;
        }
        if (json_val.has("frequency")) {
            _frequency = Math.Round(json_val.getDouble("frequency") / 65.536) / 1000.0;
        }
        if (json_val.has("period")) {
            _period = Math.Round(json_val.getDouble("period") / 65.536) / 1000.0;
        }
        if (json_val.has("dutyCycle")) {
            _dutyCycle = Math.Round(json_val.getDouble("dutyCycle") / 65.536) / 1000.0;
        }
        if (json_val.has("pulseDuration")) {
            _pulseDuration = Math.Round(json_val.getDouble("pulseDuration") / 65.536) / 1000.0;
        }
        if (json_val.has("pwmTransition")) {
            _pwmTransition = json_val.getString("pwmTransition");
        }
        if (json_val.has("invertedOutput")) {
            _invertedOutput = json_val.getInt("invertedOutput") > 0 ? 1 : 0;
        }
        if (json_val.has("enabledAtPowerOn")) {
            _enabledAtPowerOn = json_val.getInt("enabledAtPowerOn") > 0 ? 1 : 0;
        }
        if (json_val.has("dutyCycleAtPowerOn")) {
            _dutyCycleAtPowerOn = Math.Round(json_val.getDouble("dutyCycleAtPowerOn") / 65.536) / 1000.0;
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the state of the PWM generators.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YPwmOutput.ENABLED_FALSE</c> or <c>YPwmOutput.ENABLED_TRUE</c>, according to the state of
     *   the PWM generators
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.ENABLED_INVALID</c>.
     * </para>
     */
    public async Task<int> get_enabled()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ENABLED_INVALID;
            }
        }
        res = _enabled;
        return res;
    }


    /**
     * <summary>
     *   Stops or starts the PWM.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YPwmOutput.ENABLED_FALSE</c> or <c>YPwmOutput.ENABLED_TRUE</c>
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
    public async Task<int> set_enabled(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("enabled",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the PWM frequency.
     * <para>
     *   The duty cycle is kept unchanged thanks to an
     *   automatic pulse width change, in other words, the change will not be applied
     *   before the end of the current period. This can significantly affect reaction
     *   time at low frequencies. If you call the matching module <c>saveToFlash()</c>
     *   method, the frequency will be kept after a device power cycle.
     *   To stop the PWM signal, do not set the frequency to zero, use the set_enabled()
     *   method instead.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM frequency
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
    public async Task<int> set_frequency(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("frequency",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the PWM frequency in Hz.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM frequency in Hz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.FREQUENCY_INVALID</c>.
     * </para>
     */
    public async Task<double> get_frequency()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return FREQUENCY_INVALID;
            }
        }
        res = _frequency;
        return res;
    }


    /**
     * <summary>
     *   Changes the PWM period in milliseconds.
     * <para>
     *   Caution: in order to avoid  random truncation of
     *   the current pulse, the change will not be applied
     *   before the end of the current period. This can significantly affect reaction
     *   time at low frequencies. If you call the matching module <c>saveToFlash()</c>
     *   method, the frequency will be kept after a device power cycle.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM period in milliseconds
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
    public async Task<int> set_period(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("period",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the PWM period in milliseconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM period in milliseconds
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.PERIOD_INVALID</c>.
     * </para>
     */
    public async Task<double> get_period()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return PERIOD_INVALID;
            }
        }
        res = _period;
        return res;
    }


    /**
     * <summary>
     *   Changes the PWM duty cycle, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM duty cycle, in per cents
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
    public async Task<int> set_dutyCycle(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("dutyCycle",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the PWM duty cycle, in per cents.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM duty cycle, in per cents
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.DUTYCYCLE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_dutyCycle()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DUTYCYCLE_INVALID;
            }
        }
        res = _dutyCycle;
        return res;
    }


    /**
     * <summary>
     *   Changes the PWM pulse length, in milliseconds.
     * <para>
     *   A pulse length cannot be longer than period, otherwise it is truncated.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM pulse length, in milliseconds
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
    public async Task<int> set_pulseDuration(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("pulseDuration",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the PWM pulse length in milliseconds, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM pulse length in milliseconds, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.PULSEDURATION_INVALID</c>.
     * </para>
     */
    public async Task<double> get_pulseDuration()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return PULSEDURATION_INVALID;
            }
        }
        res = _pulseDuration;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_pwmTransition()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return PWMTRANSITION_INVALID;
            }
        }
        res = _pwmTransition;
        return res;
    }


    public async Task<int> set_pwmTransition(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("pwmTransition",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns true if the output signal is configured as inverted, and false otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YPwmOutput.INVERTEDOUTPUT_FALSE</c> or <c>YPwmOutput.INVERTEDOUTPUT_TRUE</c>, according
     *   to true if the output signal is configured as inverted, and false otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.INVERTEDOUTPUT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_invertedOutput()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return INVERTEDOUTPUT_INVALID;
            }
        }
        res = _invertedOutput;
        return res;
    }


    /**
     * <summary>
     *   Changes the inversion mode of the output signal.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c> method if you want
     *   the change to be kept after power cycle.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YPwmOutput.INVERTEDOUTPUT_FALSE</c> or <c>YPwmOutput.INVERTEDOUTPUT_TRUE</c>, according
     *   to the inversion mode of the output signal
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
    public async Task<int> set_invertedOutput(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("invertedOutput",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the state of the PWM at device power on.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YPwmOutput.ENABLEDATPOWERON_FALSE</c> or <c>YPwmOutput.ENABLEDATPOWERON_TRUE</c>,
     *   according to the state of the PWM at device power on
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.ENABLEDATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_enabledAtPowerOn()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ENABLEDATPOWERON_INVALID;
            }
        }
        res = _enabledAtPowerOn;
        return res;
    }


    /**
     * <summary>
     *   Changes the state of the PWM at device power on.
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YPwmOutput.ENABLEDATPOWERON_FALSE</c> or <c>YPwmOutput.ENABLEDATPOWERON_TRUE</c>,
     *   according to the state of the PWM at device power on
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
    public async Task<int> set_enabledAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("enabledAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the PWM duty cycle at device power on.
     * <para>
     *   Remember to call the matching
     *   module <c>saveToFlash()</c> method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the PWM duty cycle at device power on
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
    public async Task<int> set_dutyCycleAtPowerOn(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("dutyCycleAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the PWM generators duty cycle at device power on as a floating point number between 0 and 100.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the PWM generators duty cycle at device power on as a
     *   floating point number between 0 and 100
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmOutput.DUTYCYCLEATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<double> get_dutyCycleAtPowerOn()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DUTYCYCLEATPOWERON_INVALID;
            }
        }
        res = _dutyCycleAtPowerOn;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a PWM generator for a given identifier.
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
     *   This function does not require that the PWM generator is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmOutput.isOnline()</c> to test if the PWM generator is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM generator by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the PWM generator, for instance
     *   <c>YPWMTX01.pwmOutput1</c>.
     * </param>
     * <returns>
     *   a <c>YPwmOutput</c> object allowing you to drive the PWM generator.
     * </returns>
     */
    public static YPwmOutput FindPwmOutput(string func)
    {
        YPwmOutput obj;
        obj = (YPwmOutput) YFunction._FindFromCache("PwmOutput", func);
        if (obj == null) {
            obj = new YPwmOutput(func);
            YFunction._AddToCache("PwmOutput", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a PWM generator for a given identifier in a YAPI context.
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
     *   This function does not require that the PWM generator is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmOutput.isOnline()</c> to test if the PWM generator is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM generator by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the PWM generator, for instance
     *   <c>YPWMTX01.pwmOutput1</c>.
     * </param>
     * <returns>
     *   a <c>YPwmOutput</c> object allowing you to drive the PWM generator.
     * </returns>
     */
    public static YPwmOutput FindPwmOutputInContext(YAPIContext yctx,string func)
    {
        YPwmOutput obj;
        obj = (YPwmOutput) YFunction._FindFromCacheInContext(yctx, "PwmOutput", func);
        if (obj == null) {
            obj = new YPwmOutput(yctx, func);
            YFunction._AddToCache("PwmOutput", func, obj);
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
        _valueCallbackPwmOutput = callback;
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
        if (_valueCallbackPwmOutput != null) {
            await _valueCallbackPwmOutput(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Performs a smooth transition of the pulse duration toward a given value.
     * <para>
     *   Any period, frequency, duty cycle or pulse width change will cancel any ongoing transition process.
     * </para>
     * </summary>
     * <param name="ms_target">
     *   new pulse duration at the end of the transition
     *   (floating-point number, representing the pulse duration in milliseconds)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> pulseDurationMove(double ms_target,int ms_duration)
    {
        string newval;
        if (ms_target < 0.0) {
            ms_target = 0.0;
        }
        newval = ""+Convert.ToString((int) Math.Round(ms_target*65536))+"ms:"+Convert.ToString(ms_duration);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Performs a smooth change of the duty cycle toward a given value.
     * <para>
     *   Any period, frequency, duty cycle or pulse width change will cancel any ongoing transition process.
     * </para>
     * </summary>
     * <param name="target">
     *   new duty cycle at the end of the transition
     *   (percentage, floating-point number between 0 and 100)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> dutyCycleMove(double target,int ms_duration)
    {
        string newval;
        if (target < 0.0) {
            target = 0.0;
        }
        if (target > 100.0) {
            target = 100.0;
        }
        newval = ""+Convert.ToString((int) Math.Round(target*65536))+":"+Convert.ToString(ms_duration);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Performs a smooth frequency change toward a given value.
     * <para>
     *   Any period, frequency, duty cycle or pulse width change will cancel any ongoing transition process.
     * </para>
     * </summary>
     * <param name="target">
     *   new frequency at the end of the transition (floating-point number)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> frequencyMove(double target,int ms_duration)
    {
        string newval;
        if (target < 0.001) {
            target = 0.001;
        }
        newval = ""+YAPIContext.imm_floatToStr(target)+"Hz:"+Convert.ToString(ms_duration);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Performs a smooth transition toward a specified value of the phase shift between this channel
     *   and the other channel.
     * <para>
     *   The phase shift is executed by slightly changing the frequency
     *   temporarily during the specified duration. This function only makes sense when both channels
     *   are running, either at the same frequency, or at a multiple of the channel frequency.
     *   Any period, frequency, duty cycle or pulse width change will cancel any ongoing transition process.
     * </para>
     * </summary>
     * <param name="target">
     *   phase shift at the end of the transition, in milliseconds (floating-point number)
     * </param>
     * <param name="ms_duration">
     *   total duration of the transition, in milliseconds
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> phaseMove(double target,int ms_duration)
    {
        string newval;
        newval = ""+YAPIContext.imm_floatToStr(target)+"ps:"+Convert.ToString(ms_duration);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Trigger a given number of pulses of specified duration, at current frequency.
     * <para>
     *   At the end of the pulse train, revert to the original state of the PWM generator.
     * </para>
     * </summary>
     * <param name="ms_target">
     *   desired pulse duration
     *   (floating-point number, representing the pulse duration in milliseconds)
     * </param>
     * <param name="n_pulses">
     *   desired pulse count
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> triggerPulsesByDuration(double ms_target,int n_pulses)
    {
        string newval;
        if (ms_target < 0.0) {
            ms_target = 0.0;
        }
        newval = ""+Convert.ToString((int) Math.Round(ms_target*65536))+"ms*"+Convert.ToString(n_pulses);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Trigger a given number of pulses of specified duration, at current frequency.
     * <para>
     *   At the end of the pulse train, revert to the original state of the PWM generator.
     * </para>
     * </summary>
     * <param name="target">
     *   desired duty cycle for the generated pulses
     *   (percentage, floating-point number between 0 and 100)
     * </param>
     * <param name="n_pulses">
     *   desired pulse count
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> triggerPulsesByDutyCycle(double target,int n_pulses)
    {
        string newval;
        if (target < 0.0) {
            target = 0.0;
        }
        if (target > 100.0) {
            target = 100.0;
        }
        newval = ""+Convert.ToString((int) Math.Round(target*65536))+"*"+Convert.ToString(n_pulses);
        return await this.set_pwmTransition(newval);
    }

    /**
     * <summary>
     *   Trigger a given number of pulses at the specified frequency, using current duty cycle.
     * <para>
     *   At the end of the pulse train, revert to the original state of the PWM generator.
     * </para>
     * </summary>
     * <param name="target">
     *   desired frequency for the generated pulses (floating-point number)
     * </param>
     * <param name="n_pulses">
     *   desired pulse count
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> when the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> triggerPulsesByFrequency(double target,int n_pulses)
    {
        string newval;
        if (target < 0.001) {
            target = 0.001;
        }
        newval = ""+YAPIContext.imm_floatToStr(target)+"Hz*"+Convert.ToString(n_pulses);
        return await this.set_pwmTransition(newval);
    }

    public virtual async Task<int> markForRepeat()
    {
        return await this.set_pwmTransition(":");
    }

    public virtual async Task<int> repeatFromMark()
    {
        return await this.set_pwmTransition("R");
    }

    /**
     * <summary>
     *   Continues the enumeration of PWM generators started using <c>yFirstPwmOutput()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned PWM generators order.
     *   If you want to find a specific a PWM generator, use <c>PwmOutput.findPwmOutput()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmOutput</c> object, corresponding to
     *   a PWM generator currently online, or a <c>null</c> pointer
     *   if there are no more PWM generators to enumerate.
     * </returns>
     */
    public YPwmOutput nextPwmOutput()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPwmOutputInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWM generators currently accessible.
     * <para>
     *   Use the method <c>YPwmOutput.nextPwmOutput()</c> to iterate on
     *   next PWM generators.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmOutput</c> object, corresponding to
     *   the first PWM generator currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmOutput FirstPwmOutput()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmOutput");
        if (next_hwid == null)  return null;
        return FindPwmOutputInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWM generators currently accessible.
     * <para>
     *   Use the method <c>YPwmOutput.nextPwmOutput()</c> to iterate on
     *   next PWM generators.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPwmOutput</c> object, corresponding to
     *   the first PWM generator currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmOutput FirstPwmOutputInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmOutput");
        if (next_hwid == null)  return null;
        return FindPwmOutputInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPwmOutput implementation)
}
}

