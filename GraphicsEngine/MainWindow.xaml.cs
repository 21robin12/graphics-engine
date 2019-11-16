using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GraphicsEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // https://stackoverflow.com/questions/8713864/high-performance-graphics-using-the-wpf-visual-layer/8714107#8714107
        // https://www.i-programmer.info/programming/wpf-workings/527-writeablebitmap.html
        // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.imaging.writeablebitmap?view=netframework-4.8
        static WriteableBitmap writeableBitmap;

        public MainWindow()
        {
            InitializeComponent();

            writeableBitmap = new WriteableBitmap(
                300,
                300,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            var image = new Image
            {
                Source = writeableBitmap,
                Stretch = Stretch.None,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            Content = image;

            //var grid = FindName("grid") as Grid;
            //if (grid == null)
            //    throw new Exception("Main grid not found");

            //grid.Children.Add(image);

            //window.MouseWheel += new MouseWheelEventHandler(w_MouseWheel);

            //Application app = new Application();

            DrawPixel(100, 100);
            DrawPixel(100, 101);
            DrawPixel(100, 102);
            DrawPixel(100, 103);
            DrawPixel(100, 104);
            DrawPixel(100, 105);

            //app.Run();
        }

        static void DrawPixel(int x, int y)
        {

            try
            {
                // Reserve the back buffer for updates.
                writeableBitmap.Lock();

                unsafe
                {
                    // Get a pointer to the back buffer.
                    var pBackBuffer = writeableBitmap.BackBuffer;

                    // Find the address of the pixel to draw.
                    pBackBuffer += y * writeableBitmap.BackBufferStride;
                    pBackBuffer += x * 4;

                    // Compute the pixel's color.
                    int color_data = 255 << 16; // R
                    color_data |= 128 << 8;   // G
                    color_data |= 255 << 0;   // B

                    // Assign the color data to the pixel.
                    *((int*)pBackBuffer) = color_data;
                }

                // Specify the area of the bitmap that changed.
                writeableBitmap.AddDirtyRect(new Int32Rect(x, y, 1, 1));
            }
            finally
            {
                // Release the back buffer and make it available for display.
                writeableBitmap.Unlock();
            }
        }
    }
}
