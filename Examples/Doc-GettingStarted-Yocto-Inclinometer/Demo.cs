/*********************************************************************
 *
 *  $Id: Demo.cs 46632 2021-09-28 08:44:25Z web $
 *
 *  An example that show how to use a  Yocto-Inclinometer
 *
 *  You can find more information on our web site:
 *   Yocto-Inclinometer documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-inclinometer/doc.html
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

        YTilt anytilt, tilt1, tilt2, tilt3;

        if (Target.ToLower() == "any") {
          anytilt = YTilt.FirstTilt();
          if (anytilt == null) {
            WriteLine("No module connected (check USB cable)");
            return -1;
          }
        } else {
          anytilt = YTilt.FindTilt(Target + ".tilt1");
        }

        string serial = await (await anytilt.get_module()).get_serialNumber();
        tilt1 = YTilt.FindTilt(serial + ".tilt1");
        tilt2 = YTilt.FindTilt(serial + ".tilt2");
        tilt3 = YTilt.FindTilt(serial + ".tilt3");
        int count = 0;

        while (await tilt1.isOnline()) {
          if (count++ % 10 == 0) {
            WriteLine("tilt1   tilt2   tilt3");
          }
          Write(await tilt1.get_currentValue() + "\t");
          Write(await tilt2.get_currentValue() + "\t");
          WriteLine("" + await tilt3.get_currentValue());
          await YAPI.Sleep(250);
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