/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindGps(), the high-level API for Gps functions
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

//--- (YGps return codes)
//--- (end of YGps return codes)
//--- (YGps class start)
/**
 * <summary>
 *   YGps Class: Geolocalization control interface (GPS, GNSS, ..
 * <para>
 *   .), available for instance in the Yocto-GPS-V2
 * </para>
 * <para>
 *   The <c>YGps</c> class allows you to retrieve positioning
 *   data from a GPS/GNSS sensor. This class can provides
 *   complete positioning information. However, if you
 *   wish to define callbacks on position changes or record
 *   the position in the datalogger, you
 *   should use the <c>YLatitude</c> et <c>YLongitude</c> classes.
 * </para>
 * </summary>
 */
public class YGps : YFunction
{
//--- (end of YGps class start)
//--- (YGps definitions)
    /**
     * <summary>
     *   invalid isFixed value
     * </summary>
     */
    public const int ISFIXED_FALSE = 0;
    public const int ISFIXED_TRUE = 1;
    public const int ISFIXED_INVALID = -1;
    /**
     * <summary>
     *   invalid satCount value
     * </summary>
     */
    public const  long SATCOUNT_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid satPerConst value
     * </summary>
     */
    public const  long SATPERCONST_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid gpsRefreshRate value
     * </summary>
     */
    public const  double GPSREFRESHRATE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid coordSystem value
     * </summary>
     */
    public const int COORDSYSTEM_GPS_DMS = 0;
    public const int COORDSYSTEM_GPS_DM = 1;
    public const int COORDSYSTEM_GPS_D = 2;
    public const int COORDSYSTEM_INVALID = -1;
    /**
     * <summary>
     *   invalid constellation value
     * </summary>
     */
    public const int CONSTELLATION_GNSS = 0;
    public const int CONSTELLATION_GPS = 1;
    public const int CONSTELLATION_GLONASS = 2;
    public const int CONSTELLATION_GALILEO = 3;
    public const int CONSTELLATION_GPS_GLONASS = 4;
    public const int CONSTELLATION_GPS_GALILEO = 5;
    public const int CONSTELLATION_GLONASS_GALILEO = 6;
    public const int CONSTELLATION_INVALID = -1;
    /**
     * <summary>
     *   invalid latitude value
     * </summary>
     */
    public const  string LATITUDE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid longitude value
     * </summary>
     */
    public const  string LONGITUDE_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid dilution value
     * </summary>
     */
    public const  double DILUTION_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid altitude value
     * </summary>
     */
    public const  double ALTITUDE_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid groundSpeed value
     * </summary>
     */
    public const  double GROUNDSPEED_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid direction value
     * </summary>
     */
    public const  double DIRECTION_INVALID = YAPI.INVALID_DOUBLE;
    /**
     * <summary>
     *   invalid unixTime value
     * </summary>
     */
    public const  long UNIXTIME_INVALID = YAPI.INVALID_LONG;
    /**
     * <summary>
     *   invalid dateTime value
     * </summary>
     */
    public const  string DATETIME_INVALID = YAPI.INVALID_STRING;
    /**
     * <summary>
     *   invalid utcOffset value
     * </summary>
     */
    public const  int UTCOFFSET_INVALID = YAPI.INVALID_INT;
    /**
     * <summary>
     *   invalid command value
     * </summary>
     */
    public const  string COMMAND_INVALID = YAPI.INVALID_STRING;
    protected int _isFixed = ISFIXED_INVALID;
    protected long _satCount = SATCOUNT_INVALID;
    protected long _satPerConst = SATPERCONST_INVALID;
    protected double _gpsRefreshRate = GPSREFRESHRATE_INVALID;
    protected int _coordSystem = COORDSYSTEM_INVALID;
    protected int _constellation = CONSTELLATION_INVALID;
    protected string _latitude = LATITUDE_INVALID;
    protected string _longitude = LONGITUDE_INVALID;
    protected double _dilution = DILUTION_INVALID;
    protected double _altitude = ALTITUDE_INVALID;
    protected double _groundSpeed = GROUNDSPEED_INVALID;
    protected double _direction = DIRECTION_INVALID;
    protected long _unixTime = UNIXTIME_INVALID;
    protected string _dateTime = DATETIME_INVALID;
    protected int _utcOffset = UTCOFFSET_INVALID;
    protected string _command = COMMAND_INVALID;
    protected ValueCallback _valueCallbackGps = null;

    public new delegate Task ValueCallback(YGps func, string value);
    public new delegate Task TimedReportCallback(YGps func, YMeasure measure);
    //--- (end of YGps definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YGps(YAPIContext ctx, string func)
        : base(ctx, func, "Gps")
    {
        //--- (YGps attributes initialization)
        //--- (end of YGps attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YGps(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (YGps implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("isFixed")) {
            _isFixed = json_val.getInt("isFixed") > 0 ? 1 : 0;
        }
        if (json_val.has("satCount")) {
            _satCount = json_val.getLong("satCount");
        }
        if (json_val.has("satPerConst")) {
            _satPerConst = json_val.getLong("satPerConst");
        }
        if (json_val.has("gpsRefreshRate")) {
            _gpsRefreshRate = Math.Round(json_val.getDouble("gpsRefreshRate") / 65.536) / 1000.0;
        }
        if (json_val.has("coordSystem")) {
            _coordSystem = json_val.getInt("coordSystem");
        }
        if (json_val.has("constellation")) {
            _constellation = json_val.getInt("constellation");
        }
        if (json_val.has("latitude")) {
            _latitude = json_val.getString("latitude");
        }
        if (json_val.has("longitude")) {
            _longitude = json_val.getString("longitude");
        }
        if (json_val.has("dilution")) {
            _dilution = Math.Round(json_val.getDouble("dilution") / 65.536) / 1000.0;
        }
        if (json_val.has("altitude")) {
            _altitude = Math.Round(json_val.getDouble("altitude") / 65.536) / 1000.0;
        }
        if (json_val.has("groundSpeed")) {
            _groundSpeed = Math.Round(json_val.getDouble("groundSpeed") / 65.536) / 1000.0;
        }
        if (json_val.has("direction")) {
            _direction = Math.Round(json_val.getDouble("direction") / 65.536) / 1000.0;
        }
        if (json_val.has("unixTime")) {
            _unixTime = json_val.getLong("unixTime");
        }
        if (json_val.has("dateTime")) {
            _dateTime = json_val.getString("dateTime");
        }
        if (json_val.has("utcOffset")) {
            _utcOffset = json_val.getInt("utcOffset");
        }
        if (json_val.has("command")) {
            _command = json_val.getString("command");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns TRUE if the receiver has found enough satellites to work.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   either <c>YGps.ISFIXED_FALSE</c> or <c>YGps.ISFIXED_TRUE</c>, according to TRUE if the receiver has
     *   found enough satellites to work
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.ISFIXED_INVALID</c>.
     * </para>
     */
    public async Task<int> get_isFixed()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ISFIXED_INVALID;
            }
        }
        res = _isFixed;
        return res;
    }


    /**
     * <summary>
     *   Returns the total count of satellites used to compute GPS position.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the total count of satellites used to compute GPS position
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.SATCOUNT_INVALID</c>.
     * </para>
     */
    public async Task<long> get_satCount()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return SATCOUNT_INVALID;
            }
        }
        res = _satCount;
        return res;
    }


    /**
     * <summary>
     *   Returns the count of visible satellites per constellation encoded
     *   on a 32 bit integer: bits 0..
     * <para>
     *   5: GPS satellites count,  bits 6..11 : Glonass, bits 12..17 : Galileo.
     *   this value is refreshed every 5 seconds only.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the count of visible satellites per constellation encoded
     *   on a 32 bit integer: bits 0.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.SATPERCONST_INVALID</c>.
     * </para>
     */
    public async Task<long> get_satPerConst()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return SATPERCONST_INVALID;
            }
        }
        res = _satPerConst;
        return res;
    }


    /**
     * <summary>
     *   Returns effective GPS data refresh frequency.
     * <para>
     *   this value is refreshed every 5 seconds only.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to effective GPS data refresh frequency
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.GPSREFRESHRATE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_gpsRefreshRate()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return GPSREFRESHRATE_INVALID;
            }
        }
        res = _gpsRefreshRate;
        return res;
    }


    /**
     * <summary>
     *   Returns the representation system used for positioning data.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YGps.COORDSYSTEM_GPS_DMS</c>, <c>YGps.COORDSYSTEM_GPS_DM</c> and
     *   <c>YGps.COORDSYSTEM_GPS_D</c> corresponding to the representation system used for positioning data
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.COORDSYSTEM_INVALID</c>.
     * </para>
     */
    public async Task<int> get_coordSystem()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return COORDSYSTEM_INVALID;
            }
        }
        res = _coordSystem;
        return res;
    }


    /**
     * <summary>
     *   Changes the representation system used for positioning data.
     * <para>
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YGps.COORDSYSTEM_GPS_DMS</c>, <c>YGps.COORDSYSTEM_GPS_DM</c> and
     *   <c>YGps.COORDSYSTEM_GPS_D</c> corresponding to the representation system used for positioning data
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_coordSystem(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("coordSystem",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the the satellites constellation used to compute
     *   positioning data.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a value among <c>YGps.CONSTELLATION_GNSS</c>, <c>YGps.CONSTELLATION_GPS</c>,
     *   <c>YGps.CONSTELLATION_GLONASS</c>, <c>YGps.CONSTELLATION_GALILEO</c>,
     *   <c>YGps.CONSTELLATION_GPS_GLONASS</c>, <c>YGps.CONSTELLATION_GPS_GALILEO</c> and
     *   <c>YGps.CONSTELLATION_GLONASS_GALILEO</c> corresponding to the the satellites constellation used to compute
     *   positioning data
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.CONSTELLATION_INVALID</c>.
     * </para>
     */
    public async Task<int> get_constellation()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return CONSTELLATION_INVALID;
            }
        }
        res = _constellation;
        return res;
    }


    /**
     * <summary>
     *   Changes the satellites constellation used to compute
     *   positioning data.
     * <para>
     *   Possible  constellations are GNSS ( = all supported constellations),
     *   GPS, Glonass, Galileo , and the 3 possible pairs. This setting has  no effect on Yocto-GPS (V1).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   a value among <c>YGps.CONSTELLATION_GNSS</c>, <c>YGps.CONSTELLATION_GPS</c>,
     *   <c>YGps.CONSTELLATION_GLONASS</c>, <c>YGps.CONSTELLATION_GALILEO</c>,
     *   <c>YGps.CONSTELLATION_GPS_GLONASS</c>, <c>YGps.CONSTELLATION_GPS_GALILEO</c> and
     *   <c>YGps.CONSTELLATION_GLONASS_GALILEO</c> corresponding to the satellites constellation used to compute
     *   positioning data
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_constellation(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("constellation",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Returns the current latitude.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the current latitude
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.LATITUDE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_latitude()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return LATITUDE_INVALID;
            }
        }
        res = _latitude;
        return res;
    }


    /**
     * <summary>
     *   Returns the current longitude.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the current longitude
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.LONGITUDE_INVALID</c>.
     * </para>
     */
    public async Task<string> get_longitude()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return LONGITUDE_INVALID;
            }
        }
        res = _longitude;
        return res;
    }


    /**
     * <summary>
     *   Returns the current horizontal dilution of precision,
     *   the smaller that number is, the better .
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current horizontal dilution of precision,
     *   the smaller that number is, the better
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.DILUTION_INVALID</c>.
     * </para>
     */
    public async Task<double> get_dilution()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DILUTION_INVALID;
            }
        }
        res = _dilution;
        return res;
    }


    /**
     * <summary>
     *   Returns the current altitude.
     * <para>
     *   Beware:  GPS technology
     *   is very inaccurate regarding altitude.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current altitude
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.ALTITUDE_INVALID</c>.
     * </para>
     */
    public async Task<double> get_altitude()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return ALTITUDE_INVALID;
            }
        }
        res = _altitude;
        return res;
    }


    /**
     * <summary>
     *   Returns the current ground speed in Km/h.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current ground speed in Km/h
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.GROUNDSPEED_INVALID</c>.
     * </para>
     */
    public async Task<double> get_groundSpeed()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return GROUNDSPEED_INVALID;
            }
        }
        res = _groundSpeed;
        return res;
    }


    /**
     * <summary>
     *   Returns the current move bearing in degrees, zero
     *   is the true (geographic) north.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the current move bearing in degrees, zero
     *   is the true (geographic) north
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.DIRECTION_INVALID</c>.
     * </para>
     */
    public async Task<double> get_direction()
    {
        double res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DIRECTION_INVALID;
            }
        }
        res = _direction;
        return res;
    }


    /**
     * <summary>
     *   Returns the current time in Unix format (number of
     *   seconds elapsed since Jan 1st, 1970).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the current time in Unix format (number of
     *   seconds elapsed since Jan 1st, 1970)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.UNIXTIME_INVALID</c>.
     * </para>
     */
    public async Task<long> get_unixTime()
    {
        long res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return UNIXTIME_INVALID;
            }
        }
        res = _unixTime;
        return res;
    }


    /**
     * <summary>
     *   Returns the current time in the form "YYYY/MM/DD hh:mm:ss".
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the current time in the form "YYYY/MM/DD hh:mm:ss"
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.DATETIME_INVALID</c>.
     * </para>
     */
    public async Task<string> get_dateTime()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return DATETIME_INVALID;
            }
        }
        res = _dateTime;
        return res;
    }


    /**
     * <summary>
     *   Returns the number of seconds between current time and UTC time (time zone).
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of seconds between current time and UTC time (time zone)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YGps.UTCOFFSET_INVALID</c>.
     * </para>
     */
    public async Task<int> get_utcOffset()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return UTCOFFSET_INVALID;
            }
        }
        res = _utcOffset;
        return res;
    }


    /**
     * <summary>
     *   Changes the number of seconds between current time and UTC time (time zone).
     * <para>
     *   The timezone is automatically rounded to the nearest multiple of 15 minutes.
     *   If current UTC time is known, the current time is automatically be updated according to the selected time zone.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the number of seconds between current time and UTC time (time zone)
     * </param>
     * <para>
     * </para>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public async Task<int> set_utcOffset(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("utcOffset",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   throws an exception on error
     * </summary>
     */
    public async Task<string> get_command()
    {
        string res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return COMMAND_INVALID;
            }
        }
        res = _command;
        return res;
    }


    public async Task<int> set_command(string  newval)
    {
        string rest_val;
        rest_val = newval;
        await _setAttr("command",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a geolocalization module for a given identifier.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the geolocalization module is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YGps.isOnline()</c> to test if the geolocalization module is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a geolocalization module by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the matching device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the geolocalization module, for instance
     *   <c>YGNSSMK2.gps</c>.
     * </param>
     * <returns>
     *   a <c>YGps</c> object allowing you to drive the geolocalization module.
     * </returns>
     */
    public static YGps FindGps(string func)
    {
        YGps obj;
        obj = (YGps) YFunction._FindFromCache("Gps", func);
        if (obj == null) {
            obj = new YGps(func);
            YFunction._AddToCache("Gps", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a geolocalization module for a given identifier in a YAPI context.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the geolocalization module is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YGps.isOnline()</c> to test if the geolocalization module is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a geolocalization module by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the geolocalization module, for instance
     *   <c>YGNSSMK2.gps</c>.
     * </param>
     * <returns>
     *   a <c>YGps</c> object allowing you to drive the geolocalization module.
     * </returns>
     */
    public static YGps FindGpsInContext(YAPIContext yctx,string func)
    {
        YGps obj;
        obj = (YGps) YFunction._FindFromCacheInContext(yctx, "Gps", func);
        if (obj == null) {
            obj = new YGps(yctx, func);
            YFunction._AddToCache("Gps", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and the character string describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public async Task<int> registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            await YFunction._UpdateValueCallbackList(this, true);
        } else {
            await YFunction._UpdateValueCallbackList(this, false);
        }
        _valueCallbackGps = callback;
        // Immediately invoke value callback with current value
        if (callback != null && await this.isOnline()) {
            val = _advertisedValue;
            if (!(val == "")) {
                await this._invokeValueCallback(val);
            }
        }
        return 0;
    }

    public override async Task<int> _invokeValueCallback(string value)
    {
        if (_valueCallbackGps != null) {
            await _valueCallbackGps(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    /**
     * <summary>
     *   Continues the enumeration of geolocalization modules started using <c>yFirstGps()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned geolocalization modules order.
     *   If you want to find a specific a geolocalization module, use <c>Gps.findGps()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YGps</c> object, corresponding to
     *   a geolocalization module currently online, or a <c>null</c> pointer
     *   if there are no more geolocalization modules to enumerate.
     * </returns>
     */
    public YGps nextGps()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindGpsInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of geolocalization modules currently accessible.
     * <para>
     *   Use the method <c>YGps.nextGps()</c> to iterate on
     *   next geolocalization modules.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YGps</c> object, corresponding to
     *   the first geolocalization module currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YGps FirstGps()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Gps");
        if (next_hwid == null)  return null;
        return FindGpsInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of geolocalization modules currently accessible.
     * <para>
     *   Use the method <c>YGps.nextGps()</c> to iterate on
     *   next geolocalization modules.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YGps</c> object, corresponding to
     *   the first geolocalization module currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YGps FirstGpsInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("Gps");
        if (next_hwid == null)  return null;
        return FindGpsInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of YGps implementation)
}
}

