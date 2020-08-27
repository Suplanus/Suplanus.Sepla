using System;
using Eplan.EplApi.DataModel;

namespace Suplanus.Example.EplanOffline.Console
{
  internal class DoSomething
  {
    public void Foo()
    {
      ProjectManager projectManager = new ProjectManager();
      var count = projectManager.OpenProjects.Length;
      System.Console.WriteLine($"Open projects: {count}");
    }
  }
}