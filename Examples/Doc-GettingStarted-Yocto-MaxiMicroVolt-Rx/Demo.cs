/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-MaxiMicroVolt-Rx
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiMicroVolt-Rx documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maximicrovolt-rx/doc.html
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

          YModule m = await tsensor.get_module();
          Target = await m.get_serialNumber();
        }

        // retreive module serial
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
          Write("Channel 1 : " + await ch1.get_currentValue() + unitSensor1);
          Write("  Channel 2 : " + await ch2.get_currentValue() + unitSensor2);
          WriteLine("  (press Ctrl-C to exit)");
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