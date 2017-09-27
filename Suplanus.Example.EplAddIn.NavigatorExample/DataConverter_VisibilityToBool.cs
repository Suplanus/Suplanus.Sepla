// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.DataConverter_VisibilityToBool
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class DataConverter_VisibilityToBool : IValueConverter
  {
    private Visibility _falseToVisibility = Visibility.Collapsed;

    public Visibility FalseToVisibility
    {
      get
      {
        return this._falseToVisibility;
      }
      set
      {
        this._falseToVisibility = value;
      }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        return (object) ((Visibility) value != this.FalseToVisibility);
      }
      catch
      {
        throw;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        return (object) (Visibility) (!(bool) value ? (int) this.FalseToVisibility : (int) this.ReverseVisibility(this.FalseToVisibility));
      }
      catch
      {
        throw;
      }
    }

    private Visibility ReverseVisibility(Visibility vsi)
    {
      Visibility visibility = Visibility.Visible;
      if (vsi != Visibility.Visible)
      {
        if (vsi == Visibility.Collapsed)
          visibility = Visibility.Visible;
      }
      else
        visibility = Visibility.Collapsed;
      return visibility;
    }
  }
}
