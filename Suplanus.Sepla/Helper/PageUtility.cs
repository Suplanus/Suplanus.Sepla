using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
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
    }
}