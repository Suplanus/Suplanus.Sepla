using System.Collections.Specialized;
using System.Linq;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Helper
{
	/// <summary>
	/// Helper class for multilanguage
	/// </summary>
	public class MultiLanguage
	{
      /// <summary>
      /// Returns the GUI language of EPLAN
      /// </summary>
		public static ISOCode.Language GuiLanguage
		{
			get
			{
				return new Languages().GuiLanguage.GetNumber();
			}
		}

      /// <summary>
      /// Returns the display languages
      /// </summary>
      /// <param name="project">EPLAN project</param>
      /// <returns>Language list</returns>
		public static StringCollection DisplayLanguages(Project project)
		{
			return GetLanguages(project, "TRANSLATEGUI.DISPLAYED_LANGUAGES");
		}

      /// <summary>
      /// Returns the project languages
      /// </summary>
      /// <param name="project">EPLAN project</param>
      /// <returns>Language list</returns>
		public static StringCollection ProjectLanguages(Project project)
		{
			return GetLanguages(project, "TRANSLATEGUI.TRANSLATE_LANGUAGES");
		}

      /// <summary>
      /// Returns languages of given settings path
      /// </summary>
      /// <param name="project">EPLAN project</param>
      /// <param name="settingsPath">EPLAN settings path</param>
      /// <returns>Language list</returns>
		private static StringCollection GetLanguages(Project project, string settingsPath)
		{
			using (new LockingStep())
			{
				ProjectSettings projectSettings = new ProjectSettings(project);
				var displayLanguagesString = projectSettings.GetStringSetting(settingsPath, 0);
				var languages = new StringCollection();
				var languagesFromSettings = displayLanguagesString.Split(';')
					.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray(); // remove empty
				languages.AddRange(languagesFromSettings);
				return languages;
			}
		}
	}
}
