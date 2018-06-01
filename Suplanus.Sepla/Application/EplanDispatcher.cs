using Eplan.EplApi.Base.Internal;
using Action = System.Action;

namespace Suplanus.Sepla.Application
{
  public class EplanDispatcher
  {
    public static void ExecuteInMainThread(Action action)
    {
      new EplanMainThreadDispatcher().ExecuteInMainThreadSync(o =>
      {
        action.Invoke();
        return null;
      }, null);
    }
  }
}
