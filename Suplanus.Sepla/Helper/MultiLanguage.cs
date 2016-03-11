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

		public static StringCollection DisplayLanguages()
		{
			using (new LockingStep())
			{
				var project = ProjectUtility.GetCurrentProject();
				ProjectSettings projectSettings = new ProjectSettings(project);
				var displayLanguagesString = projectSettings.GetStringSetting("TRANSLATEGUI.DISPLAYED_LANGUAGES", 0);

				var languages = new StringCollection();
				var displaylanguages = displayLanguagesString.Split(';')
					.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray(); // remove empty
				languages.AddRange(displaylanguages);

				return languages;
			}
		}

	}
}
