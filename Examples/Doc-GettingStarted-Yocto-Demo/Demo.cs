/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-Demo
 *
 *  You can find more information on our web site:
 *   Yocto-Demo documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-demo/doc.html
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
    public string OnOff { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);
        YLed led;
        if (Target.ToLower() == "any") {
          led = YLed.FirstLed();
          if (led == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          led = YLed.FindLed(Target + ".led");
        }

        if (await led.isOnline()) {
          if (OnOff.ToUpper() == "ON") {
            await led.set_power(YLed.POWER_ON);
          } else {
            await led.set_power(YLed.POWER_OFF);
          }
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