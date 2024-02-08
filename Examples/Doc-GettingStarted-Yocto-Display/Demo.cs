/*********************************************************************
 *
 *  $Id: Demo.cs 58172 2023-11-30 17:10:23Z martinm $
 *
 *  An example that shows how to use a  Yocto-Display
 *
 *  You can find more information on our web site:
 *   Yocto-Display documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-display/doc.html
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
        await YAPI.RegisterHub(HubURL);

        YDisplay disp;
        YDisplayLayer l0, l1;
        int h, w, y, x, vx, vy;

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

        //clean up
        await disp.resetAll();

        // retreive the display size
        w = await disp.get_displayWidth();
        h = await disp.get_displayHeight();

        // reteive the first layer
        l0 = await disp.get_displayLayer(0);

        // display a text in the middle of the screen
        await l0.drawText(w / 2, h / 2, YDisplayLayer.ALIGN.CENTER, Message);

        // visualize each corner
        await l0.moveTo(0, 5);
        await l0.lineTo(0, 0);
        await l0.lineTo(5, 0);
        await l0.moveTo(0, h - 6);
        await l0.lineTo(0, h - 1);
        await l0.lineTo(5, h - 1);
        await l0.moveTo(w - 1, h - 6);
        await l0.lineTo(w - 1, h - 1);
        await l0.lineTo(w - 6, h - 1);
        await l0.moveTo(w - 1, 5);
        await l0.lineTo(w - 1, 0);
        await l0.lineTo(w - 6, 0);

        // draw a circle in the top left corner of layer 1
        l1 = await disp.get_displayLayer(1);
        await l1.clear();
        await l1.drawCircle(h / 8, h / 8, h / 8);

        // and animate the layer
        x = 0;
        y = 0;
        vx = 1;
        vy = 1;
        while (await disp.isOnline()) {
          x += vx;
          y += vy;
          if ((x < 0) || (x > w - (h / 4))) vx = -vx;
          if ((y < 0) || (y > h - (h / 4))) vy = -vy;
          await l1.setLayerPosition(x, y, 0);
          await YAPI.Sleep(5);
        }

        WriteLine("Module not connected (check identification and USB cable) ");
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      await YAPI.FreeAPI();
      return 0;
    }
  }
}