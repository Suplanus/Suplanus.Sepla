using System.Collections.Generic;
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
   public class MacroPlaceholderUtility
   {
      public const string REAL_RECORDPLACEHOLDER_START_TEXT = @"<§"; // equals <§
      public const string REAL_RECORDPLACEHOLDER_END_TEXT = @"§>"; // equals §>
      public const string REAL_TEXTPLACEHOLDER_START_TEXT = @"<#"; // equals <#
      public const string REAL_TEXTPLACEHOLDER_END_TEXT = @"#>"; // equals #>
      public const string REAL_BRICKPLACEHOLDER_START_TEXT = @"<@"; // equals <@
      public const string REAL_BRICKPLACEHOLDER_END_TEXT = @"@>"; // equals @>

      /// <summary>
      /// Start text TextPlaceholder
      /// </summary>
      public const string TEXTPLACEHOLDER_START_TEXT = "&lt;#"; // equals <#

      /// <summary>
      /// End text for TextPlaceholder
      /// </summary>
      public const string TEXTPLACEHOLDER_END_TEXT = "#&gt;"; // equals #>

      /// <summary>
      /// Start text for BrickPlaceholder
      /// </summary>
      public const string BRICKPLACEHOLDER_START_TEXT = "&lt;@@"; // equals <@ double @ needed in eplan

      /// <summary>
      /// End text for BrickPlaceholder
      /// </summary>
      public const string BRICKPLACEHOLDER_END_TEXT = "@@&gt;"; // equals @> double @ needed in eplan

      /// <summary>
      /// Start text for RecordPlaceholder
      /// </summary>
      public const string RECORDPLACEHOLDER_START_TEXT = @"&lt;§"; // equals <§

      /// <summary>
      /// End text for RecordPlaceholder
      /// </summary>
      public const string RECORDPLACEHOLDER_END_TEXT = @"§&gt;"; // equals §>

      /// <summary>
      /// Get the Placeholder to display without start and end text
      /// </summary>
      /// <param name="placeholderPlainText">Clean value of placeholder</param>
      /// <returns></returns>
      public static string GetPlaceholderName(string placeholderPlainText)
      {
         string returnValue = placeholderPlainText
            .Replace(TEXTPLACEHOLDER_START_TEXT, "")
            .Replace(BRICKPLACEHOLDER_START_TEXT, "")
            .Replace(RECORDPLACEHOLDER_START_TEXT, "")
            .Replace("<@", "")
            .Replace(TEXTPLACEHOLDER_END_TEXT, "")
            .Replace(BRICKPLACEHOLDER_END_TEXT, "")
            .Replace(RECORDPLACEHOLDER_END_TEXT, "")
            .Replace("@>", "");
         return returnValue;
      }

      /// <summary>
      /// Returns alls Placeholder of a macro file
      /// </summary>
      /// <typeparam name="T">Type of Placeholder</typeparam>
      /// <param name="filename">Filename which be parsed</param>
      /// <param name="startText">Placeholder start text</param>
      /// <param name="endText">Placeholder end text</param>
      /// <returns>List of Placeholder</returns>
      public static IEnumerable<T> GetMacroPlaceholder<T>(string filename, string startText, string endText)
         where T : IMacroPlaceholder, new()
      {
         if (!File.Exists(filename))
         {
            return null;
         }

         // Getplaceholders
         var text = File.ReadAllText(filename, Encoding.UTF8);
         IEnumerable<string> matches = Regex.Matches(text, startText + "(.*?)" + endText)
            .OfType<Match>()
            .Select(m => m.Groups[0].Value)
            .Distinct();

         // Return placeholder objects
         List<T> placeholders = new List<T>();
         foreach (var match in matches)
         {
            var placeholderText = new T();
            var name = match.Replace(startText, "").Replace(endText, "");
            placeholderText.Name = name;
            placeholders.Add(placeholderText);
         }
         return placeholders;
      }

      /// <summary>
      /// Replaces all placeholder in file and returns a temp macro file
      /// </summary>
      /// <param name="macroFilename">Source macro file</param>
      /// <param name="placeholders">Placeholder to replace</param>
      /// <param name="removeText">Remove Text if value is not active or empty</param>
      /// <returns></returns>
      public static string ReplacePlaceholderTextAndGetTempMacro(string macroFilename, List<IMacroPlaceholder> placeholders, bool removeText)
      {
         if (!File.Exists(macroFilename))
         {
            return null;
         }

         string extension = Path.GetExtension(macroFilename);
         string tempFile = Path.Combine(Path.GetTempPath(), "Suplanus.Sepla.MacroPlaceholderUtility.TempMacro" + extension); // needed because EPLAN is checking extension
         string content = File.ReadAllText(macroFilename);

         foreach (var placeholder in placeholders)
         {
            // Skip if not active or empty
            if (!placeholder.IsActive || placeholder.Value == null || string.IsNullOrEmpty(placeholder.Value.ToString()))
            {
               continue;
            }

            // Replace
            var replaceText = placeholder.Value.ToString();
            var searchText = TEXTPLACEHOLDER_START_TEXT + placeholder.Name + TEXTPLACEHOLDER_END_TEXT;
            content = content.Replace(searchText, replaceText);
         }

         File.WriteAllText(tempFile, content, Encoding.UTF8);

         return tempFile;
      }


      public static void RemoveAllUnifishedTextPlaceholder(Project project)
      {
         // Search text
         var placeholderIdentifier = new List<string>
         {
            REAL_TEXTPLACEHOLDER_START_TEXT,
            REAL_RECORDPLACEHOLDER_START_TEXT,
            REAL_BRICKPLACEHOLDER_START_TEXT,
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
               // todo: EPLAN fix (2.6) T1085938
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
               existingValues = existingValuesWithoutEmpty;

               // Replace
               foreach (PropertyValue propertyValue in existingValues)
               {
                  propertyValue.Parent.Parent.LockObject();
                  propertyValue.Set("");
               }
            }
         }






      }
   }
}
