/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  An example that show how to use a  Yocto-Temperature-IR
 *
 *  You can find more information on our web site:
 *   Yocto-Temperature-IR documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-temperature-ir/doc.html
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

          Target = await (await tsensor.get_module()).get_serialNumber();
        }

        WriteLine("Using: " + Target);

        // retreive both channels
        YTemperature ch1 = YTemperature.FindTemperature(Target + ".temperature1");
        YTemperature ch2 = YTemperature.FindTemperature(Target + ".temperature2");

        while (await ch2.isOnline()) {
          Write("Ambiant: " + await ch1.get_currentValue() + " °C  ");
          WriteLine("Infrared: " + await ch2.get_currentValue() + " °C  ");
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