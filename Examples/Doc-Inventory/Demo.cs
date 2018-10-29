/*********************************************************************
 *
 *  $Id: Demo.cs 32629 2018-10-10 13:38:20Z seb $
 *
 *  Doc-Inventory example
 *
 *  You can find more information on our web site:
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

    public override async Task<int> Run()
    {
      YModule m;
      try {
        await YAPI.RegisterHub(HubURL);

        WriteLine("Device list");
        m = YModule.FirstModule();
        while (m != null) {
          WriteLine(await m.get_serialNumber()
                    + " (" + await m.get_productName() + ")");
          m = m.nextModule();
        }
      } catch (YAPI_Exception ex) {
        WriteLine("Error:" + ex.Message);
      }
      YAPI.FreeAPI();
      return 0;
    }
  }
}