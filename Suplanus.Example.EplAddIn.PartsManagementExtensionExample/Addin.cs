using System.IO;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.MasterData;

namespace Suplanus.Example.EplAddIn.PartsManagementExtensionExample
{
    class Addin : IEplAddIn
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
            var partsManagement = new MDPartsManagement();
            string actionName = nameof(PartsManagementExtensionExampleAction);
            string addinName = typeof(Addin).Assembly.CodeBase;
            string itemType = "eplan.part"; // default
            string tabsheetName = nameof(PartsManagementExtensionContent);

            addinName = Path.GetFileNameWithoutExtension(addinName);
            partsManagement.RegisterAddin(addinName, actionName);            
            partsManagement.RegisterItem(addinName, itemType);
            partsManagement.RegisterTabsheet(addinName, itemType, tabsheetName);
            PartsManagementExtensionExampleAction.TabsheetName = tabsheetName;

            return true;
        }

        public bool OnExit()
        {
            return true;
        }
    }
}
