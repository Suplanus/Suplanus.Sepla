using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Extensions
{
   public static class PageExtensions
   {
      public static void RemoveAllPlacements(this Page page)
      {
         page.RemoveSubPlacements(page.AllFirstLevelPlacements, true);
      }
   }
}
