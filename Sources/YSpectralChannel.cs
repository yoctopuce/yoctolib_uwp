/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindSpectralChannel(), the high-level API for SpectralChannel functions
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

//--- (YSpectralChannel return codes)
//--- (end of YSpectralChannel return codes)
//--- (YSpectralChannel class start)
/**
 * <summary>
 *   YSpectralChannel Class: spectral analysis channel control interface
 * <para>
 *   The <c>YSpectralChannel</c> class allows you to read and configure Yoctopuce spectral analysis channels.
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 * </para>
 * </summary>
 */
public class YSpectralChannel : YSensor
{
//--- (end of YSpectralChannel class start)
//--- (YSpectralChannel definitions)
    /**
     * <summary>
     *   invalid rawCount value
     * </summary>
     */
    public const  int RAWCOUNT_INVALID = YAPI.INVALID_INT;
    protected int _rawCount = RAWCOUNT_INVALID;
    protected ValueCallback _valueCallbackSpectralChannel = null;
    protected TimedReportCallback _timedReportCallbackSpectralChannel = null;

    public new delegate Task ValueCallback(YSpectralChannel func, string value);
    public new delegate Task TimedReportCallback(YSpectralChannel func, YMeasure measure);
    //--- (end of YSpectralChannel definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSpectralChannel(YAPIContext ctx, string func)
        : base(ctx, func, "SpectralChannel")
    {
        //--- (YSpectralChannel attributes initialization)
        //--- (end of YSpectralChannel attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YSpectralChannel(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YSpectralChannel implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("rawCount")) {
            _rawCount = json_val.getInt("rawCount");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Retrieves the raw count of data samples.
     * <para>
     *   This method returns the current value of rawCount, representing the total number of samples collected
     *   by the sensor.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YSpectralChannel.RAWCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<int> get_rawCount()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return RAWCOUNT_INVALID;
            }
        }
        res = _rawCount;
        return res;
    }


    /**
     * <summary>
     *   Retrieves a spectral analysis channel for a given identifier.
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
     *   This function does not require that the spectral analysis channel is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSpectralChannel.isOnline()</c> to test if the spectral analysis channel is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a spectral analysis channel by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the spectral analysis channel, for instance
     *   <c>MyDevice.spectralChannel1</c>.
     * </param>
     * <returns>
     *   a <c>YSpectralChannel</c> object allowing you to drive the spectral analysis channel.
     * </returns>
     */
    public static YSpectralChannel FindSpectralChannel(string func)
    {
        YSpectralChannel obj;
        obj = (YSpectralChannel) YFunction._FindFromCache("SpectralChannel", func);
        if (obj == null) {
            obj = new YSpectralChannel(func);
            YFunction._AddToCache("SpectralChannel", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a spectral analysis channel for a given identifier in a YAPI context.
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
     *   This function does not require that the spectral analysis channel is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YSpectralChannel.isOnline()</c> to test if the spectral analysis channel is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a spectral analysis channel by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the spectral analysis channel, for instance
     *   <c>MyDevice.spectralChannel1</c>.
     * </param>
     * <returns>
     *   a <c>YSpectralChannel</c> object allowing you to drive the spectral analysis channel.
     * </returns>
     */
    public static YSpectralChannel FindSpectralChannelInContext(YAPIContext yctx,string func)
    {
        YSpectralChannel obj;
        obj = (YSpectralChannel) YFunction._FindFromCacheInContext(yctx, "SpectralChannel", func);
        if (obj == null) {
            obj = new YSpectralChannel(yctx, func);
            YFunction._AddToCache("SpectralChannel", func, obj);
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
        _valueCallbackSpectralChannel = callback;
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
        if (_valueCallbackSpectralChannel != null) {
            await _valueCallbackSpectralChannel(this, value);
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
        _timedReportCallbackSpectralChannel = callback;
        return 0;
    }

    public override async Task<int> _invokeTimedReportCallback(YMeasure value)
    {
        if (_timedReportCallbackSpectralChannel != null) {
            await _timedReportCallbackSpectralChannel(this, value);
        } else {
            await base._invokeTimedReportCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of spectral analysis channels started using <c>yFirstSpectralChannel()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned spectral analysis channels order.
     *   If you want to find a specific a spectral analysis channel, use <c>SpectralChannel.findSpectralChannel()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSpectralChannel</c> object, corresponding to
     *   a spectral analysis channel currently online, or a <c>null</c> pointer
     *   if there are no more spectral analysis channels to enumerate.
     * </returns>
     */
    public YSpectralChannel nextSpectralChannel()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindSpectralChannelInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of spectral analysis channels currently accessible.
     * <para>
     *   Use the method <c>YSpectralChannel.nextSpectralChannel()</c> to iterate on
     *   next spectral analysis channels.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YSpectralChannel</c> object, corresponding to
     *   the first spectral analysis channel currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSpectralChannel FirstSpectralChannel()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SpectralChannel");
        if (next_hwid == null)  return null;
        return FindSpectralChannelInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of spectral analysis channels currently accessible.
     * <para>
     *   Use the method <c>YSpectralChannel.nextSpectralChannel()</c> to iterate on
     *   next spectral analysis channels.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YSpectralChannel</c> object, corresponding to
     *   the first spectral analysis channel currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YSpectralChannel FirstSpectralChannelInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("SpectralChannel");
        if (next_hwid == null)  return null;
        return FindSpectralChannelInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YSpectralChannel implementation)
}
}

