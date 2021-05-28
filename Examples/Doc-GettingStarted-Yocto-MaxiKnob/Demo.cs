/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  An example that show how to use a  Yocto-MaxiKnob
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiKnob documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maxiknob/doc.html
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

    static int notefreq(int note)
    {
      return (int) (220.0 * Math.Exp(note * Math.Log(2) / 12));
    }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YBuzzer buz;
        YColorLedCluster leds;
        YQuadratureDecoder qd;
        YAnButton button;

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
        leds = YColorLedCluster.FindColorLedCluster(serial + ".colorLedCluster");
        button = YAnButton.FindAnButton(serial + ".anButton1");
        qd = YQuadratureDecoder.FindQuadratureDecoder(serial + ".quadratureDecoder1");

        if ((!await button.isOnline()) || (!await qd.isOnline())) {
          WriteLine("Make sure the Yocto-MaxiBuzzer is configured with at least one anButton and one quadrature Decoder");
          return -1;
        }

        WriteLine("press a test button, or turn the encoder or hit Ctrl-C");
        int lastPos = (int) await qd.get_currentValue();
        await buz.set_volume(75);
        while (await button.isOnline()) {
          if ((await button.get_isPressed() == YAnButton.ISPRESSED_TRUE)
              && (lastPos != 0)) {
            lastPos = 0;
            await qd.set_currentValue(0);
            await buz.playNotes("'E32 C8");
            await leds.set_rgbColor(0, 1, 0x000000);
          } else {
            int p = (int) await qd.get_currentValue();
            if (lastPos != p) {
              lastPos = p;
              await buz.pulse(notefreq(p), 500);
              await leds.set_hslColor(0, 1, 0x00FF7f | (p % 255) << 16);
            }
          }
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}