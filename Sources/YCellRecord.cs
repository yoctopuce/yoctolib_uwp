/*********************************************************************
 *
 * $Id: YCellRecord.cs 38899 2019-12-20 17:21:03Z mvuilleu $
 *
 * Implements FindCellRecord(), the high-level API for CellRecord functions
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


using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{

    //--- (generated code: YCellRecord return codes)
//--- (end of generated code: YCellRecord return codes)
    //--- (generated code: YCellRecord class start)
/**
 * <summary>
 *   Y
 * <para>
 *   CellRecord Class: Cellular antenna description, returned by <c>cellular.quickCellSurvey</c> method
 * </para>
 * <para>
 *   <c>YCellRecord</c> objects are used to describe a wireless network.
 *   These objects are used in particular in conjunction with the
 *   <c>YCellular</c> class.
 * </para>
 * </summary>
 */
public class YCellRecord
{
//--- (end of generated code: YCellRecord class start)
//--- (generated code: YCellRecord definitions)
    protected string _oper;
    protected int _mcc = 0;
    protected int _mnc = 0;
    protected int _lac = 0;
    protected int _cid = 0;
    protected int _dbm = 0;
    protected int _tad = 0;

    //--- (end of generated code: YCellRecord definitions)


        /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */

        public YCellRecord(int mcc, int mnc, int lac, int cellId, int dbm, int tad, string oper) {
            //--- (generated code: YCellRecord attributes initialization)
        //--- (end of generated code: YCellRecord attributes initialization)
            _oper = oper;
            _mcc = mcc;
            _mnc = mnc;
            _lac = lac;
            _cid = cellId;
            _dbm = dbm;
            _tad = tad;
        }

        //--- (generated code: YCellRecord implementation)
#pragma warning disable 1998

    /**
     * <summary>
     *   Returns the name of the the cell operator, as received from the network.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with the name of the the cell operator.
     * </returns>
     */
    public virtual async Task<string> get_cellOperator()
    {
        return imm_get_cellOperator();
    }
    /**
     * <summary>
     *   Returns the name of the the cell operator, as received from the network.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with the name of the the cell operator.
     * </returns>
     */
    public virtual string imm_get_cellOperator()
    {
        return _oper;
    }

    /**
     * <summary>
     *   Returns the Mobile Country Code (MCC).
     * <para>
     *   The MCC is a unique identifier for each country.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Mobile Country Code (MCC).
     * </returns>
     */
    public virtual async Task<int> get_mobileCountryCode()
    {
        return imm_get_mobileCountryCode();
    }
    /**
     * <summary>
     *   Returns the Mobile Country Code (MCC).
     * <para>
     *   The MCC is a unique identifier for each country.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Mobile Country Code (MCC).
     * </returns>
     */
    public virtual int imm_get_mobileCountryCode()
    {
        return _mcc;
    }

    /**
     * <summary>
     *   Returns the Mobile Network Code (MNC).
     * <para>
     *   The MNC is a unique identifier for each phone
     *   operator within a country.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Mobile Network Code (MNC).
     * </returns>
     */
    public virtual async Task<int> get_mobileNetworkCode()
    {
        return imm_get_mobileNetworkCode();
    }
    /**
     * <summary>
     *   Returns the Mobile Network Code (MNC).
     * <para>
     *   The MNC is a unique identifier for each phone
     *   operator within a country.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Mobile Network Code (MNC).
     * </returns>
     */
    public virtual int imm_get_mobileNetworkCode()
    {
        return _mnc;
    }

    /**
     * <summary>
     *   Returns the Location Area Code (LAC).
     * <para>
     *   The LAC is a unique identifier for each
     *   place within a country.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Location Area Code (LAC).
     * </returns>
     */
    public virtual async Task<int> get_locationAreaCode()
    {
        return imm_get_locationAreaCode();
    }
    /**
     * <summary>
     *   Returns the Location Area Code (LAC).
     * <para>
     *   The LAC is a unique identifier for each
     *   place within a country.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Location Area Code (LAC).
     * </returns>
     */
    public virtual int imm_get_locationAreaCode()
    {
        return _lac;
    }

    /**
     * <summary>
     *   Returns the Cell ID.
     * <para>
     *   The Cell ID is a unique identifier for each
     *   base transmission station within a LAC.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Cell Id.
     * </returns>
     */
    public virtual async Task<int> get_cellId()
    {
        return imm_get_cellId();
    }
    /**
     * <summary>
     *   Returns the Cell ID.
     * <para>
     *   The Cell ID is a unique identifier for each
     *   base transmission station within a LAC.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Cell Id.
     * </returns>
     */
    public virtual int imm_get_cellId()
    {
        return _cid;
    }

    /**
     * <summary>
     *   Returns the signal strength, measured in dBm.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the signal strength.
     * </returns>
     */
    public virtual async Task<int> get_signalStrength()
    {
        return imm_get_signalStrength();
    }
    /**
     * <summary>
     *   Returns the signal strength, measured in dBm.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the signal strength.
     * </returns>
     */
    public virtual int imm_get_signalStrength()
    {
        return _dbm;
    }

    /**
     * <summary>
     *   Returns the Timing Advance (TA).
     * <para>
     *   The TA corresponds to the time necessary
     *   for the signal to reach the base station from the device.
     *   Each increment corresponds about to 550m of distance.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Timing Advance (TA).
     * </returns>
     */
    public virtual async Task<int> get_timingAdvance()
    {
        return imm_get_timingAdvance();
    }
    /**
     * <summary>
     *   Returns the Timing Advance (TA).
     * <para>
     *   The TA corresponds to the time necessary
     *   for the signal to reach the base station from the device.
     *   Each increment corresponds about to 550m of distance.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the Timing Advance (TA).
     * </returns>
     */
    public virtual int imm_get_timingAdvance()
    {
        return _tad;
    }

#pragma warning restore 1998
    //--- (end of generated code: YCellRecord implementation)
    }

}