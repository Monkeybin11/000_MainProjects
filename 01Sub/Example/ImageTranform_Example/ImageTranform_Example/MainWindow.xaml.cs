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
    using System.Drawing.Imaging;
    using System.Diagnostics;
	using LargeSizeImage_Transformation;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        double RatioW;
        double RatioH;

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

                //int[,] ResultArray = rotateGeneral(data , w ,h ,angle  ,false);

                /// Start To Bitmap
                //var bmpsource = ResultArray.ToBitmap().ToImageSource();
                //var bmpsource = ResultArray.ToBitmap_SetPixel().ToImageSource();

                //imgRotated.Source = bmpsource;
            }
        }

        private void btnxyaffineStart_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var img = new Img(ofd.FileName);

                var w = img.Width;
                var h = img.Height;


                // -- Set Ratio
                RatioW = w / cvsOriginal.Width;
                RatioH = h / cvsOriginal.Height;


                // --


                var data = img.Data.TointArray();

                // ------------
                imgOriginal.Source = new BitmapImage(new Uri(ofd.FileName));



                // ------------

                var p1 = new Point(132,103);
                var p2 = new Point(100,101);

                var srcPos = new AffinePos(
                    178,1902,
                    132,103,
                    933,57,
                    0,0
                    );

                var trgPos = new AffinePos(
                    100, 1900,
                    100, 101,
                    1901, 101,
                    0,0
                    );




                int[,] ResultArray = xytransform(data , srcPos, trgPos); 

                /// Start To Bitmap
                //var bmpsource = ResultArray.ToBitmap().ToImageSource();
                var bmpsource = ResultArray.ToBitmap_SetPixel().ToImageSource();

                imgRotated.Source = bmpsource;


                // SAve
                string path = @"C:\Data\Rotated.jpg";
                ExtLib.SaveImg(path, bmpsource);

            }
        }

        private void btnWithPointStart_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var img = new Img(ofd.FileName);

                var w = img.Width;
                var h = img.Height;


                // -- Set Ratio
                RatioW = w / cvsOriginal.ActualWidth;      
                RatioH = h / cvsOriginal.ActualHeight;
                // --

                var data = img.Data.TointArray();

                // ------------
                imgOriginal.Source = new BitmapImage(new Uri(ofd.FileName));
                // ------------


                if (LBPos == null || LTPos == null || RTPos == null) return;

                var srcPos = new AffinePos(LBPos, LTPos, RTPos , RBPos);

                var trgPos = srcPos.GetCentered(new Point(w/2,h/2) );


                var poslist = new PointD[4] { LBPos, LTPos, RTPos, RBPos };

                var afindata = new AffinePos(LBPos, LTPos, RTPos, RBPos);

                var recipe = afindata.ToTrnsData(w, h);


                int[,] ResultArray = rotateGeneral(data, recipe.W, recipe.H ,recipe.Angle , true );

                if (ResultArray == null) return;
                /// Start To Bitmap
                //var bmpsource = ResultArray.ToBitmap().ToImageSource();
                var bmpsource = ResultArray.ToBitmap_SetPixel().ToImageSource();

                imgRotated.Source = bmpsource;


                // SAve
                string path = @"C:\Data\Rotated.jpg";
                ExtLib.SaveImg(path, bmpsource);
            }
        }
		
        private void btnTestDll_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    imgOriginal.Source = new BitmapImage(new Uri(ofd.FileName));
                    var img = new Img(ofd.FileName);
                    Mat img2 = CvInvoke.Imread(ofd.FileName, Emgu.CV.CvEnum.ImreadModes.Grayscale);
                    var w = img.Width;
                    var h = img.Height;


                    // -- Set Ratio
                    RatioW = w / cvsOriginal.ActualWidth;
                    RatioH = h / cvsOriginal.ActualHeight;
                    // --



                    var data = img.Data.TointArray();
                    var dataW = img.Data.GetLength(1);
                    var dataH = img.Data.GetLength(0);

                    var strideW = dataW % 4;
                    var strideH = dataH % 4;

					img = img.Resize( dataW + ( strideW == 0 ? 0 : 4 - strideW ), dataH + ( strideH == 0? 0 : 4 - strideH ) , Inter.Cubic);

					data = img.Data.TointArray();
					dataW = img.Data.GetLength( 1 );
					dataH = img.Data.GetLength( 0 );

					strideW = dataW % 4;
					strideH = dataH % 4;




					if ( dataW != dataH || strideW != 0 || strideH != 0)
                    {
                        System.Windows.Forms.MessageBox.Show("w != h");
                        return;
                    }

                    if (LBPos == null || LTPos == null || RTPos == null) return;
                    var poslist = new PointD[4] { LBPos, LTPos, RTPos, RBPos };

                    var srcdata = img.Data.Flatten();
                    Tuple<byte[], int, int> dataXY;
                 
                    Bitmap resimgs; ;



                    var ltpoint = poslist.Select(x => new LargeSizeImage_Transformation.PointD(x.X, x.Y)).ToArray();

                    byte[][] temprers;

                    using (MemoryStream mstream = new MemoryStream(srcdata))
                    {
                        //dataXY = Run(mstream, w, h, poslist);


                        Stopwatch stw = new Stopwatch();
                        stw.Start();
                        temprers = LargeSizeImage_Transformation.Tranform.Run( mstream, dataW, dataH, ltpoint );

                        Console.WriteLine(stw.ElapsedMilliseconds / 1000.0);
                     

                    }
                    Stopwatch stw2 = new Stopwatch();
                    var temp1 = temprers.ToMat();
                    var temp2 = temp1.ToBitmap_SetPixel2();

                    imgRotated.Source = temp2.ToImageSource();
                    Console.WriteLine(stw2.ElapsedMilliseconds / 1000.0);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

		private void btnTestOnlyAngle_Click( object sender, RoutedEventArgs e )
		{

		}

		private void btnTestOnlyXY_Click( object sender, RoutedEventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
			{
				try
				{
					imgOriginal.Source = new BitmapImage( new Uri( ofd.FileName ) );
					var img = new Img(ofd.FileName);
					Mat img2 = CvInvoke.Imread(ofd.FileName, Emgu.CV.CvEnum.ImreadModes.Grayscale);
					var w = img.Width;
					var h = img.Height;


					// -- Set Ratio
					RatioW = w / cvsOriginal.ActualWidth;
					RatioH = h / cvsOriginal.ActualHeight;
					// --



					var data = img.Data.TointArray();
					var dataW = img.Data.GetLength(1);
					var dataH = img.Data.GetLength(0);

					var strideW = dataW % 4;
					var strideH = dataH % 4;

					img = img.Resize( dataW + ( strideW == 0 ? 0 : 4 - strideW ), dataH + ( strideH == 0 ? 0 : 4 - strideH ), Inter.Cubic );

					data = img.Data.TointArray();
					dataW = img.Data.GetLength( 1 );
					dataH = img.Data.GetLength( 0 );

					strideW = dataW % 4;
					strideH = dataH % 4;




					if ( dataW != dataH || strideW != 0 || strideH != 0 )
					{
						System.Windows.Forms.MessageBox.Show( "w != h" );
						//return;
					}

					if ( LBPos == null || LTPos == null || RTPos == null ) return;
					var poslist = new PointD[4] { LBPos, LTPos, RTPos, RBPos };

					var srcdata = img.Data.Flatten();
					Tuple<byte[], int, int> dataXY;

					Bitmap resimgs; ;



					var ltpoint = poslist.Select(x => new LargeSizeImage_Transformation.PointD(x.X, x.Y)).ToArray();

					byte[][] temprers;

					using ( MemoryStream mstream = new MemoryStream( srcdata ) )
					{
						//dataXY = Run(mstream, w, h, poslist);


						Stopwatch stw = new Stopwatch();
						stw.Start();
						temprers = LargeSizeImage_Transformation.Tranform.Run( mstream, dataW, dataH, ltpoint , true );

						Console.WriteLine( stw.ElapsedMilliseconds / 1000.0 );


					}
					Stopwatch stw2 = new Stopwatch();
					var temp1 = temprers.ToMat();
					var temp2 = temp1.ToBitmap_SetPixel2();

					imgRotated.Source = temp2.ToImageSource();
					Console.WriteLine( stw2.ElapsedMilliseconds / 1000.0 );

				}
				catch ( Exception ex )
				{
					Console.WriteLine( ex.ToString() );
				}
			}
		}


		private byte[] GetBytesFromImage(String imageFile)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(imageFile);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }

        public Bitmap CopyDataToBitmap(byte[] data, int w, int h)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            //Return the bitmap 
            return bmp;
        }


        private void cvsOriginal_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        PointD LBPos;
        PointD LTPos;
        PointD RTPos;
        PointD RBPos;

        System.Windows.Shapes.Ellipse LBdot;
        System.Windows.Shapes.Ellipse LTdot;
        System.Windows.Shapes.Ellipse RTdot;
        System.Windows.Shapes.Ellipse RBdot;

        private void cvsOriginal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                cvsOriginal.Children.Remove(LBdot);
                var pos = e.GetPosition(sender as Canvas);

                LBPos = new PointD( (pos.X*RatioW) , (pos.Y*RatioH)  );

                LBdot = new System.Windows.Shapes.Ellipse();
                LBdot.Width = 3;
                LBdot.Height = 3;
                LBdot.Stroke = System.Windows.Media.Brushes.OrangeRed ;
                Canvas.SetLeft(LBdot, pos.X);
                Canvas.SetTop(LBdot, pos.Y);
                cvsOriginal.Children.Add(LBdot);

            }
            else if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                cvsOriginal.Children.Remove(LTdot);
                var pos = e.GetPosition(sender as Canvas);

                LTPos = new PointD((pos.X * RatioW), (pos.Y * RatioH));

                LTdot = new System.Windows.Shapes.Ellipse();
                LTdot.Width = 3;
                LTdot.Height = 3;
                LTdot.Stroke = System.Windows.Media.Brushes.OrangeRed;
                Canvas.SetLeft(LTdot, pos.X);
                Canvas.SetTop(LTdot, pos.Y);
                cvsOriginal.Children.Add(LTdot);
            }
            else if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                cvsOriginal.Children.Remove(RTdot);
                var pos = e.GetPosition(sender as Canvas);

                RTPos = new PointD((pos.X * RatioW), (pos.Y * RatioH));

                RTdot = new System.Windows.Shapes.Ellipse();
                RTdot.Width = 3;
                RTdot.Height = 3;
                RTdot.Stroke = System.Windows.Media.Brushes.OrangeRed;
                Canvas.SetLeft(RTdot, pos.X);
                Canvas.SetTop(RTdot, pos.Y);
                cvsOriginal.Children.Add(RTdot);
            }
            else if (Keyboard.IsKeyDown(Key.Tab))
            {   
                cvsOriginal.Children.Remove(RBdot);
                var pos = e.GetPosition(sender as Canvas);

                RBPos = new PointD((pos.X * RatioW), (pos.Y * RatioH));

                RBdot = new System.Windows.Shapes.Ellipse();
                RBdot.Width = 3;
                RBdot.Height = 3;
                RBdot.Stroke = System.Windows.Media.Brushes.OrangeRed;
                Canvas.SetLeft(RBdot, pos.X);
                Canvas.SetTop(RBdot, pos.Y);
                cvsOriginal.Children.Add(RBdot);
            }



        }

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog fd = new SaveFileDialog();
			if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				var path = fd.FileName;

				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add( BitmapFrame.Create( (BitmapSource)imgRotated.Source ) );
				using (FileStream stream = new FileStream( path, FileMode.Create ))
					encoder.Save( stream );

			}
		}
	}

	public static class ExtLib
    {
        public static void SaveImg(string path, BitmapSource bmp)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            using (FileStream file = File.OpenWrite(path))
            {
                encoder.Save(file);
            }

        }


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
                bitmapImage.BeginInit() ;
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static Bitmap ToBitmap_SetPixel2(this byte[,] src)
        {
            int h = src.GetLength(0);
            int w = src.GetLength(1);

            Bitmap img = new Bitmap(w, h);

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    Color clr = new Color();
                    img.SetPixel(i, j, Color.FromArgb(src[j, i], src[j, i], src[j, i]));
                }
            }
            return img;
        }

    }

}
