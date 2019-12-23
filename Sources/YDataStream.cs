using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
///*******************************************************************
/// 
/// $Id: YDataStream.cs 38899 2019-12-20 17:21:03Z mvuilleu $
/// 
/// YDataStream Class: Sequence of measured data, stored by the data logger
/// 
/// - - - - - - - - - License information: - - - - - - - - -
/// 
///  Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
/// 
///  Yoctopuce Sarl (hereafter Licensor) grants to you a perpetual
///  non-exclusive license to use, modify, copy and integrate this
///  file into your software for the sole purpose of interfacing
///  with Yoctopuce products.
/// 
///  You may reproduce and distribute copies of this file in
///  source or object form, as long as the sole purpose of this
///  code is to interface with Yoctopuce products. You must retain
///  this notice in the distributed source file.
/// 
///  You should refer to Yoctopuce General Terms and Conditions
///  for additional information regarding your rights and
///  obligations.
/// 
///  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
///  WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
///  WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS
///  FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
///  EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
///  INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA,
///  COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR
///  SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT
///  LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
///  CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
///  BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
///  WARRANTY, OR OTHERWISE.
/// 
/// ********************************************************************
/// </summary>

namespace com.yoctopuce.YoctoAPI {

    //--- (generated code: YDataStream class start)
/**
 * <summary>
 *   YDataStream Class: Unformatted data sequence
 * <para>
 *   <c>DataStream</c> objects represent bare recorded measure sequences,
 *   exactly as found within the data logger present on Yoctopuce
 *   sensors.
 * </para>
 * <para>
 *   In most cases, it is not necessary to use <c>DataStream</c> objects
 *   directly, as the <c>DataSet</c> objects (returned by the
 *   <c>get_recordedData()</c> method from sensors and the
 *   <c>get_dataSets()</c> method from the data logger) provide
 *   a more convenient interface.
 * </para>
 * </summary>
 */
public class YDataStream
{
//--- (end of generated code: YDataStream class start)
        public const double DATA_INVALID = YAPI.INVALID_DOUBLE;

        //--- (generated code: YDataStream definitions)
    protected YFunction _parent;
    protected int _runNo = 0;
    protected long _utcStamp = 0;
    protected int _nCols = 0;
    protected int _nRows = 0;
    protected double _startTime = 0;
    protected double _duration = 0;
    protected double _dataSamplesInterval = 0;
    protected double _firstMeasureDuration = 0;
    protected List<string> _columnNames = new List<string>();
    protected string _functionId;
    protected bool _isClosed;
    protected bool _isAvg;
    protected double _minVal = 0;
    protected double _avgVal = 0;
    protected double _maxVal = 0;
    protected int _caltyp = 0;
    protected List<int> _calpar = new List<int>();
    protected List<double> _calraw = new List<double>();
    protected List<double> _calref = new List<double>();
    protected List<List<double>> _values = new List<List<double>>();

    //--- (end of generated code: YDataStream definitions)
        protected internal YAPI.CalibrationHandler imm_calhdl = null;

        protected internal YDataStream(YFunction parent) {
            _parent = parent;
        }

        internal YDataStream(YFunction parent, YDataSet dataset, List<int> encoded) {
            _parent = parent;
            imm_initFromDataSet(dataset, encoded);
        }

        //--- (generated code: YDataStream implementation)
#pragma warning disable 1998

    public virtual int imm_initFromDataSet(YDataSet dataset,List<int> encoded)
    {
        int val;
        int i;
        int maxpos;
        int ms_offset;
        int samplesPerHour;
        double fRaw;
        double fRef;
        List<int> iCalib = new List<int>();
        // decode sequence header to extract data
        _runNo = encoded[0] + (((encoded[1]) << (16)));
        _utcStamp = encoded[2] + (((encoded[3]) << (16)));
        val = encoded[4];
        _isAvg = (((val) & (0x100)) == 0);
        samplesPerHour = ((val) & (0xff));
        if (((val) & (0x100)) != 0) {
            samplesPerHour = samplesPerHour * 3600;
        } else {
            if (((val) & (0x200)) != 0) {
                samplesPerHour = samplesPerHour * 60;
            }
        }
        _dataSamplesInterval = 3600.0 / samplesPerHour;
        ms_offset = encoded[6];
        if (ms_offset < 1000) {
            // new encoding -> add the ms to the UTC timestamp
            _startTime = _utcStamp + (ms_offset / 1000.0);
        } else {
            // legacy encoding subtract the measure interval form the UTC timestamp
            _startTime = _utcStamp -  _dataSamplesInterval;
        }
        _firstMeasureDuration = encoded[5];
        if (!(_isAvg)) {
            _firstMeasureDuration = _firstMeasureDuration / 1000.0;
        }
        val = encoded[7];
        _isClosed = (val != 0xffff);
        if (val == 0xffff) {
            val = 0;
        }
        _nRows = val;
        if (_nRows > 0) {
            if (_firstMeasureDuration > 0) {
                _duration = _firstMeasureDuration + (_nRows - 1) * _dataSamplesInterval;
            } else {
                _duration = _nRows * _dataSamplesInterval;
            }
        } else {
            _duration = 0;
        }
        // precompute decoding parameters
        iCalib = dataset.imm_get_calibration();
        _caltyp = iCalib[0];
        if (_caltyp != 0) {
            imm_calhdl = _parent._yapi.imm_getCalibrationHandler(_caltyp);
            maxpos = iCalib.Count;
            _calpar.Clear();
            _calraw.Clear();
            _calref.Clear();
            i = 1;
            while (i < maxpos) {
                _calpar.Add(iCalib[i]);
                i = i + 1;
            }
            i = 1;
            while (i + 1 < maxpos) {
                fRaw = iCalib[i];
                fRaw = fRaw / 1000.0;
                fRef = iCalib[i + 1];
                fRef = fRef / 1000.0;
                _calraw.Add(fRaw);
                _calref.Add(fRef);
                i = i + 2;
            }
        }
        // preload column names for backward-compatibility
        _functionId = dataset.imm_get_functionId();
        if (_isAvg) {
            _columnNames.Clear();
            _columnNames.Add(""+_functionId+"_min");
            _columnNames.Add(""+_functionId+"_avg");
            _columnNames.Add(""+_functionId+"_max");
            _nCols = 3;
        } else {
            _columnNames.Clear();
            _columnNames.Add(_functionId);
            _nCols = 1;
        }
        // decode min/avg/max values for the sequence
        if (_nRows > 0) {
            _avgVal = this.imm_decodeAvg(encoded[8] + (((((encoded[9]) ^ (0x8000))) << (16))), 1);
            _minVal = this.imm_decodeVal(encoded[10] + (((encoded[11]) << (16))));
            _maxVal = this.imm_decodeVal(encoded[12] + (((encoded[13]) << (16))));
        }
        return 0;
    }

    public virtual int imm_parseStream(byte[] sdata)
    {
        int idx;
        List<int> udat = new List<int>();
        List<double> dat = new List<double>();
        if ((sdata).Length == 0) {
            _nRows = 0;
            return YAPI.SUCCESS;
        }

        udat = YAPIContext.imm_decodeWords(_parent.imm_json_get_string(sdata));
        _values.Clear();
        idx = 0;
        if (_isAvg) {
            while (idx + 3 < udat.Count) {
                dat.Clear();
                dat.Add(this.imm_decodeVal(udat[idx + 2] + (((udat[idx + 3]) << (16)))));
                dat.Add(this.imm_decodeAvg(udat[idx] + (((((udat[idx + 1]) ^ (0x8000))) << (16))), 1));
                dat.Add(this.imm_decodeVal(udat[idx + 4] + (((udat[idx + 5]) << (16)))));
                idx = idx + 6;
                _values.Add(new List<double>(dat));
            }
        } else {
            while (idx + 1 < udat.Count) {
                dat.Clear();
                dat.Add(this.imm_decodeAvg(udat[idx] + (((((udat[idx + 1]) ^ (0x8000))) << (16))), 1));
                _values.Add(new List<double>(dat));
                idx = idx + 2;
            }
        }

        _nRows = _values.Count;
        return YAPI.SUCCESS;
    }

    public virtual string imm_get_url()
    {
        string url;
        url = "logger.json?id="+
        _functionId+"&run="+Convert.ToString(_runNo)+"&utc="+Convert.ToString(_utcStamp);
        return url;
    }

    public virtual async Task<int> loadStream()
    {
        return this.imm_parseStream(await _parent._download(this.imm_get_url()));
    }

    public virtual double imm_decodeVal(int w)
    {
        double val;
        val = w;
        val = val / 1000.0;
        if (_caltyp != 0) {
            if (imm_calhdl != null) {
                val = imm_calhdl(val, _caltyp, _calpar, _calraw, _calref);
            }
        }
        return val;
    }

    public virtual double imm_decodeAvg(int dw,int count)
    {
        double val;
        val = dw;
        val = val / 1000.0;
        if (_caltyp != 0) {
            if (imm_calhdl != null) {
                val = imm_calhdl(val, _caltyp, _calpar, _calraw, _calref);
            }
        }
        return val;
    }

    public virtual async Task<bool> isClosed()
    {
        return _isClosed;
    }

    /**
     * <summary>
     *   Returns the run index of the data stream.
     * <para>
     *   A run can be made of
     *   multiple datastreams, for different time intervals.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the run index.
     * </returns>
     */
    public virtual async Task<int> get_runIndex()
    {
        return _runNo;
    }

    /**
     * <summary>
     *   Returns the relative start time of the data stream, measured in seconds.
     * <para>
     *   For recent firmwares, the value is relative to the present time,
     *   which means the value is always negative.
     *   If the device uses a firmware older than version 13000, value is
     *   relative to the start of the time the device was powered on, and
     *   is always positive.
     *   If you need an absolute UTC timestamp, use <c>get_realStartTimeUTC()</c>.
     * </para>
     * <para>
     *   <b>DEPRECATED</b>: This method has been replaced by <c>get_realStartTimeUTC()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the start of the run and the beginning of this data
     *   stream.
     * </returns>
     */
    public virtual async Task<int> get_startTime()
    {
        return (int)(_utcStamp - Convert.ToUInt32((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds));
    }

    /**
     * <summary>
     *   Returns the start time of the data stream, relative to the Jan 1, 1970.
     * <para>
     *   If the UTC time was not set in the datalogger at the time of the recording
     *   of this data stream, this method returns 0.
     * </para>
     * <para>
     *   <b>DEPRECATED</b>: This method has been replaced by <c>get_realStartTimeUTC()</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the Jan 1, 1970 and the beginning of this data
     *   stream (i.e. Unix time representation of the absolute time).
     * </returns>
     */
    public virtual async Task<long> get_startTimeUTC()
    {
        return (int) Math.Round(_startTime);
    }

    /**
     * <summary>
     *   Returns the start time of the data stream, relative to the Jan 1, 1970.
     * <para>
     *   If the UTC time was not set in the datalogger at the time of the recording
     *   of this data stream, this method returns 0.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number  corresponding to the number of seconds
     *   between the Jan 1, 1970 and the beginning of this data
     *   stream (i.e. Unix time representation of the absolute time).
     * </returns>
     */
    public virtual async Task<double> get_realStartTimeUTC()
    {
        return _startTime;
    }

    /**
     * <summary>
     *   Returns the number of milliseconds between two consecutive
     *   rows of this data stream.
     * <para>
     *   By default, the data logger records one row
     *   per second, but the recording frequency can be changed for
     *   each device function
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to a number of milliseconds.
     * </returns>
     */
    public virtual async Task<int> get_dataSamplesIntervalMs()
    {
        return (int) Math.Round(_dataSamplesInterval*1000);
    }

    public virtual async Task<double> get_dataSamplesInterval()
    {
        return _dataSamplesInterval;
    }

    public virtual async Task<double> get_firstDataSamplesInterval()
    {
        return _firstMeasureDuration;
    }

    /**
     * <summary>
     *   Returns the number of data rows present in this stream.
     * <para>
     * </para>
     * <para>
     *   If the device uses a firmware older than version 13000,
     *   this method fetches the whole data stream from the device
     *   if not yet done, which can cause a little delay.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of rows.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public virtual async Task<int> get_rowCount()
    {
        if ((_nRows != 0) && _isClosed) {
            return _nRows;
        }
        await this.loadStream();
        return _nRows;
    }

    /**
     * <summary>
     *   Returns the number of data columns present in this stream.
     * <para>
     *   The meaning of the values present in each column can be obtained
     *   using the method <c>get_columnNames()</c>.
     * </para>
     * <para>
     *   If the device uses a firmware older than version 13000,
     *   this method fetches the whole data stream from the device
     *   if not yet done, which can cause a little delay.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of columns.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns zero.
     * </para>
     */
    public virtual async Task<int> get_columnCount()
    {
        if (_nCols != 0) {
            return _nCols;
        }
        await this.loadStream();
        return _nCols;
    }

    /**
     * <summary>
     *   Returns the title (or meaning) of each data column present in this stream.
     * <para>
     *   In most case, the title of the data column is the hardware identifier
     *   of the sensor that produced the data. For streams recorded at a lower
     *   recording rate, the dataLogger stores the min, average and max value
     *   during each measure interval into three columns with suffixes _min,
     *   _avg and _max respectively.
     * </para>
     * <para>
     *   If the device uses a firmware older than version 13000,
     *   this method fetches the whole data stream from the device
     *   if not yet done, which can cause a little delay.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list containing as many strings as there are columns in the
     *   data stream.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<string>> get_columnNames()
    {
        if (_columnNames.Count != 0) {
            return _columnNames;
        }
        await this.loadStream();
        return _columnNames;
    }

    /**
     * <summary>
     *   Returns the smallest measure observed within this stream.
     * <para>
     *   If the device uses a firmware older than version 13000,
     *   this method will always return YDataStream.DATA_INVALID.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the smallest value,
     *   or YDataStream.DATA_INVALID if the stream is not yet complete (still recording).
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDataStream.DATA_INVALID.
     * </para>
     */
    public virtual async Task<double> get_minValue()
    {
        return _minVal;
    }

    /**
     * <summary>
     *   Returns the average of all measures observed within this stream.
     * <para>
     *   If the device uses a firmware older than version 13000,
     *   this method will always return YDataStream.DATA_INVALID.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the average value,
     *   or YDataStream.DATA_INVALID if the stream is not yet complete (still recording).
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDataStream.DATA_INVALID.
     * </para>
     */
    public virtual async Task<double> get_averageValue()
    {
        return _avgVal;
    }

    /**
     * <summary>
     *   Returns the largest measure observed within this stream.
     * <para>
     *   If the device uses a firmware older than version 13000,
     *   this method will always return YDataStream.DATA_INVALID.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the largest value,
     *   or YDataStream.DATA_INVALID if the stream is not yet complete (still recording).
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDataStream.DATA_INVALID.
     * </para>
     */
    public virtual async Task<double> get_maxValue()
    {
        return _maxVal;
    }

    public virtual async Task<double> get_realDuration()
    {
        if (_isClosed) {
            return _duration;
        }
        return (double) Convert.ToUInt32((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) - _utcStamp;
    }

    /**
     * <summary>
     *   Returns the whole data set contained in the stream, as a bidimensional
     *   table of numbers.
     * <para>
     *   The meaning of the values present in each column can be obtained
     *   using the method <c>get_columnNames()</c>.
     * </para>
     * <para>
     *   This method fetches the whole data stream from the device,
     *   if not yet done.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list containing as many elements as there are rows in the
     *   data stream. Each row itself is a list of floating-point
     *   numbers.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<List<double>>> get_dataRows()
    {
        if ((_values.Count == 0) || !(_isClosed)) {
            await this.loadStream();
        }
        return _values;
    }

    /**
     * <summary>
     *   Returns a single measure from the data stream, specified by its
     *   row and column index.
     * <para>
     *   The meaning of the values present in each column can be obtained
     *   using the method get_columnNames().
     * </para>
     * <para>
     *   This method fetches the whole data stream from the device,
     *   if not yet done.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="row">
     *   row index
     * </param>
     * <param name="col">
     *   column index
     * </param>
     * <returns>
     *   a floating-point number
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDataStream.DATA_INVALID.
     * </para>
     */
    public virtual async Task<double> get_data(int row,int col)
    {
        if ((_values.Count == 0) || !(_isClosed)) {
            await this.loadStream();
        }
        if (row >= _values.Count) {
            return DATA_INVALID;
        }
        if (col >= _values[row].Count) {
            return DATA_INVALID;
        }
        return _values[row][col];
    }

#pragma warning restore 1998
    //--- (end of generated code: YDataStream implementation)
     }

}