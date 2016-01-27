using System;
using System.Windows;
using System.Windows.Interop;
using Eplan.EplApi.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Suplanus.Sepla.Test.Application
{
    [TestClass]
    class EplanOffline
    {
        [TestMethod]
        public void Wpf()
        {
            // Pin to EPLAN
            var binPath = Sepla.Application.Starter.Setup();

            // Test have no GUI... so we create a test-window
            var window = new Window();
            var handle = new WindowInteropHelper(window).Handle;

            // load DLLs and get the EPLAN Application Object
            var offline = new Sepla.Application.Offline();
            offline.Start(handle, binPath);
            if (offline.Application == null)
            {
                throw new NotImplementedException();
            }

	        //var project = Project.Utility.GetCurrentProject();
	        //if (project == null)
	        //{
		       // throw new NotImplementedException();
	        //}
        }
    }

}
