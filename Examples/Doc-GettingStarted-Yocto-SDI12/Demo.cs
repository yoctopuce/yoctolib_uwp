/*********************************************************************
 *
 *  $Id: Demo.cs 63137 2024-10-31 09:38:54Z tiago $
 *
 *  Doc-GettingStarted-Yocto-SDI12 example
 *
 *  You can find more information on our web site:
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

        YSdi12Port sdi12Port;

        int value = Convert.ToInt32(Value);

        if (Target.ToLower() == "any") {
          sdi12Port = YSdi12Port.FirstSdi12Port();
          if (sdi12Port == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await sdi12Port.get_module()).get_serialNumber();
        }

        sdi12Port = YSdi12Port.FindSdi12Port(Target + ".sdi12Port");
        if (await sdi12Port.isOnline()) {
          YSdi12SensorInfo singleSensor = await sdi12Port.discoverSingleSensor();
          WriteLine("Module : " + Target);
          WriteLine("Sensor address : " + await singleSensor.get_sensorAddress()) ;
          WriteLine("Sensor SDI-12 compatibility : " + await
                    singleSensor.get_sensorProtocol());
          WriteLine("Sensor company name : " + await singleSensor.get_sensorVendor());
          WriteLine("Sensor model number : " + await singleSensor.get_sensorModel());
          WriteLine("Sensor version : " + await singleSensor.get_sensorVersion());
          WriteLine("Sensor serial number : " + await singleSensor.get_sensorSerial());
          List<double> valSensor = await sdi12Port.readSensor(await
                                   singleSensor.get_sensorAddress(), "M", 5000);

          for (int i = 0; i < valSensor.Count; i++) {
            if (await singleSensor .get_measureCount() > 1) {
              WriteLine(String.Format("{0} : {1,-8:0.00} {2,-10} ({3})",
                                      await singleSensor.get_measureSymbol(i), valSensor[i],
                                      await singleSensor.get_measureUnit(i),
                                      await singleSensor.get_measureDescription(i)));
            } else {
              WriteLine(valSensor[i].ToString());
            }
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