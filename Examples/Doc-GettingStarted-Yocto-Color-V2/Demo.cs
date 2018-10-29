/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-Color-V2
 *
 *  You can find more information on our web site:
 *   Yocto-Color-V2 documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-color-v2/doc.html
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
    public string ColorStr { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);
        YColorLedCluster ledCluster;
        int color;
        ColorStr = ColorStr.ToUpper();

        if (ColorStr == "RED") color = 0xFF0000;
        else if (ColorStr == "GREEN") color = 0x00FF00;
        else if (ColorStr == "BLUE") color = 0x0000FF;
        else color = Convert.ToInt32("0x" + ColorStr, 16);

        if (Target.ToLower() == "any") {
          ledCluster = YColorLedCluster.FirstColorLedCluster();
          if (ledCluster == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          ledCluster = YColorLedCluster.FindColorLedCluster(Target + ".colorLedCluster");
        }

        if (await ledCluster.isOnline()) {
          //configure led cluster
          int nb_leds = 62;
          await ledCluster.set_activeLedCount(nb_leds);
          await ledCluster.set_ledType(YColorLedCluster.LEDTYPE_RGB);

          // immediate transition for fist half of leds
          WriteLine("immediate switch to " + color.ToString("x"));
          await ledCluster.set_rgbColor(0, nb_leds / 2, color);
          // immediate transition for second half of leds
          WriteLine("smooth transition to " + color.ToString("x"));
          await ledCluster.rgb_move(nb_leds / 2, nb_leds / 2, color, 2000);
        } else {
          WriteLine("Module not connected (check identification and USB cable)");
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}