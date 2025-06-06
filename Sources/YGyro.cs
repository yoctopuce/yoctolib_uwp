/*********************************************************************
 *
 * $Id: YGyro.cs 63510 2024-11-28 10:46:59Z seb $
 *
 * Implements FindGyro(), the high-level API for Gyro functions
 *
 * - - - - - - - - - License information: - - - - - - - - -
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
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{

    //--- (generated code: YGyro return codes)
//--- (end of generated code: YGyro return codes)
    //--- (generated code: YGyro class start)
/**
 * <summary>
 *   YGyro Class: gyroscope control interface, available for instance in the Yocto-3D-V2
 * <para>
 *   The <c>YGyro</c> class allows you to read and configure Yoctopuce gyroscopes.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 *   This class adds the possibility to access x, y and z components of the rotation
 *   vector separately, as well as the possibility to deal with quaternion-based
 *   orientation estimates.
 * </para>
 * </summary>
 */
public class YGyro : YSensor
{
//--- (end of generated code: YGyro class start)
        public delegate Task YQuatCallback(YGyro yGyro, double w, double x, double y, double z);
        public delegate Task YAnglesCallback(YGyro yGyro, double roll, double pitch, double head);
        protected static async Task yInternalGyroCallback(YQt obj, String value)
        {
            YGyro gyro = (YGyro)await obj.get_userData();
            if (gyro == null) {
                return;
            }
            string tmp = obj.imm_get_functionId().Substring(2);
            int idx = Convert.ToInt32(tmp);
            double dbl_value = Convert.ToDouble(value);
            await gyro._invokeGyroCallbacks(idx, dbl_value);
        }
        //--- (generated code: YGyro definitions)
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
    protected int _bandwidth = BANDWIDTH_INVALID;
    protected double _xValue = XVALUE_INVALID;
    protected double _yValue = YVALUE_INVALID;
    protected double _zValue = ZVALUE_INVALID;
    protected ValueCallback _valueCallbackGyro = null;
    protected TimedReportCallback _timedReportCallbackGyro = null;
    protected int _qt_stamp = 0;
    protected YQt _qt_w;
    protected YQt _qt_x;
    protected YQt _qt_y;
    protected YQt _qt_z;
    protected double _w = 0;
    protected double _x = 0;
    protected double _y = 0;
    protected double _z = 0;
    protected int _angles_stamp = 0;
    protected double _head = 0;
    protected double _pitch = 0;
    protected double _roll = 0;
    protected YQuatCallback _quatCallback;
    protected YAnglesCallback _anglesCallback;

    public new delegate Task ValueCallback(YGyro func, string value);
    public new delegate Task TimedReportCallback(YGyro func, YMeasure measure);
    //--- (end of generated code: YGyro definitions)


        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YGyro(YAPIContext ctx, String func)
            : base(ctx, func, "Gyro")
        {
            //--- (generated code: YGyro attributes initialization)
        //--- (end of generated code: YGyro attributes initialization)
        }

        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        protected YGyro(String func)
            : this(YAPI.imm_GetYCtx(), func)
        { }

        //--- (generated code: YGyro implementation)
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
     *   On failure, throws an exception or returns <c>YGyro.BANDWIDTH_INVALID</c>.
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
     *   Returns the angular velocity around the X axis of the device, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the angular velocity around the X axis of the device, as a
     *   floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGyro.XVALUE_INVALID</c>.
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
     *   Returns the angular velocity around the Y axis of the device, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the angular velocity around the Y axis of the device, as a
     *   floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGyro.YVALUE_INVALID</c>.
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
     *   Returns the angular velocity around the Z axis of the device, as a floating point number.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the angular velocity around the Z axis of the device, as a
     *   floating point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGyro.ZVALUE_INVALID</c>.
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
     *   Retrieves a gyroscope for a given identifier.
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
     *   This function does not require that the gyroscope is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YGyro.isOnline()</c> to test if the gyroscope is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a gyroscope by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the gyroscope, for instance
     *   <c>Y3DMK002.gyro</c>.
     * </param>
     * <returns>
     *   a <c>YGyro</c> object allowing you to drive the gyroscope.
     * </returns>
     */
    public static YGyro FindGyro(string func)
    {
        YGyro obj;
        obj = (YGyro) YFunction._FindFromCache("Gyro", func);
        if (obj == null) {
            obj = new YGyro(func);
            YFunction._AddToCache("Gyro", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a gyroscope for a given identifier in a YAPI context.
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
     *   This function does not require that the gyroscope is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YGyro.isOnline()</c> to test if the gyroscope is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a gyroscope by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the gyroscope, for instance
     *   <c>Y3DMK002.gyro</c>.
     * </param>
     * <returns>
     *   a <c>YGyro</c> object allowing you to drive the gyroscope.
     * </returns>
     */
    public static YGyro FindGyroInContext(YAPIContext yctx,string func)
    {
        YGyro obj;
        obj = (YGyro) YFunction._FindFromCacheInContext(yctx, "Gyro", func);
        if (obj == null) {
            obj = new YGyro(yctx, func);
            YFunction._AddToCache("Gyro", func, obj);
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
        _valueCallbackGyro = callback;
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
        if (_valueCallbackGyro != null) {
            await _valueCallbackGyro(this, value);
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
        _timedReportCallbackGyro = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackGyro != null) {
            await _timedReportCallbackGyro(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    public virtual async Task<int> _loadQuaternion()
    {
        int now_stamp;
        int age_ms;
        now_stamp = (int) ((YAPIContext.GetTickCount()) & 0x7FFFFFFF);
        age_ms = ((now_stamp - _qt_stamp) & 0x7FFFFFFF);
        if ((age_ms >= 10) || (_qt_stamp == 0)) {
            if (await this.load(10) != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
            if (_qt_stamp == 0) {
                _qt_w = YQt.FindQtInContext(_yapi, ""+_serial+".qt1");
                _qt_x = YQt.FindQtInContext(_yapi, ""+_serial+".qt2");
                _qt_y = YQt.FindQtInContext(_yapi, ""+_serial+".qt3");
                _qt_z = YQt.FindQtInContext(_yapi, ""+_serial+".qt4");
            }
            if (await _qt_w.load(9) != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
            if (await _qt_x.load(9) != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
            if (await _qt_y.load(9) != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
            if (await _qt_z.load(9) != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
            _w = await _qt_w.get_currentValue();
            _x = await _qt_x.get_currentValue();
            _y = await _qt_y.get_currentValue();
            _z = await _qt_z.get_currentValue();
            _qt_stamp = now_stamp;
        }
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> _loadAngles()
    {
        double sqw;
        double sqx;
        double sqy;
        double sqz;
        double norm;
        double delta;

        if (await this._loadQuaternion() != YAPI.SUCCESS) {
            return YAPI.DEVICE_NOT_FOUND;
        }
        if (_angles_stamp != _qt_stamp) {
            sqw = _w * _w;
            sqx = _x * _x;
            sqy = _y * _y;
            sqz = _z * _z;
            norm = sqx + sqy + sqz + sqw;
            delta = _y * _w - _x * _z;
            if (delta > 0.499 * norm) {
                // singularity at north pole
                _pitch = 90.0;
                _head  = Math.Round(2.0 * 1800.0/Math.PI * Math.Atan2(_x,-_w)) / 10.0;
            } else {
                if (delta < -0.499 * norm) {
                    // singularity at south pole
                    _pitch = -90.0;
                    _head  = Math.Round(-2.0 * 1800.0/Math.PI * Math.Atan2(_x,-_w)) / 10.0;
                } else {
                    _roll  = Math.Round(1800.0/Math.PI * Math.Atan2(2.0 * (_w * _x + _y * _z),sqw - sqx - sqy + sqz)) / 10.0;
                    _pitch = Math.Round(1800.0/Math.PI * Math.Asin(2.0 * delta / norm)) / 10.0;
                    _head  = Math.Round(1800.0/Math.PI * Math.Atan2(2.0 * (_x * _y + _z * _w),sqw + sqx - sqy - sqz)) / 10.0;
                }
            }
            _angles_stamp = _qt_stamp;
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the estimated roll angle, based on the integration of
     *   gyroscopic measures combined with acceleration and
     *   magnetic field measurements.
     * <para>
     *   The axis corresponding to the roll angle can be mapped to any
     *   of the device X, Y or Z physical directions using methods of
     *   the class <c>YRefFrame</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to roll angle
     *   in degrees, between -180 and +180.
     * </returns>
     */
    public virtual async Task<double> get_roll()
    {
        await this._loadAngles();
        return _roll;
    }

    /**
     * <summary>
     *   Returns the estimated pitch angle, based on the integration of
     *   gyroscopic measures combined with acceleration and
     *   magnetic field measurements.
     * <para>
     *   The axis corresponding to the pitch angle can be mapped to any
     *   of the device X, Y or Z physical directions using methods of
     *   the class <c>YRefFrame</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to pitch angle
     *   in degrees, between -90 and +90.
     * </returns>
     */
    public virtual async Task<double> get_pitch()
    {
        await this._loadAngles();
        return _pitch;
    }

    /**
     * <summary>
     *   Returns the estimated heading angle, based on the integration of
     *   gyroscopic measures combined with acceleration and
     *   magnetic field measurements.
     * <para>
     *   The axis corresponding to the heading can be mapped to any
     *   of the device X, Y or Z physical directions using methods of
     *   the class <c>YRefFrame</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to heading
     *   in degrees, between 0 and 360.
     * </returns>
     */
    public virtual async Task<double> get_heading()
    {
        await this._loadAngles();
        return _head;
    }

    /**
     * <summary>
     *   Returns the <c>w</c> component (real part) of the quaternion
     *   describing the device estimated orientation, based on the
     *   integration of gyroscopic measures combined with acceleration and
     *   magnetic field measurements.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the <c>w</c>
     *   component of the quaternion.
     * </returns>
     */
    public virtual async Task<double> get_quaternionW()
    {
        await this._loadQuaternion();
        return _w;
    }

    /**
     * <summary>
     *   Returns the <c>x</c> component of the quaternion
     *   describing the device estimated orientation, based on the
     *   integration of gyroscopic measures combined with acceleration and
     *   magnetic field measurements.
     * <para>
     *   The <c>x</c> component is
     *   mostly correlated with rotations on the roll axis.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the <c>x</c>
     *   component of the quaternion.
     * </returns>
     */
    public virtual async Task<double> get_quaternionX()
    {
        await this._loadQuaternion();
        return _x;
    }

    /**
     * <summary>
     *   Returns the <c>y</c> component of the quaternion
     *   describing the device estimated orientation, based on the
     *   integration of gyroscopic measures combined with acceleration and
     *   magnetic field measurements.
     * <para>
     *   The <c>y</c> component is
     *   mostly correlated with rotations on the pitch axis.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the <c>y</c>
     *   component of the quaternion.
     * </returns>
     */
    public virtual async Task<double> get_quaternionY()
    {
        await this._loadQuaternion();
        return _y;
    }

    /**
     * <summary>
     *   Returns the <c>x</c> component of the quaternion
     *   describing the device estimated orientation, based on the
     *   integration of gyroscopic measures combined with acceleration and
     *   magnetic field measurements.
     * <para>
     *   The <c>x</c> component is
     *   mostly correlated with changes of heading.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the <c>z</c>
     *   component of the quaternion.
     * </returns>
     */
    public virtual async Task<double> get_quaternionZ()
    {
        await this._loadQuaternion();
        return _z;
    }

    /**
     * <summary>
     *   Registers a callback function that will be invoked each time that the estimated
     *   device orientation has changed.
     * <para>
     *   The call frequency is typically around 95Hz during a move.
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered.
     *   For good responsiveness, remember to call one of these two functions periodically.
     *   To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to invoke, or a null pointer.
     *   The callback function should take five arguments:
     *   the YGyro object of the turning device, and the floating
     *   point values of the four components w, x, y and z
     *   (as floating-point numbers).
     * @noreturn
     * </param>
     */
    public virtual async Task<int> registerQuaternionCallback(YQuatCallback callback)
    {
        _quatCallback = callback;
        if (callback != null) {
            if (await this._loadQuaternion() != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
            await _qt_w.set_userData(this);
            await _qt_x.set_userData(this);
            await _qt_y.set_userData(this);
            await _qt_z.set_userData(this);
            await _qt_w.registerValueCallback(yInternalGyroCallback);
            await _qt_x.registerValueCallback(yInternalGyroCallback);
            await _qt_y.registerValueCallback(yInternalGyroCallback);
            await _qt_z.registerValueCallback(yInternalGyroCallback);
        } else {
            if (!(_anglesCallback != null)) {
                await _qt_w.registerValueCallback((YQt.ValueCallback) null);
                await _qt_x.registerValueCallback((YQt.ValueCallback) null);
                await _qt_y.registerValueCallback((YQt.ValueCallback) null);
                await _qt_z.registerValueCallback((YQt.ValueCallback) null);
            }
        }
        return 0;
    }

    /**
     * <summary>
     *   Registers a callback function that will be invoked each time that the estimated
     *   device orientation has changed.
     * <para>
     *   The call frequency is typically around 95Hz during a move.
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered.
     *   For good responsiveness, remember to call one of these two functions periodically.
     *   To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to invoke, or a null pointer.
     *   The callback function should take four arguments:
     *   the YGyro object of the turning device, and the floating
     *   point values of the three angles roll, pitch and heading
     *   in degrees (as floating-point numbers).
     * @noreturn
     * </param>
     */
    public virtual async Task<int> registerAnglesCallback(YAnglesCallback callback)
    {
        _anglesCallback = callback;
        if (callback != null) {
            if (await this._loadQuaternion() != YAPI.SUCCESS) {
                return YAPI.DEVICE_NOT_FOUND;
            }
            await _qt_w.set_userData(this);
            await _qt_x.set_userData(this);
            await _qt_y.set_userData(this);
            await _qt_z.set_userData(this);
            await _qt_w.registerValueCallback(yInternalGyroCallback);
            await _qt_x.registerValueCallback(yInternalGyroCallback);
            await _qt_y.registerValueCallback(yInternalGyroCallback);
            await _qt_z.registerValueCallback(yInternalGyroCallback);
        } else {
            if (!(_quatCallback != null)) {
                await _qt_w.registerValueCallback((YQt.ValueCallback) null);
                await _qt_x.registerValueCallback((YQt.ValueCallback) null);
                await _qt_y.registerValueCallback((YQt.ValueCallback) null);
                await _qt_z.registerValueCallback((YQt.ValueCallback) null);
            }
        }
        return 0;
    }

    public virtual async Task<int> _invokeGyroCallbacks(int qtIndex,double qtValue)
    {
        switch (qtIndex - 1) {
        case 0:
            _w = qtValue;
            break;
        case 1:
            _x = qtValue;
            break;
        case 2:
            _y = qtValue;
            break;
        case 3:
            _z = qtValue;
            break;
        }
        if (qtIndex < 4) {
            return 0;
        }
        _qt_stamp = (int) ((YAPIContext.GetTickCount()) & 0x7FFFFFFF);
        if (_quatCallback != null) {
            await _quatCallback(this, _w, _x, _y, _z);
        }
        if (_anglesCallback != null) {
            await this._loadAngles();
            await _anglesCallback(this, _roll, _pitch, _head);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of gyroscopes started using <c>yFirstGyro()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned gyroscopes order.
     *   If you want to find a specific a gyroscope, use <c>Gyro.findGyro()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YGyro</c> object, corresponding to
     *   a gyroscope currently online, or a <c>null</c> pointer
     *   if there are no more gyroscopes to enumerate.
     * </returns>
     */
    public YGyro nextGyro()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindGyroInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of gyroscopes currently accessible.
     * <para>
     *   Use the method <c>YGyro.nextGyro()</c> to iterate on
     *   next gyroscopes.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YGyro</c> object, corresponding to
     *   the first gyro currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YGyro FirstGyro()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Gyro");
        if (next_hwid == null)  return null;
        return FindGyroInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of gyroscopes currently accessible.
     * <para>
     *   Use the method <c>YGyro.nextGyro()</c> to iterate on
     *   next gyroscopes.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YGyro</c> object, corresponding to
     *   the first gyro currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YGyro FirstGyroInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Gyro");
        if (next_hwid == null)  return null;
        return FindGyroInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YGyro implementation)
    }

}