/*********************************************************************
 *
 *  $Id: YMultiSensController.cs 33718 2018-12-14 14:22:23Z seb $
 *
 *  Implements FindMultiSensController(), the high-level API for MultiSensController functions
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

//--- (YMultiSensController return codes)
//--- (end of YMultiSensController return codes)
//--- (YMultiSensController class start)
/**
 * <summary>
 *   YMultiSensController Class: MultiSensController function interface
 * <para>
 *   The Yoctopuce application programming interface allows you to drive a stepper motor.
 * </para>
 * </summary>
 */
public class YMultiSensController : YFunction
{
//--- (end of YMultiSensController class start)
//--- (YMultiSensController definitions)
    /**
     * <summary>
     *   invalid nSensors value
     * </summary>
     */
    public const  int NSENSORS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid maxSensors value
     * </summary>
     */
    public const  int MAXSENSORS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid maintenanceMode value
     * </summary>
     */
    public const int MAINTENANCEMODE_FALSE = 0;
    public const int MAINTENANCEMODE_TRUE = 1;
    public const int MAINTENANCEMODE_INVALID = -1;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _nSensors = NSENSORS_INVALID;
    protected int _maxSensors = MAXSENSORS_INVALID;
    protected int _maintenanceMode = MAINTENANCEMODE_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackMultiSensController = null;

    public new delegate Task ValueCallback(YMultiSensController func, string value);
    public new delegate Task TimedReportCallback(YMultiSensController func, YMeasure measure);
    //--- (end of YMultiSensController definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMultiSensController(YAPIContext ctx, string func)
        : base(ctx, func, "MultiSensController")
    {
        //--- (YMultiSensController attributes initialization)
        //--- (end of YMultiSensController attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YMultiSensController(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YMultiSensController implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("nSensors")) {
            _nSensors = json_val.getInt("nSensors");
        }
        if (json_val.has("maxSensors")) {
            _maxSensors = json_val.getInt("maxSensors");
        }
        if (json_val.has("maintenanceMode")) {
            _maintenanceMode = json_val.getInt("maintenanceMode") > 0 ? 1 : 0;
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the number of sensors to poll.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of sensors to poll
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiSensController.NSENSORS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_nSensors()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return NSENSORS_INVALID;
            }
        }
        res = _nSensors;
        return res;
    }


    /**
     * <summary>
     *   Changes the number of sensors to poll.
     * <para>
     *   Remember to call the
     *   <c>saveToFlash()</c> method of the module if the
     *   modification must be kept. It's recommended to restart the
     *   device with  <c>module->reboot()</c> after modifying
     *   (and saving) this settings
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the number of sensors to poll
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
    public async Task<int> set_nSensors(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("nSensors",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the maximum configurable sensor count allowed on this device.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the maximum configurable sensor count allowed on this device
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiSensController.MAXSENSORS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_maxSensors()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return MAXSENSORS_INVALID;
            }
        }
        res = _maxSensors;
        return res;
    }


    /**
     * <summary>
     *   Returns true when the device is in maintenance mode.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YMultiSensController.MAINTENANCEMODE_FALSE</c> or <c>YMultiSensController.MAINTENANCEMODE_TRUE</c>,
     *   according to true when the device is in maintenance mode
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YMultiSensController.MAINTENANCEMODE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_maintenanceMode()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return MAINTENANCEMODE_INVALID;
            }
        }
        res = _maintenanceMode;
        return res;
    }


    /**
     * <summary>
     *   Changes the device mode to enable maintenance and stop sensors polling.
     * <para>
     *   This way, the device will not restart automatically in case it cannot
     *   communicate with one of the sensors.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   either <c>YMultiSensController.MAINTENANCEMODE_FALSE</c> or <c>YMultiSensController.MAINTENANCEMODE_TRUE</c>,
     *   according to the device mode to enable maintenance and stop sensors polling
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
    public async Task<int> set_maintenanceMode(int  newval)
    {
        string rest_val;
        rest_val = (newval > 0 ? "1" : "0");
        await _setAttr("maintenanceMode",rest_val);
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
     *   Retrieves a multi-sensor controller for a given identifier.
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
     *   This function does not require that the multi-sensor controller is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMultiSensController.isOnline()</c> to test if the multi-sensor controller is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a multi-sensor controller by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the multi-sensor controller
     * </param>
     * <returns>
     *   a <c>YMultiSensController</c> object allowing you to drive the multi-sensor controller.
     * </returns>
     */
    public static YMultiSensController FindMultiSensController(string func)
    {
        YMultiSensController obj;
        obj = (YMultiSensController) YFunction._FindFromCache("MultiSensController", func);
        if (obj == null) {
            obj = new YMultiSensController(func);
            YFunction._AddToCache("MultiSensController",  func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a multi-sensor controller for a given identifier in a YAPI context.
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
     *   This function does not require that the multi-sensor controller is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YMultiSensController.isOnline()</c> to test if the multi-sensor controller is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a multi-sensor controller by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the multi-sensor controller
     * </param>
     * <returns>
     *   a <c>YMultiSensController</c> object allowing you to drive the multi-sensor controller.
     * </returns>
     */
    public static YMultiSensController FindMultiSensControllerInContext(YAPIContext yctx,string func)
    {
        YMultiSensController obj;
        obj = (YMultiSensController) YFunction._FindFromCacheInContext(yctx,  "MultiSensController", func);
        if (obj == null) {
            obj = new YMultiSensController(yctx, func);
            YFunction._AddToCache("MultiSensController",  func, obj);
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
        _valueCallbackMultiSensController = callback;
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
        if (_valueCallbackMultiSensController != null) {
            await _valueCallbackMultiSensController(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Configure the I2C address of the only sensor connected to the device.
     * <para>
     *   It is recommended to put the the device in maintenance mode before
     *   changing Sensors addresses.  This method is only intended to work with a single
     *   sensor connected to the device, if several sensors are connected, result
     *   is unpredictable.
     *   Note that the device is probably expecting to find a string of sensors with specific
     *   addresses. Check the device documentation to find out which addresses should be used.
     * </para>
     * </summary>
     * <param name="addr">
     *   new address of the connected sensor
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     *   On failure, throws an exception or returns a negative error code.
     * </returns>
     */
    public virtual async Task<int> setupAddress(int addr)
    {
        string cmd;
        cmd = "A"+Convert.ToString(addr);
        return await this.set_command(cmd);
    }

    /**
     * <summary>
     *   Continues the enumeration of multi-sensor controllers started using <c>yFirstMultiSensController()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned multi-sensor controllers order.
     *   If you want to find a specific a multi-sensor controller, use
     *   <c>MultiSensController.findMultiSensController()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMultiSensController</c> object, corresponding to
     *   a multi-sensor controller currently online, or a <c>null</c> pointer
     *   if there are no more multi-sensor controllers to enumerate.
     * </returns>
     */
    public YMultiSensController nextMultiSensController()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindMultiSensControllerInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of multi-sensor controllers currently accessible.
     * <para>
     *   Use the method <c>YMultiSensController.nextMultiSensController()</c> to iterate on
     *   next multi-sensor controllers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YMultiSensController</c> object, corresponding to
     *   the first multi-sensor controller currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMultiSensController FirstMultiSensController()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("MultiSensController");
        if (next_hwid == null)  return null;
        return FindMultiSensControllerInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of multi-sensor controllers currently accessible.
     * <para>
     *   Use the method <c>YMultiSensController.nextMultiSensController()</c> to iterate on
     *   next multi-sensor controllers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YMultiSensController</c> object, corresponding to
     *   the first multi-sensor controller currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YMultiSensController FirstMultiSensControllerInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("MultiSensController");
        if (next_hwid == null)  return null;
        return FindMultiSensControllerInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YMultiSensController implementation)
}
}

