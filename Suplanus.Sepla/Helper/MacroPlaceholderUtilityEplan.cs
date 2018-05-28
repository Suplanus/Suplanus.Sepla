using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.HEServices;
using Suplanus.Sepla.Objects;

namespace Suplanus.Sepla.Helper
{
  /// <summary>
  /// Helper for placeholders in EPLAN macros
  /// </summary>
  public class MacroPlaceholderUtilityEplan
  {
    public static void RemoveAllUnifishedTextPlaceholder(Project project)
    {
      // Search text
      var placeholderIdentifier = new List<string>
         {
           PlaceholderUtility.REAL_TEXTPLACEHOLDER_START_TEXT,
            PlaceholderUtility.REAL_RECORDPLACEHOLDER_START_TEXT,
            PlaceholderUtility.REAL_BRICKPLACEHOLDER_START_TEXT,
         };

      // Set special characters: http://eplan.help/help/platform/2.5/en-US/help/EPLAN_help.htm#htm/searchandreplacegui_k_platzhalter.htm
      for (int index = 0; index < placeholderIdentifier.Count; index++)
      {
        placeholderIdentifier[index] = placeholderIdentifier[index].Replace("#", "[#]");
      }


      Search search = new Search();
      search[Search.Settings.AllProperties] = true;
      search[Search.Settings.Placeholders] = true;
      search[Search.Settings.DeviceTag] = true;
      search[Search.Settings.GraphicPages] = true;
      search[Search.Settings.InstallationSpaces] = true;
      search[Search.Settings.LogicPages] = true;
      search[Search.Settings.NotPlaced] = true;
      search[Search.Settings.EvalutionPages] = false;
      search[Search.Settings.PageData] = true;
      search[Search.Settings.ProjectData] = true;
      search[Search.Settings.Texts] = true;
      search[Search.Settings.WholeTexts] = false;
      foreach (var identifier in placeholderIdentifier)
      {
        // Init search            
        search.ClearSearchDB(project);
        search.Project(project, identifier);

        // Get objects
        StorableObject[] foundObjects = search.GetAllSearchDBEntries(project);
        foreach (var foundObject in foundObjects)
        {
          // Filter only text objects
          // EPLAN fix (2.6) T1085938
          var existingValues = foundObject.Properties.ExistingValues
             .Where(p => !p.Definition.IsInternal &&
                         !p.Definition.IsReadOnly &&
                         (
                            p.Definition.Type == PropertyDefinition.PropertyType.MultilangString ||
                            p.Definition.Type == PropertyDefinition.PropertyType.String))
             .ToList();
          List<PropertyValue> existingValuesWithoutEmpty = new List<PropertyValue>();
          foreach (var propertyValue in existingValues)
          {
            if (propertyValue.Definition.IsIndexed)
            {
              foreach (int index in propertyValue.Indexes)
              {
                if (!propertyValue[index].IsEmpty)
                {
                  existingValuesWithoutEmpty.Add(propertyValue[index]);
                }
              }
            }
            else
            {
              if (!propertyValue.IsEmpty)
              {
                existingValuesWithoutEmpty.Add(propertyValue);
              }
            }
          }

          // Fix fields without placeholder in search result and replace the eplan specific search brackets
          existingValues = existingValuesWithoutEmpty
              .Where(obj => obj.ToString().Contains(
                  identifier.Replace("[", "").Replace("]", "")))
              .ToList();

          // Replace
          foreach (PropertyValue propertyValue in existingValues)
          {
            //propertyValue.Parent?.Parent?.SmartLock();
            propertyValue.Set("");
          }
        }
      }






    }
  }
}
