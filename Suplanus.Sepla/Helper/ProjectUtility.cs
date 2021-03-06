﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;
using Suplanus.Sepla.Objects;

namespace Suplanus.Sepla.Helper
{
  /// <summary>
  /// Project helper class
  /// </summary>
  public class ProjectUtility
  {
    /// <summary>
    /// Returns the active project
    /// </summary>
    /// <returns>Active Project</returns>
    public static Project GetCurrentProject()
    {
      SelectionSet selectionSet = new SelectionSet();
      selectionSet.LockProjectByDefault = false;
      selectionSet.LockSelectionByDefault = false;
      return selectionSet.GetCurrentProject(false);
    }

    /// <summary>
    /// Creates project or get the existing project, or overwrite the existing
    /// </summary>
    /// <param name="projectLinkFilePath">EPLAN project file (*.elk)</param>
    /// <param name="projectTemplateFilePath">EPLAN template project (*.zw9)</param>
    /// <param name="overwrite">Overwrites the existing project, False: Open the project</param>
    /// <returns>EPLAN Project</returns>
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
          // Close if project exists and should be overwritten
          var existingProject = projectManager.OpenProjects.FirstOrDefault(p => p.ProjectLinkFilePath.Equals(projectLinkFilePath));
          if (existingProject != null)
          {
            existingProject.Close();
          }

          // Remove project if exists: Removing on file layer is much faster than via EPLAN API
          DeleteProject(projectLinkFilePath, projectManager);
          project = projectManager.CreateProject(projectLinkFilePath, projectTemplateFilePath);
        }
      }

      return project;
    }

    private static void DeleteProject(string projectLinkFilePath, ProjectManager projectManager)
    {
      try
      {
        if (projectManager.ExistsProject(projectLinkFilePath))
        {
          // Project folder
          var suffix = ".elk";
          string projectFolder = projectLinkFilePath.Substring(0, projectLinkFilePath.Length - suffix.Length);
          projectFolder = projectFolder + ".edb";
          Directory.Delete(projectFolder, true);

          // Link file
          File.Delete(projectLinkFilePath);
        }
      }
      catch (Exception exception)
      {
        ExceptionHelper.HandleException(exception, "MacroTool");
        throw new Exception("Project could not be removed:" + Environment.NewLine + projectLinkFilePath, exception);
      }
    }

    /// <summary>
    /// Copy the project or get the existing project, or overwrite the existing
    /// </summary>
    /// <param name="projectSource">Source project</param>
    /// <param name="projectLinkFilePath">EPLAN project file (*.elk)</param>
    /// <param name="overwrite">Overwrites the existing project, False: Open the project</param>
    /// <param name="copyMode"></param>
    /// <returns>EPLAN Project</returns>
    public static Project Copy(Project projectSource, string projectLinkFilePath, bool overwrite, ProjectManager.CopyMode copyMode)
    {
      using (new LockingStep()) // needed
      {
        ProjectManager projectManager = new ProjectManager();

        // New
        if (!projectManager.ExistsProject(projectLinkFilePath) || overwrite == true)
        {
          projectManager.CopyProject(projectSource.ProjectLinkFilePath, projectLinkFilePath,
             copyMode);
        }

        return OpenProject(projectLinkFilePath);
      }
    }

    /// <summary>
    /// Copy the project or get the existing project, or overwrite the existing
    /// </summary>
    /// <param name="projectSource">Source project</param>
    /// <param name="projectLinkFilePath">EPLAN project file (*.elk)</param>
    /// <param name="overwrite">Overwrites the existing project, False: Open the project</param>
    /// <param name="copyMode"></param>
    /// <returns>EPLAN Project</returns>
    public static Project Copy(string projectSource, string projectLinkFilePath, bool overwrite, ProjectManager.CopyMode copyMode)
    {
      using (new LockingStep()) // needed
      {
        ProjectManager projectManager = new ProjectManager();

        // New
        if (!projectManager.ExistsProject(projectLinkFilePath) || overwrite == true)
        {
          projectManager.CopyProject(projectSource, projectLinkFilePath,
             copyMode);
        }

        return OpenProject(projectLinkFilePath);
      }
    }

    /// <summary>
    /// Opens project and checks if its open
    /// </summary>
    /// <param name="projectLinkFilePath">EPLAN project file (*.elk)</param>
    /// <param name="openMode">Set how the project should be opened</param>
    /// <param name="upgradeIfNeeded">Set if the project should be upgraded from an older version</param>
    /// <returns>EPLAN Project</returns>
    public static Project OpenProject(string projectLinkFilePath, ProjectManager.OpenMode openMode = ProjectManager.OpenMode.Standard, bool upgradeIfNeeded = true)
    {
      projectLinkFilePath = Environment.ExpandEnvironmentVariables(projectLinkFilePath);
      
      if (!File.Exists(projectLinkFilePath))
      {
        throw new FileNotFoundException("EPLAN project link file not found", projectLinkFilePath);
      }

      using (new LockingStep())
      {
        ProjectManager projectManager = new ProjectManager();
        projectManager.LockProjectByDefault = false;
        Project project = projectManager.GetProject(projectLinkFilePath);

        // Check if openMode is OK
        if (project != null)
        {
          bool reOpen = false;
          switch (openMode)
          {
            case ProjectManager.OpenMode.Standard:
              if (project.IsExclusive || project.IsReadOnly)
              {
                reOpen = true;
              }
              break;
            case ProjectManager.OpenMode.ReadOnly:
              if (!project.IsReadOnly)
              {
                reOpen = true;
              }
              break;
            case ProjectManager.OpenMode.Exclusive:
              if (!project.IsExclusive)
              {
                reOpen = true;
              }
              break;
            default: throw new ArgumentOutOfRangeException(nameof(openMode), openMode, null);
          }
          if (reOpen)
          {
            project.Close();
            project = OpenProject(projectLinkFilePath, openMode);
          }
          return project;
        }
        return projectManager.OpenProject(projectLinkFilePath, openMode, upgradeIfNeeded);
      }
    }

    /// <summary>
    /// Returns true if there is a multi user conflict in project
    /// </summary>
    /// <param name="project">EPLAN project</param>
    /// <param name="showDialog">Shows dialog if there is a conflict (optional)</param>
    /// <returns></returns>
    public static bool IsMultiUserConflict(Project project, bool showDialog = false)
    {
      var currentUsers = project.CurrentUsers.ToList();

      // No conflict
      if (currentUsers.Count <= 1)
      {
        return false;
      }

      // Conflict
      if (showDialog)
      {
        StringBuilder sb = new StringBuilder();
        foreach (var user in currentUsers)
        {
          if (!String.IsNullOrEmpty(user.Name) && !String.IsNullOrEmpty(user.Identification))
          {
            sb.AppendLine(user.ComputerName + " / " + user.Name + " / " + user.Identification);
          }
          else if (!String.IsNullOrEmpty(user.Name))
          {
            sb.AppendLine(user.ComputerName + " / " + user.Name);
          }
          else if (!String.IsNullOrEmpty(user.Identification))
          {
            sb.AppendLine(user.ComputerName + " / " + user.Identification);
          }
          else
          {
            sb.AppendLine(user.ComputerName);
          }
        }
        MessageBox.Show(sb.ToString(), "Multi user conflict", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
      }
      return true;
    }

    public static bool IsCircuitProject(Project project)
    {
      var projectType = project.Properties.PROJ_NUMERICTYPE.ToInt();
      if (projectType == 1) // 1=CircutitProject 2=MacroProject
      {
        return true;
      }
      return false;
    }
  }
}
