/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindRfidReader(), the high-level API for RfidReader functions
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

//--- (generated code: YRfidReader return codes)
//--- (end of generated code: YRfidReader return codes)
//--- (generated code: YRfidReader class start)
/**
 * <summary>
 *   YRfidReader Class: RfidReader function interface
 * <para>
 *   The <c>YRfidReader</c> class allows you to detect RFID tags, as well as
 *   read and write on these tags if the security settings allow it.
 * </para>
 * <para>
 *   Short reminder:
 * </para>
 * <para>
 * </para>
 * <para>
 *   - A tag's memory is generally organized in fixed-size blocks.
 * </para>
 * <para>
 *   - At tag level, each block must be read and written in its entirety.
 * </para>
 * <para>
 *   - Some blocks are special configuration blocks, and may alter the tag's behaviour
 *   tag behavior if they are rewritten with arbitrary data.
 * </para>
 * <para>
 *   - Data blocks can be set to read-only mode, but on many tags, this operation is irreversible.
 * </para>
 * <para>
 * </para>
 * <para>
 *   By default, the RfidReader class automatically manages these blocks so that
 *   arbitrary size data  can be manipulated of  without risk and without knowledge of
 *   tag architecture .
 * </para>
 * </summary>
 */
public class YRfidReader : YFunction
{
//--- (end of generated code: YRfidReader class start)
//--- (generated code: YRfidReader definitions)
    /**
     * <summary>
     *   invalid nTags value
     * </summary>
     */
    public const  int NTAGS_INVALID = YAPI.INVALID_UINT;
    /**
     * <summary>
     *   invalid refreshRate value
     * </summary>
     */
    public const  int REFRESHRATE_INVALID = YAPI.INVALID_UINT;
    protected int _nTags = NTAGS_INVALID;
    protected int _refreshRate = REFRESHRATE_INVALID;
    protected ValueCallback _valueCallbackRfidReader = null;
    protected YEventCallback _eventCallback;
    protected bool _isFirstCb;
    protected int _prevCbPos = 0;
    protected int _eventPos = 0;
    protected int _eventStamp = 0;

    public new delegate Task ValueCallback(YRfidReader func, string value);
    public new delegate Task TimedReportCallback(YRfidReader func, YMeasure measure);
    public delegate Task YEventCallback(YRfidReader obj, double timestamp, string eventType, string eventData);

    protected static async Task yInternalEventCallback(YRfidReader obj, String value)
    {
        await obj._internalEventHandler(value);
    }

    //--- (end of generated code: YRfidReader definitions)


    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRfidReader(YAPIContext ctx, string func)
        : base(ctx, func, "RfidReader")
    {
        //--- (generated code: YRfidReader attributes initialization)
        //--- (end of generated code: YRfidReader attributes initialization)
    }

    /**
     * <summary>
     * </summary>
     * <param name="func">
     *   functionid
     * </param>
     */
    protected YRfidReader(string func)
        : this(YAPI.imm_GetYCtx(), func)
    {
    }

    //--- (generated code: YRfidReader implementation)
#pragma warning disable 1998
    internal override void imm_parseAttr(YJSONObject json_val)
    {
        if (json_val.has("nTags")) {
            _nTags = json_val.getInt("nTags");
        }
        if (json_val.has("refreshRate")) {
            _refreshRate = json_val.getInt("refreshRate");
        }
        base.imm_parseAttr(json_val);
    }

    /**
     * <summary>
     *   Returns the number of RFID tags currently detected.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the number of RFID tags currently detected
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRfidReader.NTAGS_INVALID</c>.
     * </para>
     */
    public async Task<int> get_nTags()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return NTAGS_INVALID;
            }
        }
        res = _nTags;
        return res;
    }


    /**
     * <summary>
     *   Returns the tag list refresh rate, measured in Hz.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the tag list refresh rate, measured in Hz
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YRfidReader.REFRESHRATE_INVALID</c>.
     * </para>
     */
    public async Task<int> get_refreshRate()
    {
        int res;
        if (_cacheExpiration <= YAPIContext.GetTickCount()) {
            if (await this.load(await _yapi.GetCacheValidity()) != YAPI.SUCCESS) {
                return REFRESHRATE_INVALID;
            }
        }
        res = _refreshRate;
        return res;
    }


    /**
     * <summary>
     *   Changes the present tag list refresh rate, measured in Hz.
     * <para>
     *   The reader will do
     *   its best to respect it. Note that the reader cannot detect tag arrival or removal
     *   while it is  communicating with a tag.  Maximum frequency is limited to 100Hz,
     *   but in real life it will be difficult to do better than 50Hz.  A zero value
     *   will power off the device radio.
     *   Remember to call the <c>saveToFlash()</c> method of the module if the
     *   modification must be kept.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="newval">
     *   an integer corresponding to the present tag list refresh rate, measured in Hz
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
    public async Task<int> set_refreshRate(int  newval)
    {
        string rest_val;
        rest_val = (newval).ToString();
        await _setAttr("refreshRate",rest_val);
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Retrieves a RFID reader for a given identifier.
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
     *   This function does not require that the RFID reader is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRfidReader.isOnline()</c> to test if the RFID reader is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a RFID reader by logical name, no error is notified: the first instance
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
     *   a string that uniquely characterizes the RFID reader, for instance
     *   <c>MyDevice.rfidReader</c>.
     * </param>
     * <returns>
     *   a <c>YRfidReader</c> object allowing you to drive the RFID reader.
     * </returns>
     */
    public static YRfidReader FindRfidReader(string func)
    {
        YRfidReader obj;
        obj = (YRfidReader) YFunction._FindFromCache("RfidReader", func);
        if (obj == null) {
            obj = new YRfidReader(func);
            YFunction._AddToCache("RfidReader", func, obj);
        }
        return obj;
    }

    /**
     * <summary>
     *   Retrieves a RFID reader for a given identifier in a YAPI context.
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
     *   This function does not require that the RFID reader is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YRfidReader.isOnline()</c> to test if the RFID reader is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a RFID reader by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context
     * </param>
     * <param name="func">
     *   a string that uniquely characterizes the RFID reader, for instance
     *   <c>MyDevice.rfidReader</c>.
     * </param>
     * <returns>
     *   a <c>YRfidReader</c> object allowing you to drive the RFID reader.
     * </returns>
     */
    public static YRfidReader FindRfidReaderInContext(YAPIContext yctx,string func)
    {
        YRfidReader obj;
        obj = (YRfidReader) YFunction._FindFromCacheInContext(yctx, "RfidReader", func);
        if (obj == null) {
            obj = new YRfidReader(yctx, func);
            YFunction._AddToCache("RfidReader", func, obj);
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
        _valueCallbackRfidReader = callback;
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
        if (_valueCallbackRfidReader != null) {
            await _valueCallbackRfidReader(this, value);
        } else {
            await base._invokeValueCallback(value);
        }
        return 0;
    }

    public virtual async Task<int> _chkerror(string tagId,byte[] json,YRfidStatus status)
    {
        string jsonStr;
        int errCode;
        int errBlk;
        int fab;
        int lab;
        int retcode;

        if ((json).Length == 0) {
            errCode = await this.get_errorType();
            errBlk = -1;
            fab = -1;
            lab = -1;
        } else {
            jsonStr = YAPI.DefaultEncoding.GetString(json);
            errCode = YAPIContext.imm_atoi(this.imm_json_get_key(json, "err"));
            errBlk = YAPIContext.imm_atoi(this.imm_json_get_key(json, "errBlk"))-1;
            if ((jsonStr).IndexOf("\"fab\":") >= 0) {
                fab = YAPIContext.imm_atoi(this.imm_json_get_key(json, "fab"))-1;
                lab = YAPIContext.imm_atoi(this.imm_json_get_key(json, "lab"))-1;
            } else {
                fab = -1;
                lab = -1;
            }
        }
        status.imm_init(tagId, errCode, errBlk, fab, lab);
        retcode = await status.get_yapiError();
        if (!(retcode == YAPI.SUCCESS)) { this._throw(retcode,await status.get_errorMessage()); return retcode; }
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> reset()
    {
        byte[] json = new byte[0];
        YRfidStatus status;
        status = new YRfidStatus();

        json = await this._download("rfid.json?a=reset");
        return await this._chkerror("", json, status);
    }

    /**
     * <summary>
     *   Returns the list of RFID tags currently detected by the reader.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a list of strings, corresponding to each tag identifier (UID).
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list.
     * </para>
     */
    public virtual async Task<List<string>> get_tagIdList()
    {
        byte[] json = new byte[0];
        List<string> jsonList = new List<string>();
        List<string> taglist = new List<string>();

        json = await this._download("rfid.json?a=list");
        taglist.Clear();
        if ((json).Length > 3) {
            jsonList = this.imm_json_get_array(json);
            for (int ii_0 = 0; ii_0 < jsonList.Count; ii_0++) {
                taglist.Add(this.imm_json_get_string(YAPI.DefaultEncoding.GetBytes(jsonList[ii_0])));
            }
        }
        return taglist;
    }

    /**
     * <summary>
     *   Retourne la description des propriétés d'un tag RFID présent.
     * <para>
     *   Cette fonction peut causer des communications avec le tag.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to check
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   a <c>YRfidTagInfo</c> object.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty <c>YRfidTagInfo</c> objact.
     *   When it happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<YRfidTagInfo> get_tagInfo(string tagId,YRfidStatus status)
    {
        string url;
        byte[] json = new byte[0];
        int tagType;
        int size;
        int usable;
        int blksize;
        int fblk;
        int lblk;
        YRfidTagInfo res;
        url = "rfid.json?a=info&t="+tagId;

        json = await this._download(url);
        await this._chkerror(tagId, json, status);
        tagType = YAPIContext.imm_atoi(this.imm_json_get_key(json, "type"));
        size = YAPIContext.imm_atoi(this.imm_json_get_key(json, "size"));
        usable = YAPIContext.imm_atoi(this.imm_json_get_key(json, "usable"));
        blksize = YAPIContext.imm_atoi(this.imm_json_get_key(json, "blksize"));
        fblk = YAPIContext.imm_atoi(this.imm_json_get_key(json, "fblk"));
        lblk = YAPIContext.imm_atoi(this.imm_json_get_key(json, "lblk"));
        res = new YRfidTagInfo();
        res.imm_init(tagId, tagType, size, usable, blksize, fblk, lblk);
        return res;
    }

    /**
     * <summary>
     *   Change an RFID tag configuration to prevents any further write to
     *   the selected blocks.
     * <para>
     *   This operation is definitive and irreversible.
     *   Depending on the tag type and block index, adjascent blocks may become
     *   read-only as well, based on the locking granularity.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   first block to lock
     * </param>
     * <param name="nBlocks">
     *   number of blocks to lock
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagLockBlocks(string tagId,int firstBlock,int nBlocks,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        optstr = options.imm_getParams();
        url = "rfid.json?a=lock&t="+tagId+"&b="+Convert.ToString(firstBlock)+"&n="+Convert.ToString(nBlocks)+""+optstr;

        json = await this._download(url);
        return await this._chkerror(tagId, json, status);
    }

    /**
     * <summary>
     *   Reads the locked state for RFID tag memory data blocks.
     * <para>
     *   FirstBlock cannot be a special block, and any special
     *   block encountered in the middle of the read operation will be
     *   skipped automatically.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   number of the first block to check
     * </param>
     * <param name="nBlocks">
     *   number of blocks to check
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   a list of booleans with the lock state of selected blocks
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<List<bool>> get_tagLockState(string tagId,int firstBlock,int nBlocks,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        byte[] binRes = new byte[0];
        List<bool> res = new List<bool>();
        int idx;
        int val;
        bool isLocked;
        optstr = options.imm_getParams();
        url = "rfid.json?a=chkl&t="+tagId+"&b="+Convert.ToString(firstBlock)+"&n="+Convert.ToString(nBlocks)+""+optstr;

        json = await this._download(url);
        await this._chkerror(tagId, json, status);
        if (await status.get_yapiError() != YAPI.SUCCESS) {
            return res;
        }
        binRes = YAPIContext.imm_hexStrToBin(this.imm_json_get_key(json, "bitmap"));
        idx = 0;
        while (idx < nBlocks) {
            val = binRes[(idx >> 3)];
            isLocked = ((val & (1 << (idx & 7))) != 0);
            res.Add(isLocked);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Tells which block of a RFID tag memory are special and cannot be used
     *   to store user data.
     * <para>
     *   Mistakely writing a special block can lead to
     *   an irreversible alteration of the tag.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   number of the first block to check
     * </param>
     * <param name="nBlocks">
     *   number of blocks to check
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   a list of booleans with the lock state of selected blocks
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<List<bool>> get_tagSpecialBlocks(string tagId,int firstBlock,int nBlocks,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        byte[] binRes = new byte[0];
        List<bool> res = new List<bool>();
        int idx;
        int val;
        bool isLocked;
        optstr = options.imm_getParams();
        url = "rfid.json?a=chks&t="+tagId+"&b="+Convert.ToString(firstBlock)+"&n="+Convert.ToString(nBlocks)+""+optstr;

        json = await this._download(url);
        await this._chkerror(tagId, json, status);
        if (await status.get_yapiError() != YAPI.SUCCESS) {
            return res;
        }
        binRes = YAPIContext.imm_hexStrToBin(this.imm_json_get_key(json, "bitmap"));
        idx = 0;
        while (idx < nBlocks) {
            val = binRes[(idx >> 3)];
            isLocked = ((val & (1 << (idx & 7))) != 0);
            res.Add(isLocked);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Reads data from an RFID tag memory, as an hexadecimal string.
     * <para>
     *   The read operation may span accross multiple blocks if the requested
     *   number of bytes is larger than the RFID tag block size. By default
     *   firstBlock cannot be a special block, and any special block encountered
     *   in the middle of the read operation will be skipped automatically.
     *   If you rather want to read special blocks, use the <c>EnableRawAccess</c>
     *   field from the <c>options</c> parameter.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where read should start
     * </param>
     * <param name="nBytes">
     *   total number of bytes to read
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   an hexadecimal string if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty binary buffer. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<string> tagReadHex(string tagId,int firstBlock,int nBytes,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        string hexbuf;
        optstr = options.imm_getParams();
        url = "rfid.json?a=read&t="+tagId+"&b="+Convert.ToString(firstBlock)+"&n="+Convert.ToString(nBytes)+""+optstr;

        json = await this._download(url);
        await this._chkerror(tagId, json, status);
        if (await status.get_yapiError() == YAPI.SUCCESS) {
            hexbuf = this.imm_json_get_key(json, "res");
        } else {
            hexbuf = "";
        }
        return hexbuf;
    }

    /**
     * <summary>
     *   Reads data from an RFID tag memory, as a binary buffer.
     * <para>
     *   The read operation
     *   may span accross multiple blocks if the requested number of bytes
     *   is larger than the RFID tag block size.  By default
     *   firstBlock cannot be a special block, and any special block encountered
     *   in the middle of the read operation will be skipped automatically.
     *   If you rather want to read special blocks, use the <c>EnableRawAccess</c>
     *   field frrm the <c>options</c> parameter.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where read should start
     * </param>
     * <param name="nBytes">
     *   total number of bytes to read
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   a binary object with the data read if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty binary buffer. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<byte[]> tagReadBin(string tagId,int firstBlock,int nBytes,YRfidOptions options,YRfidStatus status)
    {
        return YAPIContext.imm_hexStrToBin(await this.tagReadHex(tagId, firstBlock, nBytes, options, status));
    }

    /**
     * <summary>
     *   Reads data from an RFID tag memory, as a byte list.
     * <para>
     *   The read operation
     *   may span accross multiple blocks if the requested number of bytes
     *   is larger than the RFID tag block size.  By default
     *   firstBlock cannot be a special block, and any special block encountered
     *   in the middle of the read operation will be skipped automatically.
     *   If you rather want to read special blocks, use the <c>EnableRawAccess</c>
     *   field from the <c>options</c> parameter.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where read should start
     * </param>
     * <param name="nBytes">
     *   total number of bytes to read
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   a byte list with the data read if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty list. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<List<int>> tagReadArray(string tagId,int firstBlock,int nBytes,YRfidOptions options,YRfidStatus status)
    {
        byte[] blk = new byte[0];
        int idx;
        int endidx;
        List<int> res = new List<int>();
        blk = await this.tagReadBin(tagId, firstBlock, nBytes, options, status);
        endidx = (blk).Length;
        idx = 0;
        while (idx < endidx) {
            res.Add(blk[idx]);
            idx = idx + 1;
        }
        return res;
    }

    /**
     * <summary>
     *   Reads data from an RFID tag memory, as a text string.
     * <para>
     *   The read operation
     *   may span accross multiple blocks if the requested number of bytes
     *   is larger than the RFID tag block size.  By default
     *   firstBlock cannot be a special block, and any special block encountered
     *   in the middle of the read operation will be skipped automatically.
     *   If you rather want to read special blocks, use the <c>EnableRawAccess</c>
     *   field from the <c>options</c> parameter.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where read should start
     * </param>
     * <param name="nChars">
     *   total number of characters to read
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   a text string with the data read if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns an empty string. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<string> tagReadStr(string tagId,int firstBlock,int nChars,YRfidOptions options,YRfidStatus status)
    {
        return YAPI.DefaultEncoding.GetString(await this.tagReadBin(tagId, firstBlock, nChars, options, status));
    }

    /**
     * <summary>
     *   Writes data provided as a binary buffer to an RFID tag memory.
     * <para>
     *   The write operation may span accross multiple blocks if the
     *   number of bytes to write is larger than the RFID tag block size.
     *   By default firstBlock cannot be a special block, and any special block
     *   encountered in the middle of the write operation will be skipped
     *   automatically. The last data block affected by the operation will
     *   be automatically padded with zeros if neccessary.  If you rather want
     *   to rewrite special blocks as well,
     *   use the <c>EnableRawAccess</c> field from the <c>options</c> parameter.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where write should start
     * </param>
     * <param name="buff">
     *   the binary buffer to write
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagWriteBin(string tagId,int firstBlock,byte[] buff,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string hexstr;
        int buflen;
        string fname;
        byte[] json = new byte[0];
        buflen = (buff).Length;
        if (buflen <= 16) {
            // short data, use an URL-based command
            hexstr = YAPIContext.imm_bytesToHexStr(buff, 0, buff.Length);
            return await this.tagWriteHex(tagId, firstBlock, hexstr, options, status);
        } else {
            // long data, use an upload command
            optstr = options.imm_getParams();
            fname = "Rfid:t="+tagId+"&b="+Convert.ToString(firstBlock)+"&n="+Convert.ToString(buflen)+""+optstr;
            json = await this._uploadEx(fname, buff);
            return await this._chkerror(tagId, json, status);
        }
    }

    /**
     * <summary>
     *   Writes data provided as a list of bytes to an RFID tag memory.
     * <para>
     *   The write operation may span accross multiple blocks if the
     *   number of bytes to write is larger than the RFID tag block size.
     *   By default firstBlock cannot be a special block, and any special block
     *   encountered in the middle of the write operation will be skipped
     *   automatically. The last data block affected by the operation will
     *   be automatically padded with zeros if neccessary.
     *   If you rather want to rewrite special blocks as well,
     *   use the <c>EnableRawAccess</c> field from the <c>options</c> parameter.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where write should start
     * </param>
     * <param name="byteList">
     *   a list of byte to write
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagWriteArray(string tagId,int firstBlock,List<int> byteList,YRfidOptions options,YRfidStatus status)
    {
        int bufflen;
        byte[] buff = new byte[0];
        int idx;
        int hexb;
        bufflen = byteList.Count;
        buff = new byte[bufflen];
        idx = 0;
        while (idx < bufflen) {
            hexb = byteList[idx];
            buff[idx] = (byte)(hexb & 0xff);
            idx = idx + 1;
        }

        return await this.tagWriteBin(tagId, firstBlock, buff, options, status);
    }

    /**
     * <summary>
     *   Writes data provided as an hexadecimal string to an RFID tag memory.
     * <para>
     *   The write operation may span accross multiple blocks if the
     *   number of bytes to write is larger than the RFID tag block size.
     *   By default firstBlock cannot be a special block, and any special block
     *   encountered in the middle of the write operation will be skipped
     *   automatically. The last data block affected by the operation will
     *   be automatically padded with zeros if neccessary.
     *   If you rather want to rewrite special blocks as well,
     *   use the <c>EnableRawAccess</c> field from the <c>options</c> parameter.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where write should start
     * </param>
     * <param name="hexString">
     *   a string of hexadecimal byte codes to write
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagWriteHex(string tagId,int firstBlock,string hexString,YRfidOptions options,YRfidStatus status)
    {
        int bufflen;
        string optstr;
        string url;
        byte[] json = new byte[0];
        byte[] buff = new byte[0];
        int idx;
        int hexb;
        bufflen = (hexString).Length;
        bufflen = (bufflen >> 1);
        if (bufflen <= 16) {
            // short data, use an URL-based command
            optstr = options.imm_getParams();
            url = "rfid.json?a=writ&t="+tagId+"&b="+Convert.ToString(firstBlock)+"&w="+hexString+""+optstr;
            json = await this._download(url);
            return await this._chkerror(tagId, json, status);
        } else {
            // long data, use an upload command
            buff = new byte[bufflen];
            idx = 0;
            while (idx < bufflen) {
                hexb = Convert.ToInt32((hexString).Substring(2 * idx, 2), 16);
                buff[idx] = (byte)(hexb & 0xff);
                idx = idx + 1;
            }
            return await this.tagWriteBin(tagId, firstBlock, buff, options, status);
        }
    }

    /**
     * <summary>
     *   Writes data provided as an ASCII string to an RFID tag memory.
     * <para>
     *   The write operation may span accross multiple blocks if the
     *   number of bytes to write is larger than the RFID tag block size.
     *   Note that only the characters présent  in  the provided string
     *   will be written, there is no notion of string length. If your
     *   string data have variable length, you'll have to encode the
     *   string length yourself, with a terminal zero for instannce.
     * </para>
     * <para>
     *   This function only works with ISO-latin characters, if you wish to
     *   write strings encoded with alternate character sets, you'll have to
     *   use tagWriteBin() function.
     * </para>
     * <para>
     *   By default firstBlock cannot be a special block, and any special block
     *   encountered in the middle of the write operation will be skipped
     *   automatically. The last data block affected by the operation will
     *   be automatically padded with zeros if neccessary.
     *   If you rather want to rewrite special blocks as well,
     *   use the <c>EnableRawAccess</c> field from the <c>options</c> parameter
     *   (definitely not recommanded).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="firstBlock">
     *   block number where write should start
     * </param>
     * <param name="text">
     *   the text string to write
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagWriteStr(string tagId,int firstBlock,string text,YRfidOptions options,YRfidStatus status)
    {
        byte[] buff = new byte[0];
        buff = YAPI.DefaultEncoding.GetBytes(text);

        return await this.tagWriteBin(tagId, firstBlock, buff, options, status);
    }

    /**
     * <summary>
     *   Reads an RFID tag AFI byte (ISO 15693 only).
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   the AFI value (0...255)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagGetAFI(string tagId,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        int res;
        optstr = options.imm_getParams();
        url = "rfid.json?a=rdsf&t="+tagId+"&b=0"+optstr;

        json = await this._download(url);
        await this._chkerror(tagId, json, status);
        if (await status.get_yapiError() == YAPI.SUCCESS) {
            res = YAPIContext.imm_atoi(this.imm_json_get_key(json, "res"));
        } else {
            res = await status.get_yapiError();
        }
        return res;
    }

    /**
     * <summary>
     *   Change an RFID tag AFI byte (ISO 15693 only).
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="afi">
     *   the AFI value to write (0...255)
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagSetAFI(string tagId,int afi,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        optstr = options.imm_getParams();
        url = "rfid.json?a=wrsf&t="+tagId+"&b=0&v="+Convert.ToString(afi)+""+optstr;

        json = await this._download(url);
        return await this._chkerror(tagId, json, status);
    }

    /**
     * <summary>
     *   Locks the RFID tag AFI byte (ISO 15693 only).
     * <para>
     *   This operation is definitive and irreversible.
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagLockAFI(string tagId,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        optstr = options.imm_getParams();
        url = "rfid.json?a=lksf&t="+tagId+"&b=0"+optstr;

        json = await this._download(url);
        return await this._chkerror(tagId, json, status);
    }

    /**
     * <summary>
     *   Reads an RFID tag DSFID byte (ISO 15693 only).
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   the DSFID value (0...255)
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagGetDSFID(string tagId,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        int res;
        optstr = options.imm_getParams();
        url = "rfid.json?a=rdsf&t="+tagId+"&b=1"+optstr;

        json = await this._download(url);
        await this._chkerror(tagId, json, status);
        if (await status.get_yapiError() == YAPI.SUCCESS) {
            res = YAPIContext.imm_atoi(this.imm_json_get_key(json, "res"));
        } else {
            res = await status.get_yapiError();
        }
        return res;
    }

    /**
     * <summary>
     *   Change an RFID tag DSFID byte (ISO 15693 only).
     * <para>
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="dsfid">
     *   the DSFID value to write (0...255)
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagSetDSFID(string tagId,int dsfid,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        optstr = options.imm_getParams();
        url = "rfid.json?a=wrsf&t="+tagId+"&b=1&v="+Convert.ToString(dsfid)+""+optstr;

        json = await this._download(url);
        return await this._chkerror(tagId, json, status);
    }

    /**
     * <summary>
     *   Locks the RFID tag DSFID byte (ISO 15693 only).
     * <para>
     *   This operation is definitive and irreversible.
     * </para>
     * </summary>
     * <param name="tagId">
     *   identifier of the tag to use
     * </param>
     * <param name="options">
     *   an <c>YRfidOptions</c> object with the optional
     *   command execution parameters, such as security key
     *   if required
     * </param>
     * <param name="status">
     *   an <c>RfidStatus</c> object that will contain
     *   the detailled status of the operation
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code. When it
     *   happens, you can get more information from the <c>status</c> object.
     * </para>
     */
    public virtual async Task<int> tagLockDSFID(string tagId,YRfidOptions options,YRfidStatus status)
    {
        string optstr;
        string url;
        byte[] json = new byte[0];
        optstr = options.imm_getParams();
        url = "rfid.json?a=lksf&t="+tagId+"&b=1"+optstr;

        json = await this._download(url);
        return await this._chkerror(tagId, json, status);
    }

    /**
     * <summary>
     *   Returns a string with last tag arrival/removal events observed.
     * <para>
     *   This method return only events that are still buffered in the device memory.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with last events observed (one per line).
     * </returns>
     * <para>
     *   On failure, throws an exception or returns  <c>YAPI.INVALID_STRING</c>.
     * </para>
     */
    public virtual async Task<string> get_lastEvents()
    {
        byte[] content = new byte[0];

        content = await this._download("events.txt?pos=0");
        return YAPI.DefaultEncoding.GetString(content);
    }

    /**
     * <summary>
     *   Registers a callback function to be called each time that an RFID tag appears or
     *   disappears.
     * <para>
     *   The callback is invoked only during the execution of
     *   <c>ySleep</c> or <c>yHandleEvents</c>. This provides control over the time when
     *   the callback is triggered. For good responsiveness, remember to call one of these
     *   two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer.
     *   The callback function should take four arguments:
     *   the <c>YRfidReader</c> object that emitted the event, the
     *   UTC timestamp of the event, a character string describing
     *   the type of event ("+" or "-") and a character string with the
     *   RFID tag identifier.
     *   On failure, throws an exception or returns a negative error code.
     * </param>
     */
    public virtual async Task<int> registerEventCallback(YEventCallback callback)
    {
        _eventCallback = callback;
        _isFirstCb = true;
        if (callback != null) {
            await this.registerValueCallback(yInternalEventCallback);
        } else {
            await this.registerValueCallback((ValueCallback) null);
        }
        return 0;
    }

    public virtual async Task<int> _internalEventHandler(string cbVal)
    {
        int cbPos;
        int cbDPos;
        string url;
        byte[] content = new byte[0];
        string contentStr;
        List<string> eventArr = new List<string>();
        int arrLen;
        string lenStr;
        int arrPos;
        string eventStr;
        int eventLen;
        string hexStamp;
        int typePos;
        int dataPos;
        int intStamp;
        byte[] binMStamp = new byte[0];
        int msStamp;
        double evtStamp;
        string evtType;
        string evtData;
        // detect possible power cycle of the reader to clear event pointer
        cbPos = YAPIContext.imm_atoi(cbVal);
        cbPos = (cbPos / 1000);
        cbDPos = ((cbPos - _prevCbPos) & 0x7ffff);
        _prevCbPos = cbPos;
        if (cbDPos > 16384) {
            _eventPos = 0;
        }
        if (!(_eventCallback != null)) {
            return YAPI.SUCCESS;
        }
        if (_isFirstCb) {
            // first emulated value callback caused by registerValueCallback:
            // retrieve arrivals of all tags currently present to emulate arrival
            _isFirstCb = false;
            _eventStamp = 0;
            content = await this._download("events.txt");
            contentStr = YAPI.DefaultEncoding.GetString(content);
            eventArr = new List<string>(contentStr.Split(new char[] {'\n'}));
            arrLen = eventArr.Count;
            if (!(arrLen > 0)) { this._throw(YAPI.IO_ERROR,"fail to download events"); return YAPI.IO_ERROR; }
            // first element of array is the new position preceeded by '@'
            arrPos = 1;
            lenStr = eventArr[0];
            lenStr = (lenStr).Substring(1, (lenStr).Length-1);
            // update processed event position pointer
            _eventPos = YAPIContext.imm_atoi(lenStr);
        } else {
            // load all events since previous call
            url = "events.txt?pos="+Convert.ToString(_eventPos);
            content = await this._download(url);
            contentStr = YAPI.DefaultEncoding.GetString(content);
            eventArr = new List<string>(contentStr.Split(new char[] {'\n'}));
            arrLen = eventArr.Count;
            if (!(arrLen > 0)) { this._throw(YAPI.IO_ERROR,"fail to download events"); return YAPI.IO_ERROR; }
            // last element of array is the new position preceeded by '@'
            arrPos = 0;
            arrLen = arrLen - 1;
            lenStr = eventArr[arrLen];
            lenStr = (lenStr).Substring(1, (lenStr).Length-1);
            // update processed event position pointer
            _eventPos = YAPIContext.imm_atoi(lenStr);
        }
        // now generate callbacks for each real event
        while (arrPos < arrLen) {
            eventStr = eventArr[arrPos];
            eventLen = (eventStr).Length;
            typePos = (eventStr).IndexOf(":")+1;
            if ((eventLen >= 14) && (typePos > 10)) {
                hexStamp = (eventStr).Substring(0, 8);
                intStamp = Convert.ToInt32(hexStamp, 16);
                if (intStamp >= _eventStamp) {
                    _eventStamp = intStamp;
                    binMStamp = YAPI.DefaultEncoding.GetBytes((eventStr).Substring(8, 2));
                    msStamp = (binMStamp[0]-64) * 32 + binMStamp[1];
                    evtStamp = intStamp + (0.001 * msStamp);
                    dataPos = (eventStr).IndexOf("=")+1;
                    evtType = (eventStr).Substring(typePos, 1);
                    evtData = "";
                    if (dataPos > 10) {
                        evtData = (eventStr).Substring(dataPos, eventLen-dataPos);
                    }
                    if (_eventCallback != null) {
                        await _eventCallback(this, evtStamp, evtType, evtData);
                    }
                }
            }
            arrPos = arrPos + 1;
        }
        return YAPI.SUCCESS;
    }

    /**
     * <summary>
     *   Continues the enumeration of RFID readers started using <c>yFirstRfidReader()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned RFID readers order.
     *   If you want to find a specific a RFID reader, use <c>RfidReader.findRfidReader()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRfidReader</c> object, corresponding to
     *   a RFID reader currently online, or a <c>null</c> pointer
     *   if there are no more RFID readers to enumerate.
     * </returns>
     */
    public YRfidReader nextRfidReader()
    {
        string next_hwid;
        try {
            string hwid = _yapi._yHash.imm_resolveHwID(_className, _func);
            next_hwid = _yapi._yHash.imm_getNextHardwareId(_className, hwid);
        } catch (YAPI_Exception) {
            next_hwid = null;
        }
        if(next_hwid == null) return null;
        return FindRfidReaderInContext(_yapi, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of RFID readers currently accessible.
     * <para>
     *   Use the method <c>YRfidReader.nextRfidReader()</c> to iterate on
     *   next RFID readers.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YRfidReader</c> object, corresponding to
     *   the first RFID reader currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRfidReader FirstRfidReader()
    {
        YAPIContext yctx = YAPI.imm_GetYCtx();
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RfidReader");
        if (next_hwid == null)  return null;
        return FindRfidReaderInContext(yctx, next_hwid);
    }

    /**
     * <summary>
     *   Starts the enumeration of RFID readers currently accessible.
     * <para>
     *   Use the method <c>YRfidReader.nextRfidReader()</c> to iterate on
     *   next RFID readers.
     * </para>
     * </summary>
     * <param name="yctx">
     *   a YAPI context.
     * </param>
     * <returns>
     *   a pointer to a <c>YRfidReader</c> object, corresponding to
     *   the first RFID reader currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YRfidReader FirstRfidReaderInContext(YAPIContext yctx)
    {
        string next_hwid = yctx._yHash.imm_getFirstHardwareId("RfidReader");
        if (next_hwid == null)  return null;
        return FindRfidReaderInContext(yctx, next_hwid);
    }

#pragma warning restore 1998
    //--- (end of generated code: YRfidReader implementation)
}
}

