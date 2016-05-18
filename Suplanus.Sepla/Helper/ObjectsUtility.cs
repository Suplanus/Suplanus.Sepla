using System.Collections.Generic;
using System.Linq;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
	public class ObjectsUtility
	{
		public static List<T> GetAllObjectsOfType<T>(Project project)
		{
         DMObjectsFinder dmObjectsFinder = new DMObjectsFinder(project);
			return dmObjectsFinder.GetPlacements(null).OfType<T>().ToList();       
		}

		public static List<Placement> GetAllObjects(Project project)
		{
			DMObjectsFinder dmObjectsFinder = new DMObjectsFinder(project);
			return dmObjectsFinder.GetPlacements(null).ToList();
		}

		public static List<StorableObject> GetSelectedStorableObjects()
		{
			SelectionSet selectionSet = new SelectionSet();

			// navigators
			List<StorableObject> storableObjects = selectionSet.SelectionRecursive.ToList();

			// GED
			if (!storableObjects.Any())
			{
				storableObjects = selectionSet.Selection.ToList();
			}
			return storableObjects;
		}
	}
}
