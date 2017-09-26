using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Eplan.EplApi.ApplicationFramework;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
    class NavigatorExampleAction : IEplAction
    {
        public void GetActionProperties(ref ActionProperties actionProperties) { }

        public bool OnRegister(ref string name, ref int ordinal)
        {
            // ReSharper disable once PossibleNullReferenceException
            name = MethodBase.GetCurrentMethod().DeclaringType.Name; // Get name from class
            ordinal = 20;
            return true;
        }

        public bool Execute(ActionCallingContext oActionCallingContext)
        {
            var multiuserMonitor = new MultiuserMonitor();
            multiuserMonitor.init();

            return true;
        }
    }
}
