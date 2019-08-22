using System.Diagnostics;
using System.IO;
using System.Linq;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Suplanus.Sepla.Helper;

namespace Suplanus.Example.EplAddIn.SvgExport
{
  class Action : IEplAction
  {
    private const string OUTPUT_PATH = @"\\Mac\Home\Downloads\Test\";
    private const string MACRO_FILENAME_PAGE = @"\\Mac\Home\Documents\GitHub\Suplanus.Sepla\Suplanus.Example.EplAddIn.SvgExport\DemoData\PageMacro.emp";
    private const string MACRO_FILENAME_WINDOW = @"\\Mac\Home\Documents\GitHub\Suplanus.Sepla\Suplanus.Example.EplAddIn.SvgExport\DemoData\WindowMacro.ema";
    private const string MACRO_FILENAME_SYMBOL = @"\\Mac\Home\Documents\GitHub\Suplanus.Sepla\Suplanus.Example.EplAddIn.SvgExport\DemoData\SymbolMacro.ems";

    private Project _project;

    public bool OnRegister(ref string name, ref int ordinal)
    {
      name = "SvgExportExample";
      ordinal = 20;
      return true;
    }

    public bool Execute(ActionCallingContext oActionCallingContext)
    {
      _project = ProjectUtility.GetCurrentProject();

      // Clean output path
      if (Directory.Exists(OUTPUT_PATH))
      {
        Directory.Delete(OUTPUT_PATH, true);
      }

      // Export
      ExportMacros();
      ExportPage();
      ExportPageMacro(); // todo: Not working

      // Show files
      Process.Start(OUTPUT_PATH);

      return true;
    }

    private void ExportMacros()
    {
      string filenameWindowMacro = Path.Combine(OUTPUT_PATH, "WindowMacro.svg");
      SvgExportUtility.ExportMacro(_project, MACRO_FILENAME_WINDOW, filenameWindowMacro, 0,
                                   WindowMacro.Enums.RepresentationType.MultiLine);
      string filenameSymbolMacro = Path.Combine(OUTPUT_PATH, "SymbolMacro.svg");
      SvgExportUtility.ExportMacro(_project, MACRO_FILENAME_SYMBOL, filenameSymbolMacro, 0,
                                   WindowMacro.Enums.RepresentationType.MultiLine);
    }

    private void ExportPageMacro()
    {
      string filename = Path.Combine(OUTPUT_PATH, "PageMacro.svg");
      SvgExportUtility.ExportPageMacro(_project, MACRO_FILENAME_PAGE, filename);
    }

    private void ExportPage()
    {
      var page = _project.Pages.First();
      string filename = Path.Combine(OUTPUT_PATH, "Page.svg");
      SvgExportUtility.ExportPage(page, filename);
    }

    public void GetActionProperties(ref ActionProperties actionProperties) { }
  }
}