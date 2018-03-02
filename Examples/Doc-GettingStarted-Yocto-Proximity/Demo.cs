﻿using System;
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
        await YAPI.RegisterHub(HubURL);
        YLightSensor ir, al;
        YProximity p;

        if (Target.ToLower() == "any") {
          p = YProximity.FirstProximity();
          if (p == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }

          Target = await (await p.get_module()).get_serialNumber();
        } else p = YProximity.FindProximity(Target + ".proximity1");

        al = YLightSensor.FindLightSensor(Target + ".lightSensor1");
        ir = YLightSensor.FindLightSensor(Target + ".lightSensor2");

        while (await p.isOnline()) {
          Write("Proximity: " + await p.get_currentValue());
          Write("\tAmbiant: " + await al.get_currentValue());
          WriteLine("\tIR: " + await ir.get_currentValue());
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