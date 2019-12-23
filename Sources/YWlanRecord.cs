/*********************************************************************
 *
 * $Id: YWlanRecord.cs 38899 2019-12-20 17:21:03Z mvuilleu $
 *
 * Implements FindWlanRecord(), the high-level API for WlanRecord functions
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

    //--- (generated code: YWlanRecord return codes)
//--- (end of generated code: YWlanRecord return codes)
    //--- (generated code: YWlanRecord class start)
/**
 * <summary>
 *   Y
 * <para>
 *   WlanRecord Class: Wireless network description, returned by <c>wireless.get_detectedWlans</c> method
 * </para>
 * <para>
 *   <c>YWlanRecord</c> objects are used to describe a wireless network.
 *   These objects are  used in particular in conjunction with the
 *   <c>YWireless</c> class.
 * </para>
 * </summary>
 */
public class YWlanRecord
{
//--- (end of generated code: YWlanRecord class start)
//--- (generated code: YWlanRecord definitions)
    protected string _ssid;
    protected int _channel = 0;
    protected string _sec;
    protected int _rssi = 0;

    //--- (end of generated code: YWlanRecord definitions)

    internal YWlanRecord(string json_str)
    {
        YJSONObject json = new YJSONObject(json_str);
        json.parse();
        _ssid = json.getString("ssid");
        _channel = json.getInt("channel");
        _sec = json.getString("sec");
        _rssi = json.getInt("rssi");
    }

    //--- (generated code: YWlanRecord implementation)
#pragma warning disable 1998

    /**
     * <summary>
     *   Returns the name of the wireless network (SSID).
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with the name of the wireless network (SSID).
     * </returns>
     */
    public virtual async Task<string> get_ssid()
    {
        return imm_get_ssid();
    }
    /**
     * <summary>
     *   Returns the name of the wireless network (SSID).
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with the name of the wireless network (SSID).
     * </returns>
     */
    public virtual string imm_get_ssid()
    {
        return _ssid;
    }

    /**
     * <summary>
     *   Returns the 802.11 b/g/n channel number used by this network.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the channel.
     * </returns>
     */
    public virtual async Task<int> get_channel()
    {
        return imm_get_channel();
    }
    /**
     * <summary>
     *   Returns the 802.11 b/g/n channel number used by this network.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the channel.
     * </returns>
     */
    public virtual int imm_get_channel()
    {
        return _channel;
    }

    /**
     * <summary>
     *   Returns the security algorithm used by the wireless network.
     * <para>
     *   If the network implements to security, the value is <c>"OPEN"</c>.
     * </para>
     * </summary>
     * <returns>
     *   a string with the security algorithm.
     * </returns>
     */
    public virtual async Task<string> get_security()
    {
        return imm_get_security();
    }
    /**
     * <summary>
     *   Returns the security algorithm used by the wireless network.
     * <para>
     *   If the network implements to security, the value is <c>"OPEN"</c>.
     * </para>
     * </summary>
     * <returns>
     *   a string with the security algorithm.
     * </returns>
     */
    public virtual string imm_get_security()
    {
        return _sec;
    }

    /**
     * <summary>
     *   Returns the quality of the wireless network link, in per cents.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer between 0 and 100 corresponding to the signal quality.
     * </returns>
     */
    public virtual async Task<int> get_linkQuality()
    {
        return imm_get_linkQuality();
    }
    /**
     * <summary>
     *   Returns the quality of the wireless network link, in per cents.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer between 0 and 100 corresponding to the signal quality.
     * </returns>
     */
    public virtual int imm_get_linkQuality()
    {
        return _rssi;
    }

#pragma warning restore 1998
    //--- (end of generated code: YWlanRecord implementation)
    }

}