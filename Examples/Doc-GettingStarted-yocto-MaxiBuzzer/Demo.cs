/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-MaxiBuzzer
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiBuzzer documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maxibuzzer/doc.html
 *   uwp API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-uwp-EN.html
 *
 *********************************************************************/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using com.yoctopuce.YoctoAPI;

namespace Demo
{
  public class Demo : DemoBase
  {
    public string HubURL { get; set; }
    public string Target { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YBuzzer buz;
        YColorLed led;
        YAnButton button1, button2;

        if (Target.ToLower() == "any") {
          buz = YBuzzer.FirstBuzzer();
          if (buz == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else buz = YBuzzer.FindBuzzer(Target + ".buzzer");

        if (!await buz.isOnline()) {
          WriteLine("Module not connected (check identification and USB cable)");
          return -1;
        }

        string serial = await (await buz.get_module()).get_serialNumber();
        led = YColorLed.FindColorLed(serial + ".colorLed");
        button1 = YAnButton.FindAnButton(serial + ".anButton1");
        button2 = YAnButton.FindAnButton(serial + ".anButton2");

        WriteLine("press a test button");

        while (await buz.isOnline()) {
          int frequency, volume, color;
          bool b1 = (await button1.get_isPressed() == YAnButton.ISPRESSED_TRUE);
          bool b2 = (await button2.get_isPressed() == YAnButton.ISPRESSED_TRUE);
          if (b1 || b2) {
            if (b1) {
              volume = 60;
              frequency = 1500;
              color = 0xff0000;
            } else {
              volume = 30;
              color = 0x00ff00;
              frequency = 750;
            }

            await led.resetBlinkSeq();
            await led.addRgbMoveToBlinkSeq(color, 100);
            await led.addRgbMoveToBlinkSeq(0, 100);
            await led.startBlinkSeq();
            await buz.set_volume(volume);
            for (int i = 0; i < 5; i++) {
              // this can be done using sequence as well
              await buz.set_frequency(frequency);
              await buz.freqMove(2 * frequency, 250);
              await YAPI.Sleep(250);
            }

            await buz.set_frequency(0);
            await led.stopBlinkSeq();
            await led.set_rgbColor(0);
          }
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}