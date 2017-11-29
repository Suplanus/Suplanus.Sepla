using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;

namespace Suplanus.Sepla.Helper
{
    /// <summary>
    /// Helper class for symbols
    /// </summary>
    public class SymbolUtility
    {
        /// <summary>
        /// Insert a symbol on a given page
        /// </summary>
        /// <param name="page">Page where the symbol be insert</param>
        /// <param name="symbolLibraryName">Symbol library name</param>
        /// <param name="symbolName">Symbol name</param>
        /// <param name="symbolvariant">Symbol variant</param>
        public void Insert(Page page, string symbolLibraryName, string symbolName, int symbolvariant)
        {
            page.SmartLock();

            SymbolLibrary symbolLibrary = new SymbolLibrary(page.Project, symbolLibraryName);
            Symbol symbol = new Symbol(symbolLibrary, symbolName);

            Function function = new Function();
            function.Create(page.Project, symbol.Variants[symbolvariant]);
            function.Location = new PointD(200, 150);
            page.InsertSubPlacement(function);
            function.Dispose();
        }

        public static Function GetFunction(Page page, string symbolLibraryName, string symbolName, int symbolvariant)
        {
            SymbolLibrary symbolLibrary = new SymbolLibrary(page.Project, symbolLibraryName);
            Symbol symbol = new Symbol(symbolLibrary, symbolName);
            Function function = new Function();
            function.Create(page.Project, symbol.Variants[symbolvariant]);
            return function;
        }
    }
}
