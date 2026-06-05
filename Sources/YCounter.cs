/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindCounter(), the high-level API for Counter functions
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

//--- (YCounter return codes)
//--- (end of YCounter return codes)
//--- (YCounter class start)
/**
 * <summary>
 *   YCounter Class: counter control interface
 * <para>
 *   The <c>YCounter</c> class allows you to read and configure Yoctopuce gcounters.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YCounter : YSensor
{
//--- (end of YCounter class start)
//--- (YCounter definitions)
    /**
     * <summary>
     *   invalid decimalMode value
     * </summary>
     */
    public const int DECIMALMODE_FALSE = 0;
    public const int DECIMALMODE_TRUE = 1;
    public const int DECIMALMODE_INVALID = -1;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _decimalMode = DECIMALMODE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackCounter = null;
    protected TimedReportCallback _timedReportCallbackCounter = null;

    public new delegate Task ValueCallback(YCounter func, string value);
    public new delegate Task TimedReportCallback(YCounter func, YMeasure measure);
    //--- (end of YCounter definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCounter(YAPIContext ctx, string func)
        : base(ctx, func, "Counter")
    {
        //--- (YCounter attributes initialization)
        //--- (end of YCounter attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YCounter(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YCounter implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("decimalMode")) {
            _decimalMode = json_val.getInt("decimalMode") > 0 ? 1 : 0;
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns a value indicating if the senseur compute whole or fractional values.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YCounter.DECIMALMODE_FALSE</c> or <c>YCounter.DECIMALMODE_TRUE</c>, according to a value
     *   indicating if the senseur compute whole or fractional values
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YCounter.DECIMALMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_decimalMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DECIMALMODE_INVALID;
            }
        }
        res = _decimalMode;
        return res;
    }


    /**
     * <summary>
     *   Changes the sensor's operating mode so that it computes integer or decimal values.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YCounter.DECIMALMODE_FALSE</c> or <c>YCounter.DECIMALMODE_TRUE</c>, according to the
     *   sensor's operating mode so that it computes integer or decimal values
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
    public async Task<int> set_decimalMode(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("decimalMode",rest_val);
        return YAPI.SUCCESS;
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
     *   Retrieves a counter for a given identifier.
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
     *   This function does not require that the counter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCounter.isOnline()</c> to test if the counter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a counter by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the counter, for instance
     *   <c>MyDevice.counter</c>.
     * </param>
     * <returns>
     *   a <c>YCounter</c> object allowing you to drive the counter.
     * </returns>
     */
    public static YCounter FindCounter(string func)
    {
        YCounter obj;
        obj = (YCounter) YFunction._FindFromCache("Counter", func);
        if (obj == null) {
            obj = new YCounter(func);
            YFunction._AddToCache("Counter", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a counter for a given identifier in a YAPI context.
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
     *   This function does not require that the counter is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YCounter.isOnline()</c> to test if the counter is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a counter by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the counter, for instance
     *   <c>MyDevice.counter</c>.
     * </param>
     * <returns>
     *   a <c>YCounter</c> object allowing you to drive the counter.
     * </returns>
     */
    public static YCounter FindCounterInContext(YAPIContext yctx,string func)
    {
        YCounter obj;
        obj = (YCounter) YFunction._FindFromCacheInContext(yctx, "Counter", func);
        if (obj == null) {
            obj = new YCounter(yctx, func);
            YFunction._AddToCache("Counter", func, obj);
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
        _valueCallbackCounter = callback;
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
        if (_valueCallbackCounter != null) {
            await _valueCallbackCounter(this, value);
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
        _timedReportCallbackCounter = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackCounter != null) {
            await _timedReportCallbackCounter(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    public virtual async Task<int> sendCommand(string command)
    {
        return await this.set_command(command);
    }

    /**
     * <summary>
     *   Reset the counter to zero.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds. Please note that this function only resets
     *   the integer part of the counter. In <c>CONTINUOUS</c> mode, the decimal part is calculated
     *   from the angle measured by the sensor. To set the decimal part of the sensor to zero,
     *   the origin of the sensor must be changed with the <c>YOrientation.zero()</c>.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> zero()
    {
        return await this.sendCommand("Z");
    }

    /**
     * <summary>
     *   Continues the enumeration of gcounters started using <c>yFirstCounter()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned gcounters order.
     *   If you want to find a specific a counter, use <c>Counter.findCounter()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCounter</c> object, corresponding to
     *   a counter currently online, or a <c>null</c> pointer
     *   if there are no more gcounters to enumerate.
     * </returns>
     */
    public YCounter nextCounter()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindCounterInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of gcounters currently accessible.
     * <para>
     *   Use the method <c>YCounter.nextCounter()</c> to iterate on
     *   next gcounters.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YCounter</c> object, corresponding to
     *   the first counter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCounter FirstCounter()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Counter");
        if (next_hwid == null)  return null;
        return FindCounterInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of gcounters currently accessible.
     * <para>
     *   Use the method <c>YCounter.nextCounter()</c> to iterate on
     *   next gcounters.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YCounter</c> object, corresponding to
     *   the first counter currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YCounter FirstCounterInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Counter");
        if (next_hwid == null)  return null;
        return FindCounterInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YCounter implementation)
}
}

