using Eplan.EplApi.MasterData;
using Eplan.EplSDK.WPF.EEvent;
using Eplan.EplSDK.WPF.Interfaces.DialogServices;

namespace Suplanus.Example.EplAddIn.PartsManagementExtensionExample
{
    public partial class PartsManagementExtensionContent : IDialog
    {
        public string Caption => nameof(PartsManagementExtensionContent);
        public bool IsTabsheet => true;
        public ViewModel ViewModel { get; set; } 

        private readonly MDPartsManagement _partsManagement = new MDPartsManagement();

        public PartsManagementExtensionContent()
        {
            InitializeComponent();

            // Events, called from Action of this Tab
            WPFDialogEventManager dialogEventManager = new WPFDialogEventManager();
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "SelectItem").Notify += SelectItem;
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "SaveItem").Notify += SaveItem;
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "PreShowTab").Notify += PreShowTab;
        }

        private void PreShowTab(string data)
        {
            ViewModel = new ViewModel();

            // Set Readonly
            // todo: changes while choosing part with setting activated
            //bool areChangesAllowed = new Settings().GetBoolSetting("USER.PartSelectionGui.DataSourceScheme.Standard.Data.AllowEditing",0); // changes allowed while choosing part
            bool isDatabaseReadOnly = _partsManagement.SelectedPartsDatabase.IsReadOnly;
            ViewModel.IsEnabled = !isDatabaseReadOnly;
        }

        private void SelectItem(string data)
        {
            // Do something with selected items
        }

        private void SaveItem(string data)
        {
            // Do something on database save event
        }

    }
}
