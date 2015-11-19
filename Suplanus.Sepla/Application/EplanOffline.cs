using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Eplan.EplApi.Starter;
using Eplan.EplApi.System;

namespace Suplanus.Sepla.Application
{
    public class EplanOffline
    {
        public EplApplication Application;

        public bool IsRunning
        {
            get { return Application != null; }
        }

        public void Start(IntPtr handle, string binPath)
        {
            if (Application == null)
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

        public void Close()
        {
            if (Application != null)
            {
                Application.Exit();
                Application = null;
            }
        }
    }
}
