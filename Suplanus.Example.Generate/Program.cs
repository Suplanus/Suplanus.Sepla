using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Suplanus.Sepla.Application;
using Suplanus.Sepla.Helper;

namespace Suplanus.Example.Generate
{
	public class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// Demo data
			Console.WriteLine("Get Demodata...");
			string projectLinkPath = @"\\Mac\Home\Desktop\Test.elk";
			string projectTemplatePath = @"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Templates\IEC_bas001.zw9";

			List<string> pageMacros = new List<string>
			{
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Axis-X.emp",
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Supply.emp",
				// check overwrite
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Axis-X.emp",
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Supply.emp",
			};

			// Start EPLAN
			Console.WriteLine("Start EPLAN...");
			var eplanOffline = new EplanOffline();
			eplanOffline.StartWithoutGui();
			if (!eplanOffline.IsRunning)
			{
				throw new Exception("EPLAN not running");
			}

			// Generate
			Console.WriteLine("Generate...");
			ProjectUtility.Generate(projectLinkPath, projectTemplatePath, pageMacros);

			// Close EPLAN
			Console.WriteLine("Closing EPLAN...");
			eplanOffline.Close();

			// Finish
			Console.WriteLine("Finished :^) ");
			Console.ReadKey();
		}
	}
}
