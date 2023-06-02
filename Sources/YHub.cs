/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindHub(), the high-level API for Hub functions
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
//--- (generated code: YHub return codes)
//--- (end of generated code: YHub return codes)
//--- (generated code: YHub class start)
/**
 * <summary>
 *   YHub Class: Hub Interface
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YHub
{
//--- (end of generated code: YHub class start)
//--- (generated code: YHub definitions)
    protected YAPIContext _ctx;
    protected int _hubref = 0;
    protected Object _userData = null;

    //--- (end of generated code: YHub definitions)


        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
        public YHub(YAPIContext ctx, int hubref)
        {
            //--- (generated code: YHub attributes initialization)
        //--- (end of generated code: YHub attributes initialization)
            _ctx = ctx;
            _hubref = hubref;
        }


        private async Task<string> _getStrAttr_internal(string attrName)
        {
            YGenericHub hub = _ctx.getGenHub(_hubref);
            if (hub == null) {
                return await Task.FromResult("");
            }
            switch (attrName) {
                case "registeredUrl":
                    return hub._http_params.getOriginalURL();
                case "connectionUrl":
                    return hub._http_params.imm_getUrl(true, false, true);
                case "serialNumber":
                    return hub.getSerialNumber();
                case "errorMessage":
                    return hub.getLastErrorMessage();
                default:
                    return "";
            }
        }

        private async Task<int> _getIntAttr_internal(string attrName)
        {
            YGenericHub hub = _ctx.getGenHub(_hubref);
            if (attrName == "isInUse") {
                return hub != null ? 1 : 0;
            }
            if (hub == null) {
                return await Task.FromResult(-1);
            }
            switch (attrName) {
                case "isOnline":
                    return hub.isOnline() ? 1 : 0;
                case "isReadOnly":
                    return hub.isReadOnly() ? 1 : 0;
                case "networkTimeout":
                    return hub.get_networkTimeout();
                case "errorType":
                    return hub.getLastErrorType();
                default:
                    return -1;
            }
        }

        private async Task _setIntAttr_internal(string attrName, int value)
        {
            YGenericHub hub = _ctx.getGenHub(_hubref);
            if (hub != null && attrName == "networkTimeout") {
                hub.set_networkTimeout(value);
            }
            else {
                await Task.Yield();
                
            }
        }


        private Task<List<string>> get_knownUrls_internal()
        {
            throw new NotImplementedException();
        }

        //--- (generated code: YHub implementation)
#pragma warning disable 1998

    //cannot be generated for UWP:
    //public virtual async Task<string> _getStrAttr_internal(string attrName)
    public virtual async Task<string> _getStrAttr(string attrName)
    {
        return await _getStrAttr_internal(attrName);
    }

    //cannot be generated for UWP:
    //public virtual async Task<int> _getIntAttr_internal(string attrName)
    public virtual async Task<int> _getIntAttr(string attrName)
    {
        return await _getIntAttr_internal(attrName);
    }

    //cannot be generated for UWP:
    //public virtual async Task _setIntAttr_internal(string attrName,int value)
    public virtual async Task _setIntAttr(string attrName,int value)
    {
        await _setIntAttr_internal(attrName, value);
    }

    /**
     * <summary>
     *   Returns the URL that has been used first to register this hub.
     * <para>
     * </para>
     * </summary>
     */
    public virtual async Task<string> get_registeredUrl()
    {
        return await this._getStrAttr("registeredUrl");
    }

    //cannot be generated for UWP:
    //public virtual async Task<List<string>> get_knownUrls_internal()
    /**
     * <summary>
     *   Returns all known URLs that have been used to register this hub.
     * <para>
     *   URLs are pointing to the same hub when the devices connected
     *   are sharing the same serial number.
     * </para>
     * </summary>
     */
    public virtual async Task<List<string>> get_knownUrls()
    {
        return await get_knownUrls_internal();
    }

    /**
     * <summary>
     *   Returns the URL currently in use to communicate with this hub.
     * <para>
     * </para>
     * </summary>
     */
    public virtual async Task<string> get_connectionUrl()
    {
        return await this._getStrAttr("connectionUrl");
    }

    /**
     * <summary>
     *   Returns the hub serial number, if the hub was already connected once.
     * <para>
     * </para>
     * </summary>
     */
    public virtual async Task<string> get_serialNumber()
    {
        return await this._getStrAttr("serialNumber");
    }

    /**
     * <summary>
     *   Tells if this hub is still registered within the API.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>true</c> if the hub has not been unregistered.
     * </returns>
     */
    public virtual async Task<bool> isInUse()
    {
        return await this._getIntAttr("isInUse") > 0;
    }

    /**
     * <summary>
     *   Tells if there is an active communication channel with this hub.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>true</c> if the hub is currently connected.
     * </returns>
     */
    public virtual async Task<bool> isOnline()
    {
        return await this._getIntAttr("isOnline") > 0;
    }

    /**
     * <summary>
     *   Tells if write access on this hub is blocked.
     * <para>
     *   Return <c>true</c> if it
     *   is not possible to change attributes on this hub
     * </para>
     * </summary>
     * <returns>
     *   <c>true</c> if it is not possible to change attributes on this hub.
     * </returns>
     */
    public virtual async Task<bool> isReadOnly()
    {
        return await this._getIntAttr("isReadOnly") > 0;
    }

    /**
     * <summary>
     *   Modifies tthe network connection delay for this hub.
     * <para>
     *   The default value is inherited from <c>ySetNetworkTimeout</c>
     *   at the time when the hub is registered, but it can be updated
     *   afterwards for each specific hub if necessary.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="networkMsTimeout">
     *   the network connection delay in milliseconds.
     * @noreturn
     * </param>
     */
    public virtual async Task set_networkTimeout(int networkMsTimeout)
    {
        await this._setIntAttr("networkTimeout", networkMsTimeout);
    }

    /**
     * <summary>
     *   Returns the network connection delay for this hub.
     * <para>
     *   The default value is inherited from <c>ySetNetworkTimeout</c>
     *   at the time when the hub is registered, but it can be updated
     *   afterwards for each specific hub if necessary.
     * </para>
     * </summary>
     * <returns>
     *   the network connection delay in milliseconds.
     * </returns>
     */
    public virtual async Task<int> get_networkTimeout()
    {
        return await this._getIntAttr("networkTimeout");
    }

    /**
     * <summary>
     *   Returns the numerical error code of the latest error with the hub.
     * <para>
     *   This method is mostly useful when using the Yoctopuce library with
     *   exceptions disabled.
     * </para>
     * </summary>
     * <returns>
     *   a number corresponding to the code of the latest error that occurred while
     *   using the hub object
     * </returns>
     */
    public virtual async Task<int> get_errorType()
    {
        return await this._getIntAttr("errorType");
    }

    /**
     * <summary>
     *   Returns the error message of the latest error with the hub.
     * <para>
     *   This method is mostly useful when using the Yoctopuce library with
     *   exceptions disabled.
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the latest error message that occured while
     *   using the hub object
     * </returns>
     */
    public virtual async Task<string> get_errorMessage()
    {
        return await this._getStrAttr("errorMessage");
    }

    /**
     * <summary>
     *   Returns the value of the userData attribute, as previously stored
     *   using method <c>set_userData</c>.
     * <para>
     *   This attribute is never touched directly by the API, and is at
     *   disposal of the caller to store a context.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the object stored previously by the caller.
     * </returns>
     */
    public virtual async Task<Object> get_userData()
    {
        return _userData;
    }

    /**
     * <summary>
     *   Stores a user context provided as argument in the userData
     *   attribute of the function.
     * <para>
     *   This attribute is never touched by the API, and is at
     *   disposal of the caller to store a context.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="data">
     *   any kind of object to be stored
     * @noreturn
     * </param>
     */
    public virtual async Task set_userData(Object data)
    {
        _userData = data;
    }

    /**
     * <summary>
     *   Starts the enumeration of hubs currently in use by the API.
     * <para>
     *   Use the method <c>YHub.nextHubInUse()</c> to iterate on the
     *   next hubs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YHub</c> object, corresponding to
     *   the first hub currently in use by the API, or a
     *   <c>null</c> pointer if none has been registered.
     * </returns>
     */
    public static YHub FirstHubInUse()
    {
        return YAPI.nextHubInUseInternal(-1);
    }

    /**
     * <summary>
     *   Starts the enumeration of hubs currently in use by the API
     *   in a given YAPI context.
     * <para>
     *   Use the method <c>YHub.nextHubInUse()</c> to iterate on the
     *   next hubs.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <returns>
     *   a pointer to a <c>YHub</c> object, corresponding to
     *   the first hub currently in use by the API, or a
     *   <c>null</c> pointer if none has been registered.
     * </returns>
     */
    public static YHub FirstHubInUseInContext(YAPIContext yctx)
    {
        return yctx.nextHubInUseInternal(-1);
    }

    /**
     * <summary>
     *   Continues the module enumeration started using <c>YHub.FirstHubInUse()</c>.
     * <para>
     *   Caution: You can't make any assumption about the order of returned hubs.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YHub</c> object, corresponding to
     *   the next hub currenlty in use, or a <c>null</c> pointer
     *   if there are no more hubs to enumerate.
     * </returns>
     */
    public virtual YHub nextHubInUse()
    {
        return _ctx.nextHubInUseInternal(_hubref);
    }

#pragma warning restore 1998
    //--- (end of generated code: YHub implementation)
}

}