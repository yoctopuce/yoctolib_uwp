/*********************************************************************
 *
 * $Id: YSpiSnoopingRecord.cs 38899 2019-12-20 17:21:03Z mvuilleu $
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
    //--- (generated code: YSpiSnoopingRecord return codes)
//--- (end of generated code: YSpiSnoopingRecord return codes)
    //--- (generated code: YSpiSnoopingRecord class start)
/**
 * <summary>
 *   Y
 * <para>
 *   SpiSnoopingRecord Class: Intercepted SPI message description, returned by <c>spiPort.snoopMessages</c> method
 * </para>
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSpiSnoopingRecord
{
//--- (end of generated code: YSpiSnoopingRecord class start)
//--- (generated code: YSpiSnoopingRecord definitions)
    protected int _tim = 0;
    protected int _dir = 0;
    protected string _msg;

    //--- (end of generated code: YSpiSnoopingRecord definitions)

    internal YSpiSnoopingRecord(string json_str)
    {
        YJSONObject json = new YJSONObject(json_str);
        json.parse();
        _tim = json.getInt("t");
        string m = json.getString("m");
        _dir = (m[0] == '<' ? 1 : 0);
        _msg = m.Substring(1);
    }

    //--- (generated code: YSpiSnoopingRecord implementation)
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
    //--- (end of generated code: YSpiSnoopingRecord implementation)
    }

}