using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if SIGN
using Eplan.EplApi.Starter;
[assembly: EplanSignedAssembly(true)]
[assembly: AssemblyKeyFile(@"G:\ibKastl.CodingStandards\Keyfiles\2161_Public.snk")]       
[assembly: AssemblyDelaySign(true)]
#endif

[assembly: AssemblyTitle("Suplanus.Sepla")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Suplanus.Sepla")]
[assembly: AssemblyCopyright("Copyright ? Suplanus 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("a3ac82b1-6ea9-4127-80c8-0e09c49b1cc0")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
