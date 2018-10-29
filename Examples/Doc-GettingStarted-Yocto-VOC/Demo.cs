/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  An example that show how to use a  Yocto-VOC
 *
 *  You can find more information on our web site:
 *   Yocto-VOC documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-voc/doc.html
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