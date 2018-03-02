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

        YVoltage sensor;
        YVoltage sensorDC = null;
        YVoltage sensorAC = null;

        if (Target.ToLower() == "any") {
          // retreive any voltage sensor (can be AC or DC)
          sensor = YVoltage.FirstVoltage();
          if (sensor == null) {
            WriteLine("No module connected");
            return -1;
          }

          Target = await (await sensor.get_module()).get_serialNumber();
        }

        WriteLine("using " + Target);
        // we need to retreive both DC and AC voltage from the device.
        sensorDC = YVoltage.FindVoltage(Target + ".voltage1");
        sensorAC = YVoltage.FindVoltage(Target + ".voltage2");

        while (await sensorDC.isOnline()) {
          Write("DC: " + await sensorDC.get_currentValue() + " v ");
          WriteLine("AC: " + await sensorAC.get_currentValue() + " v ");
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