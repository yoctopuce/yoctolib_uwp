/*********************************************************************
 *
 *  $Id: Demo.cs 58172 2023-11-30 17:10:23Z martinm $
 *
 *  An example that shows how to use a  Yocto-Motor-DC
 *
 *  You can find more information on our web site:
 *   Yocto-Motor-DC documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-motor-dc/doc.html
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
    public string Power { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);
        YMotor motor;
        YCurrent current;
        YVoltage voltage;
        YTemperature temperature;

        if (Target.ToLower() == "any") {
          // find the serial# of the first available motor
          motor = YMotor.FirstMotor();
          if (motor == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await motor.get_module()).get_serialNumber();
        }

        int power = Convert.ToInt32(Power);
        motor = YMotor.FindMotor(Target + ".motor");
        current = YCurrent.FindCurrent(Target + ".current");
        voltage = YVoltage.FindVoltage(Target + ".voltage");
        temperature = YTemperature.FindTemperature(Target + ".temperature");

        // lets start the motor
        if (await motor.isOnline()) {
          // if motor is in error state, reset it.
          if (await motor.get_motorStatus() >= YMotor.MOTORSTATUS_LOVOLT) {
            await motor.resetStatus();
          }

          await motor.drivingForceMove(power, 2000); // ramp up to power in 2 seconds
          while (await motor.isOnline()) {
            // display motor status
            WriteLine("Status=" + await motor.get_advertisedValue() + "  "
                      + "Voltage=" + await voltage.get_currentValue() + "V  "
                      + "Current=" + await current.get_currentValue() / 1000 + "A  "
                      + "Temp=" + await temperature.get_currentValue() + "deg C");
            await YAPI.Sleep(1000); // wait for one second
          }
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}