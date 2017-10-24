using Eplan.EplApi.ApplicationFramework;
using Eplan.EplSDK.WPF.EEvent;

namespace Suplanus.Example.EplAddIn.PartsManagementExtensionExample
{
    class PartsManagementExtensionExampleAction : IEplAction
    {
        public static string TabsheetName = null;

        public void GetActionProperties(ref ActionProperties actionProperties) { }

        public bool OnRegister(ref string name, ref int ordinal)
        {
            name = nameof(PartsManagementExtensionExampleAction);
            return true;
        }

        public bool Execute(ActionCallingContext actionCallingContext)
        {
            // Sent events to WPF control from base action
            string itemType = string.Empty;
            string action = string.Empty;
            string key = string.Empty;
            actionCallingContext.GetParameter("itemtype", ref itemType);
            actionCallingContext.GetParameter("action", ref action);
            actionCallingContext.GetParameter("key", ref key);

            WPFDialogEventManager wpfDialogEventManager = new WPFDialogEventManager();

            switch (action)
            {
                case "SelectItem":
                case "SaveItem":
                case "PreShowTab":
                case "OpenDatabase":
                case "CreateDatabase":
                    wpfDialogEventManager.send("XPartsManagementDialog", action, key);
                    break;
            }

            return true;
        }
    }
}
