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
    protected string _regUrl = "";
    protected List<string> _knownUrls = new List<string>();
    protected any _userData;

    //--- (end of generated code: YHub definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YHub(YAPIContext ctx, string func)
        : base(ctx, func, "Hub")
    {
        //--- (generated code: YHub attributes initialization)
        //--- (end of generated code: YHub attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YHub(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (generated code: YHub implementation)
#pragma warning disable 1998

    /**
     * <summary>
     *   Returns the URL that has been used first to register this hub.
     * <para>
     * </para>
     * </summary>
     */
    public virtual async Task<string> get_registeredUrl()
    {
        return _regUrl;
    }

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
        List<string> knownUrls = new List<string>();
        knownUrls.Clear();
        for (int ii = 0; ii < _knownUrls.Count; ii++) {
            knownUrls.Add(_knownUrls[ii]);
        }
        return knownUrls;
    }

    public virtual void imm_inheritFrom(YHub otherHub)
    {
        for (int ii = 0; ii < otherHub._knownUrls.Count; ii++) {
            _knownUrls.Add(otherHub._knownUrls[ii]);
        }
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
    public virtual async Task<any> get_userData()
    {
        return this._userData;
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
    public virtual async Task set_userData(any data)
    {
        _userData = data;
    }

#pragma warning restore 1998
    //--- (end of generated code: YHub implementation)
}
}

