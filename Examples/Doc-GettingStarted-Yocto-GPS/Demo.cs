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

        YGps gps;

        if (Target.ToLower() == "any") {
          gps = YGps.FirstGps();
          if (gps == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          gps = YGps.FindGps(Target + ".gps");
        }

        while (await gps.isOnline()) {
          if (await gps.get_isFixed() != YGps.ISFIXED_TRUE)
            WriteLine("fixing... ");
          else
            WriteLine(await gps.get_latitude() + "  " + await gps.get_longitude());
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