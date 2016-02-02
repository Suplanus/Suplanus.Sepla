using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Suplanus.Sepla.Application;
using Suplanus.Sepla.Gui;

namespace Preview
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private EplanOffline _eplanOffline;
		private readonly string _macroPath = Path.Combine(AssemblyDirectory, "Demodata", "WindowMacro.ema");
		private readonly string _previewProject = Path.Combine(AssemblyDirectory, "Demodata", "Template.elk");

		public MainWindow()
		{
			InitializeComponent();

			Init();
		}

		private void Init()
		{
			// check files	
			if (!File.Exists(_previewProject))
			{
				throw new Exception("Preview project not found");
			}
			if (!File.Exists(_macroPath))
			{
				throw new Exception("Macro not found");
			}

			// start eplan
			_eplanOffline = new EplanOffline();
			_eplanOffline.StartWpf(this);
			if (!_eplanOffline.IsRunning)
			{
				throw new Exception("EPLAN not running");
			}

			// setup preview
			_eplanOffline.Preview = new Suplanus.Sepla.Gui.Preview(previewBorder, _previewProject);

			// first display
			_eplanOffline.Preview.Display(_macroPath, PreviewType.WindowMacro);
		}


		public static string AssemblyDirectory
		{
			get { return AppDomain.CurrentDomain.BaseDirectory; }
		}


		private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			_eplanOffline.Preview.Display(_macroPath, PreviewType.WindowMacro);
		}
	}
}
