using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suplanus.Sepla.Objects
{
	public class MacroPlaceholder : IMacroPlaceholder
	{
		public string Description { get; set; }
		public string Name { get; set; }
		public object Value { get; set; }
	   public bool IsActive { get; set; }
	}

}
