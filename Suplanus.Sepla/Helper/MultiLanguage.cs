using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eplan.EplApi.Base;

namespace Suplanus.Sepla.Helper
{
	public class MultiLanguage
	{

		public static ISOCode.Language GuiLanguage
		{
			get
			{
				return new Languages().GuiLanguage.GetNumber();
			}
		}

	}
}
