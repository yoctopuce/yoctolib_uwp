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

        YPwmInput pwm;
        YPwmInput pwm1 = null;
        YPwmInput pwm2 = null;
        YModule m = null;
        if (Target.ToLower() == "any") {
          // retreive any pwm input available
          pwm = YPwmInput.FirstPwmInput();
          if (pwm == null) {
            WriteLine("No module connected");
            return -1;
          }
        } else {
          // retreive the first pwm input from the device given on command line
          pwm = YPwmInput.FindPwmInput(Target + ".pwmInput1");
        }

        // we need to retreive both channels from the device.
        if (await pwm.isOnline()) {
          m = await pwm.get_module();
          pwm1 = YPwmInput.FindPwmInput(await m.get_serialNumber() + ".pwmInput1");
          pwm2 = YPwmInput.FindPwmInput(await m.get_serialNumber() + ".pwmInput2");
        }

        while (await m.isOnline()) {
          WriteLine("PWM1: " + await pwm1.get_frequency() + " Hz " + await
                    pwm1.get_dutyCycle() +
                    " % " + await pwm1.get_pulseCounter() + " pulse edges ");
          WriteLine("PWM2: " + await pwm2.get_frequency() + " Hz " + await
                    pwm2.get_dutyCycle() +
                    " % " + await pwm2.get_pulseCounter() + " pulse edges ");
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