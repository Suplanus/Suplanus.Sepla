using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Suplanus.Sepla.Objects;

namespace Suplanus.Sepla.Helper
{
   public class MacroPlaceholderUtility
   {
      public const string TEXTPLACEHOLDER_START_TEXT = "&lt;#"; // equals <#
      public const string TEXTPLACEHOLDER_END_TEXT = "#&gt;"; // equals #>
      public const string BRICKPLACEHOLDER_START_TEXT = "&lt;@@"; // equals <@ double @ needed in eplan
      public const string BRICKPLACEHOLDER_END_TEXT = "@@&gt;"; // equals @> double @ needed in eplan

      public static string GetPlaceholderName(string placeholderPlainText)
      {
         string returnValue = placeholderPlainText
            .Replace(TEXTPLACEHOLDER_START_TEXT, "")
            .Replace(BRICKPLACEHOLDER_START_TEXT, "")
            .Replace("<@", "")
            .Replace(TEXTPLACEHOLDER_END_TEXT, "")
            .Replace(BRICKPLACEHOLDER_END_TEXT, "")
            .Replace("@>", "");
         return returnValue;
      }

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

      public static string ReplacePlaceholderTextAndGetTempMacro(string macroFilename, List<IMacroPlaceholder> placeholders)
      {
         string extension = Path.GetExtension(macroFilename);
         string tempFile = Path.Combine(Path.GetTempPath(), "Suplanus.Sepla.MacroPlaceholderUtility.TempMacro" + extension); // needed because EPLAN is checking extension
         string content = File.ReadAllText(macroFilename);

         foreach (var placeholder in placeholders)
         {
            // Skip if not active or empty
            string replaceText;
            if (!placeholder.IsActive || placeholder.Value == null || string.IsNullOrEmpty(placeholder.Value.ToString()))
            {
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
