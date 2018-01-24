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

namespace WPF_Zoom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double ZoomMax = 5;
        double ZoomMin = 0.5;
        double ZoomSpeed = 0.001;
        double Zoom = 1;

        Point FirstPos = new Point();

        Image BackImg;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                BackImg = new Image();
                BackImg.Source = new BitmapImage( new Uri( ofd.FileName) );

                Canvas.SetLeft( BackImg, 0 );
                Canvas.SetTop( BackImg, 0 );
                RegistMovement( BackImg );
                canvas_Draw.Children.Add( BackImg );
            }
        }

        private void RegistMovement( Image img )
        {
            img.MouseLeftButtonDown += ( ss, ee ) =>
            {
                FirstPos = ee.GetPosition( this );
                BackImg.CaptureMouse();
            };

            img.MouseMove += ( ss, ee ) =>
            {
                if ( ee.LeftButton == MouseButtonState.Pressed )
                {
                    Point temp = ee.GetPosition(this);
                    Point res = new Point(FirstPos.X - temp.X , FirstPos.Y - temp.Y);
                    Canvas.SetLeft( BackImg, Canvas.GetLeft( BackImg ) - res.X );
                    Canvas.SetTop( BackImg, Canvas.GetTop( BackImg ) - res.Y );
                    FirstPos = temp;
                }
            };

            img.MouseUp += ( ss, ee ) => img.ReleaseMouseCapture();


        }

        private void Canvas_MouseWheel( object sender, MouseWheelEventArgs e )
        {
            Zoom += ZoomSpeed * e.Delta;

            if ( Zoom < ZoomMin )
            {
                Zoom = ZoomMin;
            }

            if ( Zoom > ZoomMax )
            {
                Zoom = ZoomMax;
            }

            Point mousePos = e.GetPosition(canvas_Draw);

            if ( Zoom > 1 )
            {
                canvas_Draw.RenderTransform = new ScaleTransform( Zoom, Zoom, mousePos.X, mousePos.Y );
            }
            else
            {
                canvas_Draw.RenderTransform = new ScaleTransform( Zoom, Zoom );
            }
        }

    }
}
