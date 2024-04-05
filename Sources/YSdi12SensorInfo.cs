/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindSdi12SensorInfo(), the high-level API for Sdi12SensorInfo functions
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

//--- (generated code: YSdi12SensorInfo return codes)
//--- (end of generated code: YSdi12SensorInfo return codes)
//--- (generated code: YSdi12SensorInfo class start)
/**
 * <summary>
 *   YSdi12SensorInfo Class: Description of a discovered SDI12 sensor, returned by <c>sdi12Port.discoverSingleSensor</c> and <c>sdi12Port.
 * <para>
 *   discoverAllSensors</c> methods
 * </para>
 * <para>
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YSdi12SensorInfo
{
//--- (end of generated code: YSdi12SensorInfo class start)
//--- (generated code: YSdi12SensorInfo definitions)
    protected YSdi12Port _sdi12Port;
    protected bool _isValid;
    protected string _addr;
    protected string _proto;
    protected string _mfg;
    protected string _model;
    protected string _ver;
    protected string _sn;
    protected List<List<string>> _valuesDesc = new List<List<string>>();

    //--- (end of generated code: YSdi12SensorInfo definitions)

    public YSdi12SensorInfo(YSdi12Port sdi12Port, String json_str)
    {
        _sdi12Port = sdi12Port;
        this.imm_parseInfoStr(json_str); 
        //--- (generated code: YSdi12SensorInfo attributes initialization)
        //--- (end of generated code: YSdi12SensorInfo attributes initialization)
    }

    public virtual void _throw(int errcode,string msg)
    {
        _sdi12Port._throw(errcode, msg);
    }

    //--- (generated code: YSdi12SensorInfo implementation)
#pragma warning disable 1998

    /**
     * <summary>
     *   Returns the sensor state.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor state.
     * </returns>
     */
    public virtual async Task<bool> isValid()
    {
        return imm_isValid();
    }
    /**
     * <summary>
     *   Returns the sensor state.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor state.
     * </returns>
     */
    public virtual bool imm_isValid()
    {
        return _isValid;
    }

    /**
     * <summary>
     *   Returns the sensor address.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor address.
     * </returns>
     */
    public virtual async Task<string> get_sensorAddress()
    {
        return imm_get_sensorAddress();
    }
    /**
     * <summary>
     *   Returns the sensor address.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor address.
     * </returns>
     */
    public virtual string imm_get_sensorAddress()
    {
        return _addr;
    }

    /**
     * <summary>
     *   Returns the compatible SDI-12 version of the sensor.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the compatible SDI-12 version of the sensor.
     * </returns>
     */
    public virtual async Task<string> get_sensorProtocol()
    {
        return imm_get_sensorProtocol();
    }
    /**
     * <summary>
     *   Returns the compatible SDI-12 version of the sensor.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the compatible SDI-12 version of the sensor.
     * </returns>
     */
    public virtual string imm_get_sensorProtocol()
    {
        return _proto;
    }

    /**
     * <summary>
     *   Returns the sensor vendor identification.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor vendor identification.
     * </returns>
     */
    public virtual async Task<string> get_sensorVendor()
    {
        return imm_get_sensorVendor();
    }
    /**
     * <summary>
     *   Returns the sensor vendor identification.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor vendor identification.
     * </returns>
     */
    public virtual string imm_get_sensorVendor()
    {
        return _mfg;
    }

    /**
     * <summary>
     *   Returns the sensor model number.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor model number.
     * </returns>
     */
    public virtual async Task<string> get_sensorModel()
    {
        return imm_get_sensorModel();
    }
    /**
     * <summary>
     *   Returns the sensor model number.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor model number.
     * </returns>
     */
    public virtual string imm_get_sensorModel()
    {
        return _model;
    }

    /**
     * <summary>
     *   Returns the sensor version.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor version.
     * </returns>
     */
    public virtual async Task<string> get_sensorVersion()
    {
        return imm_get_sensorVersion();
    }
    /**
     * <summary>
     *   Returns the sensor version.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor version.
     * </returns>
     */
    public virtual string imm_get_sensorVersion()
    {
        return _ver;
    }

    /**
     * <summary>
     *   Returns the sensor serial number.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor serial number.
     * </returns>
     */
    public virtual async Task<string> get_sensorSerial()
    {
        return imm_get_sensorSerial();
    }
    /**
     * <summary>
     *   Returns the sensor serial number.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the sensor serial number.
     * </returns>
     */
    public virtual string imm_get_sensorSerial()
    {
        return _sn;
    }

    /**
     * <summary>
     *   Returns the number of sensor measurements.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <returns>
     *   the number of sensor measurements.
     * </returns>
     */
    public virtual async Task<int> get_measureCount()
    {
        return imm_get_measureCount();
    }
    /**
     * <summary>
     *   Returns the number of sensor measurements.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <returns>
     *   the number of sensor measurements.
     * </returns>
     */
    public virtual int imm_get_measureCount()
    {
        return _valuesDesc.Count;
    }

    /**
     * <summary>
     *   Returns the sensor measurement command.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual async Task<string> get_measureCommand(int measureIndex)
    {
        return imm_get_measureCommand(measureIndex);
    }
    /**
     * <summary>
     *   Returns the sensor measurement command.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string imm_get_measureCommand(int measureIndex)
    {
        if (!(measureIndex < _valuesDesc.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid measure index"); return ""; }
        return _valuesDesc[measureIndex][0];
    }

    /**
     * <summary>
     *   Returns sensor measurement position.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns 0.
     * </returns>
     */
    public virtual async Task<int> get_measurePosition(int measureIndex)
    {
        return imm_get_measurePosition(measureIndex);
    }
    /**
     * <summary>
     *   Returns sensor measurement position.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns 0.
     * </returns>
     */
    public virtual int imm_get_measurePosition(int measureIndex)
    {
        if (!(measureIndex < _valuesDesc.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid measure index"); return 0; }
        return YAPIContext.imm_atoi(_valuesDesc[measureIndex][2]);
    }

    /**
     * <summary>
     *   Returns the measured value symbol.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual async Task<string> get_measureSymbol(int measureIndex)
    {
        return imm_get_measureSymbol(measureIndex);
    }
    /**
     * <summary>
     *   Returns the measured value symbol.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string imm_get_measureSymbol(int measureIndex)
    {
        if (!(measureIndex < _valuesDesc.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid measure index"); return ""; }
        return _valuesDesc[measureIndex][3];
    }

    /**
     * <summary>
     *   Returns the unit of the measured value.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual async Task<string> get_measureUnit(int measureIndex)
    {
        return imm_get_measureUnit(measureIndex);
    }
    /**
     * <summary>
     *   Returns the unit of the measured value.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string imm_get_measureUnit(int measureIndex)
    {
        if (!(measureIndex < _valuesDesc.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid measure index"); return ""; }
        return _valuesDesc[measureIndex][4];
    }

    /**
     * <summary>
     *   Returns the description of the measured value.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual async Task<string> get_measureDescription(int measureIndex)
    {
        return imm_get_measureDescription(measureIndex);
    }
    /**
     * <summary>
     *   Returns the description of the measured value.
     * <para>
     *   This function only works if the sensor is in version 1.4 SDI-12
     *   and supports metadata commands.
     * </para>
     * </summary>
     * <param name="measureIndex">
     *   measurement index
     * </param>
     * <returns>
     *   the sensor measurement command.
     *   On failure, throws an exception or returns an empty string.
     * </returns>
     */
    public virtual string imm_get_measureDescription(int measureIndex)
    {
        if (!(measureIndex < _valuesDesc.Count)) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid measure index"); return ""; }
        return _valuesDesc[measureIndex][5];
    }

    public virtual async Task<List<List<string>>> get_typeMeasure()
    {
        return imm_get_typeMeasure();
    }
    public virtual List<List<string>> imm_get_typeMeasure()
    {
        return _valuesDesc;
    }

    public virtual void imm_parseInfoStr(string infoStr)
    {
        string errmsg;

        if ((infoStr).Length > 1) {
            if ((infoStr).Substring( 0, 2) == "ER") {
                errmsg = (infoStr).Substring( 2, (infoStr).Length-2);
                _addr = errmsg;
                _proto = errmsg;
                _mfg = errmsg;
                _model = errmsg;
                _ver = errmsg;
                _sn = errmsg;
                _isValid = false;
            } else {
                _addr = (infoStr).Substring( 0, 1);
                _proto = (infoStr).Substring( 1, 2);
                _mfg = (infoStr).Substring( 3, 8);
                _model = (infoStr).Substring( 11, 6);
                _ver = (infoStr).Substring( 17, 3);
                _sn = (infoStr).Substring( 20, (infoStr).Length-20);
                _isValid = true;
            }
        }
    }

    public virtual async Task _queryValueInfo()
    {
        List<List<string>> val = new List<List<string>>();
        List<string> data = new List<string>();
        string infoNbVal;
        string cmd;
        string infoVal;
        string value;
        int nbVal;
        int k;
        int i;
        int j;
        List<string> listVal = new List<string>();
        int size;

        k = 0;
        size = 4;
        while (k < 10) {
            infoNbVal = await _sdi12Port.querySdi12(_addr,  "IM"+Convert.ToString(k), 5000);
            if ((infoNbVal).Length > 1) {
                value = (infoNbVal).Substring( 4, (infoNbVal).Length-4);
                nbVal = YAPIContext.imm_atoi(value);
                if (nbVal != 0) {
                    val.Clear();
                    i = 0;
                    while (i < nbVal) {
                        cmd = "IM"+Convert.ToString( k)+"_00"+Convert.ToString(i+1);
                        infoVal = await _sdi12Port.querySdi12(_addr,  cmd, 5000);
                        data = new List<string>(infoVal.Split(new char[] {';'}));
                        data = new List<string>(data[0].Split(new char[] {','}));
                        listVal.Clear();
                        listVal.Add("M"+Convert.ToString(k));
                        listVal.Add((i+1).ToString());
                        j = 0;
                        while (data.Count < size) {
                            data.Add("");
                        }
                        while (j < data.Count) {
                            listVal.Add(data[j]);
                            j = j + 1;
                        }
                        val.Add(new List<string>(listVal));
                        i = i + 1;
                    }
                }
            }
            k = k + 1;
        }
        _valuesDesc = val;
    }

#pragma warning restore 1998
    //--- (end of generated code: YSdi12SensorInfo implementation)
}
}

