/*********************************************************************
 *
 *  $Id: Demo.cs 54356 2023-05-04 07:15:58Z seb $
 *
 *  An example that show how to use a  Yocto-MaxiCoupler
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiCoupler documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maxicoupler/doc.html
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
    public string Channel { get; set; }

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

          Target = await (await relay.get_module()).get_serialNumber();
        }

        WriteLine("using " + Target + ".relay" + Channel);
        relay = YRelay.FindRelay(Target + ".relay" + Channel);

        if (await relay.isOnline()) {
          if (RequestedState.ToUpper() == "ON")
            await relay.set_output(YRelay.OUTPUT_ON);
          else
            await relay.set_output(YRelay.OUTPUT_OFF);
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