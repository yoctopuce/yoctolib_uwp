/*********************************************************************
 *
 *  $Id: Demo.cs 58172 2023-11-30 17:10:23Z martinm $
 *
 *  An example that shows how to use a  Yocto-4-20mA-Rx
 *
 *  You can find more information on our web site:
 *   Yocto-4-20mA-Rx documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-4-20ma-rx/doc.html
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
        YGenericSensor tsensor;

        if (Target.ToLower() == "any") {
          tsensor = YGenericSensor.FirstGenericSensor();

          if (tsensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await tsensor.get_module()).get_serialNumber();
        }

        WriteLine("Using: " + Target);

        // retreive both channels
        YGenericSensor ch1, ch2;
        ch1 = YGenericSensor.FindGenericSensor(Target + ".genericSensor1");
        ch2 = YGenericSensor.FindGenericSensor(Target + ".genericSensor2");

        string unitSensor1 = "", unitSensor2 = "";
        if (await ch1.isOnline()) {
          unitSensor1 = await ch1.get_unit();
        }

        if (await ch2.isOnline()) {
          unitSensor2 = await ch2.get_unit();
        }

        while (await ch1.isOnline() && await ch2.isOnline()) {
          Write("Channel 1: " + await ch1.get_currentValue() + unitSensor1);
          WriteLine(" Channel 2: " + await ch2.get_currentValue() + unitSensor2);
          await YAPI.Sleep(1000);
        }

        await YAPI.FreeAPI();
        WriteLine("Module not connected (check identification and USB cable)");
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}