using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Eplan.EplApi.System;

namespace Suplanus.Sepla.Application
{
	public class EplanOffline
	{
		public EplApplication Application;
	   public string BinPath;

		public EplanOffline(string binPath)
		{
		   BinPath = binPath;
			AppDomain.CurrentDomain.AssemblyResolve += EplanAssemblyResolver;
		}

		public Assembly EplanAssemblyResolver(object sender, ResolveEventArgs args)
		{
        BinPath = BinPath.Replace("Electric P8", "Platform"); // dlls in Platform directory

         string[] fields = args.Name.Split(',');
			string name = fields[0];

			// failing to ignore queries for satellite resource assemblies or using [assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)] 
			// in AssemblyInfo.cs will crash the program on non en-US based system cultures.
			// http://stackoverflow.com/questions/4368201/appdomain-currentdomain-assemblyresolve-asking-for-a-appname-resources-assembl
			if (fields.Length > 2)
			{
				string culture = fields[2];
				if (name.EndsWith(".resources") && !culture.EndsWith("neutral"))
				{
					return null;
				}
			}

			var filename = Path.Combine(BinPath, name + ".dll");
			if (!File.Exists(filename))
			{
			   return null;
			}
		   Assembly assembly = Assembly.LoadFile(filename);
			return assembly;
		}

		/// <summary>
		/// Starts EPLAN
		/// </summary>
		public void StartWithoutGui()
		{
			BinPath = Starter.GetBinPathLastVersion();
			Start();
		}

		/// <summary>
		/// Starts EPLAN with the last version of Electric P8 and attach to (WPF) window
		/// </summary>
		/// <param name="window"></param>
		public void StartWpf(Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			Start(handle);
		}

		/// <summary>
		/// Starts EPLAN with the last version of Electric P8 and attach to (WF) form
		/// </summary>
		/// <param name="form"></param>
		public void StartWindowsForms(Form form)
		{
			IntPtr handle = form.Handle;
			Start(handle);
		}

		/// <summary>
		/// Returns if the EPLAN-Application is running
		/// </summary>
		public bool IsRunning
		{
			get
			{
				var running = Application != null;
				return running;
			}
		}

		/// <summary>
		/// Starts the application
		/// </summary>
		/// <param name="handle"></param>
		private void Start(IntPtr handle)
		{
			if (!IsRunning)
			{
				try
				{
					EplApplication eplApplication = new EplApplication();
					eplApplication.EplanBinFolder = BinPath;
					eplApplication.ResetQuietMode();
					eplApplication.SetMainFrame(handle);
					eplApplication.Init("", true, true);
					Application = eplApplication;
				}
				catch
				{
					Application = null;
				}
			}
		}

		/// <summary>
		/// Starts the application without Gui
		/// </summary>
		private void Start()
		{
			if (!IsRunning)
			{
				try
				{
					EplApplication eplApplication = new EplApplication();
					eplApplication.EplanBinFolder = BinPath;
					eplApplication.ResetQuietMode();
					eplApplication.Init("", true, true);
					Application = eplApplication;
				}
				catch
				{
					Application = null;
				}
			}
		}

		/// <summary>
		/// Release all objects
		/// </summary>
		public void Close()
		{
			if (Application != null)
			{
			   try // needed for eplan runtime exceptions
			   {
			      Application.Exit();
			   }
			   finally
			   {
               Application = null;
            }
			}
			
		}
	}
}
