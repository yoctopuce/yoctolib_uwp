/*********************************************************************
 *
 * $Id: YAPI.cs 66103 2025-05-02 06:55:47Z seb $
 *
 * High-level programming interface, common to all modules
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using com.yoctopuce.YoctoAPI;

namespace com.yoctopuce.YoctoAPI
{

    public sealed class YRefParam
    {
        public string Value { get; set; }
    }

/**
 *
 */
public class YAPI
{

        // Return value for invalid strings
        public const string INVALID_STRING = "!INVALID!";
        public const double INVALID_DOUBLE = -1.79769313486231E+308;
        public const int INVALID_INT = -2147483648;
        public const long INVALID_LONG = -9223372036854775807L;
        public const int INVALID_UINT = -1;
        public const string YOCTO_API_VERSION_STR = "1.11";
        public const string YOCTO_API_BUILD_STR = "66320";
        public const int YOCTO_VENDORID = 0x24e0;
        public const int YOCTO_DEVID_FACTORYBOOT = 1;
        public const int YOCTO_DEVID_BOOTLOADER = 2;
        public const double MAX_DOUBLE = Double.MaxValue;
        public const double MIN_DOUBLE = Double.MinValue;
        // --- (generated code: YFunction return codes)
    // Yoctopuce error codes, used by default as function return value
        public const int SUCCESS = 0;                   // everything worked all right
        public const int NOT_INITIALIZED = -1;          // call yInitAPI() first !
        public const int INVALID_ARGUMENT = -2;         // one of the arguments passed to the function is invalid
        public const int NOT_SUPPORTED = -3;            // the operation attempted is (currently) not supported
        public const int DEVICE_NOT_FOUND = -4;         // the requested device is not reachable
        public const int VERSION_MISMATCH = -5;         // the device firmware is incompatible with this API version
        public const int DEVICE_BUSY = -6;              // the device is busy with another task and cannot answer
        public const int TIMEOUT = -7;                  // the device took too long to provide an answer
        public const int IO_ERROR = -8;                 // there was an I/O problem while talking to the device
        public const int NO_MORE_DATA = -9;             // there is no more data to read from
        public const int EXHAUSTED = -10;               // you have run out of a limited resource, check the documentation
        public const int DOUBLE_ACCES = -11;            // you have two process that try to access to the same device
        public const int UNAUTHORIZED = -12;            // unauthorized access to password-protected device
        public const int RTC_NOT_READY = -13;           // real-time clock has not been initialized (or time was lost)
        public const int FILE_NOT_FOUND = -14;          // the file is not found
        public const int SSL_ERROR = -15;               // Error reported by mbedSSL
        public const int RFID_SOFT_ERROR = -16;         // Recoverable error with RFID tag (eg. tag out of reach), check YRfidStatus for details
        public const int RFID_HARD_ERROR = -17;         // Serious RFID error (eg. write-protected, out-of-boundary), check YRfidStatus for details
        public const int BUFFER_TOO_SMALL = -18;        // The buffer provided is too small
        public const int DNS_ERROR = -19;               // Error during name resolutions (invalid hostname or dns communication error)
        public const int SSL_UNK_CERT = -20;            // The certificate is not correctly signed by the trusted CA

//--- (end of generated code: YFunction return codes)
        internal static Encoding DefaultEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

        // Encoding types
        internal const int YOCTO_CALIB_TYPE_OFS = 30;

        // Yoctopuce generic constant
        internal const int YOCTO_MANUFACTURER_LEN = 20;
        internal const int YOCTO_SERIAL_LEN = 20;
        internal const int YOCTO_BASE_SERIAL_LEN = 8;
        internal const int YOCTO_PRODUCTNAME_LEN = 28;
        internal const int YOCTO_FIRMWARE_LEN = 22;
        internal const int YOCTO_LOGICAL_LEN = 20;
        internal const int YOCTO_FUNCTION_LEN = 20;
        internal const int YOCTO_PUBVAL_SIZE = 6; // Size of the data (can be non null
        internal const int YOCTO_PUBVAL_LEN = 16; // Temporary storage, >=
        internal const int YOCTO_PASS_LEN = 20;
        internal const int YOCTO_REALM_LEN = 20;
        internal const int HASH_BUF_SIZE = 28;


        // yInitAPI argument

        public const int DETECT_NONE = 0;
        public const int DETECT_USB = 1;
        public const int DETECT_NET = 2;
        public const int RESEND_MISSING_PKT = 4;
        public static readonly int DETECT_ALL = DETECT_USB | DETECT_NET;
        public const int DEFAULT_PKT_RESEND_DELAY = 50;
        //todo: move to YAPIContex
        internal static int pktAckDelay = DEFAULT_PKT_RESEND_DELAY;


        // - Types used for public yocto_api callbacks
        public delegate Task LogHandler(string log);
        public delegate Task DeviceUpdateHandler(YModule m);
        public delegate double CalibrationHandler(double rawValue, int calibType, List<int> parameters, List<double> rawValues, List<double> refValues);
        public delegate Task HubDiscoveryHandler(string serial, string url);

        private static YAPIContext _SingleYAPI = null;

        //todo: Look how to impement YAPIContext strategy
        internal static YAPIContext imm_GetYCtx()
        {
            if (_SingleYAPI == null) {
                _SingleYAPI = new YAPIContext();
            }
            return _SingleYAPI;
        }


        //PUBLIC STATIC METHOD:


//--- (generated code: YAPIContext yapiwrapper)
    /**
     * <summary>
     *   Modifies the delay between each forced enumeration of the used YoctoHubs.
     * <para>
     *   By default, the library performs a full enumeration every 10 seconds.
     *   To reduce network traffic, you can increase this delay.
     *   It's particularly useful when a YoctoHub is connected to the GSM network
     *   where traffic is billed. This parameter doesn't impact modules connected by USB,
     *   nor the working of module arrival/removal callbacks.
     *   Note: you must call this function after <c>yInitAPI</c>.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="deviceListValidity">
     *   nubmer of seconds between each enumeration.
     * @noreturn
     * </param>
     */
    public static async Task SetDeviceListValidity(int deviceListValidity)
    {
        await imm_GetYCtx().SetDeviceListValidity(deviceListValidity);
    }
    /**
     * <summary>
     *   Returns the delay between each forced enumeration of the used YoctoHubs.
     * <para>
     *   Note: you must call this function after <c>yInitAPI</c>.
     * </para>
     * </summary>
     * <returns>
     *   the number of seconds between each enumeration.
     * </returns>
     */
    public static async Task<int> GetDeviceListValidity()
    {
        return await imm_GetYCtx().GetDeviceListValidity();
    }
    /**
     * <summary>
     *   Adds a UDEV rule which authorizes all users to access Yoctopuce modules
     *   connected to the USB ports.
     * <para>
     *   This function works only under Linux. The process that
     *   calls this method must have root privileges because this method changes the Linux configuration.
     * </para>
     * </summary>
     * <param name="force">
     *   if true, overwrites any existing rule.
     * </param>
     * <returns>
     *   an empty string if the rule has been added.
     * </returns>
     * <para>
     *   On failure, returns a string that starts with "error:".
     * </para>
     */
    public static async Task<string> AddUdevRule(bool force)
    {
        return await imm_GetYCtx().AddUdevRule(force);
    }
    /**
     * <summary>
     *   Modifies the network connection delay for <c>yRegisterHub()</c> and <c>yUpdateDeviceList()</c>.
     * <para>
     *   This delay impacts only the YoctoHubs and VirtualHub
     *   which are accessible through the network. By default, this delay is of 20000 milliseconds,
     *   but depending or you network you may want to change this delay,
     *   gor example if your network infrastructure is based on a GSM connection.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="networkMsTimeout">
     *   the network connection delay in milliseconds.
     * @noreturn
     * </param>
     */
    public static async Task SetNetworkTimeout(int networkMsTimeout)
    {
        await imm_GetYCtx().SetNetworkTimeout(networkMsTimeout);
    }
    /**
     * <summary>
     *   Returns the network connection delay for <c>yRegisterHub()</c> and <c>yUpdateDeviceList()</c>.
     * <para>
     *   This delay impacts only the YoctoHubs and VirtualHub
     *   which are accessible through the network. By default, this delay is of 20000 milliseconds,
     *   but depending or you network you may want to change this delay,
     *   for example if your network infrastructure is based on a GSM connection.
     * </para>
     * </summary>
     * <returns>
     *   the network connection delay in milliseconds.
     * </returns>
     */
    public static async Task<int> GetNetworkTimeout()
    {
        return await imm_GetYCtx().GetNetworkTimeout();
    }
    /**
     * <summary>
     *   Change the validity period of the data loaded by the library.
     * <para>
     *   By default, when accessing a module, all the attributes of the
     *   module functions are automatically kept in cache for the standard
     *   duration (5 ms). This method can be used to change this standard duration,
     *   for example in order to reduce network or USB traffic. This parameter
     *   does not affect value change callbacks
     *   Note: This function must be called after <c>yInitAPI</c>.
     * </para>
     * </summary>
     * <param name="cacheValidityMs">
     *   an integer corresponding to the validity attributed to the
     *   loaded function parameters, in milliseconds.
     * @noreturn
     * </param>
     */
    public static async Task SetCacheValidity(ulong cacheValidityMs)
    {
        await imm_GetYCtx().SetCacheValidity(cacheValidityMs);
    }
    /**
     * <summary>
     *   Returns the validity period of the data loaded by the library.
     * <para>
     *   This method returns the cache validity of all attributes
     *   module functions.
     *   Note: This function must be called after <c>yInitAPI </c>.
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the validity attributed to the
     *   loaded function parameters, in milliseconds
     * </returns>
     */
    public static async Task<ulong> GetCacheValidity()
    {
        return await imm_GetYCtx().GetCacheValidity();
    }
    public static YHub nextHubInUseInternal(int hubref)
    {
        return imm_GetYCtx().nextHubInUseInternal(hubref);
    }
    public static YHub getYHubObj(int hubref)
    {
        return imm_GetYCtx().getYHubObj(hubref);
    }
//--- (end of generated code: YAPIContext yapiwrapper)


        /**
         * <summary>
         *   Returns the version identifier for the Yoctopuce library in use.
         * <para>
         *   The version is a string in the form <c>"Major.Minor.Build"</c>,
         *   for instance <c>"1.01.5535"</c>. For languages using an external
         *   DLL (for instance C#, VisualBasic or Delphi), the character string
         *   includes as well the DLL version, for instance
         *   <c>"1.01.5535 (1.01.5439)"</c>.
         * </para>
         * <para>
         *   If you want to verify in your code that the library version is
         *   compatible with the version that you have used during development,
         *   verify that the major number is strictly equal and that the minor
         *   number is greater or equal. The build number is not relevant
         *   with respect to the library compatibility.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <returns>
         *   a character string describing the library version.
         * </returns>
         */
        public static string GetAPIVersion()
        {
            return "1.11.6320" + YUSBHub.imm_getAPIVersion();
        }

        /**
         * <summary>
         *   Initializes the Yoctopuce programming library explicitly.
         * <para>
         *   It is not strictly needed to call <c>yInitAPI()</c>, as the library is
         *   automatically  initialized when calling <c>yRegisterHub()</c> for the
         *   first time.
         * </para>
         * <para>
         *   When <c>YAPI.DETECT_NONE</c> is used as detection <c>mode</c>,
         *   you must explicitly use <c>yRegisterHub()</c> to point the API to the
         *   VirtualHub on which your devices are connected before trying to access them.
         * </para>
         * </summary>
         * <param name="mode">
         *   an integer corresponding to the type of automatic
         *   device detection to use. Possible values are
         *   <c>YAPI.DETECT_NONE</c>, <c>YAPI.DETECT_USB</c>, <c>YAPI.DETECT_NET</c>,
         *   and <c>YAPI.DETECT_ALL</c>.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> InitAPI(int mode, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await yctx.InitAPI(mode);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }

        /**
         * <summary>
         *   Initializes the Yoctopuce programming library explicitly.
         * <para>
         *   It is not strictly needed to call <c>yInitAPI()</c>, as the library is
         *   automatically  initialized when calling <c>yRegisterHub()</c> for the
         *   first time.
         * </para>
         * <para>
         *   When <c>YAPI.DETECT_NONE</c> is used as detection <c>mode</c>,
         *   you must explicitly use <c>yRegisterHub()</c> to point the API to the
         *   VirtualHub on which your devices are connected before trying to access them.
         * </para>
         * </summary>
         * <param name="mode">
         *   an integer corresponding to the type of automatic
         *   device detection to use. Possible values are
         *   <c>YAPI.DETECT_NONE</c>, <c>YAPI.DETECT_USB</c>, <c>YAPI.DETECT_NET</c>,
         *   and <c>YAPI.DETECT_ALL</c>.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> InitAPI(int mode)
        {
            YAPIContext yctx = imm_GetYCtx();
            return await yctx.InitAPI(mode);
        }



        /**
         * <summary>
         *   Waits for all pending communications with Yoctopuce devices to be
         *   completed then frees dynamically allocated resources used by
         *   the Yoctopuce library.
         * <para>
         * </para>
         * <para>
         *   From an operating system standpoint, it is generally not required to call
         *   this function since the OS will automatically free allocated resources
         *   once your program is completed. However, there are two situations when
         *   you may really want to use that function:
         * </para>
         * <para>
         *   - Free all dynamically allocated memory blocks in order to
         *   track a memory leak.
         * </para>
         * <para>
         *   - Send commands to devices right before the end
         *   of the program. Since commands are sent in an asynchronous way
         *   the program could exit before all commands are effectively sent.
         * </para>
         * <para>
         *   You should not call any other library function after calling
         *   <c>yFreeAPI()</c>, or your program will crash.
         * </para>
         * </summary>
         */
        public static async Task FreeAPI()
        {
            YAPIContext yctx;
            yctx = _SingleYAPI;
            if (yctx != null) {
                await yctx.FreeAPI();
            }
            _SingleYAPI = null;
        }


        /**
         * <summary>
         *   Set up the Yoctopuce library to use modules connected on a given machine.
         * <para>
         *   Idealy this
         *   call will be made once at the begining of your application.  The
         *   parameter will determine how the API will work. Use the following values:
         * </para>
         * <para>
         *   <b>usb</b>: When the <c>usb</c> keyword is used, the API will work with
         *   devices connected directly to the USB bus. Some programming languages such a JavaScript,
         *   PHP, and Java don't provide direct access to USB hardware, so <c>usb</c> will
         *   not work with these. In this case, use a VirtualHub or a networked YoctoHub (see below).
         * </para>
         * <para>
         *   <b><i>x.x.x.x</i></b> or <b><i>hostname</i></b>: The API will use the devices connected to the
         *   host with the given IP address or hostname. That host can be a regular computer
         *   running a <i>native VirtualHub</i>, a <i>VirtualHub for web</i> hosted on a server,
         *   or a networked YoctoHub such as YoctoHub-Ethernet or
         *   YoctoHub-Wireless. If you want to use the VirtualHub running on you local
         *   computer, use the IP address 127.0.0.1. If the given IP is unresponsive, <c>yRegisterHub</c>
         *   will not return until a time-out defined by <c>ySetNetworkTimeout</c> has elapsed.
         *   However, it is possible to preventively test a connection  with <c>yTestHub</c>.
         *   If you cannot afford a network time-out, you can use the non blocking <c>yPregisterHub</c>
         *   function that will establish the connection as soon as it is available.
         * </para>
         * <para>
         * </para>
         * <para>
         *   <b>callback</b>: that keyword make the API run in "<i>HTTP Callback</i>" mode.
         *   This a special mode allowing to take control of Yoctopuce devices
         *   through a NAT filter when using a VirtualHub or a networked YoctoHub. You only
         *   need to configure your hub to call your server script on a regular basis.
         *   This mode is currently available for PHP and Node.JS only.
         * </para>
         * <para>
         *   Be aware that only one application can use direct USB access at a
         *   given time on a machine. Multiple access would cause conflicts
         *   while trying to access the USB modules. In particular, this means
         *   that you must stop the VirtualHub software before starting
         *   an application that uses direct USB access. The workaround
         *   for this limitation is to set up the library to use the VirtualHub
         *   rather than direct USB access.
         * </para>
         * <para>
         *   If access control has been activated on the hub, virtual or not, you want to
         *   reach, the URL parameter should look like:
         * </para>
         * <para>
         *   <c>http://username:password@address:port</c>
         * </para>
         * <para>
         *   You can call <i>RegisterHub</i> several times to connect to several machines. On
         *   the other hand, it is useless and even counterproductive to call <i>RegisterHub</i>
         *   with to same address multiple times during the life of the application.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> RegisterHub(string url, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().RegisterHub(url);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }



        /**
         * <summary>
         *   Set up the Yoctopuce library to use modules connected on a given machine.
         * <para>
         *   Idealy this
         *   call will be made once at the begining of your application.  The
         *   parameter will determine how the API will work. Use the following values:
         * </para>
         * <para>
         *   <b>usb</b>: When the <c>usb</c> keyword is used, the API will work with
         *   devices connected directly to the USB bus. Some programming languages such a JavaScript,
         *   PHP, and Java don't provide direct access to USB hardware, so <c>usb</c> will
         *   not work with these. In this case, use a VirtualHub or a networked YoctoHub (see below).
         * </para>
         * <para>
         *   <b><i>x.x.x.x</i></b> or <b><i>hostname</i></b>: The API will use the devices connected to the
         *   host with the given IP address or hostname. That host can be a regular computer
         *   running a <i>native VirtualHub</i>, a <i>VirtualHub for web</i> hosted on a server,
         *   or a networked YoctoHub such as YoctoHub-Ethernet or
         *   YoctoHub-Wireless. If you want to use the VirtualHub running on you local
         *   computer, use the IP address 127.0.0.1. If the given IP is unresponsive, <c>yRegisterHub</c>
         *   will not return until a time-out defined by <c>ySetNetworkTimeout</c> has elapsed.
         *   However, it is possible to preventively test a connection  with <c>yTestHub</c>.
         *   If you cannot afford a network time-out, you can use the non blocking <c>yPregisterHub</c>
         *   function that will establish the connection as soon as it is available.
         * </para>
         * <para>
         * </para>
         * <para>
         *   <b>callback</b>: that keyword make the API run in "<i>HTTP Callback</i>" mode.
         *   This a special mode allowing to take control of Yoctopuce devices
         *   through a NAT filter when using a VirtualHub or a networked YoctoHub. You only
         *   need to configure your hub to call your server script on a regular basis.
         *   This mode is currently available for PHP and Node.JS only.
         * </para>
         * <para>
         *   Be aware that only one application can use direct USB access at a
         *   given time on a machine. Multiple access would cause conflicts
         *   while trying to access the USB modules. In particular, this means
         *   that you must stop the VirtualHub software before starting
         *   an application that uses direct USB access. The workaround
         *   for this limitation is to set up the library to use the VirtualHub
         *   rather than direct USB access.
         * </para>
         * <para>
         *   If access control has been activated on the hub, virtual or not, you want to
         *   reach, the URL parameter should look like:
         * </para>
         * <para>
         *   <c>http://username:password@address:port</c>
         * </para>
         * <para>
         *   You can call <i>RegisterHub</i> several times to connect to several machines. On
         *   the other hand, it is useless and even counterproductive to call <i>RegisterHub</i>
         *   with to same address multiple times during the life of the application.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> RegisterHub(string url)
        {
            return await imm_GetYCtx().RegisterHub(url);
        }

        /**
         * <summary>
         *   Fault-tolerant alternative to <c>yRegisterHub()</c>.
         * <para>
         *   This function has the same
         *   purpose and same arguments as <c>yRegisterHub()</c>, but does not trigger
         *   an error when the selected hub is not available at the time of the function call.
         *   If the connexion cannot be established immediately, a background task will automatically
         *   perform periodic retries. This makes it possible to register a network hub independently of the current
         *   connectivity, and to try to contact it only when a device is actively needed.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> PreregisterHub(string url, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().PreregisterHub(url);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }

        /**
         * <summary>
         *   Fault-tolerant alternative to <c>yRegisterHub()</c>.
         * <para>
         *   This function has the same
         *   purpose and same arguments as <c>yRegisterHub()</c>, but does not trigger
         *   an error when the selected hub is not available at the time of the function call.
         *   If the connexion cannot be established immediately, a background task will automatically
         *   perform periodic retries. This makes it possible to register a network hub independently of the current
         *   connectivity, and to try to contact it only when a device is actively needed.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> PreregisterHub(string url)
        {
            return await imm_GetYCtx().PreregisterHub(url);
        }


        /**
         * <summary>
         *   Set up the Yoctopuce library to no more use modules connected on a previously
         *   registered machine with RegisterHub.
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c> or the
         *   root URL of the hub to monitor
         * </param>
         */
        public static async Task UnregisterHub(string url)
        {
            await imm_GetYCtx().UnregisterHub(url);
        }


        /**
         * <summary>
         *   Test if the hub is reachable.
         * <para>
         *   This method do not register the hub, it only test if the
         *   hub is usable. The url parameter follow the same convention as the <c>yRegisterHub</c>
         *   method. This method is useful to verify the authentication parameters for a hub. It
         *   is possible to force this method to return after mstimeout milliseconds.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="mstimeout">
         *   the number of millisecond available to test the connection.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> TestHub(string url, uint mstimeout, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().TestHub(url, mstimeout);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }


        /**
         * <summary>
         *   Test if the hub is reachable.
         * <para>
         *   This method do not register the hub, it only test if the
         *   hub is usable. The url parameter follow the same convention as the <c>yRegisterHub</c>
         *   method. This method is useful to verify the authentication parameters for a hub. It
         *   is possible to force this method to return after mstimeout milliseconds.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="url">
         *   a string containing either <c>"usb"</c>,<c>"callback"</c> or the
         *   root URL of the hub to monitor
         * </param>
         * <param name="mstimeout">
         *   the number of millisecond available to test the connection.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> TestHub(string url, uint mstimeout)
        {
            return await imm_GetYCtx().TestHub(url, mstimeout);
        }

        /**
         * <summary>
         *   Triggers a (re)detection of connected Yoctopuce modules.
         * <para>
         *   The library searches the machines or USB ports previously registered using
         *   <c>yRegisterHub()</c>, and invokes any user-defined callback function
         *   in case a change in the list of connected devices is detected.
         * </para>
         * <para>
         *   This function can be called as frequently as desired to refresh the device list
         *   and to make the application aware of hot-plug events. However, since device
         *   detection is quite a heavy process, UpdateDeviceList shouldn't be called more
         *   than once every two seconds.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> UpdateDeviceList(YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().UpdateDeviceList();
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }



        /**
         * <summary>
         *   Triggers a (re)detection of connected Yoctopuce modules.
         * <para>
         *   The library searches the machines or USB ports previously registered using
         *   <c>yRegisterHub()</c>, and invokes any user-defined callback function
         *   in case a change in the list of connected devices is detected.
         * </para>
         * <para>
         *   This function can be called as frequently as desired to refresh the device list
         *   and to make the application aware of hot-plug events. However, since device
         *   detection is quite a heavy process, UpdateDeviceList shouldn't be called more
         *   than once every two seconds.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> UpdateDeviceList()
        {
            return await imm_GetYCtx().UpdateDeviceList();
        }

        /**
         * <summary>
         *   Maintains the device-to-library communication channel.
         * <para>
         *   If your program includes significant loops, you may want to include
         *   a call to this function to make sure that the library takes care of
         *   the information pushed by the modules on the communication channels.
         *   This is not strictly necessary, but it may improve the reactivity
         *   of the library for the following commands.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> HandleEvents(YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().HandleEvents();
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }

        }



        /**
         * <summary>
         *   Maintains the device-to-library communication channel.
         * <para>
         *   If your program includes significant loops, you may want to include
         *   a call to this function to make sure that the library takes care of
         *   the information pushed by the modules on the communication channels.
         *   This is not strictly necessary, but it may improve the reactivity
         *   of the library for the following commands.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> HandleEvents()
        {
            return await imm_GetYCtx().HandleEvents();
        }


        /**
         * <summary>
         *   Pauses the execution flow for a specified duration.
         * <para>
         *   This function implements a passive waiting loop, meaning that it does not
         *   consume CPU cycles significantly. The processor is left available for
         *   other threads and processes. During the pause, the library nevertheless
         *   reads from time to time information from the Yoctopuce modules by
         *   calling <c>yHandleEvents()</c>, in order to stay up-to-date.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="ms_duration">
         *   an integer corresponding to the duration of the pause,
         *   in milliseconds.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> Sleep(int ms_duration, YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().Sleep(ms_duration);
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }


        /**
         * <summary>
         *   Pauses the execution flow for a specified duration.
         * <para>
         *   This function implements a passive waiting loop, meaning that it does not
         *   consume CPU cycles significantly. The processor is left available for
         *   other threads and processes. During the pause, the library nevertheless
         *   reads from time to time information from the Yoctopuce modules by
         *   calling <c>yHandleEvents()</c>, in order to stay up-to-date.
         * </para>
         * <para>
         *   This function may signal an error in case there is a communication problem
         *   while contacting a module.
         * </para>
         * </summary>
         * <param name="ms_duration">
         *   an integer corresponding to the duration of the pause,
         *   in milliseconds.
         * </param>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         * </returns>
         * <para>
         *   On failure returns a negative error code.
         * </para>
         */
        public static async Task<int> Sleep(int ms_duration)
        {
            return await imm_GetYCtx().Sleep(ms_duration);
        }


        /**
         * <summary>
         *   Force a hub discovery, if a callback as been registered with <c>yRegisterHubDiscoveryCallback</c> it
         *   will be called for each net work hub that will respond to the discovery.
         * <para>
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         *   On failure returns a negative error code.
         * </returns>
         */
        public static async Task<int> TriggerHubDiscovery(YRefParam errmsg)
        {
            YAPIContext yctx = imm_GetYCtx();
            try {
                return await imm_GetYCtx().TriggerHubDiscovery();
            } catch (YAPI_Exception ex) {
                errmsg.Value = ex.Message;
                return ex.errorType;
            }
        }

        /**
         * <summary>
         *   Force a hub discovery, if a callback as been registered with <c>yRegisterHubDiscoveryCallback</c> it
         *   will be called for each net work hub that will respond to the discovery.
         * <para>
         * </para>
         * </summary>
         * <param name="errmsg">
         *   a string passed by reference to receive any error message.
         * </param>
         * <returns>
         *   <c>YAPI.SUCCESS</c> when the call succeeds.
         *   On failure returns a negative error code.
         * </returns>
         */
        public static async Task<int> TriggerHubDiscovery()
        {
            return await imm_GetYCtx().TriggerHubDiscovery();
        }



        /**
         * <summary>
         *   Returns the current value of a monotone millisecond-based time counter.
         * <para>
         *   This counter can be used to compute delays in relation with
         *   Yoctopuce devices, which also uses the millisecond as timebase.
         * </para>
         * </summary>
         * <returns>
         *   a long integer corresponding to the millisecond counter.
         * </returns>
         */
        public static ulong GetTickCount()
        {
            return (ulong)DateTime.Now.Ticks / 10000;
        }

        /**
         * <summary>
         *   Checks if a given string is valid as logical name for a module or a function.
         * <para>
         *   A valid logical name has a maximum of 19 characters, all among
         *   <c>A...Z</c>, <c>a...z</c>, <c>0...9</c>, <c>_</c>, and <c>-</c>.
         *   If you try to configure a logical name with an incorrect string,
         *   the invalid characters are ignored.
         * </para>
         * </summary>
         * <param name="name">
         *   a string containing the name to check.
         * </param>
         * <returns>
         *   <c>true</c> if the name is valid, <c>false</c> otherwise.
         * </returns>
         */
        public static bool CheckLogicalName(string name)
        {
            return name != null && (name != "" || name.Length <= 19 && Regex.IsMatch(name, "^[A-Za-z0-9_-]*$"));
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time
         *   a device is plugged.
         * <para>
         *   This callback will be invoked while <c>yUpdateDeviceList</c>
         *   is running. You will have to call this function on a regular basis.
         * </para>
         * </summary>
         * <param name="arrivalCallback">
         *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public static void RegisterDeviceArrivalCallback(YAPI.DeviceUpdateHandler arrivalCallback)
        {
            imm_GetYCtx().RegisterDeviceArrivalCallback(arrivalCallback);
        }

        public static void RegisterDeviceChangeCallback(YAPI.DeviceUpdateHandler changeCallback)
        {
            imm_GetYCtx().RegisterDeviceChangeCallback(changeCallback);
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time
         *   a device is unplugged.
         * <para>
         *   This callback will be invoked while <c>yUpdateDeviceList</c>
         *   is running. You will have to call this function on a regular basis.
         * </para>
         * </summary>
         * <param name="removalCallback">
         *   a procedure taking a <c>YModule</c> parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public static void RegisterDeviceRemovalCallback(YAPI.DeviceUpdateHandler removalCallback)
        {
            imm_GetYCtx().RegisterDeviceRemovalCallback(removalCallback);
        }

        /**
         * <summary>
         *   Register a callback function, to be called each time an Network Hub send
         *   an SSDP message.
         * <para>
         *   The callback has two string parameter, the first one
         *   contain the serial number of the hub and the second contain the URL of the
         *   network hub (this URL can be passed to RegisterHub). This callback will be invoked
         *   while yUpdateDeviceList is running. You will have to call this function on a regular basis.
         * </para>
         * <para>
         * </para>
         * </summary>
         * <param name="hubDiscoveryCallback">
         *   a procedure taking two string parameter, the serial
         *   number and the hub URL. Use <c>null</c> to unregister a previously registered  callback.
         * </param>
         */
        public static async Task RegisterHubDiscoveryCallback(YAPI.HubDiscoveryHandler hubDiscoveryCallback)
        {
            await imm_GetYCtx().RegisterHubDiscoveryCallback(hubDiscoveryCallback);
        }

        /**
         * <summary>
         *   Registers a log callback function.
         * <para>
         *   This callback will be called each time
         *   the API have something to say. Quite useful to debug the API.
         * </para>
         * </summary>
         * <param name="logfun">
         *   a procedure taking a string parameter, or <c>null</c>
         *   to unregister a previously registered  callback.
         * </param>
         */
        public static void RegisterLogFunction(YAPI.LogHandler logfun)
        {
            imm_GetYCtx().RegisterLogFunction(logfun);
        }

        public static string get_debugMsg(string serial)
        {
            return imm_GetYCtx().get_debugMsg(serial);
        }
    }
}