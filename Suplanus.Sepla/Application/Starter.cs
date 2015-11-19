using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Eplan.EplApi.Starter;

namespace Suplanus.Sepla.Application
{
    /// <summary>
    /// EPLAN Starter Helper: No other EPLAN-Namespaces are allowd
    /// </summary>
    public class Starter
    {
        public static string GetBinPath()
        {
            List<EplanData> eplanVersions = new List<EplanData>();

            List<EplanData> eplanVersions32bit = new List<EplanData>();
            new EplanFinder().GetInstalledEplanVersions(ref eplanVersions32bit);
            eplanVersions.AddRange(eplanVersions32bit);

            List<EplanData> eplanVersions64bit = new List<EplanData>();
            new EplanFinder().GetInstalledEplanVersions(ref eplanVersions64bit, true);
            eplanVersions.AddRange(eplanVersions64bit);

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
