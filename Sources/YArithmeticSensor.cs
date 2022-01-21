/*********************************************************************
 *
 *  $Id: YArithmeticSensor.cs 48028 2022-01-12 09:20:48Z seb $
 *
 *  Implements FindArithmeticSensor(), the high-level API for ArithmeticSensor functions
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

//--- (YArithmeticSensor return codes)
//--- (end of YArithmeticSensor return codes)
//--- (YArithmeticSensor class start)
/**
 * <summary>
 *   YArithmeticSensor Class: arithmetic sensor control interface, available for instance in the
 *   Yocto-MaxiMicroVolt-Rx
 * <para>
 *   The <c>YArithmeticSensor</c> class allows some Yoctopuce devices to compute in real-time
 *   values based on an arithmetic formula involving one or more measured signals as
 *   well as the temperature. As for any physical sensor, the computed values can be
 *   read by callback and stored in the built-in datalogger.
 * </para>
 * </summary>
 */
public class YArithmeticSensor : YSensor
{
//--- (end of YArithmeticSensor class start)
//--- (YArithmeticSensor definitions)
    /**
     * <summary>
     *   invalid description value
     * </summary>
     */
    public const  string DESCRIPTION_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected string _description = DESCRIPTION_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackArithmeticSensor = null;
    protected TimedReportCallback _timedReportCallbackArithmeticSensor = null;

    public new delegate Task ValueCallback(YArithmeticSensor func, string value);
    public new delegate Task TimedReportCallback(YArithmeticSensor func, YMeasure measure);
    //--- (end of YArithmeticSensor definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YArithmeticSensor(YAPIContext ctx, string func)
        : base(ctx, func, "ArithmeticSensor")
    {
        //--- (YArithmeticSensor attributes initialization)
        //--- (end of YArithmeticSensor attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YArithmeticSensor(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YArithmeticSensor implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("description")) {
            _description = json_val.getString("description");
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Changes the measuring unit for the arithmetic sensor.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a string corresponding to the measuring unit for the arithmetic sensor
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
     *   Returns a short informative description of the formula.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to a short informative description of the formula
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YArithmeticSensor.DESCRIPTION_INVALID</c>.
     * </para>
     */
    public async Task<string> get_description()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DESCRIPTION_INVALID;
            }
        }
        res = _description;
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
     *   Retrieves an arithmetic sensor for a given identifier.
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
     *   This function does not require that the arithmetic sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YArithmeticSensor.isOnline()</c> to test if the arithmetic sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an arithmetic sensor by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the arithmetic sensor, for instance
     *   <c>RXUVOLT1.arithmeticSensor1</c>.
     * </param>
     * <returns>
     *   a <c>YArithmeticSensor</c> object allowing you to drive the arithmetic sensor.
     * </returns>
     */
    public static YArithmeticSensor FindArithmeticSensor(string func)
    {
        YArithmeticSensor obj;
        obj = (YArithmeticSensor) YFunction._FindFromCache("ArithmeticSensor", func);
        if (obj == null) {
            obj = new YArithmeticSensor(func);
            YFunction._AddToCache("ArithmeticSensor",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves an arithmetic sensor for a given identifier in a YAPI context.
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
     *   This function does not require that the arithmetic sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YArithmeticSensor.isOnline()</c> to test if the arithmetic sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   an arithmetic sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the arithmetic sensor, for instance
     *   <c>RXUVOLT1.arithmeticSensor1</c>.
     * </param>
     * <returns>
     *   a <c>YArithmeticSensor</c> object allowing you to drive the arithmetic sensor.
     * </returns>
     */
    public static YArithmeticSensor FindArithmeticSensorInContext(YAPIContext yctx,string func)
    {
        YArithmeticSensor obj;
        obj = (YArithmeticSensor) YFunction._FindFromCacheInContext(yctx,  "ArithmeticSensor", func);
        if (obj == null) {
            obj = new YArithmeticSensor(yctx, func);
            YFunction._AddToCache("ArithmeticSensor",  func, obj);
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
        _valueCallbackArithmeticSensor = callback;
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
        if (_valueCallbackArithmeticSensor != null) {
            await _valueCallbackArithmeticSensor(this, value);
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
        _timedReportCallbackArithmeticSensor = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackArithmeticSensor != null) {
            await _timedReportCallbackArithmeticSensor(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Defines the arithmetic function by means of an algebraic expression.
     * <para>
     *   The expression
     *   may include references to device sensors, by their physical or logical name, to
     *   usual math functions and to auxiliary functions defined separately.
     * </para>
     * </summary>
     * <param name="expr">
     *   the algebraic expression defining the function.
     * </param>
     * <param name="descr">
     *   short informative description of the expression.
     * </param>
     * <returns>
     *   the current expression value if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YAPI.INVALID_DOUBLE.
     * </para>
     */
    public virtual async Task<double> defineExpression(string expr,string descr)
    {
        string id;
        string fname;
        string content;
        byte[] data = new byte[0];
        string diags;
        double resval;
        id = await this.get_functionId();
        id = (id).Substring( 16, (id).Length - 16);
        fname = "arithmExpr"+id+".txt";

        content = "// "+ descr+"\n"+expr;
        data = await this._uploadEx(fname, YAPI.DefaultEncoding.GetBytes(content));
        diags = YAPI.DefaultEncoding.GetString(data);
        if (!((diags).Substring(0, 8) == "Result: ")) { this._throw( YAPI.INVALID_ARGUMENT, diags); return YAPI.INVALID_DOUBLE; }
        resval = Double.Parse((diags).Substring( 8, (diags).Length-8));
        return resval;
    }

    /**
     * <summary>
     *   Retrieves the algebraic expression defining the arithmetic function, as previously
     *   configured using the <c>defineExpression</c> function.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing the mathematical expression.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<string> loadExpression()
    {
        string id;
        string fname;
        string content;
        int idx;
        id = await this.get_functionId();
        id = (id).Substring( 16, (id).Length - 16);
        fname = "arithmExpr"+id+".txt";

        content = YAPI.DefaultEncoding.GetString(await this._download(fname));
        idx = (content).IndexOf("\n");
        if (idx > 0) {
            content = (content).Substring( idx+1, (content).Length-(idx+1));
        }
        return content;
    }

    /**
     * <summary>
     *   Defines a auxiliary function by means of a table of reference points.
     * <para>
     *   Intermediate values
     *   will be interpolated between specified reference points. The reference points are given
     *   as pairs of floating point numbers.
     *   The auxiliary function will be available for use by all ArithmeticSensor objects of the
     *   device. Up to nine auxiliary function can be defined in a device, each containing up to
     *   96 reference points.
     * </para>
     * </summary>
     * <param name="name">
     *   auxiliary function name, up to 16 characters.
     * </param>
     * <param name="inputValues">
     *   array of floating point numbers, corresponding to the function input value.
     * </param>
     * <param name="outputValues">
     *   array of floating point numbers, corresponding to the output value
     *   desired for each of the input value, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> defineAuxiliaryFunction(string name,List<double> inputValues,List<double> outputValues)
    {
        int siz;
        string defstr;
        int idx;
        double inputVal;
        double outputVal;
        string fname;
        siz = inputValues.Count;
        if (!(siz > 1)) { this._throw( YAPI.INVALID_ARGUMENT, "auxiliary function must be defined by at least two points"); return YAPI.INVALID_ARGUMENT; }
        if (!(siz == outputValues.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "table sizes mismatch"); return YAPI.INVALID_ARGUMENT; }
        defstr = "";
        idx = 0;
        while (idx < siz) {
            inputVal = inputValues[idx];
            outputVal = outputValues[idx];
            defstr = ""+ defstr+""+YAPIContext.imm_floatToStr( inputVal)+":"+YAPIContext.imm_floatToStr(outputVal)+"\n";
            idx = idx + 1;
        }
        fname = "userMap"+name+".txt";

        return await this._upload(fname, YAPI.DefaultEncoding.GetBytes(defstr));
    }

    /**
     * <summary>
     *   Retrieves the reference points table defining an auxiliary function previously
     *   configured using the <c>defineAuxiliaryFunction</c> function.
     * <para>
     * </para>
     * </summary>
     * <param name="name">
     *   auxiliary function name, up to 16 characters.
     * </param>
     * <param name="inputValues">
     *   array of floating point numbers, that is filled by the function
     *   with all the function reference input value.
     * </param>
     * <param name="outputValues">
     *   array of floating point numbers, that is filled by the function
     *   output value for each of the input value, index by index.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> loadAuxiliaryFunction(string name,List<double> inputValues,List<double> outputValues)
    {
        string fname;
        byte[] defbin = new byte[0];
        int siz;

        fname = "userMap"+name+".txt";
        defbin = await this._download(fname);
        siz = (defbin).Length;
        if (!(siz > 0)) { this._throw( YAPI.INVALID_ARGUMENT, "auxiliary function does not exist"); return YAPI.INVALID_ARGUMENT; }
        inputValues.Clear();
        outputValues.Clear();
        // FIXME: decode line by line
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the enumeration of arithmetic sensors started using <c>yFirstArithmeticSensor()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned arithmetic sensors order.
     *   If you want to find a specific an arithmetic sensor, use <c>ArithmeticSensor.findArithmeticSensor()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YArithmeticSensor</c> object, corresponding to
     *   an arithmetic sensor currently online, or a <c>null</c> pointer
     *   if there are no more arithmetic sensors to enumerate.
     * </returns>
     */
    public YArithmeticSensor nextArithmeticSensor()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindArithmeticSensorInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of arithmetic sensors currently accessible.
     * <para>
     *   Use the method <c>YArithmeticSensor.nextArithmeticSensor()</c> to iterate on
     *   next arithmetic sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YArithmeticSensor</c> object, corresponding to
     *   the first arithmetic sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YArithmeticSensor FirstArithmeticSensor()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("ArithmeticSensor");
        if (next_hwid == null)  return null;
        return FindArithmeticSensorInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of arithmetic sensors currently accessible.
     * <para>
     *   Use the method <c>YArithmeticSensor.nextArithmeticSensor()</c> to iterate on
     *   next arithmetic sensors.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YArithmeticSensor</c> object, corresponding to
     *   the first arithmetic sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YArithmeticSensor FirstArithmeticSensorInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("ArithmeticSensor");
        if (next_hwid == null)  return null;
        return FindArithmeticSensorInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YArithmeticSensor implementation)
}
}

