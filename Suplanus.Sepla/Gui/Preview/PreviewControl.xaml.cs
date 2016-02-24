using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Eplan.EplApi.DataModel.MasterData;

namespace Suplanus.Sepla.Gui
{
	public partial class PreviewControl : System.Windows.Controls.UserControl
	{
		public PreviewType previewType { get; set; }


		public ObservableCollection<Preview.VariantCombination> VariantsCombinations { get; set; }

		private Preview.VariantCombination selectedVariantCombination;
		public Preview.VariantCombination SelectedVariantCombination
		{
			get
			{
				if (selectedVariantCombination == null)
				{
					return VariantsCombinations.FirstOrDefault();
				}
				return selectedVariantCombination;
			}
			set { selectedVariantCombination = value; }
		}

		private Preview.Variant SelectedVariant { get; set; }

		public Preview Preview { get; set; }

		public PreviewControl()
		{
			InitializeComponent();
		}


		#region Navigation

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

			if (previewType == PreviewType.PageMacro)
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
				switch (previewType)
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
