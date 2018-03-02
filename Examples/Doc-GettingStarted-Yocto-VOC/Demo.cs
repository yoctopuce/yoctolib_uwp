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
        YVoc vocsensor;
        // Setup the API to use local USB devices
        await YAPI.RegisterHub(HubURL);

        if (Target.ToLower() == "any") {
          vocsensor = YVoc.FirstVoc();

          if (vocsensor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          YModule m = await vocsensor.get_module();
          WriteLine("using " + await m.get_serialNumber());
        } else {
          vocsensor = YVoc.FindVoc(Target + ".voc");
        }

        while (await vocsensor.isOnline()) {
          WriteLine("VOC: " + await vocsensor.get_currentValue() + " ppm");
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