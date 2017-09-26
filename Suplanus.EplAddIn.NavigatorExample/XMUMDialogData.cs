// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.XMUMDialogData
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplExt.EMultiuserMonitor;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class XMUMDialogData : INotifyPropertyChanged, IDisposable
  {
    private XMUMDialogData.ItemCollection _Collection = new XMUMDialogData.ItemCollection();

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.PropertyChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    ~XMUMDialogData()
    {
    }

    public void Dispose()
    {
    }

    public void Cleanup()
    {
      this._Collection.Clear();
    }

    public XMUMDialogData.ItemCollection Collection
    {
      get
      {
        return this._Collection;
      }
    }

    public class XMUMDialogDataItem : INotifyPropertyChanged
    {
      private string _ProjectName = string.Empty;
      private string _EditingArea = string.Empty;
      private string _HostName = string.Empty;
      private string _User = string.Empty;
      private string _UserName = string.Empty;
      private string _UserEmail = string.Empty;
      private string _UserPhone = string.Empty;
      private string _UserComputer = string.Empty;
      private string _ProductInfo = string.Empty;
      private string _WorkingTitle = string.Empty;
      private string _ProjectPath = string.Empty;
      private long _ProcessId;
      private double _PercentDone;
      private bool _AtWork;
      private InfoType _LastOperationDone;

      public event PropertyChangedEventHandler PropertyChanged;

      protected virtual void OnPropertyChanged(string propertyName)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.PropertyChanged == null)
          return;
        // ISSUE: reference to a compiler-generated field
        this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
      }

      public string ProjectName
      {
        get
        {
          return this._ProjectName;
        }
        set
        {
          if (!(this._ProjectName != value))
            return;
          this._ProjectName = value;
          this.OnPropertyChanged(nameof (ProjectName));
        }
      }

      public string EditingArea
      {
        get
        {
          return this._EditingArea;
        }
        set
        {
          if (!(this._EditingArea != value))
            return;
          this._EditingArea = value;
          this.OnPropertyChanged(nameof (EditingArea));
        }
      }

      public long ProcessId
      {
        get
        {
          return this._ProcessId;
        }
        set
        {
          if (this._ProcessId == value)
            return;
          this._ProcessId = value;
          this.OnPropertyChanged(nameof (ProcessId));
        }
      }

      public string HostName
      {
        get
        {
          return this._HostName;
        }
        set
        {
          if (!(this._HostName != value))
            return;
          this._HostName = value;
          this.OnPropertyChanged(nameof (HostName));
        }
      }

      public string User
      {
        get
        {
          return this._User;
        }
        set
        {
          if (!(this._User != value))
            return;
          this._User = value;
          this.OnPropertyChanged(nameof (User));
        }
      }

      public string Name
      {
        get
        {
          return this._UserName;
        }
        set
        {
          if (!(this._UserName != value))
            return;
          this._UserName = value;
          this.OnPropertyChanged(nameof (Name));
          this.OnPropertyChanged("NameOrMail");
        }
      }

      public string NameOrMail
      {
        get
        {
          if (this._UserName.Length > 0)
            return this._UserName;
          return this._UserEmail;
        }
      }

      public string EmailUri
      {
        get
        {
          return "mailto:" + this._UserEmail;
        }
      }

      public string Email
      {
        get
        {
          return this._UserEmail;
        }
        set
        {
          if (!(this._UserEmail != value))
            return;
          this._UserEmail = value;
          this.OnPropertyChanged(nameof (Email));
          this.OnPropertyChanged("EmailUri");
          this.OnPropertyChanged("NameOrMail");
        }
      }

      public string Phone
      {
        get
        {
          return this._UserPhone;
        }
        set
        {
          if (!(this._UserPhone != value))
            return;
          this._UserPhone = value;
          this.OnPropertyChanged(nameof (Phone));
        }
      }

      public string Computer
      {
        get
        {
          return this._UserComputer;
        }
        set
        {
          if (!(this._UserComputer != value))
            return;
          this._UserComputer = value;
          this.OnPropertyChanged(nameof (Computer));
        }
      }

      public string ProductInfo
      {
        get
        {
          return this._ProductInfo;
        }
        set
        {
          if (!(this._ProductInfo != value))
            return;
          this._ProductInfo = value;
          this.OnPropertyChanged(nameof (ProductInfo));
        }
      }

      public string WorkingTitle
      {
        get
        {
          return this._WorkingTitle;
        }
        set
        {
          if (!(this._WorkingTitle != value))
            return;
          this._WorkingTitle = value;
          this.OnPropertyChanged(nameof (WorkingTitle));
        }
      }

      public double PercentDone
      {
        get
        {
          return this._PercentDone;
        }
        set
        {
          if (this._PercentDone == value)
            return;
          this._PercentDone = value;
          this.OnPropertyChanged(nameof (PercentDone));
        }
      }

      public string ProjectPath
      {
        get
        {
          return this._ProjectPath;
        }
        set
        {
          if (!(this._ProjectPath != value))
            return;
          this._ProjectPath = value;
          this.OnPropertyChanged(nameof (ProjectPath));
        }
      }

      public bool AtWork
      {
        get
        {
          return this._AtWork;
        }
        set
        {
          if (this._AtWork == value)
            return;
          this._AtWork = value;
          this.OnPropertyChanged(nameof (AtWork));
        }
      }

      public InfoType LastOperationDone
      {
        get
        {
          return this._LastOperationDone;
        }
        set
        {
          if (this._LastOperationDone == value)
            return;
          this._LastOperationDone = value;
          this.OnPropertyChanged(nameof (LastOperationDone));
        }
      }
    }

    public class ItemCollection : ObservableCollection<XMUMDialogData.XMUMDialogDataItem>
    {
    }
  }
}
