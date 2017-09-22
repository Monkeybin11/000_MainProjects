using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeedyCoding;
using Emgu.CV.Structure;
using Emgu.CV;
using System.IO;
using Emgu.CV.CvEnum;

namespace VisionTestTool
{
    public partial class Form1 : Form
    {
        public BaseData Data;
        public Form1()
        {
            InitializeComponent();
            Data = new BaseData( true );
        }

        private void btnConvColor_Click( object sender , EventArgs e )
        {
            var blue = rtbblue.Text.Split('\n')
                                .Select( f => f.Split(',')
                                                .Select( s => int.Parse(s) )
                                                .ToArray())
                                .ToArray()
                                .ToMat();

            var green = rtbblue.Text.Split('\n')
                                .Select( f => f.Split(',')
                                                .Select( s => int.Parse(s) )
                                                .ToArray())
                                .ToArray()
                                .ToMat();

            var red = rtbblue.Text.Split('\n')
                                .Select( f => f.Split(',')
                                                .Select( s => int.Parse(s) )
                                                .ToArray())
                                .ToArray()
                                .ToMat();
        }

        private void btnThresColor_Click( object sender , EventArgs e )
        {
            var bgr = rtbThresColor.Text.Split(',')
                                    .Select(f => int.Parse(f) ).ToArray();


        }

        private void btnNormalizeColor_Click( object sender , EventArgs e )
        {
            var bgr = rtbNormalizeColor.Text.Split(',')
                                    .Select(f => int.Parse(f) ).ToArray();


        }

        private void btnGammaColor_Click( object sender , EventArgs e )
        {
            var bgr = rtbGammaColor.Text.Split(',')
                                    .Select( x => int.Parse(x) ).ToArray();

            var bdata = Data.WorkingImgColor.Data.PickChenel(0);
            var gdata = Data.WorkingImgColor.Data.PickChenel(1);
            var rdata = Data.WorkingImgColor.Data.PickChenel(2);

            var f = bdata.GetLength(0);
            var s = bdata.GetLength(1);

            var bimg = new Image<Gray, byte>(bdata);
            var gimg = new Image<Gray, byte>(gdata);
            var rimg = new Image<Gray, byte>(rdata);

            string pathbase = @"C:\000_ProjectCode\01Sub\VisionTestTool\VisionTestTool\test";
            bimg.Save( pathbase + "\\bimg.bmp" );
            gimg.Save( pathbase + "\\gimg.bmp" );
            rimg.Save( pathbase + "\\rimg.bmp" );

            var grimg = gimg + rimg/3;
            grimg.Save( pathbase + "\\grimg2.bmp" );

            bimg._GammaCorrect( bgr [ 0 ] );
            gimg._GammaCorrect( bgr [ 1 ] );
            rimg._GammaCorrect( bgr [ 2 ] );

            var bgrdata = Data.WorkingImgColor.Data;

            bgrdata = bimg.Data.GraytoBgr( bgrdata , 0 );
            bgrdata = bgrdata.GraytoBgr( bgrdata , 1 );
            bgrdata = bgrdata.GraytoBgr( bgrdata , 2 );

            Data.WorkingImgColor = new Image<Bgr , byte>( bgrdata );


            rtxLog.AppendText( "btnGammaColor_Click  " + Environment.NewLine );

        }


        void RegistHisroty( Image<Bgr , byte> img , bool display = true )
        {
            History();
            if ( display ) imgBox.Image = img;
        }

        void History()
        {
            if ( Data.WorkingImgColor != null )
            {
                if ( Data.HistoryImgBgr.Count < 20 )
                {
                    Data.HistoryImgBgr.Add( Data.WorkingImgColor.Clone() );
                }
                else
                {
                    Data.HistoryImgBgr.RemoveAt( 0 );
                    History();
                }
            }
        }

        private void btnBack_Click( object sender , EventArgs e )
        {
            Back();
        }

        private void btnReset_Click( object sender , EventArgs e )
        {
            rtxLog.AppendText( "Reset" + Environment.NewLine );
            Data.WorkingImgColor = Data.RootColor.Clone();
            Data.HistoryImgBgr.Clear();
            RegistHisroty( Data.WorkingImgColor );
        }

        void Back()
        {
            if ( Data.HistoryImgBgr.Count > 1 )
            {
                Data.WorkingImgColor = Data.HistoryImgBgr [ Data.HistoryImgBgr.Count - 2 ].Clone();
                Data.HistoryImgBgr.RemoveAt( Data.HistoryImgBgr.Count - 1 );
                imgBox.Image = Data.WorkingImgColor;

                var num = rtxLog.Lines.Length - 2;
                rtxLog.Lines = rtxLog.Lines.Take( num ).ToArray();
                rtxLog.AppendText( Environment.NewLine );

            }
            else if ( Data.HistoryImgBgr.Count == 1 )
            {
                Data.WorkingImgColor = Data.HistoryImgBgr [ 0 ].Clone();
                Data.HistoryImgBgr.RemoveAt( 0 );
                imgBox.Image = Data.WorkingImgColor;
                rtxLog.AppendText( Environment.NewLine );
            }
        }

        void Reset()
        {
            rtxLog.Clear();
            Data.RootColor = null;
            Data.WorkingImgColor = null;
            Data.HistoryImgBgr.Clear();
        }

        string basepath;
        private void btnLoadColor_Click( object sender , EventArgs e )
        {
           
            OpenFileDialog ofd = new OpenFileDialog();
            if ( ofd.ShowDialog() == DialogResult.OK )
            {
                Reset();

                Data.RootColor = new Image<Bgr , byte>( ofd.FileName );
                Data.WorkingImgColor = new Image<Bgr , byte>( ofd.FileName );
                var templatepath = System.IO.Path.GetDirectoryName(ofd.FileName);

                rtxLog.AppendText( "Load" + Environment.NewLine );

                imgBox.Image = Data.WorkingImgColor;
                History();
                basepath = System.IO.Path.GetFullPath( ofd.FileName );
            }
        }





        private void btnAllResize_Click( object sender , EventArgs e )
        {
            foreach ( var path in Data.GroupModifyPath )
            {
                var reName = Path.GetFullPath( path ).Replace('\r',' ').Split('.')[0].Split('\n')[0] + "_Resized.bmp";
                var img = new  Image<Gray,byte>(path);
                var reimg = img.Resize((double)(1/nudScale.Value), Inter.Cubic );
                reimg.Save( reName );
                
            }
        }

        private void btnAdd2List_Click( object sender , EventArgs e )
        {
            OpenFileDialog of = new OpenFileDialog();
            if ( of.ShowDialog() == DialogResult.OK )
            {
                Data.GroupModifyPath.Add( of.FileName );
                lblstatus.Text = of.FileName;
            }
        }

        private void btnClearAllList_Click( object sender , EventArgs e )
        {
            Data.GroupModifyPath.Clear();
        }

        private void btnAllHStack_Click( object sender , EventArgs e )
        {
            var reimg = Data.GroupModifyPath.Select( fir => new Image<Gray , byte>( fir ) ).Aggregate( ( fir , sec ) => fir.ConcateHorizontal( sec ) );
            var reName = Path.GetFullPath( Data.GroupModifyPath[0] ).Replace('\r',' ').Split('.')[0].Split('\n')[0] + "_Stacked.bmp";
            reimg.Save( reName );
        }

        private void btnNormalizeAll_Click( object sender , EventArgs e )
        {
            var level = (double)nudNormalizeLevel.Value;

            foreach ( var path in Data.GroupModifyPath )
            {
                var reName = Path.GetFullPath( path ).Replace('\r',' ').Split('.')[0].Split('\n')[0] + "_Scaled.bmp";
                var img = new  Image<Gray,byte>(path);
                img = img.Mul( 255.0 / level );
                img.Save( reName );
            }
        }

        Image<Gray,byte> TestImg; 
        private void btnCheckNorm_Click( object sender , EventArgs e )
        {
            var level = (double)nudNormalizeLevel.Value;
            var path = Data.GroupModifyPath[0];
            if(TestImg == null) TestImg = new  Image<Gray,byte>(path);
            TestImg = TestImg.Mul( 255.0 / level );
            imgBox.Image = null;
            imgBox.Image = TestImg;
        }

        private void btnTestImgClear_Click( object sender , EventArgs e )
        {
            imgBox.Image = null;
            TestImg = null;
        }

        private void btnTotal_Click( object sender , EventArgs e )
        {
            var level = (double)nudNormalizeLevel.Value;
            List<Image<Gray,byte>> reimglist = new List<Image<Gray, byte>>();

            foreach ( var path in Data.GroupModifyPath )
            {
                var normName = Path.GetFullPath( path ).Replace('\r',' ').Split('.')[0].Split('\n')[0] + "_Scaled.bmp";
                var img = new  Image<Gray,byte>(path);
                img = img.Mul( 255.0 / level );
                img.Save( normName );

                var resizeName = Path.GetFullPath( path ).Replace('\r',' ').Split('.')[0].Split('\n')[0] + "_Resized.bmp";
                var reimg = img.Resize((double)(1/nudScale.Value), Inter.Cubic );
                reimg.Save( resizeName );
                reimglist.Add( reimg );
            }

            var stackedimg = reimglist.Aggregate( ( f , s ) => f.ConcateHorizontal( s ) );
            var stackName = Path.GetFullPath( Data.GroupModifyPath[0] ).Replace('\r',' ').Split('.')[0].Split('\n')[0] + "_Stacked.bmp";
            stackedimg.Save( stackName );
        }
    }
}
