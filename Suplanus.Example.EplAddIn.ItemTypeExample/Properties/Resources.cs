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

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                    resourceMan = new ResourceManager("Eplan.EplCoreAddin.MultiuserMonitor.Properties.Resources",
                        typeof(Resources).Assembly);
                return resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static CultureInfo Culture { get; set; }

        public static string Connected => ResourceManager.GetString(nameof(Connected), Culture);

        public static string Connecting => ResourceManager.GetString(nameof(Connecting), Culture);

        public static string ConnectionErrorText => ResourceManager.GetString(nameof(ConnectionErrorText), Culture);

        public static string ConnectionErrorTitle => ResourceManager.GetString(nameof(ConnectionErrorTitle), Culture);

        public static string MultiuserMonitorTitle => ResourceManager.GetString(nameof(MultiuserMonitorTitle), Culture);

        public static string NotConnected => ResourceManager.GetString(nameof(NotConnected), Culture);

        public static string SettingsDialogTitle => ResourceManager.GetString(nameof(SettingsDialogTitle), Culture);

        public static string SettingsDialogValuePortInvalid =>
            ResourceManager.GetString(nameof(SettingsDialogValuePortInvalid), Culture);
    }
}