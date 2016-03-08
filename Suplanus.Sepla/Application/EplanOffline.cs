using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Eplan.EplApi.System;
using Suplanus.Sepla.Gui;

namespace Suplanus.Sepla.Application
{
    public class EplanOffline
    {
	    public EplApplication Application;

	    public EplanOffline()
	    {
			AppDomain.CurrentDomain.AssemblyResolve += EplanAssemblyResolver;
		}

		private Assembly EplanAssemblyResolver(object sender, ResolveEventArgs args)
		{
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

			var filename = Path.Combine(@"C:\Program Files\EPLAN\Platform\2.5.4\Bin\", name + ".dll");
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(filename);
			}
			Assembly assembly = Assembly.LoadFile(filename);
			return assembly;
		}

		/// <summary>
		/// Starts EPLAN
		/// </summary>
		public void StartWithoutGui()
		{
			string binPath = Starter.GetBinPathLastVersion();
			Start(binPath);
		}

	    /// <summary>
		/// Starts EPLAN with the last version of Electric P8 and attach to (WPF) window
		/// </summary>
		/// <param name="window"></param>
		public void StartWpf(Window window)
	    {
			IntPtr handle = new WindowInteropHelper(window).Handle;
			//string test = Starter.GetBinPathLastVersion();
		    //Debug.WriteLine(test);
			string binPath = @"C:\Program Files\EPLAN\Electric P8\2.5.4\Bin";
		    Debug.WriteLine(binPath);

			Start(handle,binPath);
		}

		/// <summary>
		/// Starts EPLAN with the last version of Electric P8 and attach to (WF) form
		/// </summary>
		/// <param name="form"></param>
		public void StartWindowsForms(Form form)
		{
			IntPtr handle = form.Handle;
			string binPath = Starter.GetBinPathLastVersion();
			Start(handle, binPath);
		}

		/// <summary>
		/// Starts EPLAN with the given version of program variant and attach to (WPF) window
		/// </summary>
		/// <param name="window"></param>
		/// <param name="binPath"></param>
		public void StartWpf(Window window, string binPath)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			Start(handle, binPath);
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
		/// <param name="binPath"></param>
        private void Start(IntPtr handle, string binPath)
        {
            if (!IsRunning)
            {
                try
                {
                    EplApplication eplApplication = new EplApplication();
                    eplApplication.EplanBinFolder = binPath;
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
		/// <param name="binPath"></param>
		private void Start(string binPath)
		{
			if (!IsRunning)
			{
				try
				{
					EplApplication eplApplication = new EplApplication();
					eplApplication.EplanBinFolder = binPath;
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
            if (IsRunning)
            {
                Application.Exit();
                Application = null;
            }
        }


    }
}
