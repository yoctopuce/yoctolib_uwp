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
        YColorLed led1;
        YColorLed led2;
        int color;
        ColorStr = ColorStr.ToUpper();

        if (ColorStr == "RED") color = 0xFF0000;
        else if (ColorStr == "GREEN") color = 0x00FF00;
        else if (ColorStr == "BLUE") color = 0x0000FF;
        else color = Convert.ToInt32("0x" + ColorStr, 16);

        if (Target.ToLower() == "any") {
          led1 = YColorLed.FirstColorLed();
          if (led1 == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          led2 = led1.nextColorLed();
        } else {
          led1 = YColorLed.FindColorLed(Target + ".colorLed1");
          led2 = YColorLed.FindColorLed(Target + ".colorLed2");
        }

        if (await led1.isOnline()) {
          WriteLine("immediate switch to " + color.ToString("x"));
          await led1.set_rgbColor(color);
          WriteLine("smooth transition to " + color.ToString("x"));
          await led2.rgbMove(color, 1000);
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