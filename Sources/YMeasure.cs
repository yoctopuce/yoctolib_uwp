/*********************************************************************
 *
 * $Id: YMeasure.cs 64034 2025-01-06 15:37:18Z seb $
 *
 * Implements yFindMeasure(), the high-level API for Measure functions
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
using System.Threading.Tasks;


namespace com.yoctopuce.YoctoAPI {


    //--- (generated code: YMeasure class start)
/**
 * <summary>
 *   YMeasure Class: Measured value, returned in particular by the methods of the <c>YDataSet</c> class.
 * <para>
 * </para>
 * <para>
 *   <c>YMeasure</c> objects are used within the API to represent
 *   a value measured at a specified time. These objects are
 *   used in particular in conjunction with the <c>YDataSet</c> class,
 *   but also for sensors periodic timed reports
 *   (see <c>sensor.registerTimedReportCallback</c>).
 * </para>
 * </summary>
 */
public class YMeasure
{
//--- (end of generated code: YMeasure class start)
        //--- (generated code: YMeasure definitions)
    protected double _start = 0;
    protected double _end = 0;
    protected double _minVal = 0;
    protected double _avgVal = 0;
    protected double _maxVal = 0;

    //--- (end of generated code: YMeasure definitions)

        internal YMeasure(double start, double end, double minVal, double avgVal, double maxVal) {
            _start = start;
            _end = end;
            _minVal = minVal;
            _avgVal = avgVal;
            _maxVal = maxVal;
        }

        internal YMeasure() {
        }

        public virtual DateTime get_startTimeUTC_asDate() {
            return new DateTime((long)(_start * 1000 + 0.5));
        }

        public virtual DateTime get_endTimeUTC_asDate() {
            return new DateTime((long)(_end * 1000 + 0.5));
        }


        //--- (generated code: YMeasure implementation)
#pragma warning disable 1998

    /**
     * <summary>
     *   Returns the start time of the measure, relative to the Jan 1, 1970 UTC
     *   (Unix timestamp).
     * <para>
     *   When the recording rate is higher then 1 sample
     *   per second, the timestamp may have a fractional part.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the number of seconds
     *   between the Jan 1, 1970 UTC and the beginning of this measure.
     * </returns>
     */
    public virtual double get_startTimeUTC()
    {
        return _start;
    }

    /**
     * <summary>
     *   Returns the end time of the measure, relative to the Jan 1, 1970 UTC
     *   (Unix timestamp).
     * <para>
     *   When the recording rate is higher than 1 sample
     *   per second, the timestamp may have a fractional part.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the number of seconds
     *   between the Jan 1, 1970 UTC and the end of this measure.
     * </returns>
     */
    public virtual double get_endTimeUTC()
    {
        return _end;
    }

    /**
     * <summary>
     *   Returns the smallest value observed during the time interval
     *   covered by this measure.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the smallest value observed.
     * </returns>
     */
    public virtual double get_minValue()
    {
        return _minVal;
    }

    /**
     * <summary>
     *   Returns the average value observed during the time interval
     *   covered by this measure.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the average value observed.
     * </returns>
     */
    public virtual double get_averageValue()
    {
        return _avgVal;
    }

    /**
     * <summary>
     *   Returns the largest value observed during the time interval
     *   covered by this measure.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating-point number corresponding to the largest value observed.
     * </returns>
     */
    public virtual double get_maxValue()
    {
        return _maxVal;
    }

#pragma warning restore 1998
    //--- (end of generated code: YMeasure implementation)
     }


}