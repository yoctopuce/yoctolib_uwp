/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindRangeFinder(), the high-level API for RangeFinder functions
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

//--- (YRangeFinder return codes)
//--- (end of YRangeFinder return codes)
//--- (YRangeFinder class start)
/**
 * <summary>
 *   YRangeFinder Class: range finder control interface, available for instance in the Yocto-RangeFinder
 * <para>
 *   The <c>YRangeFinder</c> class allows you to read and configure Yoctopuce range finders.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 *   This class adds the ability to easily perform a one-point linear calibration
 *   to compensate the effect of a glass or filter placed in front of the sensor.
 * </para>
 * </summary>
 */
public class YRangeFinder : YSensor
{
//--- (end of YRangeFinder class start)
//--- (YRangeFinder definitions)
    /**
     * <summary>
     *   invalid rangeFinderMode value
     * </summary>
     */
    public const int RANGEFINDERMODE_DEFAULT = 0;
    public const int RANGEFINDERMODE_LONG_RANGE = 1;
    public const int RANGEFINDERMODE_HIGH_ACCURACY = 2;
    public const int RANGEFINDERMODE_HIGH_SPEED = 3;
    public const int RANGEFINDERMODE_INVALID = -1;
    /**
     * <summary>
     *   invalid timeFrame value
     * </summary>
     */
    public const  long TIMEFRAME_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid quality value
     * </summary>
     */
    public const  int QUALITY_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid hardwareCalibration value
     * </summary>
     */
    public const  string HARDWARECALIBRATION_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid currentTemperature value
     * </summary>
     */
    public const  double CURRENTTEMPERATURE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _rangeFinderMode = RANGEFINDERMODE_INVALID;
    protected long _timeFrame = TIMEFRAME_INVALID;
    protected int _quality = QUALITY_INVALID;
    protected string _hardwareCalibration = HARDWARECALIBRATION_INVALID;
    protected double _currentTemperature = CURRENTTEMPERATURE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackRangeFinder = null;
    protected TimedReportCallback _timedReportCallbackRangeFinder = null;

    public new delegate Task ValueCallback(YRangeFinder func, string value);
    public new delegate Task TimedReportCallback(YRangeFinder func, YMeasure measure);
    //--- (end of YRangeFinder definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRangeFinder(YAPIContext ctx, string func)
        : base(ctx, func, "RangeFinder")
    {
        //--- (YRangeFinder attributes initialization)
        //--- (end of YRangeFinder attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRangeFinder(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YRangeFinder implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("rangeFinderMode")) {
            _rangeFinderMode = json_val.getInt("rangeFinderMode");
        }
        if (json_val.has("timeFrame")) {
            _timeFrame = json_val.getLong("timeFrame");
        }
        if (json_val.has("quality")) {
            _quality = json_val.getInt("quality");
        }
        if (json_val.has("hardwareCalibration")) {
            _hardwareCalibration = json_val.getString("hardwareCalibration");
        }
        if (json_val.has("currentTemperature")) {
            _currentTemperature = Math.Round(json_val.getDouble("currentTemperature") / 65.536) / 1000.0;
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the measured range.
     * <para>
     *   That unit is a string.
     *   String value can be <c>"</c> or <c>mm</c>. Any other value is ignored.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     *   WARNING: if a specific calibration is defined for the rangeFinder function, a
     *   unit system change will probably break it.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the measured range
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
     *   Returns the range finder running mode.
     * <para>
     *   The rangefinder running mode
     *   allows you to put priority on precision, speed or maximum range.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YRangeFinder.RANGEFINDERMODE_DEFAULT</c>, <c>YRangeFinder.RANGEFINDERMODE_LONG_RANGE</c>,
     *   <c>YRangeFinder.RANGEFINDERMODE_HIGH_ACCURACY</c> and <c>YRangeFinder.RANGEFINDERMODE_HIGH_SPEED</c>
     *   corresponding to the range finder running mode
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRangeFinder.RANGEFINDERMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rangeFinderMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return RANGEFINDERMODE_INVALID;
            }
        }
        res = _rangeFinderMode;
        return res;
    }


    /**
     * <summary>
     *   Changes the rangefinder running mode, allowing you to put priority on
     *   precision, speed or maximum range.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YRangeFinder.RANGEFINDERMODE_DEFAULT</c>, <c>YRangeFinder.RANGEFINDERMODE_LONG_RANGE</c>,
     *   <c>YRangeFinder.RANGEFINDERMODE_HIGH_ACCURACY</c> and <c>YRangeFinder.RANGEFINDERMODE_HIGH_SPEED</c>
     *   corresponding to the rangefinder running mode, allowing you to put priority on
     *   precision, speed or maximum range
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
    public async Task<int> set_rangeFinderMode(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("rangeFinderMode",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the time frame used to measure the distance and estimate the measure
     *   reliability.
     * <para>
     *   The time frame is expressed in milliseconds.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the time frame used to measure the distance and estimate the measure
     *   reliability
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRangeFinder.TIMEFRAME_INVALID</c>.
     * </para>
     */
    public async Task<long> get_timeFrame()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return TIMEFRAME_INVALID;
            }
        }
        res = _timeFrame;
        return res;
    }


    /**
     * <summary>
     *   Changes the time frame used to measure the distance and estimate the measure
     *   reliability.
     * <para>
     *   The time frame is expressed in milliseconds. A larger timeframe
     *   improves stability and reliability, at the cost of higher latency, but prevents
     *   the detection of events shorter than the time frame.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the time frame used to measure the distance and estimate the measure
     *   reliability
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
    public async Task<int> set_timeFrame(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("timeFrame",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns a measure quality estimate, based on measured dispersion.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to a measure quality estimate, based on measured dispersion
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRangeFinder.QUALITY_INVALID</c>.
     * </para>
     */
    public async Task<int> get_quality()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return QUALITY_INVALID;
            }
        }
        res = _quality;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_hardwareCalibration()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return HARDWARECALIBRATION_INVALID;
            }
        }
        res = _hardwareCalibration;
        return res;
    }


    public async Task<int> set_hardwareCalibration(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("hardwareCalibration",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current sensor temperature, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current sensor temperature, as a floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRangeFinder.CURRENTTEMPERATURE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_currentTemperature()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CURRENTTEMPERATURE_INVALID;
            }
        }
        res = _currentTemperature;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_command()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return COMMAND_INVALID;
            }
        }
        res = _command;
        return res;
    }


    public async Task<int> set_command(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("command",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a range finder for a given identifier.
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
     *   This function does not require that the range finder is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRangeFinder.isOnline()</c> to test if the range finder is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a range finder by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the range finder, for instance
     *   <c>YRNGFND1.rangeFinder1</c>.
     * </param>
     * <returns>
     *   a <c>YRangeFinder</c> object allowing you to drive the range finder.
     * </returns>
     */
    public static YRangeFinder FindRangeFinder(string func)
    {
        YRangeFinder obj;
        obj = (YRangeFinder) YFunction._FindFromCache("RangeFinder", func);
        if (obj == null) {
            obj = new YRangeFinder(func);
            YFunction._AddToCache("RangeFinder", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a range finder for a given identifier in a YAPI context.
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
     *   This function does not require that the range finder is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRangeFinder.isOnline()</c> to test if the range finder is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a range finder by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the range finder, for instance
     *   <c>YRNGFND1.rangeFinder1</c>.
     * </param>
     * <returns>
     *   a <c>YRangeFinder</c> object allowing you to drive the range finder.
     * </returns>
     */
    public static YRangeFinder FindRangeFinderInContext(YAPIContext yctx,string func)
    {
        YRangeFinder obj;
        obj = (YRangeFinder) YFunction._FindFromCacheInContext(yctx, "RangeFinder", func);
        if (obj == null) {
            obj = new YRangeFinder(yctx, func);
            YFunction._AddToCache("RangeFinder", func, obj);
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
        _valueCallbackRangeFinder = callback;
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
        if (_valueCallbackRangeFinder != null) {
            await _valueCallbackRangeFinder(this, value);
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
        _timedReportCallbackRangeFinder = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackRangeFinder != null) {
            await _timedReportCallbackRangeFinder(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Returns the temperature at the time when the latest calibration was performed.
     * <para>
     *   This function can be used to determine if a new calibration for ambient temperature
     *   is required.
     * </para>
     * </summary>
     * <returns>
     *   a temperature, as a floating point number.
     *   On failure, throws an exception or return YAPI.INVALID_DOUBLE.
     * </returns>
     */
    public virtual async Task<double> get_hardwareCalibrationTemperature()
    {
        string hwcal;
        hwcal = await this.get_hardwareCalibration();
        if (!((hwcal).Substring(0, 1) == "@")) {
            return YAPI.INVALID_DOUBLE;
        }
        return YAPIContext.imm_atoi((hwcal).Substring(1, (hwcal).Length));
    }

    /**
     * <summary>
     *   Triggers a sensor calibration according to the current ambient temperature.
     * <para>
     *   That
     *   calibration process needs no physical interaction with the sensor. It is performed
     *   automatically at device startup, but it is recommended to start it again when the
     *   temperature delta since the latest calibration exceeds 8 degrees Celsius.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> triggerTemperatureCalibration()
    {
        return await this.set_command("T");
    }

    /**
     * <summary>
     *   Triggers the photon detector hardware calibration.
     * <para>
     *   This function is part of the calibration procedure to compensate for the effect
     *   of a cover glass. Make sure to read the chapter about hardware calibration for details
     *   on the calibration procedure for proper results.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> triggerSpadCalibration()
    {
        return await this.set_command("S");
    }

    /**
     * <summary>
     *   Triggers the hardware offset calibration of the distance sensor.
     * <para>
     *   This function is part of the calibration procedure to compensate for the the effect
     *   of a cover glass. Make sure to read the chapter about hardware calibration for details
     *   on the calibration procedure for proper results.
     * </para>
     * </summary>
     * <param name="targetDist">
     *   true distance of the calibration target, in mm or inches, depending
     *   on the unit selected in the device
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> triggerOffsetCalibration(double targetDist)
    {
        int distmm;
        if (await this.get_unit() == "\"") {
            distmm = (int) Math.Round(targetDist * 25.4);
        } else {
            distmm = (int) Math.Round(targetDist);
        }
        return await this.set_command("O"+Convert.ToString(distmm));
    }

    /**
     * <summary>
     *   Triggers the hardware cross-talk calibration of the distance sensor.
     * <para>
     *   This function is part of the calibration procedure to compensate for the effect
     *   of a cover glass. Make sure to read the chapter about hardware calibration for details
     *   on the calibration procedure for proper results.
     * </para>
     * </summary>
     * <param name="targetDist">
     *   true distance of the calibration target, in mm or inches, depending
     *   on the unit selected in the device
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> triggerXTalkCalibration(double targetDist)
    {
        int distmm;
        if (await this.get_unit() == "\"") {
            distmm = (int) Math.Round(targetDist * 25.4);
        } else {
            distmm = (int) Math.Round(targetDist);
        }
        return await this.set_command("X"+Convert.ToString(distmm));
    }

    /**
     * <summary>
     *   Cancels the effect of previous hardware calibration procedures to compensate
     *   for cover glass, and restores factory settings.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> cancelCoverGlassCalibrations()
    {
        return await this.set_hardwareCalibration("");
    }

    /**
     * <summary>
     *   Continues the enumeration of range finders started using <c>yFirstRangeFinder()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned range finders order.
     *   If you want to find a specific a range finder, use <c>RangeFinder.findRangeFinder()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRangeFinder</c> object, corresponding to
     *   a range finder currently online, or a <c>null</c> pointer
     *   if there are no more range finders to enumerate.
     * </returns>
     */
    public YRangeFinder nextRangeFinder()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindRangeFinderInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of range finders currently accessible.
     * <para>
     *   Use the method <c>YRangeFinder.nextRangeFinder()</c> to iterate on
     *   next range finders.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRangeFinder</c> object, corresponding to
     *   the first range finder currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRangeFinder FirstRangeFinder()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RangeFinder");
        if (next_hwid == null)  return null;
        return FindRangeFinderInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of range finders currently accessible.
     * <para>
     *   Use the method <c>YRangeFinder.nextRangeFinder()</c> to iterate on
     *   next range finders.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YRangeFinder</c> object, corresponding to
     *   the first range finder currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRangeFinder FirstRangeFinderInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RangeFinder");
        if (next_hwid == null)  return null;
        return FindRangeFinderInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YRangeFinder implementation)
}
}

