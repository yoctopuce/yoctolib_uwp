/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindYInputCaptureData(), the high-level API for YInputCaptureData functions
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
    //--- (generated code: YInputCaptureData return codes)
//--- (end of generated code: YInputCaptureData return codes)
    //--- (generated code: YInputCaptureData class start)
/**
 * <summary>
 *   YInputCaptureData Class: Sampled data from a Yoctopuce electrical sensor
 * <para>
 *   <c>InputCaptureData</c> objects represent raw data
 *   sampled by the analog/digital converter present in
 *   a Yoctopuce electrical sensor. When several inputs
 *   are samples simultaneously, their data are provided
 *   as distinct series.
 * </para>
 * </summary>
 */
public class YInputCaptureData
{
//--- (end of generated code: YInputCaptureData class start)

    protected YAPIContext _yapi;

    //--- (generated code: YInputCaptureData definitions)
    protected int _fmt = 0;
    protected int _var1size = 0;
    protected int _var2size = 0;
    protected int _var3size = 0;
    protected int _nVars = 0;
    protected int _recOfs = 0;
    protected int _nRecs = 0;
    protected int _samplesPerSec = 0;
    protected int _trigType = 0;
    protected double _trigVal = 0;
    protected int _trigPos = 0;
    protected double _trigUTC = 0;
    protected string _var1unit;
    protected string _var2unit;
    protected string _var3unit;
    protected List<double> _var1samples = new List<double>();
    protected List<double> _var2samples = new List<double>();
    protected List<double> _var3samples = new List<double>();

    //--- (end of generated code: YInputCaptureData definitions)

    internal YInputCaptureData(YFunction yfun, byte[] sdata)
    {
        this._yapi = yfun._yapi;
        this.imm_decodeSnapBin(sdata);
    }
    public int _throw(int errType, string errMsg)
    {
        if (!(_yapi._exceptionsDisabled))
        {
            throw new YAPI_Exception(errType, "YoctoApi error : " + errMsg);
        }
        return errType;
    }

    //--- (generated code: YInputCaptureData implementation)
#pragma warning disable 1998

    public virtual int imm_decodeU16(byte[] sdata,int ofs)
    {
        int v;
        v = sdata[ofs];
        v = v + 256 * sdata[ofs+1];
        return v;
    }

    public virtual double imm_decodeU32(byte[] sdata,int ofs)
    {
        double v;
        v = this.imm_decodeU16(sdata, ofs);
        v = v + 65536.0 * this.imm_decodeU16(sdata, ofs+2);
        return v;
    }

    public virtual double imm_decodeVal(byte[] sdata,int ofs,int len)
    {
        double v;
        double b;
        v = this.imm_decodeU16(sdata, ofs);
        b = 65536.0;
        ofs = ofs + 2;
        len = len - 2;
        while (len > 0) {
            v = v + b * sdata[ofs];
            b = b * 256;
            ofs = ofs + 1;
            len = len - 1;
        }
        if (v > (b/2)) {
            // negative number
            v = v - b;
        }
        return v;
    }

    public virtual int imm_decodeSnapBin(byte[] sdata)
    {
        int buffSize;
        int recOfs;
        int ms;
        int recSize;
        int count;
        int mult1;
        int mult2;
        int mult3;
        double v;

        buffSize = (sdata).Length;
        if (!(buffSize >= 24)) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid snapshot data (too short)"); return YAPI.INVALID_ARGUMENT; }
        _fmt = sdata[0];
        _var1size = sdata[1] - 48;
        _var2size = sdata[2] - 48;
        _var3size = sdata[3] - 48;
        if (!(_fmt == 83)) { this._throw( YAPI.INVALID_ARGUMENT, "Unsupported snapshot format"); return YAPI.INVALID_ARGUMENT; }
        if (!((_var1size >= 2) && (_var1size <= 4))) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid sample size"); return YAPI.INVALID_ARGUMENT; }
        if (!((_var2size >= 0) && (_var1size <= 4))) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid sample size"); return YAPI.INVALID_ARGUMENT; }
        if (!((_var3size >= 0) && (_var1size <= 4))) { this._throw( YAPI.INVALID_ARGUMENT, "Invalid sample size"); return YAPI.INVALID_ARGUMENT; }
        if (_var2size == 0) {
            _nVars = 1;
        } else {
            if (_var3size == 0) {
                _nVars = 2;
            } else {
                _nVars = 3;
            }
        }
        recSize = _var1size + _var2size + _var3size;
        _recOfs = this.imm_decodeU16(sdata, 4);
        _nRecs = this.imm_decodeU16(sdata, 6);
        _samplesPerSec = this.imm_decodeU16(sdata, 8);
        _trigType = this.imm_decodeU16(sdata, 10);
        _trigVal = this.imm_decodeVal(sdata,  12, 4) / 1000;
        _trigPos = this.imm_decodeU16(sdata, 16);
        ms = this.imm_decodeU16(sdata, 18);
        _trigUTC = this.imm_decodeVal(sdata,  20, 4);
        _trigUTC = _trigUTC + (ms / 1000.0);
        recOfs = 24;
        while (sdata[recOfs] >= 32) {
            _var1unit = ""+ _var1unit+""+((char)(sdata[recOfs])).ToString();
            recOfs = recOfs + 1;
        }
        if (_var2size > 0) {
            recOfs = recOfs + 1;
            while (sdata[recOfs] >= 32) {
                _var2unit = ""+ _var2unit+""+((char)(sdata[recOfs])).ToString();
                recOfs = recOfs + 1;
            }
        }
        if (_var3size > 0) {
            recOfs = recOfs + 1;
            while (sdata[recOfs] >= 32) {
                _var3unit = ""+ _var3unit+""+((char)(sdata[recOfs])).ToString();
                recOfs = recOfs + 1;
            }
        }
        if (((recOfs) & (1)) == 1) {
            // align to next word
            recOfs = recOfs + 1;
        }
        mult1 = 1;
        mult2 = 1;
        mult3 = 1;
        if (recOfs < _recOfs) {
            // load optional value multiplier
            mult1 = this.imm_decodeU16(sdata, recOfs);
            recOfs = recOfs + 2;
            if (_var2size > 0) {
                mult2 = this.imm_decodeU16(sdata, recOfs);
                recOfs = recOfs + 2;
            }
            if (_var3size > 0) {
                mult3 = this.imm_decodeU16(sdata, recOfs);
                recOfs = recOfs + 2;
            }
        }
        recOfs = _recOfs;
        count = _nRecs;
        while ((count > 0) && (recOfs + _var1size <= buffSize)) {
            v = this.imm_decodeVal(sdata,  recOfs, _var1size) / 1000.0;
            _var1samples.Add(v*mult1);
            recOfs = recOfs + recSize;
        }
        if (_var2size > 0) {
            recOfs = _recOfs + _var1size;
            count = _nRecs;
            while ((count > 0) && (recOfs + _var2size <= buffSize)) {
                v = this.imm_decodeVal(sdata,  recOfs, _var2size) / 1000.0;
                _var2samples.Add(v*mult2);
                recOfs = recOfs + recSize;
            }
        }
        if (_var3size > 0) {
            recOfs = _recOfs + _var1size + _var2size;
            count = _nRecs;
            while ((count > 0) && (recOfs + _var3size <= buffSize)) {
                v = this.imm_decodeVal(sdata,  recOfs, _var3size) / 1000.0;
                _var3samples.Add(v*mult3);
                recOfs = recOfs + recSize;
            }
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the number of series available in the capture.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   simultaneous data series available.
     * </returns>
     */
    public virtual async Task<int> get_serieCount()
    {
        return imm_get_serieCount();
    }
    /**
     * <summary>
     *   Returns the number of series available in the capture.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   simultaneous data series available.
     * </returns>
     */
    public virtual int imm_get_serieCount()
    {
        return _nVars;
    }

    /**
     * <summary>
     *   Returns the number of records captured (in a serie).
     * <para>
     *   In the exceptional case where it was not possible
     *   to transfer all data in time, the number of records
     *   actually present in the series might be lower than
     *   the number of records captured
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   records expected in each serie.
     * </returns>
     */
    public virtual async Task<int> get_recordCount()
    {
        return imm_get_recordCount();
    }
    /**
     * <summary>
     *   Returns the number of records captured (in a serie).
     * <para>
     *   In the exceptional case where it was not possible
     *   to transfer all data in time, the number of records
     *   actually present in the series might be lower than
     *   the number of records captured
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   records expected in each serie.
     * </returns>
     */
    public virtual int imm_get_recordCount()
    {
        return _nRecs;
    }

    /**
     * <summary>
     *   Returns the effective sampling rate of the device.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   samples taken each second.
     * </returns>
     */
    public virtual async Task<int> get_samplingRate()
    {
        return imm_get_samplingRate();
    }
    /**
     * <summary>
     *   Returns the effective sampling rate of the device.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of
     *   samples taken each second.
     * </returns>
     */
    public virtual int imm_get_samplingRate()
    {
        return _samplesPerSec;
    }

    /**
     * <summary>
     *   Returns the type of automatic conditional capture
     *   that triggered the capture of this data sequence.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the type of conditional capture.
     * </returns>
     */
    public virtual async Task<int> get_captureType()
    {
        return imm_get_captureType();
    }
    /**
     * <summary>
     *   Returns the type of automatic conditional capture
     *   that triggered the capture of this data sequence.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the type of conditional capture.
     * </returns>
     */
    public virtual int imm_get_captureType()
    {
        return (int) _trigType;
    }

    /**
     * <summary>
     *   Returns the threshold value that triggered
     *   this automatic conditional capture, if it was
     *   not an instant captured triggered manually.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the conditional threshold value
     *   at the time of capture.
     * </returns>
     */
    public virtual async Task<double> get_triggerValue()
    {
        return imm_get_triggerValue();
    }
    /**
     * <summary>
     *   Returns the threshold value that triggered
     *   this automatic conditional capture, if it was
     *   not an instant captured triggered manually.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the conditional threshold value
     *   at the time of capture.
     * </returns>
     */
    public virtual double imm_get_triggerValue()
    {
        return _trigVal;
    }

    /**
     * <summary>
     *   Returns the index in the series of the sample
     *   corresponding to the exact time when the capture
     *   was triggered.
     * <para>
     *   In case of trigger based on average
     *   or RMS value, the trigger index corresponds to
     *   the end of the averaging period.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to a position
     *   in the data serie.
     * </returns>
     */
    public virtual async Task<int> get_triggerPosition()
    {
        return imm_get_triggerPosition();
    }
    /**
     * <summary>
     *   Returns the index in the series of the sample
     *   corresponding to the exact time when the capture
     *   was triggered.
     * <para>
     *   In case of trigger based on average
     *   or RMS value, the trigger index corresponds to
     *   the end of the averaging period.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to a position
     *   in the data serie.
     * </returns>
     */
    public virtual int imm_get_triggerPosition()
    {
        return _trigPos;
    }

    /**
     * <summary>
     *   Returns the absolute time when the capture was
     *   triggered, as a Unix timestamp.
     * <para>
     *   Milliseconds are
     *   included in this timestamp (floating-point number).
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to
     *   the number of seconds between the Jan 1,
     *   1970 and the moment where the capture
     *   was triggered.
     * </returns>
     */
    public virtual async Task<double> get_triggerRealTimeUTC()
    {
        return imm_get_triggerRealTimeUTC();
    }
    /**
     * <summary>
     *   Returns the absolute time when the capture was
     *   triggered, as a Unix timestamp.
     * <para>
     *   Milliseconds are
     *   included in this timestamp (floating-point number).
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to
     *   the number of seconds between the Jan 1,
     *   1970 and the moment where the capture
     *   was triggered.
     * </returns>
     */
    public virtual double imm_get_triggerRealTimeUTC()
    {
        return _trigUTC;
    }

    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the first serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual async Task<string> get_serie1Unit()
    {
        return imm_get_serie1Unit();
    }
    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the first serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual string imm_get_serie1Unit()
    {
        return _var1unit;
    }

    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the second serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual async Task<string> get_serie2Unit()
    {
        return imm_get_serie2Unit();
    }
    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the second serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual string imm_get_serie2Unit()
    {
        if (!(_nVars >= 2)) { this._throw( YAPI.INVALID_ARGUMENT, "There is no serie 2 in this capture data"); return ""; }
        return _var2unit;
    }

    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the third serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual async Task<string> get_serie3Unit()
    {
        return imm_get_serie3Unit();
    }
    /**
     * <summary>
     *   Returns the unit of measurement for data points in
     *   the third serie.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string containing to a physical unit of
     *   measurement.
     * </returns>
     */
    public virtual string imm_get_serie3Unit()
    {
        if (!(_nVars >= 3)) { this._throw( YAPI.INVALID_ARGUMENT, "There is no serie 3 in this capture data"); return ""; }
        return _var3unit;
    }

    /**
     * <summary>
     *   Returns the sampled data corresponding to the first serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie1Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 1.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<double>> get_serie1Values()
    {
        return imm_get_serie1Values();
    }
    /**
     * <summary>
     *   Returns the sampled data corresponding to the first serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie1Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 1.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<double> imm_get_serie1Values()
    {
        return _var1samples;
    }

    /**
     * <summary>
     *   Returns the sampled data corresponding to the second serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie2Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 2.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<double>> get_serie2Values()
    {
        return imm_get_serie2Values();
    }
    /**
     * <summary>
     *   Returns the sampled data corresponding to the second serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie2Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 2.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<double> imm_get_serie2Values()
    {
        if (!(_nVars >= 2)) { this._throw( YAPI.INVALID_ARGUMENT, "There is no serie 2 in this capture data"); return _var2samples; }
        return _var2samples;
    }

    /**
     * <summary>
     *   Returns the sampled data corresponding to the third serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie3Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 3.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<double>> get_serie3Values()
    {
        return imm_get_serie3Values();
    }
    /**
     * <summary>
     *   Returns the sampled data corresponding to the third serie.
     * <para>
     *   The corresponding physical unit can be obtained
     *   using the method <c>get_serie3Unit()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of real numbers corresponding to all
     *   samples received for serie 3.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual List<double> imm_get_serie3Values()
    {
        if (!(_nVars >= 3)) { this._throw( YAPI.INVALID_ARGUMENT, "There is no serie 3 in this capture data"); return _var3samples; }
        return _var3samples;
    }

#pragma warning restore 1998
    //--- (end of generated code: YInputCaptureData implementation)
    }

}