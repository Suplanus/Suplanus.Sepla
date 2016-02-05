using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
	public class ProjectUtility
	{
		public static Project GetCurrentProject()
		{
			SelectionSet selectionSet = new SelectionSet();
			selectionSet.LockProjectByDefault = true;
			selectionSet.LockSelectionByDefault = true;
			return selectionSet.GetCurrentProject(true);
		}

		public static Project Create(string projectLinkFilePath, string projectTemplateFilePath)
		{
			ProjectManager projectManager = new ProjectManager();
			var project = projectManager.CreateProject(projectLinkFilePath, projectTemplateFilePath);
			return project;
		}
	}
}
