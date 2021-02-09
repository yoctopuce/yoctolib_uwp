/*********************************************************************
 *
 *  $Id: YConsolidatedDataSet.cs 43337 2021-01-18 10:36:22Z web $
 *
 *  Implements FindConsolidatedDataSet(), the high-level API for ConsolidatedDataSet functions
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

//--- (generated code: YConsolidatedDataSet return codes)
//--- (end of generated code: YConsolidatedDataSet return codes)

//--- (generated code: YConsolidatedDataSet class start)
/**
 * <summary>
 *   YConsolidatedDataSet Class: Cross-sensor consolidated data sequence.
 * <para>
 * </para>
 * <para>
 *   <c>YConsolidatedDataSet</c> objects make it possible to retrieve a set of
 *   recorded measures from multiple sensors, for a specified time interval.
 *   They can be used to load data points progressively, and to receive
 *   data records by timestamp, one by one..
 * </para>
 * </summary>
 */
public class YConsolidatedDataSet
{
//--- (end of generated code: YConsolidatedDataSet class start)
//--- (generated code: YConsolidatedDataSet definitions)
    protected double _start = 0;
    protected double _end = 0;
    protected int _nsensors = 0;
    protected List<YSensor> _sensors = new List<YSensor>();
    protected List<YDataSet> _datasets = new List<YDataSet>();
    protected List<int> _progresss = new List<int>();
    protected List<int> _nextidx = new List<int>();
    protected List<double> _nexttim = new List<double>();

    //--- (end of generated code: YConsolidatedDataSet definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    public YConsolidatedDataSet(double startTime, double endTime, List<YSensor> sensorList)
    {
            //--- (generated code: YConsolidatedDataSet attributes initialization)
        //--- (end of generated code: YConsolidatedDataSet attributes initialization)
            this.imm_init(startTime, endTime, sensorList);
        }


    //--- (generated code: YConsolidatedDataSet implementation)
#pragma warning disable 1998

    public virtual int imm_init(double startt,double endt,List<YSensor> sensorList)
    {
        _start = startt;
        _end = endt;
        _sensors = sensorList;
        _nsensors = -1;
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns an object holding historical data for multiple
     *   sensors, for a specified time interval.
     * <para>
     *   The measures will be retrieved from the data logger, which must have been turned
     *   on at the desired time. The resulting object makes it possible to load progressively
     *   a large set of measures from multiple sensors, consolidating data on the fly
     *   to align records based on measurement timestamps.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="sensorNames">
     *   array of logical names or hardware identifiers of the sensors
     *   for which data must be loaded from their data logger.
     * </param>
     * <param name="startTime">
     *   the start of the desired measure time interval,
     *   as a Unix timestamp, i.e. the number of seconds since
     *   January 1, 1970 UTC. The special value 0 can be used
     *   to include any measure, without initial limit.
     * </param>
     * <param name="endTime">
     *   the end of the desired measure time interval,
     *   as a Unix timestamp, i.e. the number of seconds since
     *   January 1, 1970 UTC. The special value 0 can be used
     *   to include any measure, without ending limit.
     * </param>
     * <returns>
     *   an instance of <c>YConsolidatedDataSet</c>, providing access to
     *   consolidated historical data. Records can be loaded progressively
     *   using the <c>YConsolidatedDataSet.nextRecord()</c> method.
     * </returns>
     */
    public static YConsolidatedDataSet Init(List<string> sensorNames,double startTime,double endTime)
    {
        int nSensors;
        List<YSensor> sensorList = new List<YSensor>();
        int idx;
        string sensorName;
        YSensor s;
        YConsolidatedDataSet obj;
        nSensors = sensorNames.Count;
        sensorList.Clear();
        idx = 0;
        while (idx < nSensors) {
            sensorName = sensorNames[idx];
            s = YSensor.FindSensor(sensorName);
            sensorList.Add(s);
            idx = idx + 1;
        }
        obj = new YConsolidatedDataSet(startTime, endTime, sensorList);
        return obj;
    }

    /**
     * <summary>
     *   Extracts the next data record from the data logger of all sensors linked to this
     *   object.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="datarec">
     *   array of floating point numbers, that will be filled by the
     *   function with the timestamp of the measure in first position,
     *   followed by the measured value in next positions.
     * </param>
     * <returns>
     *   an integer in the range 0 to 100 (percentage of completion),
     *   or a negative error code in case of failure.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> nextRecord(List<double> datarec)
    {
        int s;
        int idx;
        YSensor sensor;
        YDataSet newdataset;
        int globprogress;
        int currprogress;
        double currnexttim;
        double newvalue;
        List<YMeasure> measures = new List<YMeasure>();
        double nexttime;
        //
        // Ensure the dataset have been retrieved
        //
        if (_nsensors == -1) {
            _nsensors = _sensors.Count;
            _datasets.Clear();
            _progresss.Clear();
            _nextidx.Clear();
            _nexttim.Clear();
            s = 0;
            while (s < _nsensors) {
                sensor = _sensors[s];
                newdataset = await sensor.get_recordedData(_start, _end);
                _datasets.Add(newdataset);
                _progresss.Add(0);
                _nextidx.Add(0);
                _nexttim.Add(0.0);
                s = s + 1;
            }
        }
        datarec.Clear();
        //
        // Find next timestamp to process
        //
        nexttime = 0;
        s = 0;
        while (s < _nsensors) {
            currnexttim = _nexttim[s];
            if (currnexttim == 0) {
                idx = _nextidx[s];
                measures = await _datasets[s].get_measures();
                currprogress = _progresss[s];
                while ((idx >= measures.Count) && (currprogress < 100)) {
                    currprogress = await _datasets[s].loadMore();
                    if (currprogress < 0) {
                        currprogress = 100;
                    }
                    _progresss[s] = currprogress;
                    measures = await _datasets[s].get_measures();
                }
                if (idx < measures.Count) {
                    currnexttim =  measures[idx].get_endTimeUTC();
                    _nexttim[s] = currnexttim;
                }
            }
            if (currnexttim > 0) {
                if ((nexttime == 0) || (nexttime > currnexttim)) {
                    nexttime = currnexttim;
                }
            }
            s = s + 1;
        }
        if (nexttime == 0) {
            return 100;
        }
        //
        // Extract data for this timestamp
        //
        datarec.Clear();
        datarec.Add(nexttime);
        globprogress = 0;
        s = 0;
        while (s < _nsensors) {
            if (_nexttim[s] == nexttime) {
                idx = _nextidx[s];
                measures = await _datasets[s].get_measures();
                newvalue =  measures[idx].get_averageValue();
                datarec.Add(newvalue);
                _nexttim[s] = 0.0;
                _nextidx[s] = idx+1;
            } else {
                datarec.Add(Double.NaN);
            }
            currprogress = _progresss[s];
            globprogress = globprogress + currprogress;
            s = s + 1;
        }
        if (globprogress > 0) {
            globprogress = ((globprogress) / (_nsensors));
            if (globprogress > 99) {
                globprogress = 99;
            }
        }
        return globprogress;
    }

#pragma warning restore 1998
    //--- (end of generated code: YConsolidatedDataSet implementation)
}
}

