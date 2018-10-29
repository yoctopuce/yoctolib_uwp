/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-4-20mA-Tx
 *
 *  You can find more information on our web site:
 *   Yocto-4-20mA-Tx documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-4-20ma-tx/doc.html
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
    public string LoopCurrent { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);
        YCurrentLoopOutput loop;
        double value = Convert.ToDouble(LoopCurrent);

        if (Target.ToLower() == "any") {
          loop = YCurrentLoopOutput.FirstCurrentLoopOutput();
          if (loop == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          loop = YCurrentLoopOutput.FindCurrentLoopOutput(
                   Target + ".currentLoopOutput");
        }

        if (await loop.isOnline()) {
          await loop.set_current(value);
          int loopPower = await loop.get_loopPower();
          if (loopPower == YCurrentLoopOutput.LOOPPOWER_NOPWR) {
            WriteLine("Current loop not powered");
          } else if (loopPower == YCurrentLoopOutput.LOOPPOWER_LOWPWR) {
            WriteLine("Insufficient voltage on current loop");
          } else {
            WriteLine("current loop set to " + value + " mA");
          }
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