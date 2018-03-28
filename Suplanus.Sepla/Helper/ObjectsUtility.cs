using System.Collections.Generic;
using System.Linq;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
	/// <summary>
	/// Objects helper
	/// </summary>
	public class ObjectsUtility
	{
      /// <summary>
      /// Returns all objects of given type
      /// </summary>
      /// <typeparam name="T">EPLAN object type</typeparam>
      /// <param name="project">EPLAN Object</param>
      /// <returns>List of objects</returns>
public static List<T> GetAllObjectsOfType<T>(Project project)
{
      DMObjectsFinder dmObjectsFinder = new DMObjectsFinder(project);
	return dmObjectsFinder.GetPlacements(null).OfType<T>().ToList();       
}

      /// <summary>
      /// Returns all objects of a EPLAN project
      /// </summary>
      /// <param name="project">EPLAN project</param>
      /// <returns>List of Placements</returns>
		public static List<Placement> GetAllObjects(Project project)
		{
			DMObjectsFinder dmObjectsFinder = new DMObjectsFinder(project);
			return dmObjectsFinder.GetPlacements(null).ToList();
		}

      /// <summary>
      /// Returns the selected objects in navigator or in GED
      /// </summary>
      /// <returns></returns>
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
