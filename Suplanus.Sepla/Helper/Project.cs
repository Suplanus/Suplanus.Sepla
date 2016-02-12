using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;
using Suplanus.Sepla.Objects;

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

		public static void Generate(string projectLinkFilePath, string projectTemplateFilePath,
			List<GeneratablePageMacro> generatablePageMacros)
		{
			var project = Create(projectLinkFilePath, projectTemplateFilePath, false);

#if DEBUG
			project.RemoveAllPages(); // test
#endif

			Insert insert = new Insert();
			var pageCount = project.Pages.Length; // needed cause of overwrite
			foreach (var generatablePageMacro in generatablePageMacros)
			{
				// Load pages from macro
				PageMacro pageMacro = new PageMacro();
				pageMacro.Open(generatablePageMacro.Filename, project);
				foreach (var page in pageMacro.Pages)
				{
					// Rename
					pageCount++;

					PagePropertyList pagePropertyList = page.NameParts;
					pagePropertyList[Properties.Page.DESIGNATION_FUNCTIONALASSIGNMENT] =
						generatablePageMacro.LocationIdentifierIdentifier.FunctionAssignment;
					pagePropertyList[Properties.Page.DESIGNATION_PLANT] =
						generatablePageMacro.LocationIdentifierIdentifier.Plant;
                    pagePropertyList[Properties.Page.DESIGNATION_PLACEOFINSTALLATION] =
						generatablePageMacro.LocationIdentifierIdentifier.PlaceOfInstallation;
					pagePropertyList[Properties.Page.DESIGNATION_LOCATION] =
						generatablePageMacro.LocationIdentifierIdentifier.Location;
					pagePropertyList[Properties.Page.DESIGNATION_USERDEFINED] =
						generatablePageMacro.LocationIdentifierIdentifier.UserDefinied;

					pagePropertyList[Properties.Page.PAGE_COUNTER] = pageCount;
					page.NameParts = pagePropertyList;

					new NameService(page).EvaluateAndSetAllNames();
				}

				// Insert pagemacro
				insert.PageMacro(pageMacro, project, null, PageMacro.Enums.NumerationMode.Number);
			}






		}

	}
}
