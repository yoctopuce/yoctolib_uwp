/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindSpectralSensor(), the high-level API for SpectralSensor functions
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

//--- (YSpectralSensor return codes)
//--- (end of YSpectralSensor return codes)
//--- (YSpectralSensor class start)
/**
 * <summary>
 *   YSpectralSensor Class: spectral sensor control interface
 * <para>
 *   The <c>YSpectralSensor</c> class allows you to read and configure Yoctopuce spectral sensors.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YSpectralSensor : YFunction
{
//--- (end of YSpectralSensor class start)
//--- (YSpectralSensor definitions)
    /**
     * <summary>
     *   invalid ledCurrent value
     * </summary>
     */
    public const  int LEDCURRENT_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid resolution value
     * </summary>
     */
    public const  double RESOLUTION_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid integrationTime value
     * </summary>
     */
    public const  int INTEGRATIONTIME_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid gain value
     * </summary>
     */
    public const  int GAIN_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid saturation value
     * </summary>
     */
    public const  int SATURATION_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid estimatedRGB value
     * </summary>
     */
    public const  int ESTIMATEDRGB_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid estimatedHSL value
     * </summary>
     */
    public const  int ESTIMATEDHSL_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid estimatedXYZ value
     * </summary>
     */
    public const  string ESTIMATEDXYZ_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid estimatedOkLab value
     * </summary>
     */
    public const  string ESTIMATEDOKLAB_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid estimatedRAL value
     * </summary>
     */
    public const  string ESTIMATEDRAL_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid ledCurrentAtPowerOn value
     * </summary>
     */
    public const  int LEDCURRENTATPOWERON_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid integrationTimeAtPowerOn value
     * </summary>
     */
    public const  int INTEGRATIONTIMEATPOWERON_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid gainAtPowerOn value
     * </summary>
     */
    public const  int GAINATPOWERON_INVALID = YAPI.INVALID_INT;
    protected int _ledCurrent = LEDCURRENT_INVALID;
    protected double _resolution = RESOLUTION_INVALID;
    protected int _integrationTime = INTEGRATIONTIME_INVALID;
    protected int _gain = GAIN_INVALID;
    protected int _saturation = SATURATION_INVALID;
    protected int _estimatedRGB = ESTIMATEDRGB_INVALID;
    protected int _estimatedHSL = ESTIMATEDHSL_INVALID;
    protected string _estimatedXYZ = ESTIMATEDXYZ_INVALID;
    protected string _estimatedOkLab = ESTIMATEDOKLAB_INVALID;
    protected string _estimatedRAL = ESTIMATEDRAL_INVALID;
    protected int _ledCurrentAtPowerOn = LEDCURRENTATPOWERON_INVALID;
    protected int _integrationTimeAtPowerOn = INTEGRATIONTIMEATPOWERON_INVALID;
    protected int _gainAtPowerOn = GAINATPOWERON_INVALID;
    protected ValueCallback _valueCallbackSpectralSensor = null;

    public new delegate Task ValueCallback(YSpectralSensor func, string value);
    public new delegate Task TimedReportCallback(YSpectralSensor func, YMeasure measure);
    //--- (end of YSpectralSensor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSpectralSensor(YAPIContext ctx, string func)
        : base(ctx, func, "SpectralSensor")
    {
        //--- (YSpectralSensor attributes initialization)
        //--- (end of YSpectralSensor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSpectralSensor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YSpectralSensor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("ledCurrent")) {
            _ledCurrent = json_val.getInt("ledCurrent");
        }
        if (json_val.has("resolution")) {
            _resolution = Math.Round(json_val.getDouble("resolution") / 65.536) / 1000.0;
        }
        if (json_val.has("integrationTime")) {
            _integrationTime = json_val.getInt("integrationTime");
        }
        if (json_val.has("gain")) {
            _gain = json_val.getInt("gain");
        }
        if (json_val.has("saturation")) {
            _saturation = json_val.getInt("saturation");
        }
        if (json_val.has("estimatedRGB")) {
            _estimatedRGB = json_val.getInt("estimatedRGB");
        }
        if (json_val.has("estimatedHSL")) {
            _estimatedHSL = json_val.getInt("estimatedHSL");
        }
        if (json_val.has("estimatedXYZ")) {
            _estimatedXYZ = json_val.getString("estimatedXYZ");
        }
        if (json_val.has("estimatedOkLab")) {
            _estimatedOkLab = json_val.getString("estimatedOkLab");
        }
        if (json_val.has("estimatedRAL")) {
            _estimatedRAL = json_val.getString("estimatedRAL");
        }
        if (json_val.has("ledCurrentAtPowerOn")) {
            _ledCurrentAtPowerOn = json_val.getInt("ledCurrentAtPowerOn");
        }
        if (json_val.has("integrationTimeAtPowerOn")) {
            _integrationTimeAtPowerOn = json_val.getInt("integrationTimeAtPowerOn");
        }
        if (json_val.has("gainAtPowerOn")) {
            _gainAtPowerOn = json_val.getInt("gainAtPowerOn");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the current value of the LED.
     * <para>
     *   This method retrieves the current flowing through the LED
     *   and returns it as an integer or an object.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current value of the LED
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.LEDCURRENT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_ledCurrent()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return LEDCURRENT_INVALID;
            }
        }
        res = _ledCurrent;
        return res;
    }


    /**
     * <summary>
     *   Changes the luminosity of the module leds.
     * <para>
     *   The parameter is a
     *   value between 0 and 100.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the luminosity of the module leds
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
    public async Task<int> set_ledCurrent(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("ledCurrent",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Changes the resolution of the measured physical values.
     * <para>
     *   The resolution corresponds to the numerical precision
     *   when displaying value. It does not change the precision of the measure itself.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a floating point number corresponding to the resolution of the measured physical values
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
    public async Task<int> set_resolution(double  newval)
    {
        string rest_val;
        rest_val = Math.Round(newval * 65536.0).ToString();
        await _setAttr("resolution",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the resolution of the measured values.
     * <para>
     *   The resolution corresponds to the numerical precision
     *   of the measures, which is not always the same as the actual precision of the sensor.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the resolution of the measured values
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.RESOLUTION_INVALID</c>.
     * </para>
     */
    public async Task<double> get_resolution()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return RESOLUTION_INVALID;
            }
        }
        res = _resolution;
        return res;
    }


    /**
     * <summary>
     *   Returns the current integration time.
     * <para>
     *   This method retrieves the integration time value
     *   used for data processing and returns it as an integer or an object.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current integration time
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.INTEGRATIONTIME_INVALID</c>.
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
     *   Sets the integration time for data processing.
     * <para>
     *   This method takes a parameter `val` and assigns it
     *   as the new integration time. This affects the duration
     *   for which data is integrated before being processed.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
     *   Retrieves the current gain.
     * <para>
     *   This method updates the gain value.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.GAIN_INVALID</c>.
     * </para>
     */
    public async Task<int> get_gain()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return GAIN_INVALID;
            }
        }
        res = _gain;
        return res;
    }


    /**
     * <summary>
     *   Sets the gain for signal processing.
     * <para>
     *   This method takes a parameter `val` and assigns it
     *   as the new gain. This affects the sensitivity and
     *   intensity of the processed signal.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public async Task<int> set_gain(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("gain",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current saturation of the sensor.
     * <para>
     *   This function updates the sensor's saturation value.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current saturation of the sensor
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.SATURATION_INVALID</c>.
     * </para>
     */
    public async Task<int> get_saturation()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return SATURATION_INVALID;
            }
        }
        res = _saturation;
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in RGB format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an RGB object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the estimated color in RGB format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDRGB_INVALID</c>.
     * </para>
     */
    public async Task<int> get_estimatedRGB()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ESTIMATEDRGB_INVALID;
            }
        }
        res = _estimatedRGB;
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in HSL format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an HSL object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the estimated color in HSL format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDHSL_INVALID</c>.
     * </para>
     */
    public async Task<int> get_estimatedHSL()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ESTIMATEDHSL_INVALID;
            }
        }
        res = _estimatedHSL;
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in XYZ format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an XYZ object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the estimated color in XYZ format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDXYZ_INVALID</c>.
     * </para>
     */
    public async Task<string> get_estimatedXYZ()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ESTIMATEDXYZ_INVALID;
            }
        }
        res = _estimatedXYZ;
        return res;
    }


    /**
     * <summary>
     *   Returns the estimated color in OkLab format.
     * <para>
     *   This method retrieves the estimated color values
     *   and returns them as an OkLab object or structure.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the estimated color in OkLab format
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.ESTIMATEDOKLAB_INVALID</c>.
     * </para>
     */
    public async Task<string> get_estimatedOkLab()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ESTIMATEDOKLAB_INVALID;
            }
        }
        res = _estimatedOkLab;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_estimatedRAL()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ESTIMATEDRAL_INVALID;
            }
        }
        res = _estimatedRAL;
        return res;
    }


    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_ledCurrentAtPowerOn()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return LEDCURRENTATPOWERON_INVALID;
            }
        }
        res = _ledCurrentAtPowerOn;
        return res;
    }


    /**
     * <summary>
     *   Sets the LED current at power-on.
     * <para>
     *   This method takes a parameter `val` and assigns it to startupLumin, representing the LED current defined
     *   at startup.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public async Task<int> set_ledCurrentAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("ledCurrentAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves the integration time at power-on.
     * <para>
     *   This method updates the power-on integration time value.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralSensor.INTEGRATIONTIMEATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_integrationTimeAtPowerOn()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return INTEGRATIONTIMEATPOWERON_INVALID;
            }
        }
        res = _integrationTimeAtPowerOn;
        return res;
    }


    /**
     * <summary>
     *   Sets the integration time at power-on.
     * <para>
     *   This method takes a parameter `val` and assigns it to startupIntegTime, representing the integration time
     *   defined at startup.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public async Task<int> set_integrationTimeAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("integrationTimeAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<int> get_gainAtPowerOn()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return GAINATPOWERON_INVALID;
            }
        }
        res = _gainAtPowerOn;
        return res;
    }


    /**
     * <summary>
     *   Sets the gain at power-on.
     * <para>
     *   This method takes a parameter `val` and assigns it to startupGain, representing the gain defined at startup.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer
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
    public async Task<int> set_gainAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("gainAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a spectral sensor for a given identifier.
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
     *   This function does not require that the spectral sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSpectralSensor.isOnline()</c> to test if the spectral sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a spectral sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the spectral sensor, for instance
     *   <c>MyDevice.spectralSensor</c>.
     * </param>
     * <returns>
     *   a <c>YSpectralSensor</c> object allowing you to drive the spectral sensor.
     * </returns>
     */
    public static YSpectralSensor FindSpectralSensor(string func)
    {
        YSpectralSensor obj;
        obj = (YSpectralSensor) YFunction._FindFromCache("SpectralSensor", func);
        if (obj == null) {
            obj = new YSpectralSensor(func);
            YFunction._AddToCache("SpectralSensor", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a spectral sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the spectral sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSpectralSensor.isOnline()</c> to test if the spectral sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a spectral sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the spectral sensor, for instance
     *   <c>MyDevice.spectralSensor</c>.
     * </param>
     * <returns>
     *   a <c>YSpectralSensor</c> object allowing you to drive the spectral sensor.
     * </returns>
     */
    public static YSpectralSensor FindSpectralSensorInContext(YAPIContext yctx,string func)
    {
        YSpectralSensor obj;
        obj = (YSpectralSensor) YFunction._FindFromCacheInContext(yctx, "SpectralSensor", func);
        if (obj == null) {
            obj = new YSpectralSensor(yctx, func);
            YFunction._AddToCache("SpectralSensor", func, obj);
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
        _valueCallbackSpectralSensor = callback;
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
        if (_valueCallbackSpectralSensor != null) {
            await _valueCallbackSpectralSensor(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of spectral sensors started using <c>yFirstSpectralSensor()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned spectral sensors order.
     *   If you want to find a specific a spectral sensor, use <c>SpectralSensor.findSpectralSensor()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSpectralSensor</c> object, corresponding to
     *   a spectral sensor currently online, or a <c>null</c> pointer
     *   if there are no more spectral sensors to enumerate.
     * </returns>
     */
    public YSpectralSensor nextSpectralSensor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindSpectralSensorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of spectral sensors currently accessible.
     * <para>
     *   Use the method <c>YSpectralSensor.nextSpectralSensor()</c> to iterate on
     *   next spectral sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSpectralSensor</c> object, corresponding to
     *   the first spectral sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSpectralSensor FirstSpectralSensor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SpectralSensor");
        if (next_hwid == null)  return null;
        return FindSpectralSensorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of spectral sensors currently accessible.
     * <para>
     *   Use the method <c>YSpectralSensor.nextSpectralSensor()</c> to iterate on
     *   next spectral sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YSpectralSensor</c> object, corresponding to
     *   the first spectral sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSpectralSensor FirstSpectralSensorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SpectralSensor");
        if (next_hwid == null)  return null;
        return FindSpectralSensorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YSpectralSensor implementation)
}
}

