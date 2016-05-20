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
		public const string TextplaceholderStartText = "&lt;#";
		public const string TextplaceholderEndText = "#&gt;";
		public const string BrickplaceholderStartText = "&lt;@@";
		public const string BrickplaceholderEndText = "@@&gt;";

		public static IEnumerable<T> GetMacroPlaceholder<T>(string filename, string startText, string endText)
			where T : IMacroPlaceholder, new()
		{
			if (!File.Exists(filename))
			{
				return null;
			}

			var text = File.ReadAllText(filename, Encoding.UTF8);
			IEnumerable<string> matches = Regex.Matches(text, startText + "(.*?)" + endText)
				.OfType<Match>()
				.Select(m => m.Groups[0].Value)
				.Distinct();

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
	}
}
