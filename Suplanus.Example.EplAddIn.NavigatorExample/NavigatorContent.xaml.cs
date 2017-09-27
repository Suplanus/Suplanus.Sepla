using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using Eplan.EplSDK.WPF;
using Eplan.EplSDK.WPF.Interfaces;
using Eplan.EplSDK.WPF.Interfaces.DialogServices;

namespace Suplanus.EplAddIn.NavigatorExample
{
    public partial class NavigatorContent : IDialog, IDialogBar, IDialogComponentAccess, ICallingContext, IDialogState, IDialogAction, IDialogClose, IElementStateAccess, IComponentConnector, IStyleConnector
    {
        public NavigatorContent()
        {
            ElementStateCollection = new Eplan.EplSDK.WPF.Controls.Persistency.ElementStateCollection();
            ElementStateCollection.Load(nameof(NavigatorContent));

            InitializeComponent();

            Caption = "NavigatorExample Caption";
            IsTabsheet = false;
            UniqueBarID = 42;            
            
        }

        public string Caption { get; set; }
        public bool IsTabsheet { get; set; }
        public int UniqueBarID { get; set; }
        public IDialogComponent Component { get; set; }
        public object Context { get; set; }
        public IDialogStateManager DialogStateManager { get; set; }
        public IElementStateCollection ElementStateCollection { get; set; }

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


        public void Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                //case 1:
                //    this.UserControl = (Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor) target;
                //    this.UserControl.Unloaded += new RoutedEventHandler(this.Usercontrol_Unloaded);
                //    break;
                //case 2:
                //    this.LayoutRoot = (Grid) target;
                //    break;
                //case 3:
                //    ((FrameworkElement) target).ContextMenuOpening += new ContextMenuEventHandler(this.NoContextMenu);
                //    break;
                //case 4:
                //    this.ServerState = (Label) target;
                //    break;
                //case 5:
                //    this.send = (Path) target;
                //    break;
                //case 6:
                //    this.receive = (Path) target;
                //    break;
                //case 7:
                //    this.theGrid = (DataGrid) target;
                //    break;
                //case 9:
                //    this.theGrid_Opened = (DataGrid) target;
                //    break;
                //default:
                //    this._contentLoaded = true;
                //    break;
            }
        }
    }
}
