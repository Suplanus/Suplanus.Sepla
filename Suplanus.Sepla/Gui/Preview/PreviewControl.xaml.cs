using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Eplan.EplApi.DataModel.MasterData;

namespace Suplanus.Sepla.Gui
{
   /// <summary>
   /// UserControl to Display objects from EPLAN and so on
   /// </summary>
	public partial class PreviewControl
	{
      /// <summary>
      /// Type of file
      /// </summary>
		public PreviewType PreviewType { get; set; }

      /// <summary>
      /// VariantCominations which inherts the preview object
      /// </summary>
		public ObservableCollection<Preview.VariantCombination> VariantsCombinations { get; set; }

		private Preview.VariantCombination _selectedVariantCombination;
      /// <summary>
      /// Selected VariantCombination
      /// </summary>
		public Preview.VariantCombination SelectedVariantCombination
		{
			get
			{
				if (_selectedVariantCombination == null)
				{
					return VariantsCombinations.FirstOrDefault();
				}
				return _selectedVariantCombination;
			}
			set { _selectedVariantCombination = value; }
		}

		private Preview.Variant SelectedVariant { get; set; }

      /// <summary>
      /// Preview Class
      /// </summary>
		public Preview Preview { get; set; }

      /// <summary>
      /// Default Constructor
      /// </summary>
		public PreviewControl()
		{
			InitializeComponent();
		}


		#region Navigation
      /// <summary>
      /// Checks all controls and set the IsEnabled or Visibility property
      /// </summary>
		public void CheckControls()
		{
			// Enable
			if (VariantsCombinations.Any())
			{
				BtnPreviewBack.IsEnabled = true;
				BtnPreviewNext.IsEnabled = true;
				CbbRepresentationType.IsEnabled = true;
				CbbVariant.IsEnabled = true;
			}
			else
			{
				BtnPreviewBack.IsEnabled = false;
				BtnPreviewNext.IsEnabled = false;
				CbbRepresentationType.IsEnabled = false;
				CbbVariant.IsEnabled = false;
			}

			if (PreviewType == PreviewType.PageMacro)
			{
				CbbRepresentationType.Visibility = Visibility.Collapsed;
			}
			else
			{
				CbbRepresentationType.Visibility = Visibility.Visible;
			}

			// RepresentationType
			CbbRepresentationType.ItemsSource = VariantsCombinations;
			CbbRepresentationType.SelectedItem = VariantsCombinations.FirstOrDefault();

			CbbVariant.ItemsSource = SelectedVariantCombination.Variants;
			CbbVariant.SelectedItem = SelectedVariantCombination.Variants.FirstOrDefault();
		}

		private void BtnPreviewBack_OnClick(object sender, RoutedEventArgs e)
		{
			SelectVariant(-1);
		}

		private void BtnPreviewNext_OnClick(object sender, RoutedEventArgs e)
		{
			SelectVariant(1);
		}

		private void SelectVariant(int offset)
		{
			int nextVariant = SelectedVariant.Index + offset;
			var nextItem = SelectedVariantCombination.Variants.FirstOrDefault(obj => obj.Index.Equals(nextVariant));
			if (nextItem != null)
			{
				CbbVariant.SelectedItem = nextItem;
			}
			else
			{
				switch (offset)
				{
					case 1:
						CbbVariant.SelectedItem = SelectedVariantCombination.Variants.FirstOrDefault();
						break;

					case -1:
						CbbVariant.SelectedItem = SelectedVariantCombination.Variants.LastOrDefault();
						break;
				}
			}
		}
		#endregion

		#region Binding
		private void CbbVariant_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedVariant = CbbVariant.SelectedItem as Preview.Variant;
			Draw();
		}

		private void CbbRepresentationType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedVariantCombination = CbbRepresentationType.SelectedItem as Preview.VariantCombination;
			if (SelectedVariantCombination != null)
			{
				CbbVariant.ItemsSource = SelectedVariantCombination.Variants;
				CbbVariant.SelectedItem = SelectedVariantCombination.Variants.FirstOrDefault();
			}
			Draw();
		}
		#endregion

		#region Draw
		private void Draw()
		{
			if (SelectedVariant != null)
			{
				switch (PreviewType)
				{
					case PreviewType.WindowMacro:
						WindowMacro windowMacro = (WindowMacro)SelectedVariantCombination.PreviewObject;
						windowMacro.ChangeCurrentVariant(SelectedVariantCombination.RepresentationType, SelectedVariant.Index);
						Preview.DrawingService.CreateDisplayList(windowMacro);
						Preview.DrawEplan();
						break;
					case PreviewType.SymbolMacro:
						SymbolMacro symbolMacro = (SymbolMacro)SelectedVariantCombination.PreviewObject;
						symbolMacro.ChangeCurrentVariant(SelectedVariantCombination.RepresentationType, SelectedVariant.Index);
						Preview.DrawingService.CreateDisplayList(symbolMacro);
						Preview.DrawEplan();
                  symbolMacro.Dispose();
						break;
					case PreviewType.PageMacro:
						PageMacro pageMacro = (PageMacro)SelectedVariantCombination.PreviewObject;
						Preview.DrawingService.CreateDisplayList(pageMacro.Pages[SelectedVariant.Index]);
						Preview.DrawEplan();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void PreviewControl_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			Draw();
		} 
		#endregion
	}
}
