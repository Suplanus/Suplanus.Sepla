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

            ViewModel = new ViewModel();
            DataContext = ViewModel;

            ViewModel.IsReadOnly = _partsManagement.SelectedPartsDatabase.IsReadOnly;

            // Events, called from Action of this Tab
            WPFDialogEventManager dialogEventManager = new WPFDialogEventManager();
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "SelectItem").Notify += SelectItem;
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "SaveItem").Notify += SaveItem;
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "PreShowTab").Notify += PreShowTab;
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "OpenDatabase").Notify += OpenDatabase;
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "CreateDatabase").Notify += CreateDatabase;
        }

        private void CreateDatabase(string data)
        {
            
        }

        private void OpenDatabase(string data)
        {
            
        }

        private void PreShowTab(string data)
        {
            
        }

        private void SelectItem(string data)
        {
            
        }

        private void SaveItem(string data)
        {
            
        }

    }
}
