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
    public string Command { get; set; }

    public override async Task<int> Run()
    {
      try {
        await YAPI.RegisterHub(HubURL);
        YWatchdog watchdog;

        if (Target.ToLower() == "any") {
          watchdog = YWatchdog.FirstWatchdog();
          if (watchdog == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          watchdog = YWatchdog.FindWatchdog(Target + ".watchdog1");
        }

        if (await watchdog.isOnline()) {
          if (Command.ToUpper() == "ON") {
            await watchdog.set_running(YWatchdog.RUNNING_ON);
          }
          if (Command.ToUpper() == "OFF") {
            await watchdog.set_running(YWatchdog.RUNNING_OFF);
          }
          if (Command.ToUpper() == "RESET") {
            await watchdog.resetWatchdog();
          }
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