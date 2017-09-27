// Decompiled with JetBrains decompiler
// Type: Eplan.EplCoreAddin.MultiuserMonitor.GridColumnSelectionBehavior
// Assembly: Eplan.EplCoreAddin.MultiuserMonitoru, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ecea1219c93267ce
// MVID: 5BE5CF07-02F2-4FBC-8C79-A9C745452AA1
// Assembly location: C:\Program Files\EPLAN\Platform\2.7.3\Bin\Eplan.EplCoreAddin.MultiuserMonitoru.dll

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Eplan.EplCoreAddin.MultiuserMonitor
{
  public class GridColumnSelectionBehavior
  {
    public static readonly DependencyProperty GridSelectColOnClickProperty = DependencyProperty.RegisterAttached("GridSelectColOnClick", typeof (bool), typeof (GridColumnSelectionBehavior), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(GridColumnSelectionBehavior.OnGridSelectColOnClickChanged)));
    public static readonly DependencyProperty IsColumnOfCellSelectedProperty = DependencyProperty.RegisterAttached("IsColumnOfCellSelected", typeof (bool), typeof (GridColumnSelectionBehavior), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(GridColumnSelectionBehavior.OnIsColumnOfCellSelectedChanged)));
    public static readonly DependencyProperty IsColumnSelectedProperty = DependencyProperty.RegisterAttached("IsColumnSelected", typeof (bool), typeof (GridColumnSelectionBehavior), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(GridColumnSelectionBehavior.OnIsColumnSelectedProperty)));
    public static readonly DependencyProperty RowHeaderContentTopProperty = DependencyProperty.RegisterAttached("RowHeaderContentTop", typeof (bool), typeof (GridColumnSelectionBehavior), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(GridColumnSelectionBehavior.OnRowHeaderContentTop)));

    public static bool GetGridSelectColOnClick(DependencyObject obj)
    {
      return (bool) obj.GetValue(GridColumnSelectionBehavior.GridSelectColOnClickProperty);
    }

    public static void SetGridSelectColOnClick(DependencyObject obj, bool value)
    {
      obj.SetValue(GridColumnSelectionBehavior.GridSelectColOnClickProperty, (object) value);
    }

    private static void OnGridSelectColOnClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      DataGrid dataGrid = target as DataGrid;
      if (dataGrid == null)
        throw new InvalidOperationException("This behavior can be attached to DataGrid only.");
      if ((bool) e.NewValue && !(bool) e.OldValue)
      {
        dataGrid.Loaded += new RoutedEventHandler(GridColumnSelectionBehavior.LoadedEvent);
        GridColumnSelectionBehavior.AddClickEvent(target);
        GridColumnSelectionBehavior.AddDblClickEvent(target);
      }
      else
      {
        if ((bool) e.NewValue || !(bool) e.OldValue)
          return;
        dataGrid.Loaded -= new RoutedEventHandler(GridColumnSelectionBehavior.LoadedEvent);
        GridColumnSelectionBehavior.RemoveClickEvent(target);
        GridColumnSelectionBehavior.RemoveDblClickEvent(target);
      }
    }

    private static void LoadedEvent(object sender, RoutedEventArgs e)
    {
      DataGrid dataGrid = sender as DataGrid;
      if (dataGrid == null || !GridColumnSelectionBehavior.GetGridSelectColOnClick((DependencyObject) dataGrid))
        return;
      GridColumnSelectionBehavior.AddClickEvent((DependencyObject) dataGrid);
      GridColumnSelectionBehavior.AddDblClickEvent((DependencyObject) dataGrid);
    }

    private static void AddClickEvent(DependencyObject dep)
    {
      if (dep == null)
        return;
      int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(dep, childIndex);
        DataGridColumnHeader gridColumnHeader = child as DataGridColumnHeader;
        if (gridColumnHeader != null)
          GridColumnSelectionBehavior.AddRemoveEventForButton(true, true, (DependencyObject) gridColumnHeader);
        else
          GridColumnSelectionBehavior.AddClickEvent(child);
      }
    }

    private static void RemoveClickEvent(DependencyObject dep)
    {
      if (dep == null)
        return;
      int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(dep, childIndex);
        DataGridColumnHeader gridColumnHeader = child as DataGridColumnHeader;
        if (gridColumnHeader != null)
          GridColumnSelectionBehavior.AddRemoveEventForButton(false, true, (DependencyObject) gridColumnHeader);
        else
          GridColumnSelectionBehavior.RemoveClickEvent(child);
      }
    }

    private static void AddRemoveEventForButton(bool bAdd, bool bClick, DependencyObject dep)
    {
      if (dep == null)
        return;
      int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(dep, childIndex);
        Button button = child as Button;
        if (button != null)
        {
          if (bClick)
          {
            if (bAdd)
              button.Click += new RoutedEventHandler(GridColumnSelectionBehavior.SelectColumn);
            else
              button.Click -= new RoutedEventHandler(GridColumnSelectionBehavior.SelectColumn);
          }
          else if (bAdd)
            button.MouseDoubleClick += new MouseButtonEventHandler(GridColumnSelectionBehavior.OnDoubleClick);
          else
            button.MouseDoubleClick -= new MouseButtonEventHandler(GridColumnSelectionBehavior.OnDoubleClick);
        }
        GridColumnSelectionBehavior.AddRemoveEventForButton(bAdd, bClick, child);
      }
    }

    private static void AddDblClickEvent(DependencyObject dep)
    {
      if (dep == null)
        return;
      int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(dep, childIndex);
        DataGridColumnHeader gridColumnHeader = child as DataGridColumnHeader;
        if (gridColumnHeader != null)
          GridColumnSelectionBehavior.AddRemoveEventForButton(true, false, (DependencyObject) gridColumnHeader);
        else
          GridColumnSelectionBehavior.AddDblClickEvent(child);
      }
    }

    private static void RemoveDblClickEvent(DependencyObject dep)
    {
      if (dep == null)
        return;
      int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(dep, childIndex);
        DataGridColumnHeader gridColumnHeader = child as DataGridColumnHeader;
        if (gridColumnHeader != null)
          GridColumnSelectionBehavior.AddRemoveEventForButton(false, false, (DependencyObject) gridColumnHeader);
        else
          GridColumnSelectionBehavior.RemoveDblClickEvent(child);
      }
    }

    public static bool GetIsColumnOfCellSelected(DependencyObject obj)
    {
      return (bool) obj.GetValue(GridColumnSelectionBehavior.IsColumnOfCellSelectedProperty);
    }

    public static void SetIsColumnOfCellSelected(DependencyObject obj, bool value)
    {
      obj.SetValue(GridColumnSelectionBehavior.IsColumnOfCellSelectedProperty, (object) value);
    }

    private static void OnIsColumnOfCellSelectedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      if (!(target is DataGridCell))
        throw new InvalidOperationException("This behavior can be attached to DataGrid item only.");
    }

    public static bool GetIsColumnSelected(DependencyObject obj)
    {
      return (bool) obj.GetValue(GridColumnSelectionBehavior.IsColumnSelectedProperty);
    }

    public static void SetIsColumnSelected(DependencyObject obj, bool value)
    {
      obj.SetValue(GridColumnSelectionBehavior.IsColumnSelectedProperty, (object) value);
    }

    private static void OnIsColumnSelectedProperty(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      DataGridColumnHeader except = target as DataGridColumnHeader;
      UIElement element = target as UIElement;
      if (element == null)
        return;
      DataGrid grid = GridColumnSelectionBehavior.FindGrid(element);
      if (!(bool) e.NewValue || (bool) e.OldValue)
        return;
      GridColumnSelectionBehavior.UnsetSelected((DependencyObject) grid, except);
    }

    private static void UnsetSelected(DependencyObject dep, DataGridColumnHeader except)
    {
      if (dep == null)
        return;
      int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(dep, childIndex);
        DataGridColumnHeader gridColumnHeader = child as DataGridColumnHeader;
        if (gridColumnHeader != null)
        {
          if (gridColumnHeader != except)
            GridColumnSelectionBehavior.SetIsColumnSelected((DependencyObject) gridColumnHeader, false);
        }
        else
          GridColumnSelectionBehavior.UnsetSelected(child, except);
      }
    }

    private static void SelectColumn(object sender, RoutedEventArgs e)
    {
      DataGridColumnHeader visualParent = GridColumnSelectionBehavior.FindVisualParent((UIElement) sender);
      DataGrid grid = GridColumnSelectionBehavior.FindGrid((UIElement) sender);
      grid.SelectedCellsChanged += new SelectedCellsChangedEventHandler(GridColumnSelectionBehavior.grid_SelectedCellsChanged);
      if (visualParent == null || grid == null)
        return;
      grid.SelectedCells.Clear();
      DataGridColumn column = visualParent.Column;
      for (int index = 0; index < grid.Items.Count; ++index)
        grid.SelectedCells.Add(new DataGridCellInfo(grid.Items[index], column));
      GridColumnSelectionBehavior.SetIsColumnSelected((DependencyObject) visualParent, true);
    }

    private static void grid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      if (e.RemovedCells.Count <= 0)
        return;
      GridColumnSelectionBehavior.UnsetSelected((DependencyObject) (sender as DataGrid), (DataGridColumnHeader) null);
    }

    private static void OnDoubleClick(object sender, MouseButtonEventArgs e)
    {
      DataGridColumnHeader visualParent = GridColumnSelectionBehavior.FindVisualParent((UIElement) sender);
      DataGrid grid = GridColumnSelectionBehavior.FindGrid((UIElement) sender);
      if (visualParent == null || !visualParent.CanUserSort || grid == null)
        return;
      ListSortDirection? sortDirection = visualParent.Column.SortDirection;
      ListSortDirection listSortDirection = ListSortDirection.Ascending;
      ListSortDirection direction = (sortDirection.GetValueOrDefault() == listSortDirection ? (!sortDirection.HasValue ? 1 : 0) : 1) != 0 ? ListSortDirection.Ascending : ListSortDirection.Descending;
      foreach (DataGridColumn column in (Collection<DataGridColumn>) grid.Columns)
        column.SortDirection = new ListSortDirection?();
      visualParent.Column.SortDirection = new ListSortDirection?(direction);
      ICollectionView defaultView = CollectionViewSource.GetDefaultView((object) grid.ItemsSource);
      defaultView.SortDescriptions.Clear();
      defaultView.SortDescriptions.Add(new SortDescription(visualParent.Column.SortMemberPath, direction));
      defaultView.Refresh();
    }

    private static DataGridColumnHeader FindVisualParent(UIElement element)
    {
      for (UIElement uiElement = element; uiElement != null; uiElement = VisualTreeHelper.GetParent((DependencyObject) uiElement) as UIElement)
      {
        DataGridColumnHeader gridColumnHeader = uiElement as DataGridColumnHeader;
        if (gridColumnHeader != null)
          return gridColumnHeader;
      }
      return (DataGridColumnHeader) null;
    }

    private static DataGrid FindGrid(UIElement element)
    {
      for (UIElement uiElement = element; uiElement != null; uiElement = VisualTreeHelper.GetParent((DependencyObject) uiElement) as UIElement)
      {
        DataGrid dataGrid = uiElement as DataGrid;
        if (dataGrid != null)
          return dataGrid;
      }
      return (DataGrid) null;
    }

    public static DataGridColumn TryFindParentDataGridColumn(DependencyObject child)
    {
      DependencyObject parentObject = GridColumnSelectionBehavior.GetParentObject(child);
      if (parentObject == null)
        return (DataGridColumn) null;
      return parentObject as DataGridColumn ?? GridColumnSelectionBehavior.TryFindParentDataGridColumn(parentObject);
    }

    public static DataGridRow TryFindParentDataGridRow(DependencyObject child)
    {
      DependencyObject parentObject = GridColumnSelectionBehavior.GetParentObject(child);
      if (parentObject == null)
        return (DataGridRow) null;
      return parentObject as DataGridRow ?? GridColumnSelectionBehavior.TryFindParentDataGridRow(parentObject);
    }

    public static DependencyObject GetParentObject(DependencyObject child)
    {
      if (child == null)
        return (DependencyObject) null;
      ContentElement reference = child as ContentElement;
      if (reference != null)
      {
        DependencyObject parent = ContentOperations.GetParent(reference);
        if (parent != null)
          return parent;
        FrameworkContentElement frameworkContentElement = reference as FrameworkContentElement;
        if (frameworkContentElement == null)
          return (DependencyObject) null;
        return frameworkContentElement.Parent;
      }
      FrameworkElement frameworkElement = child as FrameworkElement;
      if (frameworkElement != null)
      {
        DependencyObject parent = frameworkElement.Parent;
        if (parent != null)
          return parent;
      }
      return VisualTreeHelper.GetParent(child);
    }

    public static bool GetRowHeaderContentTop(DependencyObject obj)
    {
      return (bool) obj.GetValue(GridColumnSelectionBehavior.IsColumnSelectedProperty);
    }

    public static void SetRowHeaderContentTop(DependencyObject obj, bool value)
    {
      obj.SetValue(GridColumnSelectionBehavior.IsColumnSelectedProperty, (object) value);
    }

    private static void OnRowHeaderContentTop(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      DataGridRowHeader dataGridRowHeader = target as DataGridRowHeader;
      if (dataGridRowHeader == null)
        return;
      if ((bool) e.NewValue && !(bool) e.OldValue)
      {
        dataGridRowHeader.Loaded += new RoutedEventHandler(GridColumnSelectionBehavior.LoadedEventRowHeader);
        GridColumnSelectionBehavior.ChangeRowHeaderContentTop(target);
      }
      else
      {
        if ((bool) e.NewValue || !(bool) e.OldValue)
          return;
        dataGridRowHeader.Loaded -= new RoutedEventHandler(GridColumnSelectionBehavior.LoadedEventRowHeader);
      }
    }

    private static void LoadedEventRowHeader(object sender, RoutedEventArgs e)
    {
      DataGridRowHeader dataGridRowHeader = sender as DataGridRowHeader;
      if (dataGridRowHeader == null || !GridColumnSelectionBehavior.GetRowHeaderContentTop((DependencyObject) dataGridRowHeader))
        return;
      GridColumnSelectionBehavior.ChangeRowHeaderContentTop((DependencyObject) dataGridRowHeader);
    }

    private static void ChangeRowHeaderContentTop(DependencyObject dep)
    {
      if (dep == null)
        return;
      int childrenCount = VisualTreeHelper.GetChildrenCount(dep);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(dep, childIndex);
        ContentPresenter contentPresenter = child as ContentPresenter;
        if (contentPresenter != null)
          contentPresenter.VerticalAlignment = VerticalAlignment.Top;
        else
          GridColumnSelectionBehavior.ChangeRowHeaderContentTop(child);
      }
    }
  }
}
