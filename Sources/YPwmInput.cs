/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindPwmInput(), the high-level API for PwmInput functions
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

//--- (YPwmInput return codes)
//--- (end of YPwmInput return codes)
//--- (YPwmInput class start)
/**
 * <summary>
 *   YPwmInput Class: PWM input control interface, available for instance in the Yocto-PWM-Rx
 * <para>
 *   The <c>YPwmInput</c> class allows you to read and configure Yoctopuce PWM inputs.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 *   This class adds the ability to configure the signal parameter used to transmit
 *   information: the duty cycle, the frequency or the pulse width.
 * </para>
 * </summary>
 */
public class YPwmInput : YSensor
{
//--- (end of YPwmInput class start)
//--- (YPwmInput definitions)
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
     *   invalid pulseCounter value
     * </summary>
     */
    public const  long PULSECOUNTER_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid pulseTimer value
     * </summary>
     */
    public const  long PULSETIMER_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid pwmReportMode value
     * </summary>
     */
    public const int PWMREPORTMODE_PWM_DUTYCYCLE = 0;
    public const int PWMREPORTMODE_PWM_FREQUENCY = 1;
    public const int PWMREPORTMODE_PWM_PULSEDURATION = 2;
    public const int PWMREPORTMODE_PWM_EDGECOUNT = 3;
    public const int PWMREPORTMODE_PWM_PULSECOUNT = 4;
    public const int PWMREPORTMODE_PWM_CPS = 5;
    public const int PWMREPORTMODE_PWM_CPM = 6;
    public const int PWMREPORTMODE_PWM_STATE = 7;
    public const int PWMREPORTMODE_PWM_FREQ_CPS = 8;
    public const int PWMREPORTMODE_PWM_FREQ_CPM = 9;
    public const int PWMREPORTMODE_PWM_PERIODCOUNT = 10;
    public const int PWMREPORTMODE_INVALID = -1;
    /**
     * <summary>
     *   invalid debouncePeriod value
     * </summary>
     */
    public const  int DEBOUNCEPERIOD_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid minFrequency value
     * </summary>
     */
    public const  double MINFREQUENCY_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid bandwidth value
     * </summary>
     */
    public const  int BANDWIDTH_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid edgesPerPeriod value
     * </summary>
     */
    public const  int EDGESPERPERIOD_INVALID = YAPI.INVALID_UINT;
    protected double _dutyCycle = DUTYCYCLE_INVALID;
    protected double _pulseDuration = PULSEDURATION_INVALID;
    protected double _frequency = FREQUENCY_INVALID;
    protected double _period = PERIOD_INVALID;
    protected long _pulseCounter = PULSECOUNTER_INVALID;
    protected long _pulseTimer = PULSETIMER_INVALID;
    protected int _pwmReportMode = PWMREPORTMODE_INVALID;
    protected int _debouncePeriod = DEBOUNCEPERIOD_INVALID;
    protected double _minFrequency = MINFREQUENCY_INVALID;
    protected int _bandwidth = BANDWIDTH_INVALID;
    protected int _edgesPerPeriod = EDGESPERPERIOD_INVALID;
    protected ValueCallback _valueCallbackPwmInput = null;
    protected TimedReportCallback _timedReportCallbackPwmInput = null;

    public new delegate Task ValueCallback(YPwmInput func, string value);
    public new delegate Task TimedReportCallback(YPwmInput func, YMeasure measure);
    //--- (end of YPwmInput definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmInput(YAPIContext ctx, string func)
        : base(ctx, func, "PwmInput")
    {
        //--- (YPwmInput attributes initialization)
        //--- (end of YPwmInput attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YPwmInput(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YPwmInput implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("dutyCycle")) {
            _dutyCycle = Math.Round(json_val.getDouble("dutyCycle") / 65.536) / 1000.0;
        }
        if (json_val.has("pulseDuration")) {
            _pulseDuration = Math.Round(json_val.getDouble("pulseDuration") / 65.536) / 1000.0;
        }
        if (json_val.has("frequency")) {
            _frequency = Math.Round(json_val.getDouble("frequency") / 65.536) / 1000.0;
        }
        if (json_val.has("period")) {
            _period = Math.Round(json_val.getDouble("period") / 65.536) / 1000.0;
        }
        if (json_val.has("pulseCounter")) {
            _pulseCounter = json_val.getLong("pulseCounter");
        }
        if (json_val.has("pulseTimer")) {
            _pulseTimer = json_val.getLong("pulseTimer");
        }
        if (json_val.has("pwmReportMode")) {
            _pwmReportMode = json_val.getInt("pwmReportMode");
        }
        if (json_val.has("debouncePeriod")) {
            _debouncePeriod = json_val.getInt("debouncePeriod");
        }
        if (json_val.has("minFrequency")) {
            _minFrequency = Math.Round(json_val.getDouble("minFrequency") / 65.536) / 1000.0;
        }
        if (json_val.has("bandwidth")) {
            _bandwidth = json_val.getInt("bandwidth");
        }
        if (json_val.has("edgesPerPeriod")) {
            _edgesPerPeriod = json_val.getInt("edgesPerPeriod");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the measured quantity.
     * <para>
     *   That unit
     *   is just a string which is automatically initialized each time
     *   the measurement mode is changed. But is can be set to an
     *   arbitrary value.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the measured quantity
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
     *   On failure, throws an exception or returns <c>YPwmInput.DUTYCYCLE_INVALID</c>.
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
     *   On failure, throws an exception or returns <c>YPwmInput.PULSEDURATION_INVALID</c>.
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
     *   On failure, throws an exception or returns <c>YPwmInput.FREQUENCY_INVALID</c>.
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
     *   On failure, throws an exception or returns <c>YPwmInput.PERIOD_INVALID</c>.
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
     *   Returns the pulse counter value.
     * <para>
     *   Actually that
     *   counter is incremented twice per period. That counter is
     *   limited  to 1 billion.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the pulse counter value
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PULSECOUNTER_INVALID</c>.
     * </para>
     */
    public async Task<long> get_pulseCounter()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return PULSECOUNTER_INVALID;
            }
        }
        res = _pulseCounter;
        return res;
    }


    public async Task<int> set_pulseCounter(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("pulseCounter",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the timer of the pulses counter (ms).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the timer of the pulses counter (ms)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PULSETIMER_INVALID</c>.
     * </para>
     */
    public async Task<long> get_pulseTimer()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return PULSETIMER_INVALID;
            }
        }
        res = _pulseTimer;
        return res;
    }


    /**
     * <summary>
     *   Returns the parameter (frequency/duty cycle, pulse width, edges count) returned by the get_currentValue function and callbacks.
     * <para>
     *   Attention
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YPwmInput.PWMREPORTMODE_PWM_DUTYCYCLE</c>, <c>YPwmInput.PWMREPORTMODE_PWM_FREQUENCY</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PULSEDURATION</c>, <c>YPwmInput.PWMREPORTMODE_PWM_EDGECOUNT</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PULSECOUNT</c>, <c>YPwmInput.PWMREPORTMODE_PWM_CPS</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_CPM</c>, <c>YPwmInput.PWMREPORTMODE_PWM_STATE</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_FREQ_CPS</c>, <c>YPwmInput.PWMREPORTMODE_PWM_FREQ_CPM</c> and
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PERIODCOUNT</c> corresponding to the parameter (frequency/duty
     *   cycle, pulse width, edges count) returned by the get_currentValue function and callbacks
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.PWMREPORTMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_pwmReportMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return PWMREPORTMODE_INVALID;
            }
        }
        res = _pwmReportMode;
        return res;
    }


    /**
     * <summary>
     *   Changes the  parameter  type (frequency/duty cycle, pulse width, or edge count) returned by the get_currentValue function and callbacks.
     * <para>
     *   The edge count value is limited to the 6 lowest digits. For values greater than one million, use
     *   get_pulseCounter().
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YPwmInput.PWMREPORTMODE_PWM_DUTYCYCLE</c>, <c>YPwmInput.PWMREPORTMODE_PWM_FREQUENCY</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PULSEDURATION</c>, <c>YPwmInput.PWMREPORTMODE_PWM_EDGECOUNT</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PULSECOUNT</c>, <c>YPwmInput.PWMREPORTMODE_PWM_CPS</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_CPM</c>, <c>YPwmInput.PWMREPORTMODE_PWM_STATE</c>,
     *   <c>YPwmInput.PWMREPORTMODE_PWM_FREQ_CPS</c>, <c>YPwmInput.PWMREPORTMODE_PWM_FREQ_CPM</c> and
     *   <c>YPwmInput.PWMREPORTMODE_PWM_PERIODCOUNT</c> corresponding to the  parameter  type
     *   (frequency/duty cycle, pulse width, or edge count) returned by the get_currentValue function and callbacks
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
    public async Task<int> set_pwmReportMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("pwmReportMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the shortest expected pulse duration, in ms.
     * <para>
     *   Any shorter pulse will be automatically ignored (debounce).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the shortest expected pulse duration, in ms
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.DEBOUNCEPERIOD_INVALID</c>.
     * </para>
     */
    public async Task<int> get_debouncePeriod()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DEBOUNCEPERIOD_INVALID;
            }
        }
        res = _debouncePeriod;
        return res;
    }


    /**
     * <summary>
     *   Changes the shortest expected pulse duration, in ms.
     * <para>
     *   Any shorter pulse will be automatically ignored (debounce).
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the shortest expected pulse duration, in ms
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
    public async Task<int> set_debouncePeriod(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("debouncePeriod",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the minimum detected frequency, in Hz.
     * <para>
     *   Slower signals will be consider as zero frequency.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the minimum detected frequency, in Hz
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
    public async Task<int> set_minFrequency(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("minFrequency",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the minimum detected frequency, in Hz.
     * <para>
     *   Slower signals will be consider as zero frequency.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the minimum detected frequency, in Hz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.MINFREQUENCY_INVALID</c>.
     * </para>
     */
    public async Task<double> get_minFrequency()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return MINFREQUENCY_INVALID;
            }
        }
        res = _minFrequency;
        return res;
    }


    /**
     * <summary>
     *   Returns the input signal sampling rate, in kHz.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the input signal sampling rate, in kHz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.BANDWIDTH_INVALID</c>.
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
     *   Changes the input signal sampling rate, measured in kHz.
     * <para>
     *   A lower sampling frequency can be used to hide hide-frequency bounce effects,
     *   for instance on electromechanical contacts, but limits the measure resolution.
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the input signal sampling rate, measured in kHz
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
     *   Returns the number of edges detected per preiod.
     * <para>
     *   For a clean PWM signal, this should be exactly two,
     *   but in cas the signal is created by a mechanical contact with bounces, it can get higher.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of edges detected per preiod
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPwmInput.EDGESPERPERIOD_INVALID</c>.
     * </para>
     */
    public async Task<int> get_edgesPerPeriod()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return EDGESPERPERIOD_INVALID;
            }
        }
        res = _edgesPerPeriod;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a PWM input for a given identifier.
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
     *   This function does not require that the PWM input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmInput.isOnline()</c> to test if the PWM input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM input by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the PWM input, for instance
     *   <c>YPWMRX01.pwmInput1</c>.
     * </param>
     * <returns>
     *   a <c>YPwmInput</c> object allowing you to drive the PWM input.
     * </returns>
     */
    public static YPwmInput FindPwmInput(string func)
    {
        YPwmInput obj;
        obj = (YPwmInput) YFunction._FindFromCache("PwmInput", func);
        if (obj == null) {
            obj = new YPwmInput(func);
            YFunction._AddToCache("PwmInput", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a PWM input for a given identifier in a YAPI context.
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
     *   This function does not require that the PWM input is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPwmInput.isOnline()</c> to test if the PWM input is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a PWM input by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the PWM input, for instance
     *   <c>YPWMRX01.pwmInput1</c>.
     * </param>
     * <returns>
     *   a <c>YPwmInput</c> object allowing you to drive the PWM input.
     * </returns>
     */
    public static YPwmInput FindPwmInputInContext(YAPIContext yctx,string func)
    {
        YPwmInput obj;
        obj = (YPwmInput) YFunction._FindFromCacheInContext(yctx, "PwmInput", func);
        if (obj == null) {
            obj = new YPwmInput(yctx, func);
            YFunction._AddToCache("PwmInput", func, obj);
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
        _valueCallbackPwmInput = callback;
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
        if (_valueCallbackPwmInput != null) {
            await _valueCallbackPwmInput(this, value);
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
        _timedReportCallbackPwmInput = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackPwmInput != null) {
            await _timedReportCallbackPwmInput(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Resets the periodicity detection algorithm.
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
    public virtual async Task<int> resetPeriodDetection()
    {
        return await this.set_bandwidth(await this.get_bandwidth());
    }

    /**
     * <summary>
     *   Resets the pulse counter value as well as its timer.
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
    public virtual async Task<int> resetCounter()
    {
        return await this.set_pulseCounter(0);
    }

    /**
     * <summary>
     *   Continues the enumeration of PWM inputs started using <c>yFirstPwmInput()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned PWM inputs order.
     *   If you want to find a specific a PWM input, use <c>PwmInput.findPwmInput()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmInput</c> object, corresponding to
     *   a PWM input currently online, or a <c>null</c> pointer
     *   if there are no more PWM inputs to enumerate.
     * </returns>
     */
    public YPwmInput nextPwmInput()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindPwmInputInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWM inputs currently accessible.
     * <para>
     *   Use the method <c>YPwmInput.nextPwmInput()</c> to iterate on
     *   next PWM inputs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPwmInput</c> object, corresponding to
     *   the first PWM input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmInput FirstPwmInput()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmInput");
        if (next_hwid == null)  return null;
        return FindPwmInputInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of PWM inputs currently accessible.
     * <para>
     *   Use the method <c>YPwmInput.nextPwmInput()</c> to iterate on
     *   next PWM inputs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YPwmInput</c> object, corresponding to
     *   the first PWM input currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPwmInput FirstPwmInputInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("PwmInput");
        if (next_hwid == null)  return null;
        return FindPwmInputInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YPwmInput implementation)
}
}

