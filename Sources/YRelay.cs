/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindRelay(), the high-level API for Relay functions
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

//--- (YRelay return codes)
//--- (end of YRelay return codes)
//--- (YRelay class start)
/**
 * <summary>
 *   YRelay Class: relay control interface, available for instance in the Yocto-LatchedRelay, the
 *   Yocto-MaxiPowerRelay, the Yocto-PowerRelay-V3 or the Yocto-Relay
 * <para>
 *   The <c>YRelay</c> class allows you to drive a Yoctopuce relay or optocoupled output.
 *   It can be used to simply switch the output on or off, but also to automatically generate short
 *   pulses of determined duration.
 *   On devices with two output for each relay (double throw), the two outputs are named A and B,
 *   with output A corresponding to the idle position (normally closed) and the output B corresponding to the
 *   active state (normally open).
 * </para>
 * </summary>
 */
public class YRelay : YFunction
{
//--- (end of YRelay class start)
//--- (YRelay definitions)
    public class YDelayedPulse
    {
        public int target = YAPI.INVALID_INT;
        public int ms = YAPI.INVALID_INT;
        public int moving = YAPI.INVALID_UINT;
        public YDelayedPulse(){}
    }

    /**
     * <summary>
     *   invalid state value
     * </summary>
     */
    public const int STATE_A = 0;
    public const int STATE_B = 1;
    public const int STATE_INVALID = -1;
    /**
     * <summary>
     *   invalid stateAtPowerOn value
     * </summary>
     */
    public const int STATEATPOWERON_UNCHANGED = 0;
    public const int STATEATPOWERON_A = 1;
    public const int STATEATPOWERON_B = 2;
    public const int STATEATPOWERON_INVALID = -1;
    /**
     * <summary>
     *   invalid maxTimeOnStateA value
     * </summary>
     */
    public const  long MAXTIMEONSTATEA_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid maxTimeOnStateB value
     * </summary>
     */
    public const  long MAXTIMEONSTATEB_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid output value
     * </summary>
     */
    public const int OUTPUT_OFF = 0;
    public const int OUTPUT_ON = 1;
    public const int OUTPUT_INVALID = -1;
    /**
     * <summary>
     *   invalid pulseTimer value
     * </summary>
     */
    public const  long PULSETIMER_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid countdown value
     * </summary>
     */
    public const  long COUNTDOWN_INVALID = YAPI.INVALID_LONG;
    public static readonly YDelayedPulse DELAYEDPULSETIMER_INVALID = null;
    protected int _state = STATE_INVALID;
    protected int _stateAtPowerOn = STATEATPOWERON_INVALID;
    protected long _maxTimeOnStateA = MAXTIMEONSTATEA_INVALID;
    protected long _maxTimeOnStateB = MAXTIMEONSTATEB_INVALID;
    protected int _output = OUTPUT_INVALID;
    protected long _pulseTimer = PULSETIMER_INVALID;
    protected YDelayedPulse _delayedPulseTimer = new YDelayedPulse();
    protected long _countdown = COUNTDOWN_INVALID;
    protected ValueCallback _valueCallbackRelay = null;
    protected int _firm = 0;

    public new delegate Task ValueCallback(YRelay func, string value);
    public new delegate Task TimedReportCallback(YRelay func, YMeasure measure);
    //--- (end of YRelay definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRelay(YAPIContext ctx, string func)
        : base(ctx, func, "Relay")
    {
        //--- (YRelay attributes initialization)
        //--- (end of YRelay attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRelay(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YRelay implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("state")) {
            _state = json_val.getInt("state") > 0 ? 1 : 0;
        }
        if (json_val.has("stateAtPowerOn")) {
            _stateAtPowerOn = json_val.getInt("stateAtPowerOn");
        }
        if (json_val.has("maxTimeOnStateA")) {
            _maxTimeOnStateA = json_val.getLong("maxTimeOnStateA");
        }
        if (json_val.has("maxTimeOnStateB")) {
            _maxTimeOnStateB = json_val.getLong("maxTimeOnStateB");
        }
        if (json_val.has("output")) {
            _output = json_val.getInt("output") > 0 ? 1 : 0;
        }
        if (json_val.has("pulseTimer")) {
            _pulseTimer = json_val.getLong("pulseTimer");
        }
        if (json_val.has("delayedPulseTimer")) {
            YJSONObject subjson = json_val.getYJSONObject("delayedPulseTimer");
            if (subjson.has("moving")) {
                _delayedPulseTimer.moving = subjson.getInt("moving");
            }
            if (subjson.has("target")) {
                _delayedPulseTimer.target = subjson.getInt("target");
            }
            if (subjson.has("ms")) {
                _delayedPulseTimer.ms = subjson.getInt("ms");
            }
        }
        if (json_val.has("countdown")) {
            _countdown = json_val.getLong("countdown");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   invalid delayedPulseTimer
     * </summary>
     */
    /**
     * <summary>
     *   Returns the state of the relays (A for the idle position, B for the active position).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YRelay.STATE_A</c> or <c>YRelay.STATE_B</c>, according to the state of the relays (A for
     *   the idle position, B for the active position)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRelay.STATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_state()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return STATE_INVALID;
            }
        }
        res = _state;
        return res;
    }


    /**
     * <summary>
     *   Changes the state of the relays (A for the idle position, B for the active position).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YRelay.STATE_A</c> or <c>YRelay.STATE_B</c>, according to the state of the relays (A for
     *   the idle position, B for the active position)
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
    public async Task<int> set_state(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("state",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the state of the relays at device startup (A for the idle position,
     *   B for the active position, UNCHANGED to leave the relay state as is).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YRelay.STATEATPOWERON_UNCHANGED</c>, <c>YRelay.STATEATPOWERON_A</c> and
     *   <c>YRelay.STATEATPOWERON_B</c> corresponding to the state of the relays at device startup (A for
     *   the idle position,
     *   B for the active position, UNCHANGED to leave the relay state as is)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRelay.STATEATPOWERON_INVALID</c>.
     * </para>
     */
    public async Task<int> get_stateAtPowerOn()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return STATEATPOWERON_INVALID;
            }
        }
        res = _stateAtPowerOn;
        return res;
    }


    /**
     * <summary>
     *   Changes the state of the relays at device startup (A for the idle position,
     *   B for the active position, UNCHANGED to leave the relay state as is).
     * <para>
     *   Remember to call the matching module <c>saveToFlash()</c>
     *   method, otherwise this call will have no effect.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YRelay.STATEATPOWERON_UNCHANGED</c>, <c>YRelay.STATEATPOWERON_A</c> and
     *   <c>YRelay.STATEATPOWERON_B</c> corresponding to the state of the relays at device startup (A for
     *   the idle position,
     *   B for the active position, UNCHANGED to leave the relay state as is)
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
    public async Task<int> set_stateAtPowerOn(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("stateAtPowerOn",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the maximum time (ms) allowed for the relay to stay in state
     *   A before automatically switching back in to B state.
     * <para>
     *   Zero means no time limit.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum time (ms) allowed for the relay to stay in state
     *   A before automatically switching back in to B state
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRelay.MAXTIMEONSTATEA_INVALID</c>.
     * </para>
     */
    public async Task<long> get_maxTimeOnStateA()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return MAXTIMEONSTATEA_INVALID;
            }
        }
        res = _maxTimeOnStateA;
        return res;
    }


    /**
     * <summary>
     *   Changes the maximum time (ms) allowed for the relay to stay in state A
     *   before automatically switching back in to B state.
     * <para>
     *   Use zero for no time limit.
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the maximum time (ms) allowed for the relay to stay in state A
     *   before automatically switching back in to B state
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
    public async Task<int> set_maxTimeOnStateA(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("maxTimeOnStateA",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retourne the maximum time (ms) allowed for the relay to stay in state B
     *   before automatically switching back in to A state.
     * <para>
     *   Zero means no time limit.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRelay.MAXTIMEONSTATEB_INVALID</c>.
     * </para>
     */
    public async Task<long> get_maxTimeOnStateB()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return MAXTIMEONSTATEB_INVALID;
            }
        }
        res = _maxTimeOnStateB;
        return res;
    }


    /**
     * <summary>
     *   Changes the maximum time (ms) allowed for the relay to stay in state B before
     *   automatically switching back in to A state.
     * <para>
     *   Use zero for no time limit.
     *   Remember to call the <c>saveToFlash()</c>
     *   method of the module if the modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the maximum time (ms) allowed for the relay to stay in state B before
     *   automatically switching back in to A state
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
    public async Task<int> set_maxTimeOnStateB(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("maxTimeOnStateB",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the output state of the relays, when used as a simple switch (single throw).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YRelay.OUTPUT_OFF</c> or <c>YRelay.OUTPUT_ON</c>, according to the output state of the
     *   relays, when used as a simple switch (single throw)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRelay.OUTPUT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_output()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return OUTPUT_INVALID;
            }
        }
        res = _output;
        return res;
    }


    /**
     * <summary>
     *   Changes the output state of the relays, when used as a simple switch (single throw).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YRelay.OUTPUT_OFF</c> or <c>YRelay.OUTPUT_ON</c>, according to the output state of the
     *   relays, when used as a simple switch (single throw)
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
    public async Task<int> set_output(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("output",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the number of milliseconds remaining before the relays is returned to idle position
     *   (state A), during a measured pulse generation.
     * <para>
     *   When there is no ongoing pulse, returns zero.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of milliseconds remaining before the relays is returned to idle position
     *   (state A), during a measured pulse generation
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRelay.PULSETIMER_INVALID</c>.
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


    public async Task<int> set_pulseTimer(long  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("pulseTimer",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Sets the relay to output B (active) for a specified duration, then brings it
     *   automatically back to output A (idle state).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="ms_duration">
     *   pulse duration, in milliseconds
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
    public async Task<int> pulse(int ms_duration)
    {
        string rest_val;
        rest_val = (ms_duration).ToString();
        await _setAttr("pulseTimer",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<YDelayedPulse> get_delayedPulseTimer()
    {
        YDelayedPulse res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DELAYEDPULSETIMER_INVALID;
            }
        }
        res = _delayedPulseTimer;
        return res;
    }


    public async Task<int> set_delayedPulseTimer(YDelayedPulse  newval)
    {
        string rest_val;
        rest_val = (newval.target).ToString()+":"+(newval.ms).ToString();
        await _setAttr("delayedPulseTimer",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Schedules a pulse.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="ms_delay">
     *   waiting time before the pulse, in milliseconds
     * </param>
     * <param name="ms_duration">
     *   pulse duration, in milliseconds
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
    public async Task<int> delayedPulse(int ms_delay,int ms_duration)
    {
        string rest_val;
        rest_val = (ms_delay).ToString()+":"+(ms_duration).ToString();
        await _setAttr("delayedPulseTimer",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the number of milliseconds remaining before a pulse (delayedPulse() call)
     *   When there is no scheduled pulse, returns zero.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of milliseconds remaining before a pulse (delayedPulse() call)
     *   When there is no scheduled pulse, returns zero
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRelay.COUNTDOWN_INVALID</c>.
     * </para>
     */
    public async Task<long> get_countdown()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return COUNTDOWN_INVALID;
            }
        }
        res = _countdown;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a relay for a given identifier.
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
     *   This function does not require that the relay is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRelay.isOnline()</c> to test if the relay is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a relay by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the relay, for instance
     *   <c>YLTCHRL1.relay1</c>.
     * </param>
     * <returns>
     *   a <c>YRelay</c> object allowing you to drive the relay.
     * </returns>
     */
    public static YRelay FindRelay(string func)
    {
        YRelay obj;
        obj = (YRelay) YFunction._FindFromCache("Relay", func);
        if (obj == null) {
            obj = new YRelay(func);
            YFunction._AddToCache("Relay", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a relay for a given identifier in a YAPI context.
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
     *   This function does not require that the relay is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRelay.isOnline()</c> to test if the relay is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a relay by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the relay, for instance
     *   <c>YLTCHRL1.relay1</c>.
     * </param>
     * <returns>
     *   a <c>YRelay</c> object allowing you to drive the relay.
     * </returns>
     */
    public static YRelay FindRelayInContext(YAPIContext yctx,string func)
    {
        YRelay obj;
        obj = (YRelay) YFunction._FindFromCacheInContext(yctx, "Relay", func);
        if (obj == null) {
            obj = new YRelay(yctx, func);
            YFunction._AddToCache("Relay", func, obj);
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
        _valueCallbackRelay = callback;
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
        if (_valueCallbackRelay != null) {
            await _valueCallbackRelay(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Switch the relay to the opposite state.
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
    public virtual async Task<int> toggle()
    {
        int sta;
        string fw;
        YModule mo;
        if (_firm == 0) {
            mo = await this.get_module();
            fw = await mo.get_firmwareRelease();
            if (fw == YModule.FIRMWARERELEASE_INVALID) {
                return STATE_INVALID;
            }
            _firm = YAPIContext.imm_atoi(fw);
        }
        if (_firm < 34921) {
            sta = await this.get_state();
            if (sta == STATE_INVALID) {
                return STATE_INVALID;
            }
            if (sta == STATE_B) {
                await this.set_state(STATE_A);
            } else {
                await this.set_state(STATE_B);
            }
            return YAPI.SUCCESS;
        } else {
            return await this._setAttr("state", "X");
        }
    }

    /**
     * <summary>
     *   Continues the enumeration of relays started using <c>yFirstRelay()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned relays order.
     *   If you want to find a specific a relay, use <c>Relay.findRelay()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRelay</c> object, corresponding to
     *   a relay currently online, or a <c>null</c> pointer
     *   if there are no more relays to enumerate.
     * </returns>
     */
    public YRelay nextRelay()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindRelayInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of relays currently accessible.
     * <para>
     *   Use the method <c>YRelay.nextRelay()</c> to iterate on
     *   next relays.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRelay</c> object, corresponding to
     *   the first relay currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRelay FirstRelay()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Relay");
        if (next_hwid == null)  return null;
        return FindRelayInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of relays currently accessible.
     * <para>
     *   Use the method <c>YRelay.nextRelay()</c> to iterate on
     *   next relays.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YRelay</c> object, corresponding to
     *   the first relay currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRelay FirstRelayInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Relay");
        if (next_hwid == null)  return null;
        return FindRelayInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YRelay implementation)
}
}

