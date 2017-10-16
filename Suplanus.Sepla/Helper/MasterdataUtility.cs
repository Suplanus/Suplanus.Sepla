using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
   /// <summary>
   /// Helper class for Masterdata
   /// </summary>
   public static class MasterdataUtility
   {
      /// <summary>
      /// Check if masterdata in Project
      /// </summary>
      /// <param name="project"></param>
      /// <param name="masterdataName">Full file name</param>
      /// <returns>Is masterdata in project</returns>
      public static bool IsMasterdataInProject(Project project, string masterdataName)
      {
         return new Masterdata().get_ProjectEntries(project).Contains(masterdataName);
      }

      /// <summary>
      /// Check if masterdata is in System
      /// </summary>
      /// <param name="masterdataName">Full file name</param>
      /// <returns>Is masterdata in system</returns>
      public static bool IsMasterdataInSystem(string masterdataName)
      {
         return new Masterdata().SystemEntries.Contains(masterdataName);
      }

      /// <summary>
      /// Returns all files with given extension e.g. f01
      /// </summary>
      /// <param name="extension">File extension of masterdata</param>
      /// <returns>List of given type of masterdata</returns>
      public static List<string> GetListOfType(string extension)
      {
         return new Masterdata().SystemEntries.Cast<string>().Where(systemEntry => systemEntry.EndsWith(extension)).ToList();
      }

      /// <summary>
      /// Add masterdata from system to project
      /// </summary>
      /// <param name="project"></param>
      /// <param name="masterdataName">Full file name</param>
      public static void AddMasterdataToProject(Project project, string masterdataName)
      {
         Masterdata masterdata = new Masterdata();
         StringCollection newMasterdatas = new StringCollection();
         StringCollection projectMasterdatas = masterdata.get_ProjectEntries(project);
         if (!projectMasterdatas.Contains(masterdataName))
         {
            newMasterdatas.Add(masterdataName);
            masterdata.AddToProjectEx(project, newMasterdatas);
         }
      }

      /// <summary>
      /// Add masterdata from system to project
      /// </summary>
      /// <param name="project"></param>
      /// <param name="symbolLibraryNameWithExtesion">Filename with extension (without path)</param>
      public static void AddSymbolLibrary(Project project, string symbolLibraryNameWithExtesion)
      {
         Masterdata masterdata = new Masterdata();
         StringCollection newMasterdatas = new StringCollection();
         StringCollection projectMasterdatas = masterdata.get_ProjectEntries(project);
         if (!projectMasterdatas.Contains(symbolLibraryNameWithExtesion))
         {
            Masterdata md = new Masterdata();
            newMasterdatas.Add(symbolLibraryNameWithExtesion);
            md.AddToProjectEx(project, newMasterdatas);
         }
      }

      public static void RemoveSymbolLibrary(Project project, string symbolLibraryNameWithExtesion)
      {
         Masterdata masterdata = new Masterdata();
         StringCollection newMasterdatas = new StringCollection();
         StringCollection projectMasterdatas = masterdata.get_ProjectEntries(project);
         if (projectMasterdatas.Contains(symbolLibraryNameWithExtesion))
         {
            newMasterdatas.Remove(symbolLibraryNameWithExtesion);
         }
      }
   }
}
