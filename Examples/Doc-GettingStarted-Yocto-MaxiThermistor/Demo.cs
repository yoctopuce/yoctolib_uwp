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

        YTemperature tsensor;

        if (Target.ToLower() == "any") {
          tsensor = YTemperature.FirstTemperature();

          if (tsensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await tsensor.get_module()).get_serialNumber();
        }

        WriteLine("Using: " + Target);
        // retreive all 6 channels
        YTemperature ch1 = YTemperature.FindTemperature(Target + ".temperature1");
        YTemperature ch2 = YTemperature.FindTemperature(Target + ".temperature2");
        YTemperature ch3 = YTemperature.FindTemperature(Target + ".temperature3");
        YTemperature ch4 = YTemperature.FindTemperature(Target + ".temperature4");
        YTemperature ch5 = YTemperature.FindTemperature(Target + ".temperature5");
        YTemperature ch6 = YTemperature.FindTemperature(Target + ".temperature6");

        while (await ch2.isOnline()) {
          Write("| 1: " + (await ch1.get_currentValue()).ToString(" 0.0"));
          Write(" | 2: " + (await ch2.get_currentValue()).ToString(" 0.0"));
          Write(" | 3: " + (await ch3.get_currentValue()).ToString(" 0.0"));
          Write(" | 4: " + (await ch4.get_currentValue()).ToString(" 0.0"));
          Write(" | 5: " + (await ch5.get_currentValue()).ToString(" 0.0"));
          Write(" | 6: " + (await ch6.get_currentValue()).ToString(" 0.0"));
          WriteLine("|  °C  ");
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