using Eplan.EplApi.ApplicationFramework;

namespace Suplanus.Example.EplAddIn.SvgExport
{
  public class Addin : IEplAddIn
  {
    public bool OnInitGui()
    {
      return true;
    }

    public bool OnExit()
    {
      return true;
    }

    public bool OnInit()
    {
      return true;
    }

    public bool OnRegister(ref bool loadOnStart)
    {
      loadOnStart = true;
      return true;
    }

    public bool OnUnregister()
    {
      return true;
    }
  }
}