/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  Doc-GettingStarted-Yocto-RFID example
 *
 *  You can find more information on our web site:
 *   uwp API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-uwp-EN.html
 *
 *********************************************************************/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using com.yoctopuce.YoctoAPI;
using System.Collections.Generic;

namespace Demo
{
    public class Demo : DemoBase
    {
        public string HubURL { get; set; }
        public string Target { get; set; }
        public string ToSend { get; set; }

        public override async Task<int> Run()
        {
            try {
                await YAPI.RegisterHub(HubURL);

                YRfidReader reader;

                if (Target.ToLower() == "any") {
                    reader = YRfidReader.FirstRfidReader();
                    if (reader == null) {
                        WriteLine("No module connected (check USB cable) ");
                        return -1;
                    }
                } else {
                    reader = YRfidReader.FindRfidReader(Target + ".rfidReader");
                }
                YModule ymod = await reader.get_module();
                WriteLine("Using: " + await ymod.get_serialNumber());
                String serial = await ymod.get_serialNumber();
                YColorLedCluster led = YColorLedCluster.FindColorLedCluster(serial + ".colorLedCluster");
                YAnButton button = YAnButton.FindAnButton(serial + ".anButton1");
                YBuzzer buzzer = YBuzzer.FindBuzzer(serial + ".buzzer");
                await led.set_rgbColor(0, 1, 0x000000);
                await buzzer.set_volume(75);
                WriteLine("Place a RFID tag near the antenna");
                List<String> tagList;
                do
                {
                    tagList = await reader.get_tagIdList();
                    await YAPI.Sleep(250);
                } while (tagList.Count <= 0);


                string tagId = tagList[0];
                YRfidStatus opStatus = new YRfidStatus();
                YRfidOptions options = new YRfidOptions();
                YRfidTagInfo taginfo = await reader.get_tagInfo(tagId, opStatus);
                int blocksize = await taginfo.get_tagBlockSize();
                int firstBlock = await taginfo.get_tagFirstBlock();
                WriteLine("Tag ID          = " + await taginfo.get_tagId());
                WriteLine("Tag Memory size = " + (await taginfo.get_tagMemorySize()).ToString() + " bytes");
                WriteLine("Tag Block  size = " + (await taginfo.get_tagBlockSize()).ToString() + " bytes");

                string data = await reader.tagReadHex(tagId, firstBlock, 3 * blocksize, options, opStatus);
                if (await opStatus.get_errorCode() == YRfidStatus.SUCCESS)
                {
                    WriteLine("First 3 blocks  = " + data);
                    await led.set_rgbColor(0, 1, 0x00FF00);
                    await buzzer.pulse(1000, 100);
                } else
                {
                    WriteLine("Cannot read tag contents (" + opStatus.get_errorMessage() + ")");
                    await led.set_rgbColor(0, 1, 0xFF0000);
                }

                await led.rgb_move(0, 1, 0x000000, 200);
            } catch (YAPI_Exception ex) {
                WriteLine("error: " + ex.Message);
            }

            await YAPI.FreeAPI();
            return 0;
        }
    }
}