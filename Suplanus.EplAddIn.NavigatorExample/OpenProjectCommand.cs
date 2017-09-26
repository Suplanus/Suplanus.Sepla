// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.OpenProjectCommand
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  internal class OpenProjectCommand : ICommand
  {
    public List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem> GetSelList(object parameter)
    {
      List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem> projectDialogDataItemList = new List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem>();
      IList<DataGridCellInfo> dataGridCellInfoList = parameter as IList<DataGridCellInfo>;
      if (dataGridCellInfoList != null)
      {
        foreach (DataGridCellInfo dataGridCellInfo in (IEnumerable<DataGridCellInfo>) dataGridCellInfoList)
        {
          XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem = dataGridCellInfo.Item as XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem;
          if (projectDialogDataItem != null && !projectDialogDataItemList.Contains(projectDialogDataItem))
            projectDialogDataItemList.Add(projectDialogDataItem);
        }
      }
      return projectDialogDataItemList;
    }

    public bool CanExecute(object parameter)
    {
      List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem> selList = this.GetSelList(parameter);
      if (selList != null && selList.Count == 1)
      {
        XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem = selList[0];
        if (projectDialogDataItem.ProjectPath != null && projectDialogDataItem.ProjectPath.StartsWith("\\"))
          return true;
      }
      return false;
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        CommandManager.RequerySuggested += value;
      }
      remove
      {
        CommandManager.RequerySuggested -= value;
      }
    }

    public void Execute(object parameter)
    {
      List<XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem> selList = this.GetSelList(parameter);
      if (selList == null || selList.Count != 1)
        return;
      XMUMOpenProjectDialogData.XMUMOpenProjectDialogDataItem projectDialogDataItem = selList[0];
      if (projectDialogDataItem.ProjectPath == null)
        return;
      try
      {
        if (!projectDialogDataItem.ProjectPath.StartsWith("\\"))
          return;
        ProjectManager projectManager = new ProjectManager();
        projectManager.LockProjectByDefault = false;
        string str = projectDialogDataItem.ProjectPath + "\\" + projectDialogDataItem.ProjectName;
        FileInfo[] files = new DirectoryInfo(projectDialogDataItem.ProjectPath).GetFiles(projectDialogDataItem.ProjectName + ".el*");
        if (files.Length == 0)
          return;
        foreach (FileSystemInfo fileSystemInfo in files)
        {
          string fullName = fileSystemInfo.FullName;
          if (projectManager.ExistsProject(fullName))
          {
            projectManager.OpenProject(fullName);
            break;
          }
        }
      }
      catch (BaseException ex)
      {
        ex.FixMessage();
      }
      catch (Exception ex)
      {
      }
    }
  }
}
