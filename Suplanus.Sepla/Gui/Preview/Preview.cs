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
   /// <summary>
   /// Preview Helper class to display in user control PreviewControl
   /// </summary>
   public class Preview : IDisposable
   {
      /// <summary>
      /// EPLAN DrawingService
      /// </summary>
      public readonly DrawingService DrawingService;

      /// <summary>
      /// EPLAN WindowMacro which is displayed
      /// </summary>
      public WindowMacro WindowMacro;

      /// <summary>
      /// EPLAN SymbolMacro which is displayed
      /// </summary>
      public SymbolMacro SymbolMacro;

      /// <summary>
      /// EPLAN PageMacro which is displayed
      /// </summary>
      public PageMacro PageMacro;

      /// <summary>
      /// EPLAN project object
      /// </summary>
      public Project EplanProject;

      private readonly PreviewControl _previewControl = new PreviewControl();

      /// <summary>
      /// Init Preview object for WPF
      /// </summary>
      /// <param name="border">Border which should inhert the UserControl</param>
      /// <param name="projectFile">EPLAN project file to preview (*.elk)</param>
      public Preview(Border border, string projectFile)
      {
         // Clean
         WindowMacro = null;
         SymbolMacro = null;
         PageMacro = null;

         if (!File.Exists(projectFile))
         {
            throw new FileNotFoundException(projectFile);
         }

         // Check if project is open
         var projectManager = new ProjectManager();
         projectManager.LockProjectByDefault = false;
         //EplanProject = ProjectUtility.OpenProject(projectFile);
         foreach (var openProject in projectManager.OpenProjects)
         {
            if (openProject.ProjectLinkFilePath.Equals(projectFile))
            {
               EplanProject = openProject;
               break;
            }
         }
         if (EplanProject == null)
         {
            EplanProject = projectManager.OpenProject(projectFile, ProjectManager.OpenMode.Exclusive);
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
               WindowMacro = new WindowMacro();
               WindowMacro.Open(path, EplanProject);
               SetVariantCombinations(WindowMacro);
               break;

            case PreviewType.SymbolMacro:
               SymbolMacro = new SymbolMacro();
               SymbolMacro.Open(path, EplanProject);
               SetVariantCombinations(SymbolMacro);
               break;

            case PreviewType.PageMacro:
               PageMacro = new PageMacro();
               PageMacro.Open(path, EplanProject);
               SetVariantCombinations(PageMacro);
               break;

            default:
               throw new ArgumentOutOfRangeException(nameof(previewType), previewType, null);
         }

      }

      /// <summary>
      /// Displays the given WindowMacro
      /// </summary>
      /// <param name="windowMacro">EPLAN WindowMacro</param>
      public void Display(WindowMacro windowMacro)
      {
         WindowMacro = windowMacro;
         SetVariantCombinations(WindowMacro);
      }

      /// <summary>
      /// Displays the given SymbolMacro
      /// </summary>
      /// <param name="symbolMacro">EPLAN SymbolMacro</param>
      public void Display(SymbolMacro symbolMacro)
      {
         SymbolMacro = symbolMacro;
         SetVariantCombinations(SymbolMacro);
      }

      /// <summary>
      /// Displays the given PageMacro
      /// </summary>
      /// <param name="pageMacro">EPLAN PageMacro</param>
      public void Display(PageMacro pageMacro)
      {
         PageMacro = pageMacro;
         SetVariantCombinations(PageMacro);
      }

      private void SetVariantCombinations(WindowMacro windowMacro)
      {
         _previewControl.VariantsCombinations = new ObservableCollection<VariantCombination>();
         _previewControl.PreviewType = PreviewType.WindowMacro;

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
         _previewControl.PreviewType = PreviewType.WindowMacro;

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
         _previewControl.PreviewType = PreviewType.PageMacro;

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

      /// <summary>
      /// Variant and RepresentationType combined to display
      /// </summary>
      public class VariantCombination
      {
         /// <summary>
         /// Creates a VariantCombination with the given PreviewType
         /// </summary>
         /// <param name="previewType">Sets how display the object</param>
         public VariantCombination(PreviewType previewType)
         {
            Variants = new ObservableCollection<Variant>();
            PreviewType = previewType;
         }

         /// <summary>
         /// Returns the PresentationType of the VarianteCombination
         /// </summary>
         /// <returns>RepresentationType</returns>
         public override string ToString()
         {
            return RepresentationType.ToString();
         }

         /// <summary>
         /// Object to preview
         /// </summary>
         public object PreviewObject { get; set; }

         /// <summary>
         /// RepresentationType of EPLAN objects
         /// </summary>
         public WindowMacro.Enums.RepresentationType RepresentationType { get; set; }

         /// <summary>
         /// Variants of EPLAN objects
         /// </summary>
         public ObservableCollection<Variant> Variants { get; set; }

         /// <summary>
         /// PreviewType of PreviewObject
         /// </summary>
         public PreviewType PreviewType { get; set; }

      }

      /// <summary>
      /// Variant for Preview
      /// </summary>
      public class Variant
      {
         /// <summary>
         /// Parent object
         /// </summary>
         public VariantCombination VariantCombination { get; set; }

         /// <summary>
         /// Creates a Variant with given variantCombination
         /// </summary>
         /// <param name="variantCombination">Parent object</param>
         public Variant(VariantCombination variantCombination)
         {
            VariantCombination = variantCombination;
         }

         /// <summary>
         /// Returns the display string of the Preview
         /// </summary>
         /// <returns>Display string</returns>
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

         /// <summary>
         /// Index for variant
         /// </summary>
         public int Index { get; set; }

         /// <summary>
         /// Description of the Variant
         /// </summary>
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
      /// <returns>State</returns>
      [DllImport("gdi32.dll")]
      public static extern bool DeleteObject(IntPtr hObject);

      /// <summary>
      /// Release all Objects
      /// </summary>
      public void Dispose()
      {
         this.DrawingService.Dispose();
         if (this.PageMacro != null) this.PageMacro.Dispose();
         if (this.WindowMacro != null) this.WindowMacro.Dispose();
         if (this.SymbolMacro != null) this.SymbolMacro.Dispose();
         this.EplanProject.Close();
      }
   }


   /// <summary>
   /// Filetype to preview
   /// </summary>
   public enum PreviewType
   {
      /// <summary>
      /// Unknown file type
      /// </summary>
      Unknow,

      /// <summary>
      /// EPLAN WindowMacro
      /// </summary>
      WindowMacro,

      /// <summary>
      /// EPLAN SymbolMacro
      /// </summary>
      SymbolMacro,

      /// <summary>
      /// EPLAN PageMacro
      /// </summary>
      PageMacro
   }
}
