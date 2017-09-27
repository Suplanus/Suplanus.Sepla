// Decompiled with JetBrains decompiler
// Type: XamlGeneratedNamespace.GeneratedInternalTypeHelper
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Markup;

namespace XamlGeneratedNamespace
{
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class GeneratedInternalTypeHelper : InternalTypeHelper
  {
    protected override object CreateInstance(Type type, CultureInfo culture)
    {
      return Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, (Binder) null, (object[]) null, culture);
    }

    protected override object GetPropertyValue(PropertyInfo propertyInfo, object target, CultureInfo culture)
    {
      return propertyInfo.GetValue(target, BindingFlags.Default, (Binder) null, (object[]) null, culture);
    }

    protected override void SetPropertyValue(PropertyInfo propertyInfo, object target, object value, CultureInfo culture)
    {
      propertyInfo.SetValue(target, value, BindingFlags.Default, (Binder) null, (object[]) null, culture);
    }

    protected override Delegate CreateDelegate(Type delegateType, object target, string handler)
    {
      return (Delegate) target.GetType().InvokeMember("_CreateDelegate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, target, new object[2]
      {
        (object) delegateType,
        (object) handler
      }, (CultureInfo) null);
    }

    protected override void AddEventHandler(EventInfo eventInfo, object target, Delegate handler)
    {
      eventInfo.AddEventHandler(target, handler);
    }
  }
}
