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
        YDisplayLayer l0;

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

        WriteLine("Loot at the Yoctopuce display");

        //clean up
        await disp.resetAll();

        // retreive the display size
        int w = await disp.get_displayWidth();
        int h = await disp.get_displayHeight();

        // reteive the first layer
        l0 = await disp.get_displayLayer(0);
        int bytesPerLines = w / 8;

        byte[] data = new byte[h * bytesPerLines];
        for (int i = 0; i < data.Length; i++)
          data[i] = 0;

        int max_iteration = 50;
        int iteration;
        double xtemp;
        double centerX = 0;
        double centerY = 0;
        double targetX = 0.834555980181972;
        double targetY = 0.204552998862566;
        double x, y, x0, y0;
        double zoom = 1;
        double distance = 1;

        while (true) {
          for (int i = 0; i < data.Length; i++)
            data[i] = 0;
          distance = distance * 0.95;
          centerX = targetX * (1 - distance);
          centerY = targetY * (1 - distance);
          max_iteration = (int) Math.Round(max_iteration + Math.Sqrt(zoom));
          if (max_iteration > 1500)
            max_iteration = 1500;
          for (int j = 0; j < h; j++)
            for (int i = 0; i < w; i++) {
              x0 = (((i - w / 2.0) / (w / 8)) / zoom) - centerX;
              y0 = (((j - h / 2.0) / (w / 8)) / zoom) - centerY;

              x = 0;
              y = 0;

              iteration = 0;

              while ((x * x + y * y < 4) && (iteration < max_iteration)) {
                xtemp = x * x - y * y + x0;
                y = 2 * x * y + y0;
                x = xtemp;
                iteration += 1;
              }

              if (iteration >= max_iteration)
                data[j * bytesPerLines + (i >> 3)] |= (byte) (128 >> (i % 8));
            }

          await l0.drawBitmap(0, 0, w, data, 0);
          zoom = zoom / 0.95;
        }
      } catch (YAPI_Exception ex) {
        WriteLine("error: " + ex.Message);
      }

      YAPI.FreeAPI();
      return 0;
    }
  }
}