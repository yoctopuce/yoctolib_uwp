/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  An example that shows how to use a  Yocto-Display-ePaper
 *
 *  You can find more information on our web site:
 *   Yocto-Display-ePaper documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-display-epaper/doc.html
 *   uwp API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-uwp-EN.html
 *
 *********************************************************************/

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
    public string Message { get; set; }

    public override async Task<int> Run()
    {
      try {
        YDisplay disp;

        await YAPI.RegisterHub(HubURL);

        // find the display according to command line parameters
        if (Target.ToLower() == "any") {
          disp = YDisplay.FirstDisplay();
          if (disp == null) {
            WriteLine("No module connected (check USB cable) ");
            return -1;
          }
        } else {
          disp = YDisplay.FindDisplay(Target + ".display");
        }

        if (!await disp.isOnline()) {
          WriteLine("Module not connected (check identification and USB cable) ");
          return -1;
        }

        int[] colors = { 0xFFFFFF, 0x000000, 0xFF0000, 0xFFFF00 };

        // Makes sure the Panel type is set
        string paneltype = await disp.get_displayPanel();
        if (paneltype == "NOT_SET") {
            WriteLine("Use the virtual to Configure the panel first");
            await YAPI.Sleep(3000);
            return -1;
        }

        // retrieve the display size
        int w = await disp.get_displayWidth();
        int h = await disp.get_displayHeight();
        int middleX = (int)(w / 2);
        int middleY = (int)(h / 2);
        WriteLine("Using device " + disp.get_serialNumber() + " (panel: " + paneltype + " " + w + "x" + h + "pixels)\n");
        await disp.resetAll();

        // retrieve the first layer
        YDisplayLayer l0 = await disp.get_displayLayer(0);
        await l0.selectFont("medium.yfm");
        int interations = 0;
        bool animation = true;
        Random rnd = new Random();

        while (animation) {
            interations++;
            // prevent refreshing for 2 sec
            await disp.postponeRefresh(2000);
            await l0.clear();
            // draw a few circle
            for (int i = 0; i < 15; i++) {
                int cx = rnd.Next(w);
                int cy = rnd.Next(h);
                int r = rnd.Next((h / 20), (h / 10));
                await l0.selectFillColor(colors[rnd.Next(4)]);
                await l0.drawDisc(cx, cy, r);
                await l0.drawCircle(cx, cy, r);
            }
            // draw a rectangle with panel type in it
            await l0.selectFillColor(0xffffff);
            await l0.drawBar(middleX - 75, middleY - 10, middleX + 75, middleY + 12);
            await l0.drawRect(middleX - 75, middleY - 10, middleX + 75, middleY + 12);
            await l0.drawText(middleX, middleY, YDisplayLayer.ALIGN.CENTER, paneltype);
            // forces a full refresh only the 1rst time
            if (interations == 1) await disp.regenerateDisplay();
            await disp.triggerRefresh(); // display is allowed to refresh  again
            await YAPI.Sleep(1000);
            // if no fast refresh available, don't even try to run animations
            if (paneltype.IndexOf("KS") < 0) animation = false;
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();

      return 0;
    }
  }
}