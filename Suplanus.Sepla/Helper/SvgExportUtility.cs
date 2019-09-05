using System;
using System.IO;
using System.Linq;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
  // SVGExportAction
  // <summary> 
  // Export to SVG, one file per page with hyperlinks 
  // </summary> 
  // <remarks>internal</remarks> 
  // <remarks> 
  // </remarks> 
  // <param name = "DatabaseId">Database Id for the project to be exported</param> 
  // <param name = "ExportPath">Full path name of the target folder</param> 
  // <param name = "PageObjId">Complete object id of the page to be exported</param> 
  // <param name = "FileName">Name of the export file when exporting a single page</param> 
  // <param name = "MacroFile">Name of the macro file to be exported. Must contain Variant1..n, RepType1..n, and FileName1..n.</param> 
  // <param name = "Variant1">Variant in the macro to be exported</param> 
  // <param name = "RepType1">Representation type in the macro to be exported</param> 
  // <param name = "FileName1">Full target file name for the variant and representation type being exported</param> 
  // <param name = "DrawFrame">Draw plot frame, default = true</param> 
  // <param name = "WriteGroupIds">Write object ids, default = false</param> 

  public class SvgExportUtility
  {
    public static void ExportProject(Project project, string exportPath, bool isFrameVisible = true)
    {
      if (Directory.Exists(exportPath))
      {
        Directory.Delete(exportPath);
      }

      ActionCallingContext acc = new ActionCallingContext();
      acc.AddParameter("DatabaseId", project.DatabaseIdentifier.ToString());
      acc.AddParameter("ExportPath", exportPath);
      acc.AddParameter("DrawFrame", isFrameVisible.ToString());
      acc.AddParameter("WriteGroupIds", false.ToString());
      new CommandLineInterpreter().Execute("SVGExportAction", acc);
    }

    public static void ExportPage(Page page, string fullFilename, bool isFrameVisible = true)
    {
      if (File.Exists(fullFilename))
      {
        File.Delete(fullFilename);
      }

      ActionCallingContext acc = new ActionCallingContext();
      acc.AddParameter("ExportPath", Path.GetDirectoryName(fullFilename));
      acc.AddParameter("PageObjId", page.ToStringIdentifier());
      acc.AddParameter("Filename", Path.GetFileNameWithoutExtension(fullFilename)); // only name needed
      acc.AddParameter("DrawFrame", isFrameVisible.ToString());
      acc.AddParameter("WriteGroupIds", false.ToString());
      new CommandLineInterpreter().Execute("SVGExportAction", acc);
    }

    public static void ExportPageMacro(Project project, string pageMacroFile, string fullFilename, bool isFrameVisible = true)
    {
      using (PageMacro pageMacro = new PageMacro())
      {
        // Have to insert pages into project because its not working with pageMacro.Pages.First()
        pageMacro.Open(pageMacroFile, project);

        // Set temp structure
        for (var index = 0; index < pageMacro.Pages.Length; index++)
        {
          var pageMacroPage = pageMacro.Pages[index];
          pageMacroPage.NameParts[Eplan.EplApi.DataModel.Properties.Page.PAGE_COUNTER] = "SvgExportUtility" + index;
        }

        var storableObjects = new Insert().PageMacro(pageMacro, project, null, PageMacro.Enums.NumerationMode.None);
        var newPages = storableObjects.OfType<Page>().ToList();

        for (var index = 0; index < newPages.Count; index++)
        {
          var newPage = newPages[index];
          var path = Path.GetDirectoryName(fullFilename);
          var filename = Path.GetFileNameWithoutExtension(fullFilename) + "_" + (index + 1) + ".svg";

          // ReSharper disable once AssignNullToNotNullAttribute
          filename = Path.Combine(path, filename);

          if (File.Exists(fullFilename))
          {
            File.Delete(fullFilename);
          }

          ExportPage(newPage, filename, isFrameVisible);

          // Remove pages after export
          newPage.Remove();
        }
      }
    }

    public static void ExportMacro(string macroFile, string fullFilename, int variant, WindowMacro.Enums.RepresentationType representationType)
    {
      if (File.Exists(fullFilename))
      {
        File.Delete(fullFilename);
      }

      ActionCallingContext acc = new ActionCallingContext();
      acc.AddParameter("MacroFile", macroFile);
      acc.AddParameter("Filename1", fullFilename); // Full path needed
      acc.AddParameter("Variant1", variant.ToString());
      acc.AddParameter("RepType1", ((int)representationType).ToString());
      acc.AddParameter("WriteGroupIds", false.ToString());
      new CommandLineInterpreter().Execute("SVGExportAction", acc);
    }
  }
}
