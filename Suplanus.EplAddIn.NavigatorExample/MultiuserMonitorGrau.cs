// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitorGrau
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplCoreAddin.MultiuserMonitor.Properties;
using Eplan.EplSDK.WPF.Interfaces.DialogServices;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class MultiuserMonitorGrau : System.Windows.Controls.UserControl, IDialog, IDialogBase, IDialogBar, IDialogComponentAccess, ICallingContext, IDialogState, IDialogAction, IDialogClose, IComponentConnector
  {
    private XMUMReceiver m_receiver;
    private Eplan.EplApi.Base.Context m_Context;
    private IDialogStateManager m_dlgStateMgr;
    private IDialogComponent m_IDialogComponent;
    internal MultiuserMonitorGrau UserControl;
    internal Grid LayoutRoot;
    internal Label ServerState;
    internal Path send;
    internal Path receive;
    internal DataGrid theGrid;
    internal DataGrid theGrid_Opened;
    private bool _contentLoaded;

    public MultiuserMonitorGrau()
    {
      this.InitializeComponent();
    }

    public string Caption
    {
      get
      {
        return "Test Gray";
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
        return 211;
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
      this.m_receiver = new XMUMReceiver((Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor) null);
      this.m_receiver.AutoStartReceiveData();
      this.theGrid.DataContext = (object) this.m_receiver.WorkingProjectsContext;
      this.theGrid_Opened.DataContext = (object) this.m_receiver.OpenedProjectsContext;
      this.ServerState.SetBinding(ContentControl.ContentProperty, (BindingBase) new Binding("ServerstateText")
      {
        Mode = BindingMode.OneWay,
        NotifyOnSourceUpdated = true,
        NotifyOnTargetUpdated = true,
        UpdateSourceTrigger = UpdateSourceTrigger.Default,
        ValidatesOnDataErrors = false
      });
      this.ServerState.DataContext = (object) this.m_receiver;
      this.receive.Visibility = Visibility.Hidden;
      this.send.Visibility = Visibility.Hidden;
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

    public void close()
    {
      this.m_receiver.StopReceiveData();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Eplan.EplCoreAddin.MultiuserMonitoru;component/multiusermonitorgrau.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.UserControl = (MultiuserMonitorGrau) target;
          this.UserControl.Unloaded += new RoutedEventHandler(this.Usercontrol_Unloaded);
          break;
        case 2:
          this.LayoutRoot = (Grid) target;
          break;
        case 3:
          this.ServerState = (Label) target;
          break;
        case 4:
          this.send = (Path) target;
          break;
        case 5:
          this.receive = (Path) target;
          break;
        case 6:
          this.theGrid = (DataGrid) target;
          break;
        case 7:
          this.theGrid_Opened = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
