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
      private Sepla.Gui.Preview _preview;


      public MainWindow()
      {
         InitializeComponent();
      }

      public static string AssemblyDirectory
      {
         get { return AppDomain.CurrentDomain.BaseDirectory; }
      }

      private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
      {
         Init();
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
         _eplanOffline = new EplanOffline(@"C:\Program Files\EPLAN\Electric P8\2.5.4\Bin");
         //_eplanOffline.StartWpf(this);
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



      private void MainWindow_OnClosing(object sender, CancelEventArgs e)
      {
         _eplanOffline.Close();
      }


   }
}
