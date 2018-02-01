using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using SpeedyCoding;

namespace ImageTranform_Example
{
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lib;
    using Img = Image<Gray, byte>;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var img = new Img(ofd.FileName);

                var w = img.Width;
                var h = img.Height;


                var data = img.Data.TointArray();

                // ------------
                imgOriginal.Source = new BitmapImage(new Uri(ofd.FileName));



                // ------------



                double angle = 25;
                double radian;

                int[,] ResultArray = rotateGeneral(data , w ,h ,angle );

                /// Start To Bitmap
                //var bmpsource = ResultArray.ToBitmap().ToImageSource();
                var bmpsource = ResultArray.ToBitmap_SetPixel().ToImageSource();

                imgRotated.Source = bmpsource;
            }
        }
    }

    public static class ExtLib
    {
        public static int[,] TointArray(this byte[,,] src)
        {
            var h = src.GetLength(0);
            var w = src.GetLength(1);

            int[,] output = new int[h, w];

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    output[j, i] = (int)src[j, i, 0];

                }
            }
            return output;
        }

        public static Bitmap ToBitmap(this int[,] src)
        {


            int h = src.GetLength(0);
            int w = src.GetLength(1);

            // 이미지의 어떤부분을 락 할건지 정하는곳. 
            var rect = new System.Drawing.Rectangle(0,0,w,h);

            Bitmap bmp = new Bitmap(w, h , System.Drawing.Imaging.PixelFormat.Format8bppIndexed);


            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(
                    rect,
                    System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    System.Drawing.Imaging.PixelFormat.Format8bppIndexed);


            // 비트맵의 첫번째 주소
            IntPtr ptr = bmpData.Scan0;
            var bytearrayData = src.Flatten().Select(x => (byte)x).ToArray();
            Marshal.Copy(ptr, bytearrayData, 0, bytearrayData.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
            
        }

        public static Bitmap ToBitmap_SetPixel(this int[,] src)
        {

            
            int h = src.GetLength(0);
            int w = src.GetLength(1);
            
            Bitmap img = new Bitmap(w, h );
            
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    Color clr = new Color();
            
                    img.SetPixel(i, j, Color.FromArgb( src[j, i] , src[j, i], src[j, i]));
            
            
                }
            }
            return img;
        }


        public static BitmapImage ToImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }


        }






    }

}
