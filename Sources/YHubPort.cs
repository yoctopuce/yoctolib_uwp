/*********************************************************************
 *
 *  $Id: YHubPort.cs 43478 2021-01-21 13:49:12Z mvuilleu $
 *
 *  Implements FindHubPort(), the high-level API for HubPort functions
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

//--- (YHubPort return codes)
//--- (end of YHubPort return codes)
//--- (YHubPort class start)
/**
 * <summary>
 *   YHubPort Class: YoctoHub slave port control interface, available for instance in the
 *   YoctoHub-Ethernet, the YoctoHub-GSM-4G, the YoctoHub-Shield or the YoctoHub-Wireless-n
 * <para>
 *   The <c>YHubPort</c> class provides control over the power supply for slave ports
 *   on a YoctoHub. It provide information about the device connected to it.
 *   The logical name of a YHubPort is always automatically set to the
 *   unique serial number of the Yoctopuce device connected to it.
 * </para>
 * </summary>
 */
public class YHubPort : YFunction
{
//--- (end of YHubPort class start)
//--- (YHubPort definitions)
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
     *   invalid portState value
     * </summary>
     */
    public const int PORTSTATE_OFF = 0;
    public const int PORTSTATE_OVRLD = 1;
    public const int PORTSTATE_ON = 2;
    public const int PORTSTATE_RUN = 3;
    public const int PORTSTATE_PROG = 4;
    public const int PORTSTATE_INVALID = -1;
    /**
     * <summary>
     *   invalid baudRate value
     * </summary>
     */
    public const  int BAUDRATE_INVALID = YAPI.INVALID_UINT;
    protected int _enabled = ENABLED_INVALID;
    protected int _portState = PORTSTATE_INVALID;
    protected int _baudRate = BAUDRATE_INVALID;
    protected ValueCallback _valueCallbackHubPort = null;

    public new delegate Task ValueCallback(YHubPort func, string value);
    public new delegate Task TimedReportCallback(YHubPort func, YMeasure measure);
    //--- (end of YHubPort definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YHubPort(YAPIContext ctx, string func)
        : base(ctx, func, "HubPort")
    {
        //--- (YHubPort attributes initialization)
        //--- (end of YHubPort attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YHubPort(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YHubPort implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("enabled")) {
            _enabled = json_val.getInt("enabled") > 0 ? 1 : 0;
        }
        if (json_val.has("portState")) {
            _portState = json_val.getInt("portState");
        }
        if (json_val.has("baudRate")) {
            _baudRate = json_val.getInt("baudRate");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns true if the YoctoHub port is powered, false otherwise.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YHubPort.ENABLED_FALSE</c> or <c>YHubPort.ENABLED_TRUE</c>, according to true if the
     *   YoctoHub port is powered, false otherwise
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YHubPort.ENABLED_INVALID</c>.
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
     *   Changes the activation of the YoctoHub port.
     * <para>
     *   If the port is enabled, the
     *   connected module is powered. Otherwise, port power is shut down.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YHubPort.ENABLED_FALSE</c> or <c>YHubPort.ENABLED_TRUE</c>, according to the activation
     *   of the YoctoHub port
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
     *   Returns the current state of the YoctoHub port.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YHubPort.PORTSTATE_OFF</c>, <c>YHubPort.PORTSTATE_OVRLD</c>,
     *   <c>YHubPort.PORTSTATE_ON</c>, <c>YHubPort.PORTSTATE_RUN</c> and <c>YHubPort.PORTSTATE_PROG</c>
     *   corresponding to the current state of the YoctoHub port
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YHubPort.PORTSTATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_portState()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return PORTSTATE_INVALID;
            }
        }
        res = _portState;
        return res;
    }


    /**
     * <summary>
     *   Returns the current baud rate used by this YoctoHub port, in kbps.
     * <para>
     *   The default value is 1000 kbps, but a slower rate may be used if communication
     *   problems are encountered.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current baud rate used by this YoctoHub port, in kbps
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YHubPort.BAUDRATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_baudRate()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return BAUDRATE_INVALID;
            }
        }
        res = _baudRate;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a YoctoHub slave port for a given identifier.
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
     *   This function does not require that the YoctoHub slave port is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YHubPort.isOnline()</c> to test if the YoctoHub slave port is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a YoctoHub slave port by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the YoctoHub slave port, for instance
     *   <c>YHUBETH1.hubPort1</c>.
     * </param>
     * <returns>
     *   a <c>YHubPort</c> object allowing you to drive the YoctoHub slave port.
     * </returns>
     */
    public static YHubPort FindHubPort(string func)
    {
        YHubPort obj;
        obj = (YHubPort) YFunction._FindFromCache("HubPort", func);
        if (obj == null) {
            obj = new YHubPort(func);
            YFunction._AddToCache("HubPort",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a YoctoHub slave port for a given identifier in a YAPI context.
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
     *   This function does not require that the YoctoHub slave port is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YHubPort.isOnline()</c> to test if the YoctoHub slave port is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a YoctoHub slave port by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the YoctoHub slave port, for instance
     *   <c>YHUBETH1.hubPort1</c>.
     * </param>
     * <returns>
     *   a <c>YHubPort</c> object allowing you to drive the YoctoHub slave port.
     * </returns>
     */
    public static YHubPort FindHubPortInContext(YAPIContext yctx,string func)
    {
        YHubPort obj;
        obj = (YHubPort) YFunction._FindFromCacheInContext(yctx,  "HubPort", func);
        if (obj == null) {
            obj = new YHubPort(yctx, func);
            YFunction._AddToCache("HubPort",  func, obj);
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
        _valueCallbackHubPort = callback;
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
        if (_valueCallbackHubPort != null) {
            await _valueCallbackHubPort(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of YoctoHub slave ports started using <c>yFirstHubPort()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned YoctoHub slave ports order.
     *   If you want to find a specific a YoctoHub slave port, use <c>HubPort.findHubPort()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YHubPort</c> object, corresponding to
     *   a YoctoHub slave port currently online, or a <c>null</c> pointer
     *   if there are no more YoctoHub slave ports to enumerate.
     * </returns>
     */
    public YHubPort nextHubPort()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindHubPortInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of YoctoHub slave ports currently accessible.
     * <para>
     *   Use the method <c>YHubPort.nextHubPort()</c> to iterate on
     *   next YoctoHub slave ports.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YHubPort</c> object, corresponding to
     *   the first YoctoHub slave port currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YHubPort FirstHubPort()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("HubPort");
        if (next_hwid == null)  return null;
        return FindHubPortInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of YoctoHub slave ports currently accessible.
     * <para>
     *   Use the method <c>YHubPort.nextHubPort()</c> to iterate on
     *   next YoctoHub slave ports.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YHubPort</c> object, corresponding to
     *   the first YoctoHub slave port currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YHubPort FirstHubPortInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("HubPort");
        if (next_hwid == null)  return null;
        return FindHubPortInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YHubPort implementation)
}
}

