
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModelLib.AmplifiedType;
using SpeedyCoding;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace ProcModelGenerator
{
    using static PLProtocol;
    using static Helper;
    using static ModelLib.AmplifiedType.Handler;
    using static ProcModelGenerator.FunLib;
    using Img = Image<Gray, byte>;
    using MImg = AccumulWriter<Image<Gray, byte>>;
    using System.IO;


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsOrigonalImg= false;

        double ZoomMax = 10;
        double ZoomMin = 0.1;
        double ZoomSpeed = 0.001;
        double Zoom = 1;

        Point FirstPos = new Point();
        MImg SrcMImg;

        public MainWindow()
        {
            InitializeComponent();
            InitParam();

            canvas_Zoom.Height = brdimg.ActualHeight;
            canvas_Zoom.Width = brdimg.ActualWidth;

            canvas_Draw.Height = brdimg.ActualHeight;
            canvas_Draw.Width = brdimg.ActualWidth;
        }

        void InitParam()
        {
            nudThreshold.Value = 180;
            nudAdpThreshold.Value = 100;
            nudMedian.Value = 3;
            nudNormalize.Value = 120;
            nudAdpThreshold.Value = 71;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                imgBack.Source = new BitmapImage(new Uri(ofd.FileName));

            
                var offsetH = (imgBack.ActualHeight - brdimg.ActualHeight) / 2.0;
                var offsetW = (imgBack.ActualWidth - brdimg.ActualWidth) / 2.0;

             
                brdimg.ActualHeight.Print("border H") ;
                brdimg.ActualWidth.Print("border W");


                canvas_Zoom.ActualHeight.Print("canvas_Zoom H");
                canvas_Zoom.ActualWidth.Print("canvas_Zoom W");


                canvas_Draw.ActualHeight.Print("canvas_Draw H");
                canvas_Draw.ActualWidth.Print("canvas_Draw W");


                imgBack.ActualHeight.Print("img H");
                imgBack.ActualWidth.Print("img W");




                Canvas.SetLeft(imgBack, offsetW);
                Canvas.SetTop(imgBack, offsetH);
                imgBack.Stretch = Stretch.Fill;

                


                SrcMImg = Accmululatable( new Img(ofd.FileName) , "START", PLImagingWriter);
               
                imgBack.Source = ToBitmapSource(SrcMImg.GetLastValue());
                txbLog.Selection.Text = SrcMImg.GetLastPaper().Paper2TextHistory();

                Canvas.SetLeft(imgBack, offsetW);
                Canvas.SetTop(imgBack, offsetH);

            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(fd.FileName, SrcMImg.GetLastPaper() + "|END");
            }
        }

        private void btnOperation(object sender, RoutedEventArgs e)
        {
            var master = sender as System.Windows.Controls.Button;
            switch (master.Name)
            {
                case("btnThreshold"):
                    var parmTh = (int)nudThreshold.Value;
                    SrcMImg = SrcMImg.Add( Threshold.Apply(parmTh) , StrThreshold.With(parmTh) );
                    break;

                case ("btnAdpThreshold"):
                    var parmAdTh = (int)nudAdpThreshold.Value;
                    SrcMImg = SrcMImg.Add(AdpTHreshold.Apply(parmAdTh), StrAdpTHreashold.With(parmAdTh));
                    break;

                case ("btnMedian"):
                    var parmMdn = (int)nudMedian.Value;
                    SrcMImg = SrcMImg.Add(Median.Apply(parmMdn), StrMedian.With(parmMdn));
                    break;

                case ("btnNormalize"):
                    var parmNorm = (int)nudNormalize.Value;
                    SrcMImg = SrcMImg.Add(Normalize.Apply(parmNorm), StrNormalize.With(parmNorm));
                    break;
            }
            imgBack.Source = ToBitmapSource( SrcMImg.GetLastValue() );
            txbLog.Selection.Text = SrcMImg.GetLastPaper().Paper2TextHistory();
                var temp  = SrcMImg.GetLastPaper().Paper2TextHistory(); 
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (SrcMImg.Count() > 1)
            {
                SrcMImg = SrcMImg.Restore();
                imgBack.Source = ToBitmapSource(SrcMImg.GetLastValue());
                txbLog.Selection.Text = SrcMImg.GetLastPaper().Paper2TextHistory();
            }
        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (IsOrigonalImg)
            {
                IsOrigonalImg = false;
                imgBack.Source = ToBitmapSource( SrcMImg.GetLastValue());
            }
            else
            {
                IsOrigonalImg = true;
                imgBack.Source = ToBitmapSource(SrcMImg.GetFirstValue());
            }
        }

        #region event

        private void imgBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FirstPos = e.GetPosition(this);
            imgBack.CaptureMouse();
        }

        private void imgBack_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgBack.ReleaseMouseCapture();
        }

        private void imgBack_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point temp = e.GetPosition(this);
                Point res = new Point(FirstPos.X - temp.X, FirstPos.Y - temp.Y);
                Canvas.SetLeft(imgBack, Canvas.GetLeft(imgBack) - res.X);
                Canvas.SetTop(imgBack, Canvas.GetTop(imgBack) - res.Y);
                FirstPos = temp;
            }
        }

        private void RegistMovement(Image img)
        {
            img.MouseLeftButtonDown += (ss, ee) =>
            {
                FirstPos = ee.GetPosition(this);
                imgBack.CaptureMouse();
            };

            img.MouseMove += (ss, ee) =>
            {
                if (ee.LeftButton == MouseButtonState.Pressed)
                {
                    Point temp = ee.GetPosition(this);
                    Point res = new Point(FirstPos.X - temp.X, FirstPos.Y - temp.Y);
                    Canvas.SetLeft(imgBack, Canvas.GetLeft(imgBack) - res.X);
                    Canvas.SetTop(imgBack, Canvas.GetTop(imgBack) - res.Y);
                    FirstPos = temp;
                }
            };

            img.MouseUp += (ss, ee) => img.ReleaseMouseCapture();
        }


        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Zoom += ZoomSpeed * e.Delta;

            if (Zoom < ZoomMin)
            {
                Zoom = ZoomMin;
            }

            if (Zoom > ZoomMax)
            {
                Zoom = ZoomMax;
            }

            Point mousePos = e.GetPosition(imgBack);

            if (Zoom > 1)
            {
                imgBack.RenderTransform = new ScaleTransform(Zoom, Zoom, mousePos.X, mousePos.Y);
            }
            else
            {
                imgBack.RenderTransform = new ScaleTransform(Zoom, Zoom);
            }
        }

        #endregion

    }
}
