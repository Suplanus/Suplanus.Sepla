using System.IO;
using System.Linq;
using System.Windows;
using Suplanus.Sepla.Application;

namespace Suplanus.Example.EplanOffline.WpfCore
{
  public partial class App : Application
  {
    public static Window Window { get; set; }

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
      Window = new MainWindow();
      InitEplan();
      Window.ShowDialog();
      DoSomething();
    }

    private static void DoSomething()
    {
      // Do something: Have to be in a separate class which is not initialized
      var doSomething = new DoSomething();
      doSomething.Foo();
    }

    private void InitEplan()
    {
      string binPath = Starter.GetEplanInstallations()
                              .Last(obj => obj.EplanVariant
                                              .Equals("Electric P8"))
                              .EplanPath;
      binPath = Path.GetDirectoryName(binPath);

      Starter.PinToEplan(binPath); // Don't forget
      Sepla.Application.EplanOffline eplanOffline = new Sepla.Application.EplanOffline(binPath, "API");
      eplanOffline.StartWpf(Window);
    }
  }
}
