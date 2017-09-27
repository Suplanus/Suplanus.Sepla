// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitorSettingsDialog
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplCoreAddin.MultiuserMonitor.Properties;
using Eplan.EplExt.MultiuserMonitorClient;
using Eplan.EplSDK.WPF.Controls;
using Eplan.EplSDK.WPF.Interfaces;
using Eplan.EplSDK.WPF.Interfaces.DialogServices;
using Eplan.EplSDK.WPF.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class MultiuserMonitorSettingsDialog : UserControl, IDialog, IDialogBase, IDialogComponentAccess, ICallingContext, IDialogState, IDialogAction, IDialogClose, IComponentConnector, IStyleConnector
  {
    private string m_connectionerror = "";
    private ObservableCollection<string> m_listHosts = new ObservableCollection<string>();
    private Eplan.EplApi.Base.Context m_Context;
    private IDialogStateManager m_dlgStateMgr;
    private IDialogComponent m_IDialogComponent;
    private Dispatcher m_MainThreadDispatcher;
    internal Label lbName;
    internal EplComboBox tbName;
    internal Label lbPort;
    internal EplTextBox tbPort;
    internal EplCheckBox cbActive;
    internal TextBlock ServerState;
    internal Button btnProof;
    internal TextBlock ServerErrorState;
    private bool _contentLoaded;

    public MultiuserMonitorSettingsDialog()
    {
      this.InitializeComponent();
      this.m_MainThreadDispatcher = Dispatcher.CurrentDispatcher;
    }

    public string Caption
    {
      get
      {
        return "Test Settings";
      }
    }

    public bool IsTabsheet
    {
      get
      {
        return false;
      }
    }

    public object Context
    {
      set
      {
        this.m_Context = value as Eplan.EplApi.Base.Context;
      }
    }

    public void internal_OnClientStateHandler(object sender, ClientEventArgs e)
    {
      if (e.EventType == ClientEventType.fIamConnected)
      {
        this.m_connectionerror = "";
        this.ServerErrorState.Text = "";
        this.ServerState.Text = "Verbunden!";
      }
      else if (e.EventType == ClientEventType.fIamDisconnected)
      {
        this.ServerState.Text = "Getrennt!";
        try
        {
          Client.getInstance().isConnected(ref this.m_connectionerror);
          this.ServerErrorState.Text = "";
          if (string.IsNullOrEmpty(this.m_connectionerror))
            return;
          BaseException innerException = new BaseException(this.m_connectionerror, MessageLevel.Error);
          string connectionErrorText = "Connection Error!";
          new BaseException(connectionErrorText, MessageLevel.Warning, innerException).FixMessage();
          Decider decider = new Decider();
          string connectionErrorTitle = "Connection Error Title";
          string str = connectionErrorText + "\n" + this.m_connectionerror;
          int num1 = 6;
          string strText = str;
          string strTitle = connectionErrorTitle;
          int num2 = 1;
          int num3 = 1;
          string strDecisionId = "";
          int num4 = 0;
          int num5 = 3;
          int num6 = (int) decider.Decide((EnumDecisionType) num1, strText, strTitle, (EnumDecisionReturn) num2, (EnumDecisionReturn) num3, strDecisionId, num4 != 0, (EnumDecisionIcon) num5);
        }
        catch (Exception ex)
        {
        }
      }
      else
      {
        if (e.EventType != ClientEventType.fIamConnecting)
          return;
        this.ServerState.Text = "Connecting...!";
      }
    }

    public void OnClientStateHandler(object sender, ClientEventArgs e)
    {
      if (this.m_MainThreadDispatcher.CheckAccess())
        this.internal_OnClientStateHandler(sender, e);
      else
        this.m_MainThreadDispatcher.BeginInvoke(DispatcherPriority.Send, (Delegate) new MultiuserMonitorSettingsDialog.ClientStateHandlerDelegate(this.internal_OnClientStateHandler), sender, (object) e);
    }

    private void XButton_Click(object sender, RoutedEventArgs e)
    {
      FrameworkElement frameworkElement = sender as FrameworkElement;
      if (frameworkElement == null)
        return;
      this.m_listHosts.Remove(frameworkElement.DataContext as string);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      this.save();
      this.updateNameList();
      this.setClientState();
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
      this.m_listHosts = new ObservableCollection<string>();
      this.updateNameList();
      this.tbName.ItemsSource = (IEnumerable) this.m_listHosts;
      this.tbName.SetModel((IEnfModel) new EnfTreeModelSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", 0, this.m_IDialogComponent));
      this.tbPort.SetModel((IEnfModel) new EnfTreeModelSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Port", 0, this.m_IDialogComponent));
      this.cbActive.SetModel((IEnfModel) new EnfTreeModelSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Active", 0, this.m_IDialogComponent));
      this.tbPort.GetBindingExpression(TextBox.TextProperty).ParentBinding.ValidationRules.Add((ValidationRule) new PortValidationRule());
      this.setClientState();
      Client.m_ClientStateEvent += new System.EventHandler<ClientEventArgs>(this.OnClientStateHandler);
    }

    private void updateNameList()
    {
      Settings settings = new Settings();
      int countOfValues = settings.GetCountOfValues("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name");
      int index = 0;
      if (countOfValues > 0)
      {
        for (; index < countOfValues; ++index)
        {
          if (this.m_listHosts.Count <= index)
          {
            this.m_listHosts.Add(settings.GetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", index));
          }
          else
          {
            string stringSetting = settings.GetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", index);
            string listHost = this.m_listHosts[index];
            if (stringSetting != listHost)
              this.m_listHosts[index] = stringSetting;
          }
        }
      }
      else
      {
        string stringSetting = settings.GetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", 0);
        if (stringSetting.Length > 0)
        {
          if (this.m_listHosts.Count <= 0)
          {
            this.m_listHosts.Add(stringSetting);
          }
          else
          {
            string listHost = this.m_listHosts[0];
            if (stringSetting != listHost)
              this.m_listHosts[0] = stringSetting;
          }
          index = 1;
        }
      }
      while (this.m_listHosts.Count > index)
        this.m_listHosts.RemoveAt(index);
    }

    private void setClientState()
    {
      this.m_connectionerror = "";
      bool flag1 = false;
      bool flag2 = false;
      try
      {
        flag1 = Client.getInstance().isConnected(ref this.m_connectionerror);
        flag2 = Client.getInstance().isConnecting();
      }
      catch (Exception ex)
      {
      }
      this.ServerState.Text = !flag1 ? (!flag2 ? "Nicht Verbunden!" : "Verbinden...!") : "Verbunden!";
      this.ServerErrorState.Text = "";
    }

    private bool getErrorMessage(ref string errormsg)
    {
      errormsg = "";
      bool flag = true;
      BindingExpression bindingExpression = this.tbPort.GetBindingExpression(TextBox.TextProperty);
      if (bindingExpression != null)
      {
        bindingExpression.UpdateSource();
        if (bindingExpression.HasError)
        {
          errormsg = bindingExpression.ValidationError.ErrorContent as string;
          flag = false;
        }
      }
      return !flag;
    }

    public bool isValid()
    {
      string errormsg = "";
      if (this.getErrorMessage(ref errormsg))
        throw new Exception(errormsg);
      return true;
    }

    public void reload()
    {
    }

    public void save()
    {
      this.tbPort.SaveDlg();
      this.tbName.SaveDlg();
      this.cbActive.SaveDlg();
      Settings settings = new Settings();
      string stringSetting = settings.GetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", 0);
      settings.RemoveAllIndexedSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name");
      int nIdx1 = 0;
      settings.SetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", stringSetting, nIdx1);
      int nIdx2 = nIdx1 + 1;
      for (int index = 0; index < this.m_listHosts.Count; ++index)
      {
        string listHost = this.m_listHosts[index];
        if (listHost.Length > 0 && listHost != stringSetting)
        {
          settings.SetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", this.m_listHosts[index], nIdx2);
          ++nIdx2;
        }
      }
      new Eplan.EplApi.ApplicationFramework.EventManager().Send("onServerChanged", (IEventParameter) new EventParameterString());
    }

    public void close()
    {
      Client.m_ClientStateEvent -= new System.EventHandler<ClientEventArgs>(this.OnClientStateHandler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Eplan.EplCoreAddin.MultiuserMonitoru;component/multiusermonitorsettingsdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 2:
          this.lbName = (Label) target;
          break;
        case 3:
          this.tbName = (EplComboBox) target;
          break;
        case 4:
          this.lbPort = (Label) target;
          break;
        case 5:
          this.tbPort = (EplTextBox) target;
          break;
        case 6:
          this.cbActive = (EplCheckBox) target;
          break;
        case 7:
          this.ServerState = (TextBlock) target;
          break;
        case 8:
          this.btnProof = (Button) target;
          this.btnProof.Click += new RoutedEventHandler(this.Button_Click);
          break;
        case 9:
          this.ServerErrorState = (TextBlock) target;
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
      if (connectionId != 1)
        return;
      ((ButtonBase) target).Click += new RoutedEventHandler(this.XButton_Click);
    }

    public delegate void ClientStateHandlerDelegate(object sender, ClientEventArgs e);
  }
}
