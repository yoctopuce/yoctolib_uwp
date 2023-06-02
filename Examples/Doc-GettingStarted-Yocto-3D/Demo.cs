/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-3D
 *
 *  You can find more information on our web site:
 *   Yocto-3D documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-3d/doc.html
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

        YTilt anytilt, tilt1, tilt2;
        YCompass compass;
        YAccelerometer accel;
        YGyro gyro;

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
        compass = YCompass.FindCompass(serial + ".compass");
        accel = YAccelerometer.FindAccelerometer(serial + ".accelerometer");
        gyro = YGyro.FindGyro(serial + ".gyro");
        int count = 0;

        while (await tilt1.isOnline()) {
          if (count++ % 10 == 0) {
            WriteLine("tilt1   tilt2   compass   acc   gyro");
          }
          Write(await tilt1.get_currentValue() + "\t");
          Write(await tilt2.get_currentValue() + "\t");
          Write(await compass.get_currentValue() + "\t");
          Write(await accel.get_currentValue() + "\t");
          WriteLine("" + await gyro.get_currentValue());
          await YAPI.Sleep(250);
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