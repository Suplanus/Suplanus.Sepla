using System.IO;
using System.Reflection;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.MasterData;
using Eplan.EplSDK.WPF;
using Eplan.EplSDK.WPF.DB;

namespace Suplanus.Example.EplAddIn.PartsManagementExtensionExample
{
    class Addin : IEplAddIn
    {
        public string TabsheetName => nameof(PartsManagementExtensionContent);
        public string ItemType => "eplan.part";

        public string AddinName
        {
            get
            {
                string addinName = typeof(Addin).Assembly.CodeBase;
                addinName = Path.GetFileNameWithoutExtension(addinName);
                return addinName;
            }
        }        

        public bool OnRegister(ref bool isLoadingOnStart)
        {
            isLoadingOnStart = true;
            return true;
        }

        public bool OnUnregister()
        {
            var partsManagement = new MDPartsManagement();
            partsManagement.UnregisterAddin(AddinName);
            partsManagement.UnregisterItem(ItemType);
            partsManagement.UnregisterTabsheet(TabsheetName);

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

            partsManagement.RegisterAddin(AddinName, actionName);            
            partsManagement.RegisterItem(AddinName, ItemType);
            partsManagement.RegisterTabsheet(AddinName, ItemType, TabsheetName);
            DialogFactoryDB dialogBarFactory = new DialogFactoryDB(TabsheetName,
                typeof(PartsManagementExtensionContent));
            PartsManagementExtensionExampleAction.TabsheetName = TabsheetName;

            return true;
        }

        public bool OnExit()
        {
            return true;
        }
    }
}
