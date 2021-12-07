/*********************************************************************
 *
 *  $Id: Demo.cs 46632 2021-09-28 08:44:25Z web $
 *
 *  An example that show how to use a  Yocto-Pressure
 *
 *  You can find more information on our web site:
 *   Yocto-Pressure documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-pressure/doc.html
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

        YPressure psensor;

        if (Target.ToLower() == "any") {
          psensor = YPressure.FirstPressure();

          if (psensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          psensor = YPressure.FindPressure(Target + ".pressure");
        }

        while (await psensor.isOnline()) {
          WriteLine("Pressure: " + await psensor.get_currentValue() + " mbar");
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