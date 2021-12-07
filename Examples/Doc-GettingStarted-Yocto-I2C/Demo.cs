/*********************************************************************
 *
 *  $Id: Demo.cs 46632 2021-09-28 08:44:25Z web $
 *
 *  An example that show how to use a  Yocto-I2C
 *
 *  You can find more information on our web site:
 *   Yocto-I2C documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-i2c/doc.html
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

        YI2cPort i2cPort;

        int value = Convert.ToInt32(Value);

        if (Target.ToLower() == "any") {
          i2cPort = YI2cPort.FirstI2cPort();
          if (i2cPort == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await i2cPort.get_module()).get_serialNumber();
        }

        i2cPort = YI2cPort.FindI2cPort(Target + ".i2cPort");
        if (await i2cPort.isOnline()) {
          await i2cPort.set_i2cMode("400kbps");
          await i2cPort.set_i2cVoltageLevel(YI2cPort.I2CVOLTAGELEVEL_3V3);
          await i2cPort.reset();
          // do not forget to configure the powerOutput and
          // of the Yocto-I2C as well if used
          WriteLine("****************************");
          WriteLine("* make sure voltage levels *");
          WriteLine("* are properly configured  *");
          WriteLine("****************************");

          List<int> toSend = new List<int>(new int[] { 0x05 });
          List<int> received = await i2cPort.i2cSendAndReceiveArray(0x1f, toSend, 2);
          int tempReg = (received[0] << 8) + received[1];
          if ((tempReg & 0x1000) != 0) {
            tempReg -= 0x2000;    // perform sign extension
          } else {
            tempReg &= 0x0fff;    // clear status bits
          }
          WriteLine("Ambiant temperature: " + String.Format("{0:0.000}", (tempReg / 16.0)));
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