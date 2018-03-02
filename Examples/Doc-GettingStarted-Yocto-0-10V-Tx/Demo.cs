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
    public string RequestedVoltage { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YVoltageOutput vout1;
        YVoltageOutput vout2;
        double voltage;

        voltage = Convert.ToDouble(RequestedVoltage);
        if (Target.ToLower() == "any") {
          vout1 = YVoltageOutput.FirstVoltageOutput();
          if (vout1 == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await vout1.get_module()).get_serialNumber();
        }

        vout1 = YVoltageOutput.FindVoltageOutput(Target + ".voltageOutput1");
        vout2 = YVoltageOutput.FindVoltageOutput(Target + ".voltageOutput2");

        if (await vout1.isOnline()) {
          WriteLine("output 1 : immediate change to " + voltage);
          await vout1.set_currentVoltage(voltage);
          WriteLine("output 2 : immediate smooth change to " + voltage);
          await vout2.voltageMove(voltage, 3000);
        } else {
          WriteLine("Module not connected (check identification and USB cable)");
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}