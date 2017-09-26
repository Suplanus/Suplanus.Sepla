// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.XMUMOpenProjectDialogData
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplExt.EMultiuserMonitor;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class XMUMOpenProjectDialogData : INotifyPropertyChanged, IDisposable
  {
    private XMUMOpenProjectDialogData.ItemCollection _Collection = new XMUMOpenProjectDialogData.ItemCollection();

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.PropertyChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    ~XMUMOpenProjectDialogData()
    {
    }

    public void Dispose()
    {
    }

    public void Cleanup()
    {
      this._Collection.Clear();
    }

    public XMUMOpenProjectDialogData.ItemCollection Collection
    {
      get
      {
        return this._Collection;
      }
    }

    public class XMUMOpenProjectDialogDataItemCell
    {
      private string _HostName = string.Empty;
      private string _User = string.Empty;
      private string _UserName = string.Empty;
      private string _UserEmail = string.Empty;
      private string _UserPhone = string.Empty;
      private string _UserComputer = string.Empty;
      private string _ProductInfo = string.Empty;
      private long _ProcessId;
      private InfoType _LastOperationDone;

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
        }
      }
    }

    public class XMUMOpenProjectDialogDataItem : INotifyPropertyChanged
    {
      private string _ProjectName = string.Empty;
      private XMUMOpenProjectDialogData.CellCollection _CellDataList = new XMUMOpenProjectDialogData.CellCollection();
      private string _UserList = string.Empty;
      private string _ProjectPath = string.Empty;
      private string _EditingArea = string.Empty;

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

      public XMUMOpenProjectDialogData.CellCollection Cell
      {
        get
        {
          return this._CellDataList;
        }
      }

      public string UserList
      {
        get
        {
          return this._UserList;
        }
        set
        {
          if (!(this._UserList != value))
            return;
          this._UserList = value;
          this.OnPropertyChanged(nameof (UserList));
        }
      }

      public XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell GetIndex(int nIndex)
      {
        if (this._CellDataList.Count > nIndex)
          return this._CellDataList[nIndex];
        return (XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell) null;
      }

      public int FindIndex(long nProcessId, string strHostName)
      {
        int num = -1;
        foreach (XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell cellData in (System.Collections.ObjectModel.Collection<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell>) this._CellDataList)
        {
          ++num;
          if (cellData.ProcessId == nProcessId && cellData.HostName == strHostName)
            return num;
        }
        return -1;
      }

      public bool AddCell(XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell newCell)
      {
        StringBuilder stringBuilder = new StringBuilder();
        bool flag = true;
        foreach (XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell cellData in (System.Collections.ObjectModel.Collection<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell>) this._CellDataList)
        {
          if (!flag)
            stringBuilder.Append(", ");
          flag = false;
          stringBuilder.Append(cellData.User);
        }
        if (!flag)
          stringBuilder.Append(", ");
        stringBuilder.Append(newCell.User);
        this._CellDataList.Add(newCell);
        this.UserList = stringBuilder.ToString();
        this.OnPropertyChanged("UserList");
        return true;
      }

      public int RemoveCell(int nWantedIndex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        int num = 0;
        int index = -1;
        bool flag = true;
        foreach (XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell cellData in (System.Collections.ObjectModel.Collection<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell>) this._CellDataList)
        {
          if (nWantedIndex == num)
          {
            index = num;
          }
          else
          {
            if (!flag)
              stringBuilder.Append(", ");
            flag = false;
            stringBuilder.Append(cellData.User);
          }
          ++num;
        }
        if (index < 0)
          return num;
        this._CellDataList.RemoveAt(index);
        this.UserList = stringBuilder.ToString();
        this.OnPropertyChanged("UserList");
        return num - 1;
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
    }

    public class CellCollection : ObservableCollection<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItemCell>
    {
    }

    public class ItemCollection : ObservableCollection<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem>
    {
    }
  }
}
