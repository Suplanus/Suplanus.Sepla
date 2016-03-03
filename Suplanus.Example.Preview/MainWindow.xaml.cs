using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using Suplanus.Sepla.Application;
using Suplanus.Sepla.Gui;

namespace Suplanus.Example.Preview
{
	public partial class MainWindow
	{
		// ReSharper disable once AssignNullToNotNullAttribute
		// DemoData should exist
		static readonly string DemoData = Path.Combine(
			Path.GetDirectoryName(Path.GetDirectoryName(
				Path.GetDirectoryName(Path.GetDirectoryName(AssemblyDirectory)))), "Demodata");

		readonly string _macroPath = Path.Combine(DemoData, "PageMacro.emp");
		readonly string _previewProject = Path.Combine(DemoData, "Template.elk");

		private EplanOffline _eplanOffline;
		private Suplanus.Sepla.Gui.Preview _preview;


		public MainWindow()
		{
			InitializeComponent();

			Init();
		}

		public static string AssemblyDirectory
		{
			get { return AppDomain.CurrentDomain.BaseDirectory; }
		}

		private void Init()
		{
			// check files	
			if (!File.Exists(_previewProject))
			{
				throw new Exception("Preview project not found:" + Environment.NewLine + _previewProject);
			}
			if (!File.Exists(_macroPath))
			{
				throw new Exception("Macro not found:" + Environment.NewLine + _macroPath);
			}

			// start eplan
			AppDomain.CurrentDomain.AssemblyResolve += EplanAssemblyResolver;
			_eplanOffline = new EplanOffline();
			_eplanOffline.StartWpf(this);
			if (!_eplanOffline.IsRunning)
			{
				throw new Exception("EPLAN not running");
			}

			// setup preview
			_preview = new Sepla.Gui.Preview(previewBorder, _previewProject);

			// display
			_preview.Display(_macroPath, PreviewType.PageMacro);
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

		private void MainWindow_OnClosing(object sender, CancelEventArgs e)
		{
			_eplanOffline.Close();
		}
	}
}
