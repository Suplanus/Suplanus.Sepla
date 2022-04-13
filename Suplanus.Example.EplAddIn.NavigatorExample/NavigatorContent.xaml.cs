using System.Reflection;
using Eplan.EplSDK.WPF.Interfaces;
using Eplan.EplSDK.WPF.Interfaces.DialogServices;

namespace Suplanus.EplAddIn.NavigatorExample
{
    public partial class NavigatorContent : IDialog, IDialogBar, IDialogComponentAccess, ICallingContext, IDialogState, IDialogAction, IDialogClose, IElementStateAccess
    {
        public string Caption { get; set; }
        public bool IsTabsheet { get; set; }
        public int UniqueBarID { get; set; }
        public IDialogComponent Component { get; set; }
        public object Context { get; set; }
        public IDialogStateManager DialogStateManager { get; set; }
        public IElementStateCollection ElementStateCollection { get; set; }

        public NavigatorContent()
        {
            // Iniit
            ElementStateCollection = new Eplan.EplSDK.WPF.Controls.Persistency.ElementStateCollection();
            ElementStateCollection.Load(nameof(NavigatorContent));

            InitializeComponent();

            Caption = "NavigatorExample";
            IsTabsheet = false;

            // Use Class name for uniqueid
            // ReSharper disable once PossibleNullReferenceException
            var className = MethodBase.GetCurrentMethod().DeclaringType.Name;
            UniqueBarID = (uint)className.GetHashCode();            
            
        }
       
        public void init()
        {
            
        }

        public bool isValid()
        {
            return true;
        }

        public void reload()
        {
            
        }

        public void save()
        {
            
        }

        public void close()
        {
            
        }

    }
}
