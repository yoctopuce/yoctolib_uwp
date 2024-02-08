/*********************************************************************
 *
 *  $Id: Demo.cs 58172 2023-11-30 17:10:23Z martinm $
 *
 *  An example that shows how to use a  Yocto-SPI
 *
 *  You can find more information on our web site:
 *   Yocto-SPI documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-spi/doc.html
 *   uwp API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-uwp-EN.html
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
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
    public string Value { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YSpiPort spiPort;

        int value = Convert.ToInt32(Value);

        if (Target.ToLower() == "any") {
          spiPort = YSpiPort.FirstSpiPort();
          if (spiPort == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await spiPort.get_module()).get_serialNumber();
        }

        spiPort = YSpiPort.FindSpiPort(Target + ".spiPort");
        if (await spiPort.isOnline()) {
          await spiPort.set_spiMode("250000,3,msb");
          await spiPort.set_ssPolarity(YSpiPort.SSPOLARITY_ACTIVE_LOW);
          await spiPort.set_protocol("Frame:5ms");
          await spiPort.reset();
          // do not forget to configure the powerOutput of the Yocto-SPI
          // ( for SPI7SEGDISP8.56 powerOutput need to be set at 5v )

          await spiPort.writeHex("0c01"); // Exit from shutdown state
          await spiPort.writeHex("09ff"); // Enable BCD for all digits
          await spiPort.writeHex("0b07"); // Enable digits 0-7 (=8 in total)
          await spiPort.writeHex("0a0a"); // Set medium brightness
          for (int i = 1; i <= 8; i++) {
            int digit = value % 10; // digit value
            await spiPort.writeArray(new List<int> {i, digit});
            value = value / 10;
          }
          WriteLine("Done.");
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