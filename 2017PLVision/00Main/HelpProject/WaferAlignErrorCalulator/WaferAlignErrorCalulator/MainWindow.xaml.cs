using Emgu.CV;
using Emgu.CV.Structure;
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
//using System.Windows.Shapes;
using System.Windows.Forms;
using SpeedyCoding;
using static SpeedyCoding.SpeedyCoding_AddonFunction;
using EmguCvExtension;
using static EmguCvExtension.Processing;
using System.Threading;
using System.Data;
using System.IO;

namespace WaferAlignErrorCalulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> FileNamesList;
        const string FolderName = "output";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnLoad_Click( object sender , RoutedEventArgs e )
        {
            try
            {

            OpenFileDialog ofd = new OpenFileDialog();
            if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                mainimg.ImageSource = null;
                try
                {
                    var tsk = Task<Tuple<Image<Bgr,byte> , double>>.Run( () =>
                     {
                         return SingleProcessing( new Image<Bgr , byte>( ofd.FileName ) , true);
                     } );

                    var doneresult = tsk.Result;
                    lblerror.Content = doneresult.Item2.ToString("##.####");
                    mainimg.ImageSource = doneresult.Item1.ToBitmapSource();
                }
                catch ( Exception )
                {
                }
            }

            }
            catch ( Exception )
            {
                System.Windows.Forms.MessageBox.Show( "Error" );
            }

        }



        private async void btnAdd_Click( object sender , RoutedEventArgs e )
        {


            var ofd = new FolderBrowserDialog();
            try
            {
                ofd.SelectedPath = txbStartPath.Text ;
            }
            catch ( Exception  er)
            {
                System.Windows.Forms.MessageBox.Show( er.ToString() );
                ofd.SelectedPath = null;
            }


            if ( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                lblStatus.Content = "Working";

                #region Original
                var tsk  = Task.Run<bool>(
                     ()=>
                     {
                         try
                         {
                             FileNamesList = new List<string>();
                             var outputPath = Path.Combine( ofd.SelectedPath  , FolderName );
                             var datetime = DateTime.Now.ToString("MM dd HH_mm_ss  ");
                             var csvPath = Path.Combine(outputPath , datetime +"Result.csv");


                             var imgpathlist = Directory.GetFiles( ofd.SelectedPath ).Where( x => Path.GetExtension(x) == ".jpg").Select( x => x).ToList();
                             var resultList = imgpathlist.Select(
                                                          path => SingleProcessing( new Image<Bgr , byte>( path ) , true)).ToList();

                             var namelist = imgpathlist.Select(x => System.IO.Path.GetFileName(x)).ToList();

                             var NameError = namelist.Zip( resultList , (name , tuple)
                                 => new Result(
                                     name,
                                     tuple.Item2 == 0 ? "0.0000" : tuple.Item2.ToString("##.####"),
                                     tuple.Item3.ToString()))
                                     .ToList()
                                     .Select;  // <- Change abs error => rel error



                             var NameImg   = namelist.Zip( resultList , (name , tuple) => Tuple.Create( Path.Combine(outputPath,"Result_"+name), tuple.Item1 ));


                             CreateFolder( outputPath );
                             System.IO.File.WriteAllText( csvPath , NameError.ToTable().ToString() );

                             NameImg.ActLoop(
                                 x =>
                                 {
                                     x.Item2.Save( x.Item1 );
                                 });

                             return true;
                         }
                         catch (Exception er)
                         {
                             System.Windows.Forms.MessageBox.Show(er.ToString());
                             return false;
                         }
                     } );

                await tsk;
                #endregion

                if ( tsk.Result )
                {
                    lblStatus.Content = "Done";
                }
                else
                {
                    lblStatus.Content = "Error";
                    System.Windows.Forms.MessageBox.Show( "Task is distrubed " );
                }
                Mouse.OverrideCursor = null;
            }
        }

        public Tuple<Image<Bgr , byte> , double , double> SingleProcessing( Image<Bgr , byte> inputImg , bool needimg )
        {
            Image<Bgr,byte> workingImg;
            var rch = inputImg.Data.PickLastCh( 2 );
            workingImg = inputImg.MapChennel(
                b => b.ThresholdBinary( new Gray( 125 ) , new Gray( 255 ) ) ,
                g => g.ThresholdBinary( new Gray( 125 ) , new Gray( 255 ) ) ,
                r => r.ThresholdBinary( new Gray( 125 ) , new Gray( 255 ) )
                );


            // get only red line
            var ori = workingImg.Data;

            for ( int j = 0 ; j < ori.GetLength( 0 ) ; j++ )
            {
                for ( int i = 0 ; i < ori.GetLength( 1 ) ; i++ )
                {
                    var b= ori[j,i,0];
                    var g= ori[j,i,1];
                    var r= ori[j,i,2];
                    if ( b == 0 && g == 0 && r == 255 )
                    {
                    }
                    else
                    {
                        ori [ j , i , 0 ] = 0;
                        ori [ j , i , 1 ] = 0;
                        ori [ j , i , 2 ] = 0;
                    }
                }
            }

            workingImg = new Image<Bgr , byte>( ori );
            // Crop only line area
            var h = ori.GetLength(0);
            var w = ori.GetLength(1);

            var herode = FnMorp(morpOp.Erode , kernal.Horizontal);
            var verode = FnMorp(morpOp.Erode , kernal.Vertical);

            var grayimg = workingImg.Convert<Gray,byte>();
            grayimg = grayimg.SmoothGaussian( 3 );
            grayimg = grayimg.ThresholdBinary(new Gray(20),new Gray(255));
            
            grayimg = herode( grayimg , 3 );
            workingImg = grayimg.Convert<Bgr , byte>();

            //var img = WrkImg.Canny( 200 , 1400 );
            var data = workingImg.HoughLines( 200 , 100 , 2 , Math.PI / 180.0 ,200, 220 ,200);
            var img = new Image<Bgr,byte>(inputImg.Data);




            LineSegment2D avgLine = new LineSegment2D();
            var flatdata =data.Flatten();
         
            var selectedlines = flatdata.Where( x => x.P1.X > 15 && x.P1.Y > 80 && x.P2.X > 100 && x.P2.Y > 80 ).Select(x => x).ToArray();

            if ( selectedlines.Count() == 0 ) return null;
            var result = selectedlines.Aggregate( ( f , s ) =>
            {
                var p1 = new System.Drawing.Point
                (
                    f.P1.X + s.P1.X,
                    f.P1.Y + s.P1.Y
                );

                var p2 = new System.Drawing.Point
                (
                    f.P2.X + s.P2.X,
                    f.P2.Y + s.P2.Y
                );
                return new LineSegment2D(p1,p2);
            } );

            avgLine = new LineSegment2D(
                new System.Drawing.Point
                    (
                    result.P1.X / selectedlines.Count() ,
                    result.P1.Y / selectedlines.Count()
                    ),
                 new System.Drawing.Point
                    (
                    result.P2.X / selectedlines.Count() ,
                    result.P2.Y / selectedlines.Count()
                    )
                );
            
            var selectedline =  avgLine;

            if ( needimg ) img.Draw( selectedline , new Bgr( 100 , 200 , 10 ) , 2 );
            else img = null;

            var xlen = Math.Abs( selectedline.Direction.X );
            var ylen = Math.Abs( selectedline.Direction.Y );
            var angless = xlen <= 0 ? 0 : Math.Atan(ylen / xlen) * 180 / Math.PI; // degree

            return Tuple.Create( img , angless , (double)selectedline.P1.Y);
        }


        public void CreateFolder(string basepath)
        {
            try
            {
                string outputpath = basepath;
                if ( !Directory.Exists( outputpath ) )
                {
                    Directory.CreateDirectory( outputpath );
                }
            }
            catch ( Exception )
            {
                System.Windows.Forms.MessageBox.Show( "Access Violation" );
            }
        }
    }


    public class Result
    {
        const double Resol = 0.1178511302;
        public string Name { get; set; }
        public string Error { get; set; }
        public string Height { get; set; }

        public string RealHeight { get { return ( Resol * Convert.ToDouble( Height ) ).ToString(); } }
        public Result( string name, string err ,string height)
        {
            Name = name;
            Error = err;
            Height = height;
        }
    }

    public static class Extnesion
    {
        public static string ToTable(
            this List<Result> src )
        {
            var strb = new StringBuilder();
            strb.Append( "File Name" );
            strb.Append( "," );
            strb.Append( "Error (degree)" );
            strb.Append( "," );
            strb.Append( "Yaxis Error (pixel)" );
            strb.Append( Environment.NewLine );
            src.ActLoop( x =>
            {
                strb.Append( x.Name );
                strb.Append( "," );
                strb.Append( x.Error );
                strb.Append( "," );
                strb.Append( x.RealHeight );
                strb.Append( Environment.NewLine );
            } );
            return strb.ToString();
        }
    }
}
