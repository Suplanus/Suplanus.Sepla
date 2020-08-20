using System;
using System.IO;
using System.Linq;
using Suplanus.Sepla.Application;

namespace Suplanus.Example.EplanOffline.Console
{
  class Program
  {
    [STAThread] // important for EPLAN
    static void Main(string[] args)
    {
      // Start EPLAN
      System.Console.WriteLine("Starting EPLAN...");

      string binPath = Starter.GetEplanInstallations()
                              .Last(obj => obj.EplanVariant
                                              .Equals("Electric P8"))
                              .EplanPath;
      binPath = Path.GetDirectoryName(binPath);

      Starter.PinToEplan(binPath); // Don't forget
      Sepla.Application.EplanOffline eplanOffline = new Sepla.Application.EplanOffline(binPath, "API");
      eplanOffline.StartWithoutGui();

      // Do something: Have to be in a separate class which is not initialized
      var doSomething = new DoSomething();
      doSomething.Foo();

      // Close
      eplanOffline.Close();           
      System.Console.ReadKey();
    }
  }
}
