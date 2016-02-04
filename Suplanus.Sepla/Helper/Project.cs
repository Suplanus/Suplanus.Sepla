using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
	public class Project
	{
		public static Eplan.EplApi.DataModel.Project GetCurrentProject()
		{
			SelectionSet selSet = new SelectionSet();
			selSet.LockProjectByDefault = true;
			selSet.LockSelectionByDefault = true;
			return selSet.GetCurrentProject(true);
		}
	}
}
