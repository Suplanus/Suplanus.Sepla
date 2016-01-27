using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using Eplan.EplApi.Starter;
using Eplan.EplApi.System;

namespace Suplanus.Sepla.Application
{
	public class EplanOffline
	{
		public EplApplication Application;

		public bool IsStarted
		{
			get
			{
				return Application != null;
			}
		}

		public EplanOffline(string binPath, IntPtr handle)
		{
			Start(binPath,handle );
		}

		public EplanOffline(string binPath, Window window)
		{
			var handle = new WindowInteropHelper(window).Handle;
			Start(binPath, handle);
		}

		private void Start(string binPath, IntPtr handle)
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
}
