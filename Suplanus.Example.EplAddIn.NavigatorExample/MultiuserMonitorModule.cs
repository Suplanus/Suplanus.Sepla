// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddIn.MultiuserMonitor.MultiuserMonitorModule
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplCoreAddin.MultiuserMonitor;
using Eplan.EplCoreAddin.MultiuserMonitor.Properties;
using Eplan.EplExt.MultiuserMonitorClient;
using Eplan.EplSDK.WPF;
using System.Globalization;
using Suplanus.EplAddIn.NavigatorExample;

namespace Eplan.EplCoreAddIn.MultiuserMonitor
{
  public class MultiuserMonitorModule : IEplAddIn
  {
    private EventHandler m_myOnServerChangedHandler;

    public bool OnRegister(ref bool bLoadOnStart)
    {
      bLoadOnStart = false;
      return true;
    }

    public bool OnUnregister()
    {
      return true;
    }

    public bool OnInit()
    {
      Resources.Culture = new CultureInfo(new Languages().GuiLanguage.GetString().Replace("_", "-"));
      return true;
    }

    public bool OnInitGui()
    {
        //DialogBarFactory dialogBarFactory1 = new DialogBarFactory("MultiuserMonitorBar", typeof(NavigatorContent), 568);
            //DialogFactory dialogFactory = new DialogFactory("MultiuserMonitorSettingsDialog", typeof (MultiuserMonitorSettingsDialog), 568);
            //DialogBarFactory dialogBarFactory2 = new DialogBarFactory("MultiuserMonitorGrauBar", typeof (MultiuserMonitorGrau), 568);
            //this.m_myOnServerChangedHandler = new EventHandler();
            //this.m_myOnServerChangedHandler.SetEvent("onServerChanged");
            //this.m_myOnServerChangedHandler.EplanEvent += new EventHandlerFunction(this.m_myOnServerChangedHandler_EplanEvent);
            return true;
    }

    public bool OnExit()
    {
      if (this.m_myOnServerChangedHandler != null)
      {
        this.m_myOnServerChangedHandler.EplanEvent -= new EventHandlerFunction(this.m_myOnServerChangedHandler_EplanEvent);
        this.m_myOnServerChangedHandler.Dispose();
        this.m_myOnServerChangedHandler = (EventHandler) null;
      }
      return true;
    }

    private void m_myOnServerChangedHandler_EplanEvent(IEventParameter iEventParameter)
    {
      MultiuserMonitorReceiveSingleton.getInstance().setServerIsShutDown(false);
    }
  }
}
