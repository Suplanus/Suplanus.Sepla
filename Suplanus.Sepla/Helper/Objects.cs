using System.Collections.Generic;
using System.Linq;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Helper
{
	public class Objects
	{
		public static List<T> GetAllObjects<T>(Eplan.EplApi.DataModel.Project project)
		{
			List<T> objects = new List<T>();
			foreach (Page page in project.Pages)
			{
				objects.AddRange(page.AllPlacements.OfType<T>());
			}
			return objects;
		}

	}
}
