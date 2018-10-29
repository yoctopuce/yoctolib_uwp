/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-MaxiBridge
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiBridge documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maxibridge/doc.html
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

        YMultiCellWeighScale sensor;

        if (Target.ToLower() == "any") {
          sensor = YMultiCellWeighScale.FirstMultiCellWeighScale();
          if (sensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
          YModule ymod = await sensor.get_module();
          WriteLine("Using: " + await ymod.get_serialNumber());
        } else {
          sensor = YMultiCellWeighScale.FindMultiCellWeighScale(Target +
                   ".multiCellWeighScale");
        }

        if (await sensor.isOnline()) {
          string unit = "";
          // On startup, enable excitation and tare weigh scale
          WriteLine("Resetting tare weight...");
          await sensor.set_excitation(YMultiCellWeighScale.EXCITATION_AC);
          await YAPI.Sleep(3000);
          await sensor.tare();
          unit = await sensor.get_unit();

          // Show measured weight continuously
          while (await sensor.isOnline()) {
            Write("Weight : " + await sensor.get_currentValue() + unit);
            await YAPI.Sleep(1000);
          }
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