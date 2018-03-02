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

        YWeighScale sensor;

        if (Target.ToLower() == "any") {
          sensor = YWeighScale.FirstWeighScale();
          if (sensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
          YModule m = await sensor.get_module();
          WriteLine("Using: " + await m.get_serialNumber());
        } else {
          sensor = YWeighScale.FindWeighScale(Target + ".weighScale1");
        }

        if (await sensor.isOnline()) {
          string unit = "";
          // On startup, enable excitation and tare weigh scale
          WriteLine("Resetting tare weight...");
          await sensor.set_excitation(YWeighScale.EXCITATION_AC);
          await YAPI.Sleep(3000);
          await sensor.tare();
          unit = await sensor.get_unit();

          // Show measured weight continuously
          while (await sensor.isOnline()) {
            WriteLine("Weight : " + await sensor.get_currentValue() + unit);
            await YAPI.Sleep(1000);
          }
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}