/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-PWM-Tx
 *
 *  You can find more information on our web site:
 *   Yocto-PWM-Tx documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-pwm-tx/doc.html
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
    public string RequestedFrequency { get; set; }
    public string RequestedDutyCycle { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YPwmOutput pwmoutput1;
        YPwmOutput pwmoutput2;
        int frequency;
        double dutyCycle;

        frequency = Convert.ToInt32(RequestedFrequency);
        dutyCycle = Convert.ToDouble(RequestedDutyCycle);

        if (Target.ToLower() == "any") {
          pwmoutput1 = YPwmOutput.FirstPwmOutput();
          if (pwmoutput1 == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await pwmoutput1.get_module()).get_serialNumber();
        }

        pwmoutput1 = YPwmOutput.FindPwmOutput(Target + ".pwmOutput1");
        pwmoutput2 = YPwmOutput.FindPwmOutput(Target + ".pwmOutput2");

        if (await pwmoutput1.isOnline()) {
          // output 1 : immediate change
          await pwmoutput1.set_frequency(frequency);
          await pwmoutput1.set_enabled(YPwmOutput.ENABLED_TRUE);
          await pwmoutput1.set_dutyCycle(dutyCycle);
          // output 2 : smooth change
          await pwmoutput2.set_frequency(frequency);
          await pwmoutput2.set_enabled(YPwmOutput.ENABLED_TRUE);
          await pwmoutput2.dutyCycleMove(dutyCycle, 3000);
          WriteLine("done");
        } else {
          WriteLine("Module not connected (check identification and USB cable)");
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}