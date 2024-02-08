/*********************************************************************
 *
 * $Id: YDataSet.cs 56045 2023-08-14 15:51:05Z seb $
 *
 * Implements yFindDataSet(), the high-level API for DataSet functions
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
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED "AS IS" WITHOUT
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


    //--- (generated code: YDataSet class start)
/**
 * <summary>
 *   Y
 * <para>
 *   DataSet Class: Recorded data sequence, as returned by <c>sensor.get_recordedData()</c>
 * </para>
 * <para>
 *   <c>YDataSet</c> objects make it possible to retrieve a set of recorded measures
 *   for a given sensor and a specified time interval. They can be used
 *   to load data points with a progress report. When the <c>YDataSet</c> object is
 *   instantiated by the <c>sensor.get_recordedData()</c>  function, no data is
 *   yet loaded from the module. It is only when the <c>loadMore()</c>
 *   method is called over and over than data will be effectively loaded
 *   from the dataLogger.
 * </para>
 * <para>
 *   A preview of available measures is available using the function
 *   <c>get_preview()</c> as soon as <c>loadMore()</c> has been called
 *   once. Measures themselves are available using function <c>get_measures()</c>
 *   when loaded by subsequent calls to <c>loadMore()</c>.
 * </para>
 * <para>
 *   This class can only be used on devices that use a relatively recent firmware,
 *   as <c>YDataSet</c> objects are not supported by firmwares older than version 13000.
 * </para>
 * </summary>
 */
public class YDataSet
{
//--- (end of generated code: YDataSet class start)

        //--- (generated code: YDataSet definitions)
    protected YFunction _parent;
    protected string _hardwareId;
    protected string _functionId;
    protected string _unit;
    protected int _bulkLoad = 0;
    protected double _startTimeMs = 0;
    protected double _endTimeMs = 0;
    protected int _progress = 0;
    protected List<int> _calib = new List<int>();
    protected List<YDataStream> _streams = new List<YDataStream>();
    protected YMeasure _summary;
    protected List<YMeasure> _preview = new List<YMeasure>();
    protected List<YMeasure> _measures = new List<YMeasure>();
    protected double _summaryMinVal = 0;
    protected double _summaryMaxVal = 0;
    protected double _summaryTotalAvg = 0;
    protected double _summaryTotalTime = 0;

    //--- (end of generated code: YDataSet definitions)

        // YDataSet constructor, when instantiated directly by a function
        public YDataSet(YFunction parent, string functionId, string unit, double startTime, double endTime)
        {
            _parent = parent;
            _functionId = functionId;
            _unit = unit;
            _startTimeMs = startTime * 1000;
            _endTimeMs = endTime * 1000;
            _progress = -1;
            _hardwareId = "";
            _summary = new YMeasure();
        }

        // YDataSet constructor for the new datalogger
        public YDataSet(YFunction parent)
        {
            _parent = parent;
            _startTimeMs = 0;
            _endTimeMs = 0;
            _hardwareId = "";
            _summary = new YMeasure();
        }

        // YDataSet parser for stream list
        protected internal virtual async Task<int> _parse(string json_str)
        {
            YJSONObject json;
            YJSONArray jstreams;
            double streamStartTime;
            double streamEndTime;
            try {
                json = new YJSONObject(json_str);
                json.parse();
                _functionId = json.getString("id");
                _unit = json.getString("unit");
                if (json.has("bulk")) {
                    _bulkLoad = YAPIContext.imm_atoi(json.getString("bulk"));
                }
                if (json.has("calib")) {
                    _calib = YAPIContext.imm_decodeFloats(json.getString("calib"));
                    _calib[0] = _calib[0] / 1000;
                } else {
                    _calib = YAPIContext.imm_decodeWords(json.getString("cal"));
                }
                _streams = new List<YDataStream>();
                _preview = new List<YMeasure>();
                _measures = new List<YMeasure>();
                jstreams = json.getYJSONArray("streams");
                for (int i = 0; i < jstreams.Length; i++) {
                    YDataStream stream = _parent.imm_findDataStream(this, jstreams.getString(i));
                    // the timestamp in the data streams is the end of the measure, so the actual
                    // measurement start time is computed as one interval before the first timestamp
                    streamStartTime = await stream.get_realStartTimeUTC() * 1000;
                    streamEndTime = streamStartTime + await stream.get_realDuration() * 1000;
                    if (_startTimeMs > 0 && streamEndTime <= _startTimeMs) {
                        // this stream is too early, drop it
                    } else if (_endTimeMs > 0 && streamStartTime >= _endTimeMs) {
                        // this stream is too late, drop it
                    } else {
                        _streams.Add(stream);
                    }
                }
            } catch (Exception e) {
                throw new YAPI_Exception(YAPI.IO_ERROR, "invalid json structure for YDataSet: " + e.Message);
            }
            _progress = 0;
            return await this.get_progress();
        }

        public string imm_get_functionId()
        {
            return _functionId;
        }
        //--- (generated code: YDataSet implementation)
#pragma warning disable 1998

    public virtual List<int> imm_get_calibration()
    {
        return _calib;
    }

    public virtual async Task<int> loadSummary(byte[] data)
    {
        List<List<double>> dataRows = new List<List<double>>();
        double tim;
        double mitv;
        double itv;
        double fitv;
        double end_;
        int nCols;
        int minCol;
        int avgCol;
        int maxCol;
        int res;
        int m_pos;
        double previewTotalTime;
        double previewTotalAvg;
        double previewMinVal;
        double previewMaxVal;
        double previewAvgVal;
        double previewStartMs;
        double previewStopMs;
        double previewDuration;
        double streamStartTimeMs;
        double streamDuration;
        double streamEndTimeMs;
        double minVal;
        double avgVal;
        double maxVal;
        double summaryStartMs;
        double summaryStopMs;
        double summaryTotalTime;
        double summaryTotalAvg;
        double summaryMinVal;
        double summaryMaxVal;
        string url;
        string strdata;
        List<double> measure_data = new List<double>();

        if (_progress < 0) {
            strdata = YAPI.DefaultEncoding.GetString(data);
            if (strdata == "{}") {
                _parent._throw(YAPI.VERSION_MISMATCH, "device firmware is too old");
                return YAPI.VERSION_MISMATCH;
            }
            res = await this._parse(strdata);
            if (res < 0) {
                return res;
            }
        }
        summaryTotalTime = 0;
        summaryTotalAvg = 0;
        summaryMinVal = YAPI.MAX_DOUBLE;
        summaryMaxVal = YAPI.MIN_DOUBLE;
        summaryStartMs = YAPI.MAX_DOUBLE;
        summaryStopMs = YAPI.MIN_DOUBLE;

        // Parse complete streams
        for (int ii_0 = 0; ii_0 <  _streams.Count; ii_0++) {
            streamStartTimeMs = Math.Round(await  _streams[ii_0].get_realStartTimeUTC() * 1000);
            streamDuration = await  _streams[ii_0].get_realDuration();
            streamEndTimeMs = streamStartTimeMs + Math.Round(streamDuration * 1000);
            if ((streamStartTimeMs >= _startTimeMs) && ((_endTimeMs == 0) || (streamEndTimeMs <= _endTimeMs))) {
                // stream that are completely inside the dataset
                previewMinVal = await  _streams[ii_0].get_minValue();
                previewAvgVal = await  _streams[ii_0].get_averageValue();
                previewMaxVal = await  _streams[ii_0].get_maxValue();
                previewStartMs = streamStartTimeMs;
                previewStopMs = streamEndTimeMs;
                previewDuration = streamDuration;
            } else {
                // stream that are partially in the dataset
                // we need to parse data to filter value outside the dataset
                if (!( _streams[ii_0].imm_wasLoaded())) {
                    url =  _streams[ii_0].imm_get_url();
                    data = await _parent._download(url);
                    _streams[ii_0].imm_parseStream(data);
                }
                dataRows = await  _streams[ii_0].get_dataRows();
                if (dataRows.Count == 0) {
                    return await this.get_progress();
                }
                tim = streamStartTimeMs;
                fitv = Math.Round(await  _streams[ii_0].get_firstDataSamplesInterval() * 1000);
                itv = Math.Round(await  _streams[ii_0].get_dataSamplesInterval() * 1000);
                nCols = dataRows[0].Count;
                minCol = 0;
                if (nCols > 2) {
                    avgCol = 1;
                } else {
                    avgCol = 0;
                }
                if (nCols > 2) {
                    maxCol = 2;
                } else {
                    maxCol = 0;
                }
                previewTotalTime = 0;
                previewTotalAvg = 0;
                previewStartMs = streamEndTimeMs;
                previewStopMs = streamStartTimeMs;
                previewMinVal = YAPI.MAX_DOUBLE;
                previewMaxVal = YAPI.MIN_DOUBLE;
                m_pos = 0;
                while (m_pos < dataRows.Count) {
                    measure_data = dataRows[m_pos];
                    if (m_pos == 0) {
                        mitv = fitv;
                    } else {
                        mitv = itv;
                    }
                    end_ = tim + mitv;
                    if ((end_ > _startTimeMs) && ((_endTimeMs == 0) || (tim < _endTimeMs))) {
                        minVal = measure_data[minCol];
                        avgVal = measure_data[avgCol];
                        maxVal = measure_data[maxCol];
                        if (previewStartMs > tim) {
                            previewStartMs = tim;
                        }
                        if (previewStopMs < end_) {
                            previewStopMs = end_;
                        }
                        if (previewMinVal > minVal) {
                            previewMinVal = minVal;
                        }
                        if (previewMaxVal < maxVal) {
                            previewMaxVal = maxVal;
                        }
                        if (!(Double.IsNaN(avgVal))) {
                            previewTotalAvg = previewTotalAvg + (avgVal * mitv);
                            previewTotalTime = previewTotalTime + mitv;
                        }
                    }
                    tim = end_;
                    m_pos = m_pos + 1;
                }
                if (previewTotalTime > 0) {
                    previewAvgVal = previewTotalAvg / previewTotalTime;
                    previewDuration = (previewStopMs - previewStartMs) / 1000.0;
                } else {
                    previewAvgVal = 0.0;
                    previewDuration = 0.0;
                }
            }
            _preview.Add(new YMeasure(previewStartMs / 1000.0, previewStopMs / 1000.0, previewMinVal, previewAvgVal, previewMaxVal));
            if (summaryMinVal > previewMinVal) {
                summaryMinVal = previewMinVal;
            }
            if (summaryMaxVal < previewMaxVal) {
                summaryMaxVal = previewMaxVal;
            }
            if (summaryStartMs > previewStartMs) {
                summaryStartMs = previewStartMs;
            }
            if (summaryStopMs < previewStopMs) {
                summaryStopMs = previewStopMs;
            }
            summaryTotalAvg = summaryTotalAvg + (previewAvgVal * previewDuration);
            summaryTotalTime = summaryTotalTime + previewDuration;
        }
        if ((_startTimeMs == 0) || (_startTimeMs > summaryStartMs)) {
            _startTimeMs = summaryStartMs;
        }
        if ((_endTimeMs == 0) || (_endTimeMs < summaryStopMs)) {
            _endTimeMs = summaryStopMs;
        }
        if (summaryTotalTime > 0) {
            _summary = new YMeasure(summaryStartMs / 1000.0, summaryStopMs / 1000.0, summaryMinVal, summaryTotalAvg / summaryTotalTime, summaryMaxVal);
        } else {
            _summary = new YMeasure(0.0, 0.0, YAPI.INVALID_DOUBLE, YAPI.INVALID_DOUBLE, YAPI.INVALID_DOUBLE);
        }
        return await this.get_progress();
    }

    public virtual async Task<int> processMore(int progress,byte[] data)
    {
        YDataStream stream;
        List<List<double>> dataRows = new List<List<double>>();
        double tim;
        double itv;
        double fitv;
        double avgv;
        double end_;
        int nCols;
        int minCol;
        int avgCol;
        int maxCol;
        bool firstMeasure;
        string baseurl;
        string url;
        string suffix;
        List<string> suffixes = new List<string>();
        int idx;
        byte[] bulkFile = new byte[0];
        List<string> streamStr = new List<string>();
        int urlIdx;
        byte[] streamBin = new byte[0];

        if (progress != _progress) {
            return _progress;
        }
        if (_progress < 0) {
            return await this.loadSummary(data);
        }
        stream = _streams[_progress];
        if (!(stream.imm_wasLoaded())) {
            stream.imm_parseStream(data);
        }
        dataRows = await stream.get_dataRows();
        _progress = _progress + 1;
        if (dataRows.Count == 0) {
            return await this.get_progress();
        }
        tim = Math.Round(await stream.get_realStartTimeUTC() * 1000);
        fitv = Math.Round(await stream.get_firstDataSamplesInterval() * 1000);
        itv = Math.Round(await stream.get_dataSamplesInterval() * 1000);
        if (fitv == 0) {
            fitv = itv;
        }
        if (tim < itv) {
            tim = itv;
        }
        nCols = dataRows[0].Count;
        minCol = 0;
        if (nCols > 2) {
            avgCol = 1;
        } else {
            avgCol = 0;
        }
        if (nCols > 2) {
            maxCol = 2;
        } else {
            maxCol = 0;
        }

        firstMeasure = true;
        for (int ii_0 = 0; ii_0 < dataRows.Count; ii_0++) {
            if (firstMeasure) {
                end_ = tim + fitv;
                firstMeasure = false;
            } else {
                end_ = tim + itv;
            }
            avgv = dataRows[ii_0][avgCol];
            if ((end_ > _startTimeMs) && ((_endTimeMs == 0) || (tim < _endTimeMs)) && !(Double.IsNaN(avgv))) {
                _measures.Add(new YMeasure(tim / 1000, end_ / 1000, dataRows[ii_0][minCol], avgv, dataRows[ii_0][maxCol]));
            }
            tim = end_;
        }
        // Perform bulk preload to speed-up network transfer
        if ((_bulkLoad > 0) && (_progress < _streams.Count)) {
            stream = _streams[_progress];
            if (stream.imm_wasLoaded()) {
                return await this.get_progress();
            }
            baseurl = stream.imm_get_baseurl();
            url = stream.imm_get_url();
            suffix = stream.imm_get_urlsuffix();
            suffixes.Add(suffix);
            idx = _progress + 1;
            while ((idx < _streams.Count) && (suffixes.Count < _bulkLoad)) {
                stream = _streams[idx];
                if (!(stream.imm_wasLoaded()) && (stream.imm_get_baseurl() == baseurl)) {
                    suffix = stream.imm_get_urlsuffix();
                    suffixes.Add(suffix);
                    url = url + "," + suffix;
                }
                idx = idx + 1;
            }
            bulkFile = await _parent._download(url);
            streamStr = _parent.imm_json_get_array(bulkFile);
            urlIdx = 0;
            idx = _progress;
            while ((idx < _streams.Count) && (urlIdx < suffixes.Count) && (urlIdx < streamStr.Count)) {
                stream = _streams[idx];
                if ((stream.imm_get_baseurl() == baseurl) && (stream.imm_get_urlsuffix() == suffixes[urlIdx])) {
                    streamBin = YAPI.DefaultEncoding.GetBytes(streamStr[urlIdx]);
                    stream.imm_parseStream(streamBin);
                    urlIdx = urlIdx + 1;
                }
                idx = idx + 1;
            }
        }
        return await this.get_progress();
    }

    public virtual async Task<List<YDataStream>> get_privateDataStreams()
    {
        return _streams;
    }

    /**
     * <summary>
     *   Returns the unique hardware identifier of the function who performed the measures,
     *   in the form <c>SERIAL.FUNCTIONID</c>.
     * <para>
     *   The unique hardware identifier is composed of the
     *   device serial number and of the hardware identifier of the function
     *   (for example <c>THRMCPL1-123456.temperature1</c>)
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that uniquely identifies the function (ex: <c>THRMCPL1-123456.temperature1</c>)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YDataSet.HARDWAREID_INVALID</c>.
     * </para>
     */
    public virtual async Task<string> get_hardwareId()
    {
        YModule mo;
        if (!(_hardwareId == "")) {
            return _hardwareId;
        }
        mo = await _parent.get_module();
        _hardwareId = ""+ await mo.get_serialNumber()+"."+await this.get_functionId();
        return _hardwareId;
    }

    /**
     * <summary>
     *   Returns the hardware identifier of the function that performed the measure,
     *   without reference to the module.
     * <para>
     *   For example <c>temperature1</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that identifies the function (ex: <c>temperature1</c>)
     * </returns>
     */
    public virtual async Task<string> get_functionId()
    {
        return _functionId;
    }

    /**
     * <summary>
     *   Returns the measuring unit for the measured value.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string that represents a physical unit.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YDataSet.UNIT_INVALID</c>.
     * </para>
     */
    public virtual async Task<string> get_unit()
    {
        return _unit;
    }

    /**
     * <summary>
     *   Returns the start time of the dataset, relative to the Jan 1, 1970.
     * <para>
     *   When the <c>YDataSet</c> object is created, the start time is the value passed
     *   in parameter to the <c>get_dataSet()</c> function. After the
     *   very first call to <c>loadMore()</c>, the start time is updated
     *   to reflect the timestamp of the first measure actually found in the
     *   dataLogger within the specified range.
     * </para>
     * <para>
     *   <b>DEPRECATED</b>: This method has been replaced by <c>get_summary()</c>
     *   which contain more precise informations.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the Jan 1, 1970 and the beginning of this data
     *   set (i.e. Unix time representation of the absolute time).
     * </returns>
     */
    public virtual async Task<long> get_startTimeUTC()
    {
        return this.imm_get_startTimeUTC();
    }

    public virtual long imm_get_startTimeUTC()
    {
        return (long) (_startTimeMs / 1000.0);
    }

    /**
     * <summary>
     *   Returns the end time of the dataset, relative to the Jan 1, 1970.
     * <para>
     *   When the <c>YDataSet</c> object is created, the end time is the value passed
     *   in parameter to the <c>get_dataSet()</c> function. After the
     *   very first call to <c>loadMore()</c>, the end time is updated
     *   to reflect the timestamp of the last measure actually found in the
     *   dataLogger within the specified range.
     * </para>
     * <para>
     *   <b>DEPRECATED</b>: This method has been replaced by <c>get_summary()</c>
     *   which contain more precise informations.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an unsigned number corresponding to the number of seconds
     *   between the Jan 1, 1970 and the end of this data
     *   set (i.e. Unix time representation of the absolute time).
     * </returns>
     */
    public virtual async Task<long> get_endTimeUTC()
    {
        return this.imm_get_endTimeUTC();
    }

    public virtual long imm_get_endTimeUTC()
    {
        return (long) Math.Round(_endTimeMs / 1000.0);
    }

    /**
     * <summary>
     *   Returns the progress of the downloads of the measures from the data logger,
     *   on a scale from 0 to 100.
     * <para>
     *   When the object is instantiated by <c>get_dataSet</c>,
     *   the progress is zero. Each time <c>loadMore()</c> is invoked, the progress
     *   is updated, to reach the value 100 only once all measures have been loaded.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer in the range 0 to 100 (percentage of completion).
     * </returns>
     */
    public virtual async Task<int> get_progress()
    {
        if (_progress < 0) {
            return 0;
        }
        // index not yet loaded
        if (_progress >= _streams.Count) {
            return 100;
        }
        return ((1 + (1 + _progress) * 98) / ((1 + _streams.Count)));
    }

    /**
     * <summary>
     *   Loads the next block of measures from the dataLogger, and updates
     *   the progress indicator.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer in the range 0 to 100 (percentage of completion),
     *   or a negative error code in case of failure.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> loadMore()
    {
        string url;
        YDataStream stream;
        if (_progress < 0) {
            url = "logger.json?id="+_functionId;
            if (_startTimeMs != 0) {
                url = ""+url+"&from="+Convert.ToString(this.imm_get_startTimeUTC());
            }
            if (_endTimeMs != 0) {
                url = ""+url+"&to="+Convert.ToString(this.imm_get_endTimeUTC() + 1);
            }
        } else {
            if (_progress >= _streams.Count) {
                return 100;
            } else {
                stream = _streams[_progress];
                if (stream.imm_wasLoaded()) {
                    // Do not reload stream if it was already loaded
                    return await this.processMore(_progress, YAPI.DefaultEncoding.GetBytes(""));
                }
                url = stream.imm_get_url();
            }
        }
        try {
            return await this.processMore(_progress, await _parent._download(url));
        } catch (Exception) {
            return await this.processMore(_progress, await _parent._download(url));
        }
    }

    /**
     * <summary>
     *   Returns an <c>YMeasure</c> object which summarizes the whole
     *   <c>YDataSet</c>.
     * <para>
     *   In includes the following information:
     *   - the start of a time interval
     *   - the end of a time interval
     *   - the minimal value observed during the time interval
     *   - the average value observed during the time interval
     *   - the maximal value observed during the time interval
     * </para>
     * <para>
     *   This summary is available as soon as <c>loadMore()</c> has
     *   been called for the first time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an <c>YMeasure</c> object
     * </returns>
     */
    public virtual async Task<YMeasure> get_summary()
    {
        return _summary;
    }

    /**
     * <summary>
     *   Returns a condensed version of the measures that can
     *   retrieved in this <c>YDataSet</c>, as a list of <c>YMeasure</c>
     *   objects.
     * <para>
     *   Each item includes:
     *   - the start of a time interval
     *   - the end of a time interval
     *   - the minimal value observed during the time interval
     *   - the average value observed during the time interval
     *   - the maximal value observed during the time interval
     * </para>
     * <para>
     *   This preview is available as soon as <c>loadMore()</c> has
     *   been called for the first time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a table of records, where each record depicts the
     *   measured values during a time interval
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<YMeasure>> get_preview()
    {
        return _preview;
    }

    /**
     * <summary>
     *   Returns the detailed set of measures for the time interval corresponding
     *   to a given condensed measures previously returned by <c>get_preview()</c>.
     * <para>
     *   The result is provided as a list of <c>YMeasure</c> objects.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="measure">
     *   condensed measure from the list previously returned by
     *   <c>get_preview()</c>.
     * </param>
     * <returns>
     *   a table of records, where each record depicts the
     *   measured values during a time interval
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<YMeasure>> get_measuresAt(YMeasure measure)
    {
        double startUtcMs;
        YDataStream stream;
        List<List<double>> dataRows = new List<List<double>>();
        List<YMeasure> measures = new List<YMeasure>();
        double tim;
        double itv;
        double end_;
        int nCols;
        int minCol;
        int avgCol;
        int maxCol;

        startUtcMs = measure.get_startTimeUTC() * 1000;
        stream = null;
        for (int ii_0 = 0; ii_0 < _streams.Count; ii_0++) {
            if (Math.Round(await _streams[ii_0].get_realStartTimeUTC() *1000) == startUtcMs) {
                stream = _streams[ii_0];
            }
        }
        if (stream == null) {
            return measures;
        }
        dataRows = await stream.get_dataRows();
        if (dataRows.Count == 0) {
            return measures;
        }
        tim = Math.Round(await stream.get_realStartTimeUTC() * 1000);
        itv = Math.Round(await stream.get_dataSamplesInterval() * 1000);
        if (tim < itv) {
            tim = itv;
        }
        nCols = dataRows[0].Count;
        minCol = 0;
        if (nCols > 2) {
            avgCol = 1;
        } else {
            avgCol = 0;
        }
        if (nCols > 2) {
            maxCol = 2;
        } else {
            maxCol = 0;
        }

        for (int ii_1 = 0; ii_1 < dataRows.Count; ii_1++) {
            end_ = tim + itv;
            if ((end_ > _startTimeMs) && ((_endTimeMs == 0) || (tim < _endTimeMs))) {
                measures.Add(new YMeasure(tim / 1000.0, end_ / 1000.0, dataRows[ii_1][minCol], dataRows[ii_1][avgCol], dataRows[ii_1][maxCol]));
            }
            tim = end_;
        }
        return measures;
    }

    /**
     * <summary>
     *   Returns all measured values currently available for this DataSet,
     *   as a list of <c>YMeasure</c> objects.
     * <para>
     *   Each item includes:
     *   - the start of the measure time interval
     *   - the end of the measure time interval
     *   - the minimal value observed during the time interval
     *   - the average value observed during the time interval
     *   - the maximal value observed during the time interval
     * </para>
     * <para>
     *   Before calling this method, you should call <c>loadMore()</c>
     *   to load data from the device. You may have to call loadMore()
     *   several time until all rows are loaded, but you can start
     *   looking at available data rows before the load is complete.
     * </para>
     * <para>
     *   The oldest measures are always loaded first, and the most
     *   recent measures will be loaded last. As a result, timestamps
     *   are normally sorted in ascending order within the measure table,
     *   unless there was an unexpected adjustment of the datalogger UTC
     *   clock.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a table of records, where each record depicts the
     *   measured value for a given time interval
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty array.
     * </para>
     */
    public virtual async Task<List<YMeasure>> get_measures()
    {
        return _measures;
    }

#pragma warning restore 1998
    //--- (end of generated code: YDataSet implementation)

    }


}