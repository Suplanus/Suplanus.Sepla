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
      selectionSet.LockProjectByDefault = false;

      // navigators
      List<StorableObject> storableObjects = selectionSet.SelectionRecursive.ToList();

      // GED
      if (!storableObjects.Any())
      {
        storableObjects = selectionSet.Selection.ToList();
      }

      return storableObjects;
    }

    /// <summary>
    /// Returns all selected StorableObjects of given type in editor, single page,
    /// multi page selection or recursive if structure or project is selected.
    /// </summary>
    /// <returns>Returns an empty list if nothing selected</returns>
    public static List<T> GetSelectedObjectsOfType<T>() where T : StorableObject
    {
      var selection = new SelectionSet();
      selection.LockProjectByDefault = false;
      StorableObject[] selectedStorableObjects = selection.SelectionRecursive;

      // Selection in GED
      if (selectedStorableObjects.Length == 0)
      {
        selectedStorableObjects = selection.Selection;
      }

      var selectedObjectsOfType = GetStorableObjects<T>(selectedStorableObjects);
      return selectedObjectsOfType;
    }

    private static List<T> GetStorableObjects<T>(StorableObject[] selectedStorableObjects) where T : StorableObject
    {
      List<T> storableObjects = selectedStorableObjects
                                .OfType<T>()
                                .ToList();

      List<Group> groups = selectedStorableObjects
                           .OfType<Group>()
                           .ToList();

      foreach (var group in groups)
      {
        var storableObjectsFromGroup = GetStorableObjectsFromGroup<T>(group);
        storableObjects.AddRange(storableObjectsFromGroup);
      }

      return storableObjects;
    }

    //Recursive method for group in group
    private static List<T> GetStorableObjectsFromGroup<T>(Group group) where T : StorableObject
    {
      List<T> storableObjects = new List<T>();

      List<T> storableObjectsInGroups = group.SubPlacements
                                             .OfType<T>()
                                             .ToList();

      storableObjects.AddRange(storableObjectsInGroups);

      //get only objects of type Group without subtypes like ViewPlacement
      List<Group> groupsSub = group.SubPlacements
                                   .OfType<Group>()
                                   .Where(obj => obj.GetType() == typeof(Group))
                                   .ToList();

      foreach (var groupSub in groupsSub)
      {
        var storableObjectsSub = GetStorableObjectsFromGroup<T>(groupSub);
        storableObjects.AddRange(storableObjectsSub);
      }

      return storableObjects;
    }
  }
}