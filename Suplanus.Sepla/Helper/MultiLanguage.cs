using System.Collections.Specialized;
using System.Linq;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Helper
{
	public class MultiLanguage
	{
		public static ISOCode.Language GuiLanguage
		{
			get
			{
				return new Languages().GuiLanguage.GetNumber();
			}
		}

		public static StringCollection DisplayLanguages(Project project)
		{
			return GetLanguages(project, "TRANSLATEGUI.DISPLAYED_LANGUAGES");
		}

		public static StringCollection ProjectLanguages(Project project)
		{

			return GetLanguages(project, "TRANSLATEGUI.TRANSLATE_LANGUAGES");

		}

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
