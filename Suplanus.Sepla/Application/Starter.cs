using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eplan.EplApi.Starter;

namespace Suplanus.Sepla.Application
{
    /// <summary>
    /// EPLAN Starter Helper: No other EPLAN-Namespaces are allowd
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

            eplanVersions = new List<EplanData>(eplanVersions
                .Where(obj => obj.EplanVariant.Equals("Electric P8"))
                .OrderBy(obj => obj.EplanVersion));

            EplanData eplanData = eplanVersions.LastOrDefault();

            var binPath = Path.GetDirectoryName(eplanData.EplanPath);

            AssemblyResolver resolver = new AssemblyResolver();
            resolver.SetEplanBinPath(binPath);
            resolver.PinToEplan();

            return binPath;
        }
    }
}
