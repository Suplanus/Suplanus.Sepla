using System;
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

      /// <summary>
      /// Creates project or get the existing project, or overwrite the existing
      /// </summary>
      /// <param name="projectLinkFilePath"></param>
      /// <param name="projectTemplateFilePath"></param>
      /// <param name="overwrite"></param>
      /// <returns></returns>
      public static Project Create(string projectLinkFilePath, string projectTemplateFilePath, bool overwrite)
      {
         ProjectManager projectManager = new ProjectManager();
         Project project = null;

         using (new LockingStep()) // needed
         {
            // Exists
            if (projectManager.ExistsProject(projectLinkFilePath) && overwrite == false)
            {
               project = OpenProject(projectLinkFilePath);
            }

            // New
            if (!projectManager.ExistsProject(projectLinkFilePath) || overwrite == true)
            {

               project = projectManager.CreateProject(projectLinkFilePath, projectTemplateFilePath);
            }
         }

         return project;
      }

      /// <summary>
      /// Copy the project or get the existing project, or overwrite the existing
      /// </summary>
      /// <param name="projectSource"></param>
      /// <param name="projectLinkFilePath"></param>
      /// <param name="overwrite"></param>
      /// <returns></returns>
      public static Project Copy(Project projectSource, string projectLinkFilePath, bool overwrite)
      {
         using (new LockingStep()) // needed
         {
            ProjectManager projectManager = new ProjectManager();

            // New
            if (!projectManager.ExistsProject(projectLinkFilePath) || overwrite == true)
            {
               projectManager.CopyProject(projectSource.ProjectLinkFilePath, projectLinkFilePath,
                  ProjectManager.CopyMode.OnlyProjecthead);
            }

            return OpenProject(projectLinkFilePath);
         }
      }

      /// <summary>
      /// Opens project and checks if its open
      /// </summary>
      /// <param name="projectLinkFilePath"></param>
      /// <returns></returns>
      public static Project OpenProject(string projectLinkFilePath)
      {
         if (!File.Exists(projectLinkFilePath))
         {
            throw new FileNotFoundException("EPLAN project link file not found", projectLinkFilePath);
         }

         using (LockingStep lockingStep = new LockingStep())
         {
            ProjectManager projectManager = new ProjectManager();
            var project = projectManager.OpenProjects.FirstOrDefault(p => p.ProjectLinkFilePath.Equals(projectLinkFilePath));
            if (project != null)
            {
               return project;
            }
            else
            {
               return projectManager.OpenProject(projectLinkFilePath, ProjectManager.OpenMode.Standard, true);
            }
         }
      }

      public static void Generate(string projectLinkFilePath, string projectTemplateFilePath,
         List<GeneratablePageMacro> generatablePageMacros)
      {
         var project = Create(projectLinkFilePath, projectTemplateFilePath, false);
         project.RemoveAllPages();

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
               if (generatablePageMacro.LocationIdentifierIdentifier != null)
               {
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
               }

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
