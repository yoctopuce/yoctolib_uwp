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
        YLightSensor sensor;

        if (Target.ToLower() == "any") {
          sensor = YLightSensor.FirstLightSensor();
          if (sensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          sensor = YLightSensor.FindLightSensor(Target + ".lightSensor");
        }

        while (await sensor.isOnline()) {
          WriteLine("Ambient light: " + await sensor.get_currentValue() + " lx");
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