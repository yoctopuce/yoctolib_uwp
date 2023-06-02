/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-Buzzer
 *
 *  You can find more information on our web site:
 *   Yocto-Buzzer documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-buzzer/doc.html
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
        YLed led, led1, led2;
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
        led1 = YLed.FindLed(serial + ".led1");
        led2 = YLed.FindLed(serial + ".led2");
        button1 = YAnButton.FindAnButton(serial + ".anButton1");
        button2 = YAnButton.FindAnButton(serial + ".anButton2");

        WriteLine("press a test button");

        while (await buz.isOnline()) {
          int frequency;
          bool b1 = (await button1.get_isPressed() == YAnButton.ISPRESSED_TRUE);
          bool b2 = (await button2.get_isPressed() == YAnButton.ISPRESSED_TRUE);
          if (b1 || b2) {
            if (b1) {
              led = led1;
              frequency = 1500;
            } else {
              led = led2;
              frequency = 750;
            }

            await led.set_power(YLed.POWER_ON);
            await led.set_luminosity(100);
            await led.set_blinking(YLed.BLINKING_PANIC);
            for (int i = 0; i < 5; i++) {
              // this can be done using sequence as well
              await buz.set_frequency(frequency);
              await buz.freqMove(2 * frequency, 250);
              await YAPI.Sleep(250);
            }

            await buz.set_frequency(0);
            await led.set_power(YLed.POWER_OFF);
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