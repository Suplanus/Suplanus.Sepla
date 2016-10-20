using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Helper
{
   /// <summary>
   /// Helper for Locations in EPLAN
   /// </summary>
   public static class LocationUtility
   {
      /// <summary>
      /// Orders all location ascending
      /// </summary>
      /// <param name="project"></param>
      public static void OrderLocation(Project project)
      {
         var hierachies = Enum.GetValues(typeof(Project.Hierarchy)); // Get all types
         foreach (Project.Hierarchy hierachy in hierachies)
         {
            string[] locations = project.GetLocations(hierachy);
            if (locations != null) // could be null
            {
               locations = locations.OrderBy(o => o).ToArray(); // OrderAch
               project.SetSortedLocations(hierachy, locations);
            }
         }
      }
   }
}
