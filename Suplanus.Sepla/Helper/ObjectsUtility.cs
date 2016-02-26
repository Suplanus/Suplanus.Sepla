using System.Collections.Generic;
using System.Linq;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Helper
{
	public class ObjectsUtility
	{
		public static List<T> GetAllObjects<T>(Project project)
		{
			DMObjectsFinder dmObjectsFinder = new DMObjectsFinder(project);
			return dmObjectsFinder.GetPlacements(null).OfType<T>().ToList();
		}
	}
}
