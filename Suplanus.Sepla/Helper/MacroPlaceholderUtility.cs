using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Suplanus.Sepla.Objects;

namespace Suplanus.Sepla.Helper
{
   /// <summary>
   /// Helper for placeholders in EPLAN macros
   /// </summary>
   public class MacroPlaceholderUtility
   {
      /// <summary>
      /// Start text TextPlaceholder
      /// </summary>
      public const string TEXTPLACEHOLDER_START_TEXT = "&lt;#"; // 

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
         IEnumerable<string> matches = Regex.Matches(text,startText + "(.*?)" + endText)
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
            string replaceText;
            if (!placeholder.IsActive || placeholder.Value == null || string.IsNullOrEmpty(placeholder.Value.ToString()))
            {
               if (!removeText)
               {
                  continue;
               }
               replaceText = "";
            }
            else
            {
               replaceText = placeholder.Value.ToString();
            }

            // Replace
            var searchText = TEXTPLACEHOLDER_START_TEXT + placeholder.Name + TEXTPLACEHOLDER_END_TEXT;
            content = content.Replace(searchText, replaceText);
         }

         File.WriteAllText(tempFile, content, Encoding.UTF8);

         return tempFile;
      }
   }
}
