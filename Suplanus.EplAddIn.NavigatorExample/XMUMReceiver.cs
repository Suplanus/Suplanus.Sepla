// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.XMUMReceiver
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplCoreAddin.MultiuserMonitor.Properties;
using Eplan.EplExt.EMultiuserMonitor;
using Eplan.EplExt.MultiuserMonitorClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class XMUMReceiver : IMultiuserMonitorReceive, IMultiuserMonitorSendFeedback, INotifyPropertyChanged
  {
    private bool m_sendStoryFirst = true;
    private bool m_receiveStoryFirst = true;
    private string m_strServerName = "";
    private string m_strPort = "";
    private string m_strError = "";
    private List<EplanSystemInfoAccess> m_listReceivedAllData;
    private XMUMDialogData m_WorkingData;
    private XMUMOpenProjectDialogData m_OpenedProjectsData;
    private Storyboard m_receiveStory;
    private Storyboard m_sendStory;
    private bool m_StartReceiveDataDone;
    private bool m_bMakeLocalPathsUnique;
    private int m_ServerstateIsConnected;
    private bool m_WaitForReconnect;
    private bool m_WaitForDisConnect;
    private bool m_EventRegistered;
    private bool m_bShowAllAsOpened;
    private Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor m_master;
    private List<CollectedOpenProjectInfo> m_myOpenedProjects;
    private Eplan.EplApi.ApplicationFramework.EventHandler m_myOnServerChangedHandler;
    private Eplan.EplApi.ApplicationFramework.EventHandler m_myOpenHandler;
    private Eplan.EplApi.ApplicationFramework.EventHandler m_myCloseHandler;
    private Eplan.EplApi.ApplicationFramework.EventHandler m_myEndHandler;
    private bool m_bTrace;
    private Dispatcher m_MainThreadDispatcher;

    public XMUMReceiver(Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor master)
    {
      this.m_WorkingData = new XMUMDialogData();
      this.m_OpenedProjectsData = new XMUMOpenProjectDialogData();
      this.m_MainThreadDispatcher = Dispatcher.CurrentDispatcher;
      this.m_myOnServerChangedHandler = new Eplan.EplApi.ApplicationFramework.EventHandler();
      this.m_myOnServerChangedHandler.SetEvent("onServerChanged");
      this.m_myOnServerChangedHandler.EplanEvent += new EventHandlerFunction(this.m_myOnServerChangedHandler_EplanEvent);
      this.m_master = master;
    }

    ~XMUMReceiver()
    {
      Client.m_ClientStateEvent -= new System.EventHandler<ClientEventArgs>(this.OnClientStateHandler);
      this.m_EventRegistered = false;
      MultiuserMonitorReceiveSingleton.getInstance().unregisterReceiver((IMultiuserMonitorReceive) this);
      this.CleanupDialogEvents();
      this.CleanupReceiveEvents();
      this.m_master = (Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor) null;
    }

    public void internal_OnClientStateHandler(object sender, ClientEventArgs e)
    {
      if (e.EventType == ClientEventType.fIamConnected)
      {
        this.m_strError = "";
        this.ServerstateIsConnected = 1;
        if (!this.m_WaitForReconnect)
          return;
        this.m_WaitForReconnect = false;
        MultiuserMonitorReceiveSingleton.getInstance().setServerIsShutDown(false);
        this.AutoStartReceiveData();
      }
      else if (e.EventType == ClientEventType.fIamDisconnected)
      {
        this.ServerstateIsConnected = 0;
        try
        {
          Client.getInstance().isConnected(ref this.m_strError);
        }
        catch (Exception ex)
        {
        }
        if (!this.m_WaitForDisConnect)
          return;
        this.m_WaitForDisConnect = false;
        this.StartReceiveData();
      }
      else
      {
        if (e.EventType != ClientEventType.fIamConnecting)
          return;
        this.ServerstateIsConnected = 2;
      }
    }

    public void OnClientStateHandler(object sender, ClientEventArgs e)
    {
      if (this.m_MainThreadDispatcher.CheckAccess())
        this.internal_OnClientStateHandler(sender, e);
      else
        this.m_MainThreadDispatcher.BeginInvoke(DispatcherPriority.Send, (Delegate) new XMUMReceiver.ClientStateHandlerDelegate(this.internal_OnClientStateHandler), sender, (object) e);
    }

    public int ServerstateIsConnected
    {
      get
      {
        return this.m_ServerstateIsConnected;
      }
      set
      {
        if (this.m_ServerstateIsConnected == value)
          return;
        this.m_ServerstateIsConnected = value;
        this.OnPropertyChanged(nameof (ServerstateIsConnected));
        this.OnPropertyChanged("ServerstateText");
        this.OnPropertyChanged("ServerstateError");
        Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor.SetIsConnectedProperty((UIElement) this.m_master, this.m_ServerstateIsConnected == 1);
      }
    }

    public string ServerstateText
    {
      get
      {
        if (this.m_ServerstateIsConnected == 1)
          return Resources.Connected;
        if (this.m_ServerstateIsConnected == 2)
          return Resources.Connecting;
        return Resources.NotConnected;
      }
    }

    public string ServerstateError
    {
      get
      {
        return this.m_strError;
      }
    }

    public void setReceiveStoryBoard(Storyboard aBoard)
    {
      this.m_receiveStory = aBoard;
      this.m_receiveStoryFirst = true;
    }

    public void setSendStoryBoard(Storyboard aBoard)
    {
      this.m_sendStory = aBoard;
      this.m_sendStoryFirst = true;
    }

    private void convertPath(ref string strPath)
    {
      bool isServerFile = false;
      ProjectPathHelper.convertProjectPath(this.m_bMakeLocalPathsUnique, ref strPath, ref isServerFile);
    }

    private bool isMyProject(CollectedOpenProjectInfo iProject)
    {
      if (this.m_bShowAllAsOpened)
        return true;
      if (this.m_myOpenedProjects == null)
        return false;
      foreach (CollectedOpenProjectInfo myOpenedProject in this.m_myOpenedProjects)
      {
        if (string.Compare(myOpenedProject.ProjectName, iProject.ProjectName, true) == 0 && string.Compare(myOpenedProject.ProjectPath, iProject.ProjectPath, true) == 0)
          return true;
      }
      return false;
    }

    private void m_myOnServerChangedHandler_EplanEvent(IEventParameter iEventParameter)
    {
      MultiuserMonitorReceiveSingleton.getInstance().setServerIsShutDown(false);
      this.AutoStartReceiveData();
    }

    private void m_myOpenHandler_EplanEvent(IEventParameter iEventParameter)
    {
      this.collectOpenProjects();
    }

    private void m_myCloseHandler_EplanEvent(IEventParameter iEventParameter)
    {
      this.collectOpenProjects();
    }

    private void m_myEndHandler_EplanEvent(IEventParameter iEventParameter)
    {
      this.StopReceiveData();
    }

    private void collectOpenProjects()
    {
      if (this.m_myOpenedProjects == null)
        this.m_myOpenedProjects = new List<CollectedOpenProjectInfo>();
      List<CollectedOpenProjectInfo> pList = new List<CollectedOpenProjectInfo>();
      this.getOpenProjects(ref pList);
      foreach (CollectedOpenProjectInfo collectedInfo in pList)
      {
        bool flag = false;
        foreach (CollectedOpenProjectInfo myOpenedProject in this.m_myOpenedProjects)
        {
          if (string.Compare(collectedInfo.ProjectName, myOpenedProject.ProjectName, true) == 0 && string.Compare(collectedInfo.ProjectPath, myOpenedProject.ProjectPath, true) == 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          this.addProjectToOpen(collectedInfo);
      }
      foreach (CollectedOpenProjectInfo myOpenedProject in this.m_myOpenedProjects)
      {
        bool flag = false;
        foreach (CollectedOpenProjectInfo collectedOpenProjectInfo in pList)
        {
          if (string.Compare(collectedOpenProjectInfo.ProjectName, myOpenedProject.ProjectName, true) == 0 && string.Compare(collectedOpenProjectInfo.ProjectPath, myOpenedProject.ProjectPath, true) == 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          this.removeProjectFromOpen(myOpenedProject);
      }
      this.m_myOpenedProjects = pList;
    }

    private void addProjectToOpen(CollectedOpenProjectInfo collectedInfo)
    {
      int count = this.m_OpenedProjectsData.Collection.Count;
      while (count > 0)
      {
        --count;
        XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem = this.m_OpenedProjectsData.Collection[count];
        if (string.Compare(projectDialogDataItem.ProjectName, collectedInfo.ProjectName, true) == 0 && string.Compare(projectDialogDataItem.ProjectPath, collectedInfo.ProjectPath, true) == 0)
        {
          int nIndex = 0;
          for (XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell index = projectDialogDataItem.GetIndex(nIndex); index != null; index = projectDialogDataItem.GetIndex(nIndex))
          {
            EplanSystemInfoAccess info = new EplanSystemInfoAccess();
            info.OperationType = index.LastOperationDone;
            info.HostName = index.HostName;
            info.ProcessId = index.ProcessId;
            info.ProductInfo = index.ProductInfo;
            info.User = index.User;
            info.Email = index.Email;
            info.Computer = index.Computer;
            info.Phone = index.Phone;
            info.Name = index.Name;
            info.WorkingProjectInfo = new CollectedWorkingProjectInfo(projectDialogDataItem.ProjectName, projectDialogDataItem.ProjectPath, "", 0, projectDialogDataItem.EditingArea);
            this.addWorkingProject(info, (CollectedOpenProjectInfo) info.WorkingProjectInfo, (List<XMUMDialogData.XMUMDialogDataItem>) null);
            ++nIndex;
          }
          this.m_OpenedProjectsData.Collection.RemoveAt(count);
        }
      }
    }

    private void removeProjectFromOpen(CollectedOpenProjectInfo collectedInfo)
    {
      int count = this.m_WorkingData.Collection.Count;
      while (count > 0)
      {
        --count;
        XMUMDialogData.XMUMDialogDataItem xmumDialogDataItem = this.m_WorkingData.Collection[count];
        if (string.Compare(xmumDialogDataItem.ProjectName, collectedInfo.ProjectName, true) == 0 && string.Compare(xmumDialogDataItem.ProjectPath, collectedInfo.ProjectPath, true) == 0)
        {
          EplanSystemInfoAccess info = new EplanSystemInfoAccess();
          info.OperationType = xmumDialogDataItem.LastOperationDone;
          info.HostName = xmumDialogDataItem.HostName;
          info.ProcessId = xmumDialogDataItem.ProcessId;
          info.ProductInfo = xmumDialogDataItem.ProductInfo;
          info.User = xmumDialogDataItem.User;
          info.Email = xmumDialogDataItem.Email;
          info.Computer = xmumDialogDataItem.Computer;
          info.Phone = xmumDialogDataItem.Phone;
          info.Name = xmumDialogDataItem.Name;
          info.WorkingProjectInfo = new CollectedWorkingProjectInfo(xmumDialogDataItem.ProjectName, xmumDialogDataItem.ProjectPath, "", 0, xmumDialogDataItem.EditingArea);
          this.addOpenProject(info, (CollectedOpenProjectInfo) info.WorkingProjectInfo, (List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem>) null);
          this.m_WorkingData.Collection.RemoveAt(count);
        }
      }
    }

    private void getOpenProjects(ref List<CollectedOpenProjectInfo> pList)
    {
      if (pList == null)
        pList = new List<CollectedOpenProjectInfo>();
      using (new LockingStep())
      {
        foreach (Project openProject in new ProjectManager()
        {
          LockProjectByDefault = false
        }.OpenProjects)
        {
          CollectedOpenProjectInfo collectedOpenProjectInfo = new CollectedOpenProjectInfo();
          collectedOpenProjectInfo.ProjectName = openProject.ProjectName;
          string projectDirectoryPath = openProject.ProjectDirectoryPath;
          this.convertPath(ref projectDirectoryPath);
          collectedOpenProjectInfo.ProjectPath = projectDirectoryPath;
          pList.Add(collectedOpenProjectInfo);
        }
      }
    }

    public void AutoStartReceiveData()
    {
      this.m_WaitForReconnect = false;
      bool bOn = false;
      Settings settings = new Settings();
      if (settings.ExistSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Dialog.ReceiveMySendData"))
        bOn = settings.GetBoolSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Dialog.ReceiveMySendData", 0);
      MultiuserMonitorReceiveSingleton.getInstance().setReceiveMySendData(bOn);
      this.m_bTrace = false;
      if (settings.ExistSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Dialog.TraceReceiveData"))
        this.m_bTrace = settings.GetBoolSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Dialog.TraceReceiveData", 0);
      this.m_bShowAllAsOpened = false;
      if (settings.ExistSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Dialog.ShowAllAsOpened"))
        this.m_bShowAllAsOpened = settings.GetBoolSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Dialog.ShowAllAsOpened", 0);
      bool flag1 = false;
      bool flag2 = false;
      if (!MultiuserMonitorReceiveSingleton.getInstance().getServerIsShutDown() && settings.GetBoolSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Active", 0))
      {
        string stringSetting1 = settings.GetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Name", 0);
        if (stringSetting1 != string.Empty)
        {
          if (stringSetting1 != this.m_strServerName)
          {
            flag2 = true;
            this.m_strServerName = stringSetting1;
          }
          string stringSetting2 = settings.GetStringSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.Port", 0);
          if (stringSetting2 != string.Empty)
          {
            if (stringSetting2 != this.m_strPort)
            {
              flag2 = true;
              this.m_strPort = stringSetting2;
            }
            flag1 = true;
          }
        }
      }
      bool flag3 = flag1 | bOn;
      bool startReceiveDataDone = this.m_StartReceiveDataDone;
      bool flag4 = startReceiveDataDone;
      bool flag5 = false;
      if (startReceiveDataDone)
      {
        try
        {
          string strError = "";
          flag4 = Client.getInstance().isConnected(ref strError);
          flag5 = Client.getInstance().isConnecting();
        }
        catch (Exception ex)
        {
        }
      }
      if (flag3)
      {
        if (!flag2 && (flag4 || startReceiveDataDone & flag5))
          return;
      }
      else if (!startReceiveDataDone)
        return;
      if (startReceiveDataDone & flag3)
      {
        this.StopStartReceiveData();
      }
      else
      {
        if (startReceiveDataDone)
          this.StopReceiveDataForRestart();
        if (flag3)
        {
          this.StartReceiveData();
        }
        else
        {
          this.m_strServerName = "";
          this.m_strPort = "";
          this.StopReceiveDataForRestart();
        }
      }
    }

    public void StopStartReceiveData()
    {
      if (Client.getInstance().isConnected() || Client.getInstance().isConnecting())
      {
        this.m_WaitForDisConnect = true;
        MultiuserMonitorReceiveSingleton.getInstance().unregisterReceiver((IMultiuserMonitorReceive) this);
        this.CleanReceiveDataInternal();
      }
      else
      {
        this.StopReceiveDataForRestart();
        this.StartReceiveData();
      }
    }

    public void StartReceiveData()
    {
      if (this.m_StartReceiveDataDone)
        return;
      this.m_StartReceiveDataDone = true;
      this.m_bMakeLocalPathsUnique = false;
      Settings settings = new Settings();
      if (settings.ExistSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.MakeLocalProjectsUnique"))
        this.m_bMakeLocalPathsUnique = settings.GetBoolSetting("USER.Eplan@EplCoreAddin@MultiuserMonitor.Server.MakeLocalProjectsUnique", 0);
      this.getOpenProjects(ref this.m_myOpenedProjects);
      if (this.m_myOpenHandler == null)
      {
        this.m_myOpenHandler = new Eplan.EplApi.ApplicationFramework.EventHandler();
        this.m_myOpenHandler.SetEvent("onPostOpenProject");
        this.m_myOpenHandler.EplanEvent += new EventHandlerFunction(this.m_myOpenHandler_EplanEvent);
      }
      if (this.m_myCloseHandler == null)
      {
        this.m_myCloseHandler = new Eplan.EplApi.ApplicationFramework.EventHandler();
        this.m_myCloseHandler.SetEvent("onPostCloseProject");
        this.m_myCloseHandler.EplanEvent += new EventHandlerFunction(this.m_myCloseHandler_EplanEvent);
      }
      if (this.m_myEndHandler == null)
      {
        this.m_myEndHandler = new Eplan.EplApi.ApplicationFramework.EventHandler();
        this.m_myEndHandler.SetEvent("Eplan.EplApi.OnMainEnd");
        this.m_myEndHandler.EplanEvent += new EventHandlerFunction(this.m_myEndHandler_EplanEvent);
      }
      if (!this.m_EventRegistered)
      {
        this.m_EventRegistered = true;
        Client.m_ClientStateEvent += new System.EventHandler<ClientEventArgs>(this.OnClientStateHandler);
      }
      MultiuserMonitorReceiveSingleton.getInstance().registerReceiver((IMultiuserMonitorReceive) this, this.m_strServerName, this.m_strPort);
      this.m_strError = "";
      try
      {
        if (Client.getInstance().isConnected(ref this.m_strError))
          this.ServerstateIsConnected = 1;
        else
          this.ServerstateIsConnected = 0;
      }
      catch (Exception ex)
      {
      }
    }

    private void CleanupDialogEvents()
    {
      if (this.m_myEndHandler != null)
      {
        this.m_myEndHandler.EplanEvent -= new EventHandlerFunction(this.m_myEndHandler_EplanEvent);
        this.m_myEndHandler.Dispose();
        this.m_myEndHandler = (Eplan.EplApi.ApplicationFramework.EventHandler) null;
      }
      if (this.m_myOnServerChangedHandler == null)
        return;
      this.m_myOnServerChangedHandler.EplanEvent -= new EventHandlerFunction(this.m_myOnServerChangedHandler_EplanEvent);
      this.m_myOnServerChangedHandler.Dispose();
      this.m_myOnServerChangedHandler = (Eplan.EplApi.ApplicationFramework.EventHandler) null;
    }

    private void CleanupReceiveEvents()
    {
      if (this.m_myOpenHandler != null)
      {
        this.m_myOpenHandler.EplanEvent -= new EventHandlerFunction(this.m_myOpenHandler_EplanEvent);
        this.m_myOpenHandler.Dispose();
        this.m_myOpenHandler = (Eplan.EplApi.ApplicationFramework.EventHandler) null;
      }
      if (this.m_myCloseHandler != null)
      {
        this.m_myCloseHandler.EplanEvent -= new EventHandlerFunction(this.m_myCloseHandler_EplanEvent);
        this.m_myCloseHandler.Dispose();
        this.m_myCloseHandler = (Eplan.EplApi.ApplicationFramework.EventHandler) null;
      }
      this.ServerstateIsConnected = 0;
    }

    private void CleanReceiveDataInternal()
    {
      this.CleanupReceiveEvents();
      this.m_WorkingData.Cleanup();
      this.m_OpenedProjectsData.Cleanup();
      this.m_StartReceiveDataDone = false;
      this.ServerstateIsConnected = 0;
    }

    public void StopReceiveDataForRestart()
    {
      Client.m_ClientStateEvent -= new System.EventHandler<ClientEventArgs>(this.OnClientStateHandler);
      this.m_EventRegistered = false;
      MultiuserMonitorReceiveSingleton.getInstance().unregisterReceiver((IMultiuserMonitorReceive) this);
      this.CleanReceiveDataInternal();
    }

    public void StopReceiveData()
    {
      Client.m_ClientStateEvent -= new System.EventHandler<ClientEventArgs>(this.OnClientStateHandler);
      this.m_EventRegistered = false;
      MultiuserMonitorReceiveSingleton.getInstance().unregisterReceiver((IMultiuserMonitorReceive) this);
      this.CleanReceiveDataInternal();
      this.CleanupDialogEvents();
      this.m_master = (Eplan.EplCoreAddin.MultiuserMonitor.MultiuserMonitor) null;
    }

    public XMUMDialogData WorkingProjectsContext
    {
      get
      {
        return this.m_WorkingData;
      }
    }

    public XMUMOpenProjectDialogData OpenedProjectsContext
    {
      get
      {
        return this.m_OpenedProjectsData;
      }
    }

    private void removeUnchangedProjects(EplanSystemInfoAccess info, List<XMUMDialogData.XMUMDialogDataItem> pChangedWorking, List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem> pChanged)
    {
      if (pChangedWorking != null)
      {
        int count = this.m_WorkingData.Collection.Count;
        while (count > 0)
        {
          --count;
          XMUMDialogData.XMUMDialogDataItem xmumDialogDataItem = this.m_WorkingData.Collection[count];
          if (xmumDialogDataItem.ProcessId == info.ProcessId && xmumDialogDataItem.HostName == info.HostName && !pChangedWorking.Contains(xmumDialogDataItem))
            this.m_WorkingData.Collection.RemoveAt(count);
        }
      }
      if (pChanged == null)
        return;
      int count1 = this.m_OpenedProjectsData.Collection.Count;
      while (count1 > 0)
      {
        --count1;
        XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem = this.m_OpenedProjectsData.Collection[count1];
        int index = projectDialogDataItem.FindIndex(info.ProcessId, info.HostName);
        if (index >= 0 && !pChanged.Contains(projectDialogDataItem) && projectDialogDataItem.RemoveCell(index) == 0)
          this.m_OpenedProjectsData.Collection.RemoveAt(count1);
      }
    }

    public void receive(EplanSystemInfo info)
    {
      if (this.m_MainThreadDispatcher.CheckAccess())
        this.internal_receive(info);
      else
        this.m_MainThreadDispatcher.BeginInvoke(DispatcherPriority.Send, (Delegate) new XMUMReceiver.receiveDelegate(this.internal_receive), (object) info);
    }

    private void setUserData(EplanSystemInfoAccess userData)
    {
      if (this.m_listReceivedAllData == null)
      {
        this.m_listReceivedAllData = new List<EplanSystemInfoAccess>();
        this.m_listReceivedAllData.Add(userData);
      }
      else
      {
        bool flag = false;
        foreach (EplanSystemInfoAccess systemInfoAccess in this.m_listReceivedAllData)
        {
          if (systemInfoAccess.ProcessId == userData.ProcessId && systemInfoAccess.HostName == userData.HostName)
          {
            flag = true;
            systemInfoAccess.CopyFrom(userData);
          }
        }
        if (flag)
          return;
        this.m_listReceivedAllData.Add(userData);
      }
    }

    private EplanSystemInfoAccess findUserData(EplanSystemInfoAccess info)
    {
      if (info.OperationType == InfoType.fAll)
        return info;
      if (this.m_listReceivedAllData == null)
        return (EplanSystemInfoAccess) null;
      foreach (EplanSystemInfoAccess systemInfoAccess in this.m_listReceivedAllData)
      {
        if (systemInfoAccess.ProcessId == info.ProcessId && systemInfoAccess.HostName == info.HostName)
          return systemInfoAccess;
      }
      return (EplanSystemInfoAccess) null;
    }

    private void validateUserData(EplanSystemInfoAccess info)
    {
      if (info.Email == null)
        info.Email = "";
      if (info.Name == null)
        info.Name = "";
      if (info.Computer == null)
        info.Computer = "";
      if (info.User == null)
        info.User = "";
      if (info.ProductInfo != null)
        return;
      info.ProductInfo = "";
    }

    public static bool getInfoIsKnown(EplanSystemInfoAccess info)
    {
      int length = Enum.GetValues(typeof (InfoType)).Length;
      return info.OperationType <= (InfoType) length;
    }

    private bool getIsDeltaDataOnly(EplanSystemInfoAccess info)
    {
      return info.OperationType == InfoType.fProjectOpen || info.OperationType == InfoType.fProjectClose || (info.OperationType == InfoType.fUpdateWorkingProject || info.OperationType == InfoType.fUpdateWorkingProjectValues) || (info.OperationType == InfoType.fUpdateWorkingProjectWorkDone || info.OperationType == InfoType.fEditingArea);
    }

    public void internal_receive(EplanSystemInfo info)
    {
      if (this.m_bTrace)
        this.traceReceive(info);
      EplanSystemInfoAccess systemInfoAccess = new EplanSystemInfoAccess();
      systemInfoAccess.ConvertFrom(info);
      if (systemInfoAccess.OperationType == InfoType.fNone || !XMUMReceiver.getInfoIsKnown(systemInfoAccess))
        return;
      if (this.m_receiveStory != null && (this.m_receiveStoryFirst || this.m_receiveStory.GetCurrentState() != ClockState.Active))
      {
        this.m_receiveStoryFirst = false;
        this.m_receiveStory.Begin();
      }
      List<XMUMDialogData.XMUMDialogDataItem> xmumDialogDataItemList = (List<XMUMDialogData.XMUMDialogDataItem>) null;
      List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem> pChanged = (List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem>) null;
      if (systemInfoAccess.OperationType == InfoType.fAll)
        this.setUserData(systemInfoAccess);
      bool flag = !this.getIsDeltaDataOnly(systemInfoAccess);
      if (flag)
      {
        xmumDialogDataItemList = new List<XMUMDialogData.XMUMDialogDataItem>();
        pChanged = new List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem>();
      }
      if (((systemInfoAccess.OperationType == InfoType.fUpdateWorkingProject || systemInfoAccess.OperationType == InfoType.fUpdateWorkingProjectValues || systemInfoAccess.OperationType == InfoType.fUpdateWorkingProjectWorkDone ? 1 : (systemInfoAccess.OperationType == InfoType.fEditingArea ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        CollectedWorkingProjectInfo workingProjectInfo = systemInfoAccess.WorkingProjectInfo;
        if (workingProjectInfo != null)
        {
          if (this.isMyProject((CollectedOpenProjectInfo) workingProjectInfo))
            this.addWorkingProject(systemInfoAccess, (CollectedOpenProjectInfo) workingProjectInfo, xmumDialogDataItemList);
          else
            this.addOpenProject(systemInfoAccess, (CollectedOpenProjectInfo) workingProjectInfo, pChanged);
        }
      }
      if (((systemInfoAccess.OperationType == InfoType.fProjectOpen ? 1 : (systemInfoAccess.OperationType == InfoType.fEditingArea ? 1 : 0)) | (flag ? 1 : 0)) != 0 && systemInfoAccess.OpenedProjects != null)
      {
        foreach (CollectedOpenProjectInfo openedProject in systemInfoAccess.OpenedProjects)
        {
          if (this.isMyProject(openedProject))
            this.addWorkingProject(systemInfoAccess, openedProject, xmumDialogDataItemList);
          else
            this.addOpenProject(systemInfoAccess, openedProject, pChanged);
        }
      }
      if (systemInfoAccess.OperationType == InfoType.fProjectClose && systemInfoAccess.OpenedProjects != null)
      {
        foreach (CollectedOpenProjectInfo openedProject in systemInfoAccess.OpenedProjects)
        {
          if (this.isMyProject(openedProject))
            this.removeWorkingProject(systemInfoAccess, openedProject);
          else
            this.removeOpenProject(systemInfoAccess, openedProject);
        }
      }
      if (!flag)
        return;
      this.removeUnchangedProjects(systemInfoAccess, xmumDialogDataItemList, pChanged);
    }

    public void removeOpenProject(EplanSystemInfoAccess info, CollectedOpenProjectInfo pInfo)
    {
      int index1 = -1;
      foreach (XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem in (Collection<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem>) this.m_OpenedProjectsData.Collection)
      {
        ++index1;
        if (string.Compare(projectDialogDataItem.ProjectName, pInfo.ProjectName, true) == 0 && string.Compare(projectDialogDataItem.ProjectPath, pInfo.ProjectPath, true) == 0)
        {
          int index2 = projectDialogDataItem.FindIndex(info.ProcessId, info.HostName);
          if (index2 < 0 || projectDialogDataItem.RemoveCell(index2) != 0)
            break;
          this.m_OpenedProjectsData.Collection.RemoveAt(index1);
          break;
        }
      }
    }

    public void addOpenProject(EplanSystemInfoAccess info, CollectedOpenProjectInfo pInfo, List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem> pChanged)
    {
      bool flag = false;
      int num = -1;
      foreach (XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem in (Collection<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem>) this.m_OpenedProjectsData.Collection)
      {
        ++num;
        if (string.Compare(projectDialogDataItem.ProjectName, pInfo.ProjectName, true) == 0 && string.Compare(projectDialogDataItem.ProjectPath, pInfo.ProjectPath, true) == 0)
        {
          flag = true;
          int index1 = projectDialogDataItem.FindIndex(info.ProcessId, info.HostName);
          if (pChanged != null)
            pChanged.Add(projectDialogDataItem);
          if (index1 < 0)
          {
            XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell newCell = new XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell();
            this.setItemDataFromInfo(info, newCell);
            projectDialogDataItem.AddCell(newCell);
            break;
          }
          XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell index2 = projectDialogDataItem.GetIndex(index1);
          this.setItemDataFromInfo(info, index2);
          break;
        }
      }
      if (flag)
        return;
      XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem1 = new XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem();
      XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell newCell1 = new XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell();
      this.setItemDataFromInfo(info, newCell1);
      projectDialogDataItem1.ProjectName = pInfo.ProjectName;
      projectDialogDataItem1.ProjectPath = pInfo.ProjectPath;
      projectDialogDataItem1.EditingArea = pInfo.EditingArea;
      projectDialogDataItem1.AddCell(newCell1);
      this.m_OpenedProjectsData.Collection.Add(projectDialogDataItem1);
      if (pChanged == null)
        return;
      pChanged.Add(projectDialogDataItem1);
    }

    public bool getPacketIsInWrongOrder(XMUMDialogData.XMUMDialogDataItem item, EplanSystemInfoAccess info)
    {
      return item != null && (item.LastOperationDone == InfoType.fProjectClose && (info.OperationType == InfoType.fUpdateWorkingProject || info.OperationType == InfoType.fUpdateWorkingProjectValues || (info.OperationType == InfoType.fUpdateWorkingProjectWorkDone || info.OperationType == InfoType.fEditingArea)) || item.LastOperationDone == InfoType.fUpdateWorkingProjectWorkDone && (info.OperationType == InfoType.fUpdateWorkingProjectValues || info.OperationType == InfoType.fUpdateWorkingProjectWorkDone));
    }

    public void setItemDataFromInfo(EplanSystemInfoAccess info, XMUMDialogData.XMUMDialogDataItem item)
    {
      item.LastOperationDone = info.OperationType;
      item.User = info.User;
      item.ProductInfo = info.ProductInfo;
      item.ProcessId = info.ProcessId;
      item.HostName = info.HostName;
      EplanSystemInfoAccess userData = this.findUserData(info);
      if (userData == null)
        return;
      item.Email = userData.Email;
      item.Computer = userData.Computer;
      item.Name = userData.Name;
      item.Phone = userData.Phone;
    }

    public void setItemDataFromInfo(EplanSystemInfoAccess info, XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell item)
    {
      item.LastOperationDone = info.OperationType;
      item.User = info.User;
      item.ProductInfo = info.ProductInfo;
      item.ProcessId = info.ProcessId;
      item.HostName = info.HostName;
      EplanSystemInfoAccess userData = this.findUserData(info);
      if (userData == null)
        return;
      item.Email = userData.Email;
      item.Computer = userData.Computer;
      item.Name = userData.Name;
      item.Phone = userData.Phone;
    }

    public void addWorkingProject(EplanSystemInfoAccess info, CollectedOpenProjectInfo pInfo, List<XMUMDialogData.XMUMDialogDataItem> pChanged)
    {
      bool flag1 = false;
      foreach (XMUMDialogData.XMUMDialogDataItem xmumDialogDataItem in (Collection<XMUMDialogData.XMUMDialogDataItem>) this.m_WorkingData.Collection)
      {
        if (xmumDialogDataItem.ProcessId == info.ProcessId && xmumDialogDataItem.HostName == info.HostName && (string.Compare(xmumDialogDataItem.ProjectName, pInfo.ProjectName, true) == 0 && string.Compare(xmumDialogDataItem.ProjectPath, pInfo.ProjectPath, true) == 0))
        {
          flag1 = true;
          if (!this.getPacketIsInWrongOrder(xmumDialogDataItem, info))
          {
            if (pChanged != null)
              pChanged.Add(xmumDialogDataItem);
            this.setItemDataFromInfo(info, xmumDialogDataItem);
            CollectedWorkingProjectInfo workingProjectInfo = pInfo as CollectedWorkingProjectInfo;
            bool flag2 = true;
            if (workingProjectInfo != null && (info.OperationType == InfoType.fUpdateWorkingProject || info.OperationType == InfoType.fUpdateWorkingProjectValues))
            {
              flag2 = false;
              xmumDialogDataItem.WorkingTitle = workingProjectInfo.WhatIsDone;
              xmumDialogDataItem.PercentDone = (double) workingProjectInfo.Percent;
              xmumDialogDataItem.AtWork = true;
            }
            if (info.OperationType == InfoType.fUpdateWorkingProject || info.OperationType == InfoType.fEditingArea)
            {
              flag2 = false;
              xmumDialogDataItem.EditingArea = pInfo.EditingArea;
            }
            if (flag2)
            {
              xmumDialogDataItem.WorkingTitle = "";
              xmumDialogDataItem.PercentDone = 0.0;
              xmumDialogDataItem.AtWork = false;
            }
          }
        }
      }
      if (flag1 || pInfo == null)
        return;
      XMUMDialogData.XMUMDialogDataItem xmumDialogDataItem1 = new XMUMDialogData.XMUMDialogDataItem();
      xmumDialogDataItem1.ProjectName = pInfo.ProjectName;
      xmumDialogDataItem1.ProjectPath = pInfo.ProjectPath;
      xmumDialogDataItem1.EditingArea = pInfo.EditingArea;
      this.setItemDataFromInfo(info, xmumDialogDataItem1);
      CollectedWorkingProjectInfo workingProjectInfo1 = pInfo as CollectedWorkingProjectInfo;
      bool flag3 = true;
      if (workingProjectInfo1 != null && (info.OperationType == InfoType.fUpdateWorkingProject || info.OperationType == InfoType.fUpdateWorkingProjectValues))
      {
        flag3 = false;
        xmumDialogDataItem1.WorkingTitle = workingProjectInfo1.WhatIsDone;
        xmumDialogDataItem1.PercentDone = (double) workingProjectInfo1.Percent;
        xmumDialogDataItem1.AtWork = true;
      }
      if (info.OperationType == InfoType.fUpdateWorkingProject || info.OperationType == InfoType.fEditingArea)
      {
        flag3 = false;
        xmumDialogDataItem1.EditingArea = pInfo.EditingArea;
      }
      if (flag3)
      {
        xmumDialogDataItem1.WorkingTitle = "";
        xmumDialogDataItem1.PercentDone = 0.0;
        xmumDialogDataItem1.AtWork = false;
      }
      this.m_WorkingData.Collection.Add(xmumDialogDataItem1);
      if (pChanged == null)
        return;
      pChanged.Add(xmumDialogDataItem1);
    }

    public void removeWorkingProject(EplanSystemInfoAccess info, CollectedOpenProjectInfo pInfo)
    {
      foreach (XMUMDialogData.XMUMDialogDataItem xmumDialogDataItem in (Collection<XMUMDialogData.XMUMDialogDataItem>) this.m_WorkingData.Collection)
      {
        if (xmumDialogDataItem.ProcessId == info.ProcessId && xmumDialogDataItem.HostName == info.HostName && (string.Compare(xmumDialogDataItem.ProjectName, pInfo.ProjectName, true) == 0 && string.Compare(xmumDialogDataItem.ProjectPath, pInfo.ProjectPath, true) == 0))
        {
          this.m_WorkingData.Collection.Remove(xmumDialogDataItem);
          break;
        }
      }
    }

    public void setServerShutDown()
    {
      if (this.m_MainThreadDispatcher.CheckAccess())
        this.internal_setServerShutDown();
      else
        this.m_MainThreadDispatcher.BeginInvoke(DispatcherPriority.Send, (Delegate) new XMUMReceiver.setServerShutDownDelegate(this.internal_setServerShutDown));
    }

    private void internal_setServerShutDown()
    {
      this.m_WaitForReconnect = true;
      this.CleanReceiveDataInternal();
    }

    private void traceReceive(EplanSystemInfo info)
    {
      EplTrace eplTrace = new EplTrace();
      eplTrace.Trace("---->  MUM receive");
      foreach (KeyValuePair<string, string> parameter in info.Parameters)
        eplTrace.Trace("       " + parameter.Key + ": " + parameter.Value);
      eplTrace.Trace("---->  MUM receive end");
    }

    public void internal_sendPing()
    {
      if (this.m_sendStory == null || !this.m_sendStoryFirst && this.m_sendStory.GetCurrentState() == ClockState.Active)
        return;
      this.m_sendStoryFirst = false;
      this.m_sendStory.Begin();
    }

    public void sendPing()
    {
      if (this.m_MainThreadDispatcher.CheckAccess())
        this.internal_sendPing();
      else
        this.m_MainThreadDispatcher.BeginInvoke(DispatcherPriority.Send, (Delegate) new XMUMReceiver.sendPingDelegate(this.internal_sendPing));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.PropertyChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public delegate void ClientStateHandlerDelegate(object sender, ClientEventArgs e);

    public delegate void receiveDelegate(EplanSystemInfo info);

    public delegate void setServerShutDownDelegate();

    public delegate void sendPingDelegate();
  }
}
