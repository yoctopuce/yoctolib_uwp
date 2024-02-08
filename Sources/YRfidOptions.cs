/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Implements FindRfidOptions(), the high-level API for RfidOptions functions
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

//--- (generated code: YRfidOptions return codes)
//--- (end of generated code: YRfidOptions return codes)
//--- (generated code: YRfidOptions class start)
/**
 * <summary>
 *   YRfidOptions Class: Extra parameters for performing RFID tag operations
 * <para>
 *   <c>YRfidOptions</c> objects are used to provide optional
 *   parameters to RFID commands that interact with tags, and in
 *   particular to provide security keys when required.
 * </para>
 * </summary>
 */
public class YRfidOptions
{
//--- (end of generated code: YRfidOptions class start)
//--- (generated code: YRfidOptions definitions)
    public const int NO_RFID_KEY = 0;
    public const int MIFARE_KEY_A = 1;
    public const int MIFARE_KEY_B = 2;

    /**
     * <summary>
     *   Type of security key to be used to access the RFID tag.
     * <para>
     *   For MIFARE Classic tags, allowed values are
     *   <c>Y_MIFARE_KEY_A</c> or <c>Y_MIFARE_KEY_B</c>.
     *   The default value is <c>Y_NO_RFID_KEY</c>, in that case
     *   the reader will use the most common default key for the
     *   tag type.
     *   When a security key is required, it must be provided
     *   using property <c>HexKey</c>.
     * </para>
     * </summary>
     */
    public int KeyType;

    /**
     * <summary>
     *   Security key to be used to access the RFID tag, as an
     *   hexadecimal string.
     * <para>
     *   The key will only be used if you
     *   also specify which type of key it is, using property
     *   <c>KeyType</c>.
     * </para>
     * </summary>
     */
    public string HexKey;

    /**
     * <summary>
     *   Force the use of single-block commands to access RFID tag memory blocks.
     * <para>
     *   By default, the Yoctopuce library uses the most efficient access strategy
     *   generally available for each tag type, but you can force the use of
     *   single-block commands if the RFID tags you are using do not support
     *   multi-block commands. If opération speed is not a priority, choose
     *   single-block mode as it will work with any mode.
     * </para>
     * </summary>
     */
    public bool ForceSingleBlockAccess;

    /**
     * <summary>
     *   Force the use of multi-block commands to access RFID tag memory blocks.
     * <para>
     *   By default, the Yoctopuce library uses the most efficient access strategy
     *   generally available for each tag type, but you can force the use of
     *   multi-block commands if you know for sure that the RFID tags you are using
     *   do support multi-block commands. Be  aware that even if a tag allows multi-block
     *   operations, the maximum number of blocks that can be written or read at the same
     *   time can be (very) limited. If the tag does not support multi-block mode
     *   for the wanted opération, the option will be ignored.
     * </para>
     * </summary>
     */
    public bool ForceMultiBlockAccess;

    /**
     * <summary>
     *   Enable direct access to RFID tag control blocks.
     * <para>
     *   By default, Yoctopuce library read and write functions only work
     *   on data blocks and automatically skip special blocks, as specific functions are provided
     *   to configure security parameters found in control blocks.
     *   If you need to access control blocks in your own way using
     *   read/write functions, enable this option.  Use this option wisely,
     *   as overwriting a special block migth very well irreversibly alter your
     *   tag behavior.
     * </para>
     * </summary>
     */
    public bool EnableRawAccess;

    /**
     * <summary>
     *   Disables the tag memory overflow test.
     * <para>
     *   By default, the Yoctopuce
     *   library's read/write functions detect overruns and do not run
     *   commands that are likely to fail. If you nevertheless wish to
     *   access more memory than the tag announces, you can try to use
     *   this option.
     * </para>
     * </summary>
     */
    public bool DisableBoundaryChecks;

    /**
     * <summary>
     *   Enable simulation mode to check the affected block range as well
     *   as access rights.
     * <para>
     *   When this option is active, the operation is
     *   not fully applied to the RFID tag but the affected block range
     *   is determined and the optional access key is tested on these blocks.
     *   The access key rights are not tested though. This option applies to
     *   write / configure operations only, it is ignored for read operations.
     * </para>
     * </summary>
     */
    public bool EnableDryRun;

    //--- (end of generated code: YRfidOptions definitions)


    public YRfidOptions()
    {
        //--- (generated code: YRfidOptions attributes initialization)
        //--- (end of generated code: YRfidOptions attributes initialization)
    }

    //--- (generated code: YRfidOptions implementation)
#pragma warning disable 1998

    public virtual string imm_getParams()
    {
        int opt;
        string res;
        if (ForceSingleBlockAccess) {
            opt = 1;
        } else {
            opt = 0;
        }
        if (ForceMultiBlockAccess) {
            opt = ((opt) | (2));
        }
        if (EnableRawAccess) {
            opt = ((opt) | (4));
        }
        if (DisableBoundaryChecks) {
            opt = ((opt) | (8));
        }
        if (EnableDryRun) {
            opt = ((opt) | (16));
        }
        res = "&o="+Convert.ToString(opt);
        if (KeyType != 0) {
            res = ""+ res+"&k="+String.Format("{0:x02}", KeyType)+":"+HexKey;
        }
        return res;
    }

#pragma warning restore 1998
    //--- (end of generated code: YRfidOptions implementation)
}
}

