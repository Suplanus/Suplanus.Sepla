using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;

namespace Suplanus.Sepla.Gui
{
    public class Preview
    {
        public static void Display(object sender, Border border)
        {
            // EPLAN
            DrawingService drawingService = new DrawingService();
            drawingService.DrawConnections = true;
            if (sender is SymbolMacro)
            {
                SymbolMacro macro = (SymbolMacro) sender;
                drawingService.CreateDisplayList(macro);
                Draw(drawingService, border);
                return;
            }
            if (sender is WindowMacro)
            {
                WindowMacro macro = (WindowMacro)sender;
                drawingService.CreateDisplayList(macro);
                Draw(drawingService, border);
                return;
            }
            if (sender is PageMacro)
            {
                PageMacro macro = (PageMacro)sender;
                drawingService.CreateDisplayList(macro.Pages);
                Draw(drawingService, border);
                return;
            }

            // Siemens

            // Codesys

            throw new NotImplementedException();

        }

        private static void Draw(DrawingService drawingService, Border border)
        {
            int width = Convert.ToInt16(border.ActualWidth);
            int height = Convert.ToInt16(border.ActualHeight);

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(0, 0, width, height);
            PaintEventArgs paintEventArgs = new PaintEventArgs(g, r);

            drawingService.DrawDisplayList(paintEventArgs);

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            border.Background = new ImageBrush(bitmapSource);

            drawingService.Dispose();
        }
    }
}
