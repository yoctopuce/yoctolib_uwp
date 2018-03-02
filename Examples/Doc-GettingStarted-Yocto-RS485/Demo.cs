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
    public string Slave { get; set; }
    public string Register { get; set; }
    public string Value { get; set; }

    public override async Task<int> Run()
    {
      try {
        int slave = Convert.ToInt32(Slave);
        if (slave < 1 || slave > 255) {
          WriteLine("Invalid MODBUS slave address");
          return -1;
        }

        int reg = Convert.ToInt32(Register);
        if (reg < 1 || reg >= 50000 || (reg % 10000) == 0) {
          WriteLine("Invalid MODBUS Register");
          return -1;
        }

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

        int val;
        if (reg >= 40001) {
          val = (await serialPort.modbusReadRegisters(slave, reg - 40001, 1))[0];
        } else if (reg >= 30001) {
          val = (await serialPort.modbusReadInputRegisters(slave, reg - 30001, 1))[0];
        } else if (reg >= 10001) {
          val = (await serialPort.modbusReadInputBits(slave, reg - 10001, 1))[0];
        } else {
          val = (await serialPort.modbusReadBits(slave, reg - 1, 1))[0];
        }

        WriteLine("Current value: " + val.ToString());

        if (Value != "" && (reg % 30000) < 10000) {
          val = Convert.ToInt32(Value);
          if (reg >= 30001) {
            await serialPort.modbusWriteRegister(slave, reg - 30001, val);
          } else {
            await serialPort.modbusWriteBit(slave, reg - 1, val);
          }
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}