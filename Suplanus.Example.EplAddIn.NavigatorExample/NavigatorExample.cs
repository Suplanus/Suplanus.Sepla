using System.Reflection;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplSDK.WPF;

namespace Suplanus.EplAddIn.NavigatorExample
{
    class NavigatorExample : IEplAddIn
    {
        public bool OnRegister(ref bool isLoadingOnStart)
        {
            isLoadingOnStart = true;
            return true;
        }

        public bool OnUnregister()
        {
            return true;
        }

        public bool OnInit()
        {
            return true;
        }

        public bool OnInitGui()
        {
            // Get name from class
            // ReSharper disable once PossibleNullReferenceException
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            DialogBarFactory dialogBarFactory = new DialogBarFactory(className, typeof(NavigatorContent), DialogDockingOptions.Any, 0);

            return true;
        }

        public bool OnExit()
        {
            return true;
        }
    }
}
