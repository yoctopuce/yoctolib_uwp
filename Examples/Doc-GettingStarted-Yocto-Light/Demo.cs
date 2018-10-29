/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-Light
 *
 *  You can find more information on our web site:
 *   Yocto-Light documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-light/doc.html
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
        YLightSensor sensor;

        if (Target.ToLower() == "any") {
          sensor = YLightSensor.FirstLightSensor();
          if (sensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          sensor = YLightSensor.FindLightSensor(Target + ".lightSensor");
        }

        while (await sensor.isOnline()) {
          WriteLine("Ambient light: " + await sensor.get_currentValue() + " lx");
          await YAPI.Sleep(1000);
        }

        WriteLine("Module not connected (check identification and USB cable)");
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}