/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  An example that shows how to use a  Yocto-Spectral
 *
 *  You can find more information on our web site:
 *   Yocto-Spectral documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-spectral/doc.html
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

                YColorSensor colorSensor;

                int value = Convert.ToInt32(Value);

                if (Target.ToLower() == "any") {
                    colorSensor = YColorSensor.FirstColorSensor();
                    if (colorSensor == null) {
                        WriteLine("No module connected (check USB cable) ");
                        return -1;
                    }

                    Target = await (await colorSensor.get_module()).get_serialNumber();
                }

                colorSensor = YColorSensor.FindColorSensor(Target + ".colorSensor");
                if (await colorSensor.isOnline()) {
                    await colorSensor.set_workingMode(YColorSensor.WORKINGMODE_AUTO);
                    await colorSensor.set_estimationModel(YColorSensor.ESTIMATIONMODEL_REFLECTION);
                    WriteLine("Module : " + Target);
                    WriteLine("Near estimated color : " + await colorSensor.get_nearSimpleColor());
                    WriteLine("HEX estimated : #" + await colorSensor.get_estimatedRGB());
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