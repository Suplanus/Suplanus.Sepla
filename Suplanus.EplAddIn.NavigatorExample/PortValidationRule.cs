// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.PortValidationRule
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplCoreAddin.MultiuserMonitor.Properties;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class PortValidationRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      try
      {
        int result = 0;
        bool flag = false;
        if (((string) value).Length > 0)
          flag = int.TryParse((string) value, out result);
        if (result >= 1)
        {
          if (flag)
            goto label_6;
        }
        return new ValidationResult(false, (object) Resources.SettingsDialogValuePortInvalid);
      }
      catch (Exception ex)
      {
        return new ValidationResult(false, (object) ex.Message);
      }
label_6:
      return new ValidationResult(true, (object) null);
    }
  }
}
