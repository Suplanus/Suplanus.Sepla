using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;
using Suplanus.Sepla.Helper;

namespace Suplanus.Sepla.Gui
{
	public class Preview
	{
		public readonly DrawingService DrawingService;
		private readonly Project _project;
		private readonly PreviewControl _previewControl = new PreviewControl();

		/// <summary>
		/// Init Preview object for WPF
		/// </summary>
		/// <param name="border"></param>
		/// <param name="projectFile"></param>
		public Preview(Border border, string projectFile)
		{
			if (!File.Exists(projectFile))
			{
				throw new FileNotFoundException(projectFile);
			}

			// Check if project is open
			var projectManager = new ProjectManager();
			projectManager.LockProjectByDefault = false;
			foreach (var openProject in projectManager.OpenProjects)
			{
				if (openProject.ProjectLinkFilePath.Equals(projectFile))
				{
					_project = openProject;
					break;
				}
			}
			if (_project == null)
			{
				_project = projectManager.OpenProject(projectFile, ProjectManager.OpenMode.Exclusive);
			}

			DrawingService = new DrawingService();
			DrawingService.DrawConnections = true;

			border.Child = _previewControl;
			_previewControl.Preview = this;
		}

		/// <summary>
		/// Display a file
		/// </summary>
		/// <param name="path">Full filename</param>
		/// <param name="previewType">Type of file</param>
		public void Display(string path, PreviewType previewType)
		{
		   if (path == null)
		   {
		      return;
		   }
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(path);
			}

			switch (previewType)
			{
				case PreviewType.WindowMacro:
					WindowMacro windowMacro = new WindowMacro();
					windowMacro.Open(path, _project);
					SetVariantCombinations(windowMacro);
					break;

				case PreviewType.SymbolMacro:
					SymbolMacro symbolMacro = new SymbolMacro();
					symbolMacro.Open(path, _project);
					SetVariantCombinations(symbolMacro);
					break;

				case PreviewType.PageMacro:
					PageMacro pageMacro = new PageMacro();
					pageMacro.Open(path, _project);
					SetVariantCombinations(pageMacro);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(previewType), previewType, null);
			}

		}

		private void SetVariantCombinations(WindowMacro windowMacro)
		{
			_previewControl.VariantsCombinations = new ObservableCollection<VariantCombination>();
			_previewControl.previewType = PreviewType.WindowMacro;

			foreach (var representationType in windowMacro.RepresentationTypes)
			{
				VariantCombination variantCombination = new VariantCombination(PreviewType.WindowMacro);
				variantCombination.PreviewObject = windowMacro;
				variantCombination.RepresentationType = representationType;
				foreach (int variantIndex in windowMacro.GetVariants(representationType))
				{
					windowMacro.ChangeCurrentVariant(representationType, variantIndex);

					Variant variant = new Variant(variantCombination);
					variant.Index = variantIndex;
					variant.Description = windowMacro.Description
						.GetStringToDisplay(MultiLanguage.GuiLanguage);
					variant.Description = Regex.Replace(variant.Description, @"\r\n?|\n", " ");

					variantCombination.Variants.Add(variant);
				}

				_previewControl.VariantsCombinations.Add(variantCombination);
			}

			_previewControl.SelectedVariantCombination = _previewControl.VariantsCombinations.FirstOrDefault();
			_previewControl.CheckControls();
		}

		private void SetVariantCombinations(SymbolMacro symbolMacro)
		{
			_previewControl.VariantsCombinations = new ObservableCollection<VariantCombination>();
			_previewControl.previewType = PreviewType.WindowMacro;

			foreach (var representationType in symbolMacro.RepresentationTypes)
			{
				VariantCombination variantCombination = new VariantCombination(PreviewType.SymbolMacro);
				variantCombination.PreviewObject = symbolMacro;
				variantCombination.RepresentationType = representationType;
				foreach (int variantIndex in symbolMacro.GetVariants(representationType))
				{
					symbolMacro.ChangeCurrentVariant(representationType, variantIndex);

					Variant variant = new Variant(variantCombination);
					variant.Index = variantIndex;
					variant.Description = symbolMacro.Description
						.GetStringToDisplay(MultiLanguage.GuiLanguage);
					variant.Description = Regex.Replace(variant.Description, @"\r\n?|\n", " ");

					variantCombination.Variants.Add(variant);
				}

				_previewControl.VariantsCombinations.Add(variantCombination);
			}

			_previewControl.SelectedVariantCombination = _previewControl.VariantsCombinations.FirstOrDefault();
			_previewControl.CheckControls();
		}

		private void SetVariantCombinations(PageMacro pageMacro)
		{
			_previewControl.VariantsCombinations = new ObservableCollection<VariantCombination>();
			_previewControl.previewType = PreviewType.PageMacro;

			VariantCombination variantCombination = new VariantCombination(PreviewType.PageMacro);
			variantCombination.PreviewObject = pageMacro;
			for (int index = 0; index < pageMacro.Pages.Length; index++)
			{
				var page = pageMacro.Pages[index];

				variantCombination.RepresentationType = WindowMacro.Enums.RepresentationType.Default;
				Variant variant = new Variant(variantCombination);
				if (!page.Properties.PAGE_NOMINATIOMN.IsEmpty)
				{
					variant.Description = page.Properties.PAGE_NOMINATIOMN.ToMultiLangString()
						.GetStringToDisplay(MultiLanguage.GuiLanguage);
					variant.Description = Regex.Replace(variant.Description, @"\r\n?|\n", " ");
				}
				variant.Index = index;
				variantCombination.Variants.Add(variant);
			}
			_previewControl.VariantsCombinations.Add(variantCombination);

			_previewControl.SelectedVariantCombination = _previewControl.VariantsCombinations.FirstOrDefault();
			_previewControl.CheckControls();
		}

		public class VariantCombination
		{
			public VariantCombination(PreviewType previewType)
			{
				Variants = new ObservableCollection<Variant>();
				PreviewType = previewType;
			}

			public override string ToString()
			{
				return RepresentationType.ToString();
			}

			public object PreviewObject { get; set; }
			public WindowMacro.Enums.RepresentationType RepresentationType { get; set; }
			public ObservableCollection<Variant> Variants { get; set; }
			public PreviewType PreviewType { get; set; }

		}

		public class Variant
		{
			public VariantCombination VariantCombination { get; set; }

			public Variant(VariantCombination variantCombination)
			{
				VariantCombination = variantCombination;
			}

			public override string ToString()
			{
            // Pagemacro
            if (VariantCombination.PreviewType == PreviewType.PageMacro)
            {
               if (string.IsNullOrEmpty(Description))
               {
                  return "Seite " + Index;
               }
               else
               {
                  return Description;
               }
            }

            // Generate Variantstring from index
            StringBuilder res = new StringBuilder(Index.ToString());
				string variantString = string.Empty;
				for (int j = 0; j < res.Length; j++)
				{
					variantString += res[j] += (char)(17); // '0' is 48, 'A' is 65
				}

            // Not Pagemacro
				if (VariantCombination.PreviewType != PreviewType.PageMacro)
				{
					if (!string.IsNullOrEmpty(Description))
					{
						return "Variante " + variantString + ": " + Description;
					}
				}

				return "Variante " + variantString;
			}

			public int Index { get; set; }
			public string Description { get; set; }
		}


		/// <summary>
		/// Draw EPLAN files
		/// </summary>
		public void DrawEplan()
		{
			int width = Convert.ToInt16(_previewControl.PreviewBorder.ActualWidth);
			int height = Convert.ToInt16(_previewControl.PreviewBorder.ActualHeight);

			if (width > 0 && height > 0)
			{
				Bitmap bitmap = new Bitmap(width, height);
				Graphics graphics = Graphics.FromImage(bitmap);
				Rectangle rectangle = new Rectangle(0, 0, width, height);
				PaintEventArgs paintEventArgs = new PaintEventArgs(graphics, rectangle);

				DrawingService.DrawDisplayList(paintEventArgs);

				IntPtr hBitmap = bitmap.GetHbitmap();
				BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
					IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

				_previewControl.PreviewBorder.Background = new ImageBrush(bitmapSource);

				bitmap.Dispose();
				graphics.Dispose();
				paintEventArgs.Dispose();
				DeleteObject(hBitmap);
			}
			else
			{
				_previewControl.PreviewBorder.Background = null;
			}

		}

		/// <summary>
		/// Memory Leak: http://stackoverflow.com/questions/1546091/wpf-createbitmapsourcefromhbitmap-memory-leak
		/// </summary>
		/// <param name="hObject"></param>
		/// <returns></returns>
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);
	}


	/// <summary>
	/// Filetype to preview
	/// </summary>
	public enum PreviewType
	{
		Unknow,
		WindowMacro,
		SymbolMacro,
		PageMacro
	}
}
