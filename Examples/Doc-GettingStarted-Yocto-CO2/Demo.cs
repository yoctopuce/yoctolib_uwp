/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-CO2
 *
 *  You can find more information on our web site:
 *   Yocto-CO2 documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-co2/doc.html
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

        YCarbonDioxide co2sensor;

        if (Target.ToLower() == "any") {
          co2sensor = YCarbonDioxide.FirstCarbonDioxide();

          if (co2sensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
          YModule ymod = await co2sensor.get_module();
          WriteLine("using " + await ymod.get_serialNumber());
        } else {
          co2sensor = YCarbonDioxide.FindCarbonDioxide(Target + ".carbonDioxide");
        }

        while (await co2sensor.isOnline()) {
          WriteLine("CO2: " + await co2sensor.get_currentValue() + " ppm");
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