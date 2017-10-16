using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

namespace Suplanus.Sepla.Application
{
   public class Addin
    {
       public static void SetEplanAsOwner(Window window)
       {
           WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
           windowInteropHelper.Owner = Process.GetCurrentProcess().MainWindowHandle;
       }
    }
}
