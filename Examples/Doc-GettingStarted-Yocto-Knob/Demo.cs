/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-Knob
 *
 *  You can find more information on our web site:
 *   Yocto-Knob documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-knob/doc.html
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

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YAnButton input1;
        YAnButton input5;

        if (Target.ToLower() == "any") {
          input1 = YAnButton.FirstAnButton();
          if (input1 == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await input1.get_module()).get_serialNumber();
        }

        input1 = YAnButton.FindAnButton(Target + ".anButton1");
        input5 = YAnButton.FindAnButton(Target + ".anButton5");

        while (await input1.isOnline()) {
          if (await input1.get_isPressed() == YAnButton.ISPRESSED_TRUE)
            Write("Button 1: pressed      ");
          else
            Write("Button 1: not pressed  ");
          WriteLine("- analog value:  " + await input1.get_calibratedValue());
          if (await input5.get_isPressed() == YAnButton.ISPRESSED_TRUE)
            Write("Button 5: pressed      ");
          else
            Write("Button 5: not pressed  ");
          WriteLine("- analog value:  " + await input5.get_calibratedValue());

          await YAPI.Sleep(1000);
        }

        WriteLine("Module not connected (check identification and USB cable)");
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}