using System.Collections.Generic;
using System.IO;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
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

		public static Project Create(string projectLinkFilePath, string projectTemplateFilePath, bool overwrite)
		{
			ProjectManager projectManager = new ProjectManager();
			Project project = null;

			using (new LockingStep()) // needed
			{
				// Exists
				if (File.Exists(projectLinkFilePath) && overwrite == false)
				{
					project = projectManager.OpenProject(projectLinkFilePath);
				}

				// New
				if (!File.Exists(projectLinkFilePath) || overwrite == true)
				{

					project = projectManager.CreateProject(projectLinkFilePath, projectTemplateFilePath);
				}
			}

			return project;
		}

		public static void Generate(string projectLinkFilePath, string projectTemplateFilePath, List<string> pageMacros)
		{
			var project = Create(projectLinkFilePath, projectTemplateFilePath, false);

			Insert insert = new Insert();
			foreach (var pageMacroFile in pageMacros)
			{
				// Load pages from macro
				PageMacro pageMacroOriginal = new PageMacro();
				PageMacro pageMacroNew = new PageMacro();
				pageMacroOriginal.Open(pageMacroFile, project);

				foreach (var page in pageMacroOriginal.Pages)
				{
					PagePropertyList ppl = page.NameParts;
					ppl[Properties.Page.DESIGNATION_PLANT] = "TEST";
					ppl = page.NameParts;
					NameService nameServ = new NameService(page);
					nameServ.EvaluateAndSetAllNames();
				}
				

				var tempMacro = Path.Combine(Path.GetTempPath(), "Suplanus.Sepla.Generator.TempMacro.emp");
				pageMacroOriginal.Create(tempMacro, pageMacroOriginal.Pages, pageMacroOriginal.Description);
				pageMacroOriginal.Open(tempMacro, project);

				// Insert in Project
				StorableObject[] insertedPages = insert.PageMacro(
					pageMacroOriginal, project, null, PageMacro.Enums.NumerationMode.Number);

			}
		}

	}
}
