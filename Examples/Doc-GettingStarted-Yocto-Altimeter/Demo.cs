/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-Altimeter
 *
 *  You can find more information on our web site:
 *   Yocto-Altimeter documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-altimeter/doc.html
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
        YAltitude asensor;
        YTemperature tsensor;
        YPressure psensor;

        if (Target.ToLower() == "any") {
          asensor = YAltitude.FirstAltitude();
          tsensor = YTemperature.FirstTemperature();
          psensor = YPressure.FirstPressure();

          if ((asensor == null) || (tsensor == null) || (psensor == null)) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          asensor = YAltitude.FindAltitude(Target + ".altitude");
          tsensor = YTemperature.FindTemperature(Target + ".temperature");
          psensor = YPressure.FindPressure(Target + ".pressure");
        }
        while (await asensor.isOnline()) {
          WriteLine("Altitude:    " + await asensor.get_currentValue() + " m " +
                    "(QNH=" +
                    await asensor.get_qnh() + " hPa)");
          WriteLine("Temperature: " + await tsensor.get_currentValue() + " °C");
          WriteLine("Pressure:    " + await psensor.get_currentValue() + " hPa");
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