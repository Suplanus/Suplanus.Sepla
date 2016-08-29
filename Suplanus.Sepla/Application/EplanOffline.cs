using System;
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
	   public string LicenseFile;

		public EplanOffline(string binPath, string licenseFile = null)
		{
         BinPath = binPath;
		   LicenseFile = licenseFile;
		}

		/// <summary>
		/// Starts EPLAN
		/// </summary>
		public void StartWithoutGui()
		{
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
					eplApplication.QuietMode = EplApplication.QuietModes.ShowAllDialogs;
               eplApplication.SetMainFrame(handle);
				   if (!string.IsNullOrEmpty(LicenseFile))
				   {
				      eplApplication.LicenseFile = LicenseFile; // Set specific licence
				   }
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
               eplApplication.QuietMode = EplApplication.QuietModes.ShowAllDialogs;
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
