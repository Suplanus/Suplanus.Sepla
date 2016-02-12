using System;
using System.Collections.Generic;
using Suplanus.Sepla.Application;
using Suplanus.Sepla.Helper;
using Suplanus.Sepla.Objects;

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

			List<GeneratablePageMacro> generatablePageMacros = new List<GeneratablePageMacro>();
			generatablePageMacros.Add(new GeneratablePageMacro(
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Axis-X.emp",
				new LocationIdentifierIdentifier
				{
					FunctionAssignment = "TEST1",
					Plant = "TEST11",
					PlaceOfInstallation = "TEST111",
					Location = "TEST1111",
					UserDefinied = "TEST11111",
				}));

			generatablePageMacros.Add(new GeneratablePageMacro(
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Supply.emp",
				new LocationIdentifierIdentifier
				{
					FunctionAssignment = "TEST2",
					Plant = "TEST22",
					PlaceOfInstallation = "TEST222",
					Location = "TEST2222",
					UserDefinied = "TEST22222",
				}));

			// check overwrite
			generatablePageMacros.Add(new GeneratablePageMacro(
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Axis-X.emp",
				new LocationIdentifierIdentifier
				{
					FunctionAssignment = "TEST1",
					Plant = "TEST11",
					PlaceOfInstallation = "TEST111",
					Location = "TEST1111",
					UserDefinied = "TEST11111",
				}));

			generatablePageMacros.Add(new GeneratablePageMacro(
				@"\\Mac\Home\Documents\GitHub\ibKastl.MechatronicsConfigurator\DemoData\Macros\PageMacro_Supply.emp",
				new LocationIdentifierIdentifier
				{
					FunctionAssignment = "TEST2",
					Plant = "TEST22",
					PlaceOfInstallation = "TEST222",
					Location = "TEST2222",
					UserDefinied = "TEST22222",
				}));


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

			ProjectUtility.Generate(projectLinkPath, projectTemplatePath, generatablePageMacros);

			// Close EPLAN
			Console.WriteLine("Closing EPLAN...");
			eplanOffline.Close();

			// Finish
			Console.WriteLine("Finished :^) ");
			Console.ReadKey();
		}
	}
}
