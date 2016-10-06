using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eplan.EplApi.RemoteClient;
using Eplan.EplApi.Starter;

namespace Suplanus.Sepla.Application
{
   /// <summary>
   /// EPLAN Starter Helper: No other EPLAN-Namespaces are allowed
   /// </summary>
   public class Starter
   {
      /// <summary>
      /// Returns the bin path of the highest installed EPLAN instance
      /// </summary>
      /// <returns>bin path</returns>
      public static string GetBinPathLastVersion()
      {
         var eplanVersions = GetEplanInstallations();

         EplanData eplanData = eplanVersions.LastOrDefault();
         var binPathPlatform = Path.GetDirectoryName(eplanData.EplanPath);
         return binPathPlatform;
      }

      /// <summary>
      /// Returns all installed EPLAN instances
      /// </summary>
      /// <returns></returns>
      public static List<EplanData> GetEplanInstallations()
      {
         List<EplanData> eplanVersions = new List<EplanData>();

         var eplanFinder = new EplanFinder();

         List<EplanData> eplanVersions32Bit = new List<EplanData>();
         eplanFinder.GetInstalledEplanVersions(ref eplanVersions32Bit);
         eplanVersions.AddRange(eplanVersions32Bit);

         List<EplanData> eplanVersions64Bit = new List<EplanData>();
         eplanFinder.GetInstalledEplanVersions(ref eplanVersions64Bit, true);
         eplanVersions.AddRange(eplanVersions64Bit);

         eplanVersions = new List<EplanData>(eplanVersions.OrderBy(obj => obj.EplanVersion));
         return eplanVersions;
      }


      /// <summary>
      /// Returns all active EPLAN instances
      /// </summary>
      /// <returns></returns>
      public static List<EplanServerData> GetActiveEplanInstallations()
      {
         EplanRemoteClient eplanRemoteClient = new EplanRemoteClient();
         List<EplanServerData> eplanServerDatas = new List<EplanServerData>();
         eplanRemoteClient.GetActiveEplanServersOnLocalMachine(out eplanServerDatas);
         return eplanServerDatas;
      }

      /// <summary>
      /// Pin the Offline Application to EPLAN process
      /// </summary>
      /// <param name="binPath"></param>
      public static void PinToEplan(string binPath)
      {
         AssemblyResolver resolver = new AssemblyResolver();
         resolver.SetEplanBinPath(binPath);
         resolver.PinToEplan();
      }
   }
}
