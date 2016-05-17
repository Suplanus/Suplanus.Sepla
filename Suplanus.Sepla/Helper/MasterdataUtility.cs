using System.Collections.Specialized;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
	public class MasterdataUtility
	{
		bool IsFormInProject(Project project, string formName)
		{
			return new Masterdata().get_ProjectEntries(project).Contains(formName);
		}

		bool IsFormInSystem(string formName)
		{
			return new Masterdata().SystemEntries.Contains(formName);
		}

		bool AddFormToProject(Project project, string formName)
		{
			Masterdata masterdata = new Masterdata();
			StringCollection newForms = new StringCollection();
			StringCollection projectForms = masterdata.get_ProjectEntries(project);

			if (!IsFormInSystem(formName))
			{
				return false;
			}

			if (!projectForms.Contains(formName))
			{
				newForms.Add(formName);
				masterdata.AddToProjectEx(project, newForms);
			}
			return true;
		}
	}
}
