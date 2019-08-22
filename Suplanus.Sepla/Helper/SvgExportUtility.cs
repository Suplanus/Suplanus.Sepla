using System.IO;
using System.Linq;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;

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
    public static void ExportPage(Page page, string fullFilename, bool isFrameVisible = true)
    {
      if (File.Exists(fullFilename))
      {
        File.Delete(fullFilename);
      }

      ActionCallingContext acc = new ActionCallingContext();
      acc.AddParameter("DatabaseId", page.Project.DatabaseIdentifier.ToString());
      acc.AddParameter("ExportPath", Path.GetDirectoryName(fullFilename));
      acc.AddParameter("PageObjId", page.ToStringIdentifier());
      acc.AddParameter("Filename", Path.GetFileNameWithoutExtension(fullFilename)); // only name needed
      acc.AddParameter("DrawFrame", isFrameVisible.ToString());
      acc.AddParameter("WriteGroupIds", false.ToString());
      new CommandLineInterpreter().Execute("SVGExportAction", acc);
    }

    // todo: Not working
    public static void ExportPageMacro(Project project, string pageMacroFile, string fullFilename, bool isFrameVisible = true)
    {
      using (PageMacro pageMacro = new PageMacro())
      {
        pageMacro.Open(pageMacroFile, project);
        var page = pageMacro.Pages.First();
        ExportPage(page, fullFilename, isFrameVisible);
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
