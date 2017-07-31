using Eplan.EplApi.MasterData;
using Eplan.EplApi.System;

namespace Suplanus.Sepla.Helper
{
   public class PartUtility
   {
      public static MDPart GetPart()
      {
         EplApplication eplanApplication = new EplApplication();
         MDPartsManagement partsManagement = new MDPartsManagement();
         string partnNumber = string.Empty;
         string partVariant = string.Empty;
         eplanApplication.ShowPartSelectionDialog(ref partnNumber, ref partVariant);
         MDPartsDatabase partsDatabase = partsManagement.OpenDatabase();
         MDPart part = partsDatabase.GetPart(partnNumber, partVariant);
         return part;
      }
   }
}