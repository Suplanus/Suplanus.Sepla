using System.Linq;
using System.Windows.Controls;
using Eplan.EplApi.MasterData;
using Eplan.EplSDK.WPF.EEvent;
using Eplan.EplSDK.WPF.Interfaces.DialogServices;

namespace Suplanus.Example.EplAddIn.ItemTypeExample
{
    public partial class ItemTypeExampleExampleContent : IDialog
    {
        private readonly MDPartsManagement _partsManagement = new MDPartsManagement();
        private bool _isLoading = true;

        public ItemTypeExampleExampleContent()
        {
            InitializeComponent();

            ViewModel = new ViewModel();
            DataContext = ViewModel;

            ViewModel.IsReadOnly = _partsManagement.SelectedPartsDatabase.IsReadOnly;

            // Events, called from Action of this Tab
            var dialogEventManager = new WPFDialogEventManager();
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "SelectItem").Notify += SelectItem;
            dialogEventManager.getOnWPFNotifyEvent("XPartsManagementDialog", "SaveItem").Notify += SaveItem;
        }

        public ViewModel ViewModel { get; set; }
        public ItemTypeObject SelectedItemTypeObject { get; set; }
        public string Caption => nameof(ItemTypeExampleExampleContent);
        public bool IsTabsheet => true;

        private void SelectItem(string data)
        {
            _isLoading = true;

            // No Selected item (like in root level)
            if (IsRootLevel(data))
            {
                SelectedItemTypeObject = null;
                TextBoxName.Text = string.Empty;
                return;
            }

            // Get values
            SelectedItemTypeObject = Data.ItemTypeObjects.First(obj => obj.Key.Equals(data));
            TextBoxName.Text = SelectedItemTypeObject.Text;
            TextBoxCustomProperty.Text = SelectedItemTypeObject.CustomProperty;

            _isLoading = false;
        }


        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isLoading) _partsManagement.SetModified();
        }

        private void SaveItem(string data)
        {
            if (IsRootLevel(data)) return;

            // Set new values to object
            SelectedItemTypeObject.Text = TextBoxName.Text;
            SelectedItemTypeObject.CustomProperty = TextBoxCustomProperty.Text;

            Data.Save();
            _partsManagement.RefreshPartsManagementDialog(); // todo: Not working
        }

        private static bool IsRootLevel(string data)
        {
            return string.IsNullOrEmpty(data) || data == "0";
        }
    }
}