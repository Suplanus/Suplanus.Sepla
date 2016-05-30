using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eplan.EplApi.Starter;

namespace Suplanus.Sepla.Application
{
   /// <summary>
   /// EPLAN Starter Helper: No other EPLAN-Namespaces are allowed
   /// </summary>
   public class Starter
   {
      public static string GetBinPathLastVersion()
      {
         List<EplanData> eplanVersions = new List<EplanData>();

         List<EplanData> eplanVersions32Bit = new List<EplanData>();
         new EplanFinder().GetInstalledEplanVersions(ref eplanVersions32Bit);
         eplanVersions.AddRange(eplanVersions32Bit);

         List<EplanData> eplanVersions64Bit = new List<EplanData>();
         new EplanFinder().GetInstalledEplanVersions(ref eplanVersions64Bit, true);
         eplanVersions.AddRange(eplanVersions64Bit);

         eplanVersions = new List<EplanData>(eplanVersions.OrderBy(obj => obj.EplanVersion));

         EplanData eplanData = eplanVersions.LastOrDefault();
         return Path.GetDirectoryName(eplanData.EplanPath);
      }
   }
}
