using System.IO;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.MasterData;
using Eplan.EplSDK.WPF.DB;

namespace Suplanus.Example.EplAddIn.ItemTypeExample
{
    internal class Addin : IEplAddIn
    {
        public string TabsheetName => nameof(ItemTypeExampleExampleContent);
        public string ItemType => "suplanus.itemtype";

        public string AddinName
        {
            get
            {
                var addinName = typeof(Addin).Assembly.CodeBase;
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
            var actionName = nameof(ItemTypeExampleExampleAction);

            partsManagement.RegisterAddin(AddinName, actionName);
            partsManagement.RegisterItem(AddinName, ItemType);
            partsManagement.RegisterTabsheet(AddinName, ItemType, TabsheetName);
            var dialogBarFactory = new DialogFactoryDB(TabsheetName,
                typeof(ItemTypeExampleExampleContent));
            ItemTypeExampleExampleAction.ItemType = ItemType;

            return true;
        }

        public bool OnExit()
        {
            return true;
        }
    }
}