using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Helper
{
   public static class MasterdataUtility
   {
      /// <summary>
      /// Check if masterdata in Project
      /// </summary>
      /// <param name="project"></param>
      /// <param name="masterdataName">Full file name</param>
      /// <returns></returns>
      public static bool IsMasterdataInProject(Project project, string masterdataName)
      {
         return new Masterdata().get_ProjectEntries(project).Contains(masterdataName);
      }

      /// <summary>
      /// Check if masterdata is in System
      /// </summary>
      /// <param name="masterdataName">Full file name</param>
      /// <returns></returns>
      public static bool IsMasterdataInSystem(string masterdataName)
      {
         return new Masterdata().SystemEntries.Contains(masterdataName);
      }

      /// <summary>
      /// Returns all files with given extension e.g. f01
      /// </summary>
      /// <param name="extension"></param>
      /// <returns></returns>
      public static List<string> GetListOfType(string extension)
      {
         return new Masterdata().SystemEntries.Cast<string>().Where(systemEntry => systemEntry.EndsWith(extension)).ToList();
      }

      /// <summary>
      /// Add masterdata from system to project
      /// </summary>
      /// <param name="project"></param>
      /// <param name="masterdataName">Full file name</param>
      /// <returns></returns>
      public static void AddMasterdataToProject(Project project, string masterdataName)
      {
         Masterdata masterdata = new Masterdata();
         StringCollection newMasterdatas = new StringCollection();
         StringCollection projectMasterdatas = masterdata.get_ProjectEntries(project);
         if (!projectMasterdatas.Contains(masterdataName))
         {
            newMasterdatas.Add(masterdataName);
            var test = masterdata.AddToProjectEx(project, newMasterdatas);
         }
      }

      /// <summary>
      /// Add masterdata from system to project
      /// </summary>
      /// <param name="project"></param>
      /// <param name="symbolLibraryNameWithExtesion">Filename with extension (without path)</param>
      /// <returns></returns>
      public static void AddSymbolLibrary(Project project, string symbolLibraryNameWithExtesion)
      {
         Masterdata masterdata = new Masterdata();
         StringCollection newMasterdatas = new StringCollection();
         StringCollection projectMasterdatas = masterdata.get_ProjectEntries(project);
         if (!projectMasterdatas.Contains(symbolLibraryNameWithExtesion))
         {
            Masterdata md = new Masterdata();
            newMasterdatas.Add(symbolLibraryNameWithExtesion);
            StringCollection entries_before = md.get_ProjectEntries(project);
            var test = md.AddToProjectEx(project, newMasterdatas);
            StringCollection entries_after = md.get_ProjectEntries(project);
         }
      }
   }
}
