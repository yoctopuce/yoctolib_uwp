/*********************************************************************
 *
 *  $Id: Demo.cs 58172 2023-11-30 17:10:23Z martinm $
 *
 *  An example that shows how to use a  Yocto-milliVolt-Rx-BNC
 *
 *  You can find more information on our web site:
 *   Yocto-milliVolt-Rx-BNC documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-millivolt-rx-bnc/doc.html
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

        YGenericSensor sensor;

        if (Target.ToLower() == "any") {
          sensor = YGenericSensor.FirstGenericSensor();

          if (sensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          sensor = YGenericSensor.FindGenericSensor(Target + ".genericSensor1");
        }

        string unitSensor1 = "";
        if (await sensor.isOnline()) {
          unitSensor1 = await sensor.get_unit();
        }

        while (await sensor.isOnline()) {
          WriteLine("Value: " + await sensor.get_currentValue() + unitSensor1);
          await YAPI.Sleep(1000);
        }
        WriteLine("Module not connected (check identification and USB cable)");
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}