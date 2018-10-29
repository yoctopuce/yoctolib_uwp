/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-Temperature
 *
 *  You can find more information on our web site:
 *   Yocto-Temperature documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-temperature/doc.html
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

        YTemperature tsensor;

        if (Target.ToLower() == "any") {
          tsensor = YTemperature.FirstTemperature();

          if (tsensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          tsensor = YTemperature.FindTemperature(Target + ".temperature");
        }

        while (await tsensor.isOnline()) {
          WriteLine("Temperature: " + await tsensor.get_currentValue() + " °C");
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