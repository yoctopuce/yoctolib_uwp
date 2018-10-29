/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-Meteo
 *
 *  You can find more information on our web site:
 *   Yocto-Meteo documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-meteo/doc.html
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

        YHumidity hsensor;
        YTemperature tsensor;
        YPressure psensor;

        if (Target.ToLower() == "any") {
          hsensor = YHumidity.FirstHumidity();
          tsensor = YTemperature.FirstTemperature();
          psensor = YPressure.FirstPressure();

          if ((hsensor == null) || (tsensor == null) || (psensor == null)) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          hsensor = YHumidity.FindHumidity(Target + ".humidity");
          tsensor = YTemperature.FindTemperature(Target + ".temperature");
          psensor = YPressure.FindPressure(Target + ".pressure");
        }

        while (await hsensor.isOnline()) {
          WriteLine("Humidity:    " + await hsensor.get_currentValue() + " %RH");
          WriteLine("Temperature: " + await tsensor.get_currentValue() + " °C");
          WriteLine("Pressure:    " + await psensor.get_currentValue() + " hPa");
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