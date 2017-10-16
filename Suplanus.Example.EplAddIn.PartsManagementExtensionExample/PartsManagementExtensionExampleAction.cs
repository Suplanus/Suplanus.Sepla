using System.Reflection;
using Eplan.EplApi.ApplicationFramework;

namespace Suplanus.Example.EplAddIn.PartsManagementExtensionExample
{
    class PartsManagementExtensionExampleAction : IEplAction
    {
        public static string TabsheetName;

        public void GetActionProperties(ref ActionProperties actionProperties)
        {

        }

        public bool OnRegister(ref string name, ref int ordinal)
        {
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            name = className;

            return true;
        }

        public bool Execute(ActionCallingContext actionCallingContext)
        {
            string action = null;
            actionCallingContext.GetParameter("action", ref action);

            if (!string.IsNullOrEmpty(action) && action == "PreShowTab")
            {
                string tabsheet = null;
                actionCallingContext.GetParameter("tabsheet", ref tabsheet);
                actionCallingContext.AddParameter("show", "1");
            }

            return true;
        }


    }
}
