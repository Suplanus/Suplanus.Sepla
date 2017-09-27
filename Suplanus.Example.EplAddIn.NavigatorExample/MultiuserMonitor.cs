// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplCoreAddin.MultiuserMonitor.Properties;
using Eplan.EplSDK.WPF.Interfaces;
using Eplan.EplSDK.WPF.Interfaces.DialogServices;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class MultiuserMonitor : UserControl, IDialog, IDialogBar, IDialogComponentAccess, ICallingContext, IDialogState, IDialogAction, IDialogClose, IElementStateAccess, IComponentConnector, IStyleConnector
  {
    public static readonly DependencyProperty IsConnectedProperty = DependencyProperty.RegisterAttached("IsConnected", typeof (bool), typeof (Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor.OnIsConnectedChanged)));
    private Eplan.EplSDK.WPF.Controls.Persistency.ElementStateCollection m_eleState = new Eplan.EplSDK.WPF.Controls.Persistency.ElementStateCollection();
    private XMUMReceiver m_receiver;
    private Eplan.EplApi.Base.Context m_Context;
    private IDialogStateManager m_dlgStateMgr;
    private IDialogComponent m_IDialogComponent;
    internal Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor UserControl;
    internal Grid LayoutRoot;
    internal Label ServerState;
    internal Path send;
    internal Path receive;
    internal DataGrid theGrid;
    internal DataGrid theGrid_Opened;
    private bool _contentLoaded;

    public static void SetIsConnectedProperty(UIElement element, bool value)
    {
      if (element == null)
        return;
      element.SetValue(Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor.IsConnectedProperty, (object) value);
    }

    public static bool GetIsConnectedProperty(UIElement element)
    {
      if (element != null)
        return (bool) element.GetValue(Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor.IsConnectedProperty);
      return false;
    }

    private static void OnIsConnectedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
    }

    public MultiuserMonitor()
    {
      this.m_eleState.Load(nameof (MultiuserMonitor));
      this.InitializeComponent();
    }

    public string Caption
    {
      get
      {
        return "Test";
      }
    }

    public bool IsTabsheet
    {
      get
      {
        return false;
      }
    }

    public int UniqueBarID
    {
      get
      {
        return 201;
      }
    }

    public object Context
    {
      set
      {
        this.m_Context = value as Eplan.EplApi.Base.Context;
      }
    }

    public IDialogStateManager DialogStateManager
    {
      get
      {
        return this.m_dlgStateMgr;
      }
      set
      {
        this.m_dlgStateMgr = value;
      }
    }

    public IDialogComponent Component
    {
      get
      {
        return this.m_IDialogComponent;
      }
      set
      {
        this.m_IDialogComponent = value;
      }
    }

    public void init()
    {
      this.m_receiver = new XMUMReceiver(this);
            //this.theGrid.DataContext = (object) this.m_receiver.WorkingProjectsContext;
            //this.theGrid_Opened.DataContext = (object) this.m_receiver.OpenedProjectsContext;
            //this.ServerState.SetBinding(ContentControl.ContentProperty, (BindingBase) new Binding("ServerstateText")
            //{
            //  Mode = BindingMode.OneWay,
            //  NotifyOnSourceUpdated = true,
            //  NotifyOnTargetUpdated = true,
            //  UpdateSourceTrigger = UpdateSourceTrigger.Default,
            //  ValidatesOnDataErrors = false
            //});
            //this.ServerState.DataContext = (object) this.m_receiver;
            //this.receive.Visibility = Visibility.Hidden;
            //this.send.Visibility = Visibility.Hidden;
            //this.m_receiver.AutoStartReceiveData();
        
        }

    private bool getErrorMessage(ref string errormsg)
    {
      return true;
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

    private void Usercontrol_Unloaded(object sender, RoutedEventArgs e)
    {
      this.m_receiver.StopReceiveData();
    }

    private void OnHyperlinkClick(object sender, RoutedEventArgs e)
    {
      try
      {
        Hyperlink source = e.Source as Hyperlink;
        if (source == null || !(source.NavigateUri != (Uri) null))
          return;
        Process.Start(source.NavigateUri.ToString());
      }
      catch
      {
      }
    }

    public void close()
    {
      this.m_receiver.StopReceiveData();
    }

    private void NoContextMenu(object sender, ContextMenuEventArgs e)
    {
      e.Handled = true;
    }

    public IElementStateCollection ElementStateCollection
    {
      get
      {
        return (IElementStateCollection) this.m_eleState;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this,
          new Uri("/Suplanus.EplAddIn.NavigatorExample;component/NavigatorContent.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.UserControl = (Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor) target;
          this.UserControl.Unloaded += new RoutedEventHandler(this.Usercontrol_Unloaded);
          break;
        case 2:
          this.LayoutRoot = (Grid) target;
          break;
        case 3:
          ((FrameworkElement) target).ContextMenuOpening += new ContextMenuEventHandler(this.NoContextMenu);
          break;
        case 4:
          this.ServerState = (Label) target;
          break;
        case 5:
          this.send = (Path) target;
          break;
        case 6:
          this.receive = (Path) target;
          break;
        case 7:
          this.theGrid = (DataGrid) target;
          break;
        case 9:
          this.theGrid_Opened = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 8)
      {
        if (connectionId != 10)
          return;
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = Hyperlink.ClickEvent,
          Handler = (Delegate) new RoutedEventHandler(this.OnHyperlinkClick)
        });
      }
      else
        ((Style) target).Setters.Add((SetterBase) new EventSetter()
        {
          Event = Hyperlink.ClickEvent,
          Handler = (Delegate) new RoutedEventHandler(this.OnHyperlinkClick)
        });
    }
  }
}
