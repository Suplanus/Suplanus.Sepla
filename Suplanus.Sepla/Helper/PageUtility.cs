using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
    public class PageUtility
    {
        public enum PageCounterAlphabetical
        {
            A = 0,
            B = 1,
            C = 2,
            D = 3,
            E = 4,
            F = 5,
            G = 6,
            H = 7,
            I = 8,
            J = 9,
            K = 10,
            L = 11,
            M = 12,
            N = 13,
            P = 14,
        }

        public static void OpenPageAndSyncInNavigator(Page page)
        {
            new Edit().OpenPageWithName(page.Project.ProjectLinkFilePath, page.IdentifyingName); // Open in GED
            new CommandLineInterpreter().Execute("XGedSelectPageAction"); // Select page
            new CommandLineInterpreter().Execute("XEsSyncPDDsAction"); // Sync selection
            new CommandLineInterpreter().Execute("XGedEscapeAction"); // Escape (page selection)
        }

        public static WindowMacro.Enums.RepresentationType GetMacroRepresentationType(Page page)
        {
            switch (page.PageType)
            {
                case DocumentTypeManager.DocumentType.CircuitFluid: return WindowMacro.Enums.RepresentationType.Fluid_MultiLine;
                case DocumentTypeManager.DocumentType.Circuit: return WindowMacro.Enums.RepresentationType.MultiLine;
                case DocumentTypeManager.DocumentType.CircuitSingleLine: return WindowMacro.Enums.RepresentationType.SingleLine;
                case DocumentTypeManager.DocumentType.Overview: return WindowMacro.Enums.RepresentationType.Overview;
                case DocumentTypeManager.DocumentType.Graphics: return WindowMacro.Enums.RepresentationType.Graphics;
                case DocumentTypeManager.DocumentType.ProcessAndInstrumentationDiagram: return WindowMacro.Enums.RepresentationType.PI_FlowChart;
                case DocumentTypeManager.DocumentType.PanelLayout: return WindowMacro.Enums.RepresentationType.ArticlePlacement;
                case DocumentTypeManager.DocumentType.Topology: return WindowMacro.Enums.RepresentationType.Cabling;
                case DocumentTypeManager.DocumentType.Planning: return WindowMacro.Enums.RepresentationType.Planning;
                default: return WindowMacro.Enums.RepresentationType.Default;
            }
        }
    }
}