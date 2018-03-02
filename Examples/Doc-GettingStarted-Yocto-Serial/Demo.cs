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
    public string ToSend { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);

        YSerialPort serialPort;

        if (Target.ToLower() == "any") {
          serialPort = YSerialPort.FirstSerialPort();
          if (serialPort == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
          YModule ymod = await serialPort.get_module();
          WriteLine("Using: " + await ymod.get_serialNumber());
        } else {
          serialPort = YSerialPort.FindSerialPort(Target + ".serialPort");
        }

        await serialPort.set_serialMode("9600,8N1");
        await serialPort.set_protocol("Line");
        await serialPort.reset();
        string line;
        do {
          if (ToSend != "") {
            await serialPort.writeLine(ToSend);
            ToSend = "";
          }
          await YAPI.Sleep(500);
          do {
            line = await serialPort.readLine();
            if (line != "") {
              WriteLine("Received: " + line);
            }
          } while (line != "");

        } while (line != "");
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}