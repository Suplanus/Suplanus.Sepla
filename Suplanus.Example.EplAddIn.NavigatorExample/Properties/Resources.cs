// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Eplan.EplCoreAddin.MultiuserMonitor.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  public class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static ResourceManager ResourceManager
    {
      get
      {
        if (Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceMan == null)
          Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceMan = new ResourceManager("Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources", typeof (Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources).Assembly);
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static CultureInfo Culture
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture;
      }
      set
      {
        Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture = value;
      }
    }

    public static string Connected
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (Connected), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }

    public static string Connecting
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (Connecting), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }

    public static string ConnectionErrorText
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (ConnectionErrorText), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }

    public static string ConnectionErrorTitle
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (ConnectionErrorTitle), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }

    public static string MultiuserMonitorTitle
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (MultiuserMonitorTitle), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }

    public static string NotConnected
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (NotConnected), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }

    public static string SettingsDialogTitle
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (SettingsDialogTitle), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }

    public static string SettingsDialogValuePortInvalid
    {
      get
      {
        return Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.ResourceManager.GetString(nameof (SettingsDialogValuePortInvalid), Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources.resourceCulture);
      }
    }
  }
}
