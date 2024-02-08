/*********************************************************************
 *
 *  $Id: Demo.cs 58172 2023-11-30 17:10:23Z martinm $
 *
 *  An example that shows how to use a  Yocto-IO
 *
 *  You can find more information on our web site:
 *   Yocto-IO documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-io/doc.html
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
    public string RequestedVoltage { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YDigitalIO io;

        if (Target.ToLower() == "any") {
          io = YDigitalIO.FirstDigitalIO();
          if (io == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else io = YDigitalIO.FindDigitalIO(Target + ".digitalIO");

        // lets configure the channels direction
        // bits 0 and 1 as output
        // bits 2 and 3 as input
        await io.set_portDirection(0x03);
        await io.set_portPolarity(0); // polarity set to regular
        await io.set_portOpenDrain(0); // No open drain
        WriteLine("Channels 0..1 are configured as outputs and channels 2..3");
        WriteLine("are configred as inputs, you can connect some inputs to");
        WriteLine("ouputs and see what happens");
        int outputdata = 0;
        while (await io.isOnline()) {
          int inputdata = await io.get_portState(); // read port values
          string line = ""; // display port value as binary
          for (int i = 0; i < 4; i++) {
            if ((inputdata & (8 >> i)) > 0) {
              line = line + '1';
            } else {
              line = line + '0';
            }
          }

          WriteLine("port value = " + line);
          // cycle ouput 0..3
          outputdata = (outputdata + 1) % 4;
          // We could have used set_bitState as well
          await io.set_portState(outputdata);
          await YAPI.Sleep(1000);
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}