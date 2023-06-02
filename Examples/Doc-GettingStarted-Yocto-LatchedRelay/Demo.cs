/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-LatchedRelay
 *
 *  You can find more information on our web site:
 *   Yocto-LatchedRelay documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-latchedrelay/doc.html
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
    public string RequestedState { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YRelay relay;

        if (Target.ToLower() == "any") {
          relay = YRelay.FirstRelay();
          if (relay == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          relay = YRelay.FindRelay(Target + ".relay1");
        }

        if (await relay.isOnline()) {
          if (RequestedState.ToUpper() == "A") {
            await relay.set_state(YRelay.STATE_A);
          } else {
            await relay.set_state(YRelay.STATE_B);
          }
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