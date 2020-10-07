using Eplan.EplApi.DataModel;

namespace Suplanus.Example.EplanOffline.WpfCore
{
  internal class DoSomething
  {
    public void Foo()
    {
      ProjectManager projectManager = new ProjectManager();
      var count = projectManager.OpenProjects.Length;
      App.Window.Title = "OpenProjects: " + count;
    }
  }
}