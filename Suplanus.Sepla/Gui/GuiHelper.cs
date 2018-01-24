using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Suplanus.Sepla.Gui
{
  public class GuiHelper
  {
    public static void SetEplanAsOwner(Window window)
    {
      WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
      windowInteropHelper.Owner = Process.GetCurrentProcess().MainWindowHandle;
    }
  }
}
