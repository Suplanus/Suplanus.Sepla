using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if TRACE && !DEBUG
using Eplan.EplApi.Starter;
[assembly: EplanSignedAssembly(true)]
[assembly: AssemblyKeyFile(@"\\Mac\Home\Documents\GitHub\ibKastl.MD3.Source\Build\Keyfiles\2161_Public.snk")]
[assembly: AssemblyDelaySign(true)]
#endif

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Suplanus.Sepla")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Suplanus.Sepla")]
[assembly: AssemblyCopyright("Copyright ?? Suplanus 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a3ac82b1-6ea9-4127-80c8-0e09c49b1cc0")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
