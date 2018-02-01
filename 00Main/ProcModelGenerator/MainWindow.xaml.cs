
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


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsOrigonalImg= false;

        double ZoomMax = 5;
        double ZoomMin = 0.5;
        double ZoomSpeed = 0.001;
        double Zoom = 1;

        Point FirstPos = new Point();
        MImg SrcMImg;
        

        //Image imgBack;


        public MainWindow()
        {
            InitializeComponent();
          
            //Canvas.SetLeft(imgBack, 1);
            //Canvas.SetTop(imgBack, 1);
        }

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

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //imgBack = new Image();

                imgBack.Source = new BitmapImage(new Uri(ofd.FileName));
                //RegistMovement(imgBack);
                Canvas.SetLeft(imgBack, 1);
                Canvas.SetTop(imgBack, 1);
                imgBack.Stretch = Stretch.Fill;

                SrcMImg = Accmululatable( new Img(ofd.FileName) , "START", PLImagingWriter);

                //
                //BackImg.Height = canvas_Draw.ActualHeight;
                //BackImg.Width = canvas_Draw.ActualWidth;
                //
                //
                //canvas_Draw.Children.Clear();
                //canvas_Draw.Children.Add(imgBack);
                //dockimage.Children.Add(BackImg);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOperation(object sender, RoutedEventArgs e)
        {
            var master = sender as System.Windows.Controls.Button;
            switch (master.Name)
            {
                case("btnThreshold"):
                    var parmTh = (int)nudThreshold.Value;
                    SrcMImg = SrcMImg.Bind( Threshold.Apply(parmTh) , StrThreshold.With(parmTh) );
                    break;

                case ("btnAdpThreshold"):
                    var parmAdTh = (int)nudAdpThreshold.Value;
                    SrcMImg = SrcMImg.Bind(AdpTHreshold.Apply(parmAdTh), StrAdpTHreashold.With(parmAdTh));
                    break;

                case ("btnMedian"):
                    var parmMdn = (int)nudMedian.Value;
                    SrcMImg = SrcMImg.Bind(Median.Apply(parmMdn), StrMedian.With(parmMdn));
                    break;

                case ("btnNormalize"):
                    var parmNorm = (int)nudNormalize.Value;
                    SrcMImg = SrcMImg.Bind(Normalize.Apply(parmNorm), StrNormalize.With(parmNorm));
                    break;
            }
            imgBack.Source = ToBitmapSource( SrcMImg.GetLastValue() );
            txbLog.Text = SrcMImg.GetLastPaper();
            // 여기에 함수별로 동작을 만들어 준다. => 그리고 back 구현 , 최종적으로 레시피 제작 구현. 
            // 레시피 제작 구현후 통신으로 되는것을 테스트 한다. 
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            SrcMImg = SrcMImg.Restore();
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
    }
}
