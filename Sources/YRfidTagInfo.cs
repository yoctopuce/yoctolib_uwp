/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindRfidTagInfo(), the high-level API for RfidTagInfo functions
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

//--- (generated code: YRfidTagInfo return codes)
//--- (end of generated code: YRfidTagInfo return codes)
//--- (generated code: YRfidTagInfo class start)
/**
 * <summary>
 *   YRfidTagInfo Class: RFID tag description, used by class <c>YRfidReader</c>
 * <para>
 *   <c>YRfidTagInfo</c> objects are used to describe RFID tag attributes,
 *   such as the tag type and its storage size. These objects are returned by
 *   method <c>get_tagInfo()</c> of class <c>YRfidReader</c>.
 * </para>
 * </summary>
 */
public class YRfidTagInfo
{
//--- (end of generated code: YRfidTagInfo class start)
//--- (generated code: YRfidTagInfo definitions)
    public const int IEC_15693 = 1;
    public const int IEC_14443 = 2;
    public const int IEC_14443_MIFARE_ULTRALIGHT = 3;
    public const int IEC_14443_MIFARE_CLASSIC1K = 4;
    public const int IEC_14443_MIFARE_CLASSIC4K = 5;
    public const int IEC_14443_MIFARE_DESFIRE = 6;
    public const int IEC_14443_NTAG_213 = 7;
    public const int IEC_14443_NTAG_215 = 8;
    public const int IEC_14443_NTAG_216 = 9;
    public const int IEC_14443_NTAG_424_DNA = 10;
    protected string _tagId;
    protected int _tagType = 0;
    protected string _typeStr;
    protected int _size = 0;
    protected int _usable = 0;
    protected int _blksize = 0;
    protected int _fblk = 0;
    protected int _lblk = 0;

    //--- (end of generated code: YRfidTagInfo definitions)


    public YRfidTagInfo()
    {
        //--- (generated code: YRfidTagInfo attributes initialization)
        //--- (end of generated code: YRfidTagInfo attributes initialization)
    }

    //--- (generated code: YRfidTagInfo implementation)
#pragma warning disable 1998

    /**
     * <summary>
     *   Returns the RFID tag identifier.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with the RFID tag identifier.
     * </returns>
     */
    public virtual async Task<string> get_tagId()
    {
        return imm_get_tagId();
    }
    /**
     * <summary>
     *   Returns the RFID tag identifier.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string with the RFID tag identifier.
     * </returns>
     */
    public virtual string imm_get_tagId()
    {
        return _tagId;
    }

    /**
     * <summary>
     *   Returns the type of the RFID tag, as a numeric constant.
     * <para>
     *   (<c>IEC_14443_MIFARE_CLASSIC1K</c>, ...).
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the RFID tag type
     * </returns>
     */
    public virtual async Task<int> get_tagType()
    {
        return imm_get_tagType();
    }
    /**
     * <summary>
     *   Returns the type of the RFID tag, as a numeric constant.
     * <para>
     *   (<c>IEC_14443_MIFARE_CLASSIC1K</c>, ...).
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the RFID tag type
     * </returns>
     */
    public virtual int imm_get_tagType()
    {
        return _tagType;
    }

    /**
     * <summary>
     *   Returns the type of the RFID tag, as a string.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the RFID tag type
     * </returns>
     */
    public virtual async Task<string> get_tagTypeStr()
    {
        return imm_get_tagTypeStr();
    }
    /**
     * <summary>
     *   Returns the type of the RFID tag, as a string.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a string corresponding to the RFID tag type
     * </returns>
     */
    public virtual string imm_get_tagTypeStr()
    {
        return _typeStr;
    }

    /**
     * <summary>
     *   Returns the total memory size of the RFID tag, in bytes.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the total memory size of the RFID tag
     * </returns>
     */
    public virtual async Task<int> get_tagMemorySize()
    {
        return imm_get_tagMemorySize();
    }
    /**
     * <summary>
     *   Returns the total memory size of the RFID tag, in bytes.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the total memory size of the RFID tag
     * </returns>
     */
    public virtual int imm_get_tagMemorySize()
    {
        return _size;
    }

    /**
     * <summary>
     *   Returns the usable storage size of the RFID tag, in bytes.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the usable storage size of the RFID tag
     * </returns>
     */
    public virtual async Task<int> get_tagUsableSize()
    {
        return imm_get_tagUsableSize();
    }
    /**
     * <summary>
     *   Returns the usable storage size of the RFID tag, in bytes.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the usable storage size of the RFID tag
     * </returns>
     */
    public virtual int imm_get_tagUsableSize()
    {
        return _usable;
    }

    /**
     * <summary>
     *   Returns the block size of the RFID tag, in bytes.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the block size of the RFID tag
     * </returns>
     */
    public virtual async Task<int> get_tagBlockSize()
    {
        return imm_get_tagBlockSize();
    }
    /**
     * <summary>
     *   Returns the block size of the RFID tag, in bytes.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the block size of the RFID tag
     * </returns>
     */
    public virtual int imm_get_tagBlockSize()
    {
        return _blksize;
    }

    /**
     * <summary>
     *   Returns the index of the first usable storage block on the RFID tag.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the index of the first usable storage block on the RFID tag
     * </returns>
     */
    public virtual async Task<int> get_tagFirstBlock()
    {
        return imm_get_tagFirstBlock();
    }
    /**
     * <summary>
     *   Returns the index of the first usable storage block on the RFID tag.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the index of the first usable storage block on the RFID tag
     * </returns>
     */
    public virtual int imm_get_tagFirstBlock()
    {
        return _fblk;
    }

    /**
     * <summary>
     *   Returns the index of the last usable storage block on the RFID tag.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the index of the last usable storage block on the RFID tag
     * </returns>
     */
    public virtual async Task<int> get_tagLastBlock()
    {
        return imm_get_tagLastBlock();
    }
    /**
     * <summary>
     *   Returns the index of the last usable storage block on the RFID tag.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   the index of the last usable storage block on the RFID tag
     * </returns>
     */
    public virtual int imm_get_tagLastBlock()
    {
        return _lblk;
    }

    public virtual void imm_init(string tagId,int tagType,int size,int usable,int blksize,int fblk,int lblk)
    {
        string typeStr;
        typeStr = "unknown";
        if (tagType == IEC_15693) {
            typeStr = "IEC 15693";
        }
        if (tagType == IEC_14443) {
            typeStr = "IEC 14443";
        }
        if (tagType == IEC_14443_MIFARE_ULTRALIGHT) {
            typeStr = "MIFARE Ultralight";
        }
        if (tagType == IEC_14443_MIFARE_CLASSIC1K) {
            typeStr = "MIFARE Classic 1K";
        }
        if (tagType == IEC_14443_MIFARE_CLASSIC4K) {
            typeStr = "MIFARE Classic 4K";
        }
        if (tagType == IEC_14443_MIFARE_DESFIRE) {
            typeStr = "MIFARE DESFire";
        }
        if (tagType == IEC_14443_NTAG_213) {
            typeStr = "NTAG 213";
        }
        if (tagType == IEC_14443_NTAG_215) {
            typeStr = "NTAG 215";
        }
        if (tagType == IEC_14443_NTAG_216) {
            typeStr = "NTAG 216";
        }
        if (tagType == IEC_14443_NTAG_424_DNA) {
            typeStr = "NTAG 424 DNA";
        }
        _tagId = tagId;
        _tagType = tagType;
        _typeStr = typeStr;
        _size = size;
        _usable = usable;
        _blksize = blksize;
        _fblk = fblk;
        _lblk = lblk;
    }

#pragma warning restore 1998
    //--- (end of generated code: YRfidTagInfo implementation)
}
}

