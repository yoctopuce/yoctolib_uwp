/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-Amp
 *
 *  You can find more information on our web site:
 *   Yocto-Amp documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-amp/doc.html
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
        YCurrent sensor;
        YCurrent sensorDC = null;
        YCurrent sensorAC = null;

        if (Target.ToLower() == "any") {
          // retreive any voltage sensor (can be AC or DC)
          sensor = YCurrent.FirstCurrent();
          if (sensor == null) {
            WriteLine("No module connected");
            return -1;
          }
          Target = await (await sensor.get_module()).get_serialNumber();
        }
        WriteLine("using " + Target);
        // we need to retreive both DC and AC from the device.
        sensorDC = YCurrent.FindCurrent(Target + ".current1");
        sensorAC = YCurrent.FindCurrent(Target + ".current2");

        while (await sensorDC.isOnline()) {
          Write("DC: " + await sensorDC.get_currentValue() + " mA / ");
          WriteLine("AC: " + await sensorAC.get_currentValue() + " mA ");
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