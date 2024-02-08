/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindSdi12SnoopingRecord(), the high-level API for Sdi12SnoopingRecord functions
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
    //--- (generated code: YSdi12SnoopingRecord return codes)
//--- (end of generated code: YSdi12SnoopingRecord return codes)
    //--- (generated code: YSdi12SnoopingRecord class start)
/**
 * <summary>
 *   Y
 * <para>
 *   Sdi12SnoopingRecord Class: Intercepted SDI12 message description, returned by
 *   <c>sdi12Port.snoopMessages</c> method
 * </para>
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSdi12SnoopingRecord
{
//--- (end of generated code: YSdi12SnoopingRecord class start)
//--- (generated code: YSdi12SnoopingRecord definitions)
    protected int _tim = 0;
    protected int _pos = 0;
    protected int _dir = 0;
    protected string _msg;

    //--- (end of generated code: YSdi12SnoopingRecord definitions)


        /**
         * <summary>
         * </summary>
         * <param name="func">
         *   functionid
         * </param>
         */
        internal YSdi12SnoopingRecord(String json_str)
        {
            try {
                YJSONObject json = new YJSONObject(json_str);
                json.parse();
                //--- (generated code: YSdi12SnoopingRecord attributes initialization)
        //--- (end of generated code: YSdi12SnoopingRecord attributes initialization)
                if (json.has("t")) {
                    _tim = json.getInt("t");
                }
                if (json.has("p")) {
                    _pos = json.getInt("p");
                }
                if (json.has("m")) {
                    String m = json.getString("m");
                    _dir = (m[0] == '<' ? 1 : 0);
                    _msg = m.Substring(1);
                }
            } catch (Exception) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "invalid json struct for YSdi12SnoopingRecord");
            }
        }


        //--- (generated code: YSdi12SnoopingRecord implementation)
#pragma warning disable 1998

    /**
     * <summary>
     *   Returns the elapsed time, in ms, since the beginning of the preceding message.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the elapsed time, in ms, since the beginning of the preceding message.
     * </returns>
     */
    public virtual async Task<int> get_time()
    {
        return imm_get_time();
    }
    /**
     * <summary>
     *   Returns the elapsed time, in ms, since the beginning of the preceding message.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the elapsed time, in ms, since the beginning of the preceding message.
     * </returns>
     */
    public virtual int imm_get_time()
    {
        return _tim;
    }

    /**
     * <summary>
     *   Returns the absolute position of the message end.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the absolute position of the message end.
     * </returns>
     */
    public virtual async Task<int> get_pos()
    {
        return imm_get_pos();
    }
    /**
     * <summary>
     *   Returns the absolute position of the message end.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the absolute position of the message end.
     * </returns>
     */
    public virtual int imm_get_pos()
    {
        return _pos;
    }

    /**
     * <summary>
     *   Returns the message direction (RX=0, TX=1).
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the message direction (RX=0, TX=1).
     * </returns>
     */
    public virtual async Task<int> get_direction()
    {
        return imm_get_direction();
    }
    /**
     * <summary>
     *   Returns the message direction (RX=0, TX=1).
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the message direction (RX=0, TX=1).
     * </returns>
     */
    public virtual int imm_get_direction()
    {
        return _dir;
    }

    /**
     * <summary>
     *   Returns the message content.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the message content.
     * </returns>
     */
    public virtual async Task<string> get_message()
    {
        return imm_get_message();
    }
    /**
     * <summary>
     *   Returns the message content.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the message content.
     * </returns>
     */
    public virtual string imm_get_message()
    {
        return _msg;
    }

#pragma warning restore 1998
    //--- (end of generated code: YSdi12SnoopingRecord implementation)
    }
}