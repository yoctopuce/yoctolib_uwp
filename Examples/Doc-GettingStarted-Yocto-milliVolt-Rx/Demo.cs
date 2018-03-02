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

        YGenericSensor sensor;

        if (Target.ToLower() == "any") {
          sensor = YGenericSensor.FirstGenericSensor();

          if (sensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          sensor = YGenericSensor.FindGenericSensor(Target + ".genericSensor1");
        }

        string unitSensor1 = "";
        if (await sensor.isOnline()) {
          unitSensor1 = await sensor.get_unit();
        }

        while (await sensor.isOnline()) {
          WriteLine("Value: " + await sensor.get_currentValue() + unitSensor1);
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