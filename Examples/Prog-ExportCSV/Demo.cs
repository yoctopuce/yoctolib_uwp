/*********************************************************************
 *
 *  $Id: Demo.cs 38154 2019-11-14 15:24:55Z seb $
 *
 *  Doc-Inventory example
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

        public override async Task<int> Run()
        {
            YModule m;
            DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            try {
                await YAPI.RegisterHub(HubURL);

                // Enumerate all connected sensors
                YSensor sensor;
                List<YSensor> sensorList = new List<YSensor>();
                sensor = YSensor.FirstSensor();
                while (sensor != null) {
                    sensorList.Add(sensor);
                    sensor = sensor.nextSensor();
                }

                if (sensorList.Count == 0) {
                    WriteLine("No Yoctopuce sensor connected (check USB cable)");
                } else {
                    // Generate consolidated CSV output for all sensors
                    YConsolidatedDataSet data = new YConsolidatedDataSet(0, 0, sensorList);
                    List<Double> record = new List<Double>();
                    while (await data.nextRecord(record) < 100) {
                        string line = _epoch.AddSeconds(record[0]).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                        for (int i = 1; i < record.Count; i++) {
                            line += String.Format(";{0:0.000}", record[i]);
                        }
                        WriteLine(line);
                    }
                }
            } catch (YAPI_Exception ex) {
                WriteLine("Error:" + ex.Message);
            }


            YAPI.FreeAPI();
            return 0;
        }
    }
}