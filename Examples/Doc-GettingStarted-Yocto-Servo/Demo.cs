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
    public string Position { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YServo servo1;
        YServo servo5;

        if (Target.ToLower() == "any") {
          servo1 = YServo.FirstServo();
          if (servo1 == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await servo1.get_module()).get_serialNumber();
        }

        servo1 = YServo.FindServo(Target + ".servo1");
        servo5 = YServo.FindServo(Target + ".servo5");
        int pos = Convert.ToInt32(Position);
        if (await servo1.isOnline()) {
          await servo1.set_position(pos);
          await servo5.move(pos, 3000);
        } else {
          WriteLine("Module not connected (check identification and USB cable)");
        }
        WriteLine("Done.");
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}