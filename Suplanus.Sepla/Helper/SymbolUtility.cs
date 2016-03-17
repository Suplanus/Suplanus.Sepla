using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;

namespace Suplanus.Sepla.Helper
{
	public class SymbolUtility
	{
		public bool Insert(Page page, string symbolLibraryName, string symbolName, int symbolvariant)
		{
			page.LockObject();

			SymbolLibrary symbolLibrary = new SymbolLibrary(page.Project, symbolLibraryName);
			Symbol symbol = new Symbol(symbolLibrary, symbolName);

			Function function = new Function(page.Project, symbol.Variants[symbolvariant]);
			function.Location = new PointD(200, 150);
			page.InsertSubPlacement(function);
			function.Dispose();

			return true;
		}
	}
}
