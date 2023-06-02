/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-RangeFinder
 *
 *  You can find more information on our web site:
 *   Yocto-RangeFinder documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-rangefinder/doc.html
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

        YRangeFinder rf;
        YLightSensor ir;
        YTemperature tmp;

        if (Target.ToLower() == "any") {
          rf = YRangeFinder.FirstRangeFinder();
          if (rf == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await rf.get_module()).get_serialNumber();
        } else {
          rf = YRangeFinder.FindRangeFinder(Target + ".rangefinder1");
        }

        ir = YLightSensor.FindLightSensor(Target + ".lightSensor1");
        tmp = YTemperature.FindTemperature(Target + ".temperature1");

        while (await rf.isOnline()) {
          WriteLine("Distance    : " + await rf.get_currentValue());
          WriteLine("Ambiant IR  : " + await ir.get_currentValue());
          WriteLine("Temperature : " + await tmp.get_currentValue());
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