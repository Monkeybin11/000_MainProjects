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
using MahApps.Metro.Controls;
using MahApps.Metro;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using DALSA.SaperaLT.SapClassBasic;
using DALSA.SaperaLT;
using Accord.Math;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using MachineControl.Camera.Dalsa;
using System.Threading;

namespace PLImg_V2
{
    enum StageEnableState {
        Enabled,
        Disabled
        
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Core Core = new Core();
        public SeriesCollection seriesbox { get; set; }
        public ChartValues<int> chartV { get; set; }
        ImageBox[] TrgImgBoxArr;
        Dictionary<ScanConfig, System.Windows.Controls.RadioButton> SampleConfig;
        Dictionary<string,StageEnableState> StgState;
        SerialComHelper serilacom;
        int startlinerate = 1400;
        int linerateStep = 5;
        int scancounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitImgBox();
            InitLocalData();
            DataContext = this;
            var cd = new ConnectionData();
            Core.ConnectDevice( cd.CameraPath, cd.ControllerIP, cd.RStage )( ScanConfig.Trigger_1 );
            InitCore();
            //serilacom = SerialComHelper.Instance;
            //serilacom.SerialInit();
        }

        #region Display

        void DisplayTrgImg( Image<Gray , byte> img , int lineNum )
        {
            TrgImgBoxArr[lineNum].Image = img;


            // ADD vf Value


        }

        void DisplayAF(double input)
        {
            lblAFV.BeginInvoke(()=> lblAFV.Content = input.ToString("N4") );
        }
      
        void DisplayBuffNumber(int num)
        {
            lblBuffNum.BeginInvoke(() => lblBuffNum.Content = num.ToString());
        }




        #endregion

        #region main
        private void btnStartFixAreaScan_Click( object sender, RoutedEventArgs e )
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            Core.TrigScanData.Scan_Stage_Speed = (double)nudTrgScanSpeed.Value;

            startlinerate = (int)nudStartlinerate.Value;
            linerateStep = (int)nudStartlinerateStep.Value;

            foreach ( var item in SampleConfig )
            {
                if ( item.Value.IsChecked == true )
                {
                    ResizeTriggerImgBox( item.Key );
                    Core.StartTrigScan( item.Key );
                }
            }
        }

        void ResizeTriggerImgBox( ScanConfig config )
        {
            switch ( config )
            {
                case ScanConfig.Trigger_1:
                    windowTrig0.Width = 560;
                    imgboxTrig0.Width = 560;
                    break;

                case ScanConfig.Trigger_2:
                    windowTrig0.Width = 186;
                    windowTrig1.Width = 186;
                    windowTrig2.Width = 186;
                    windowTrig3.Width = 0;
                    imgboxTrig0.Width = 186;
                    imgboxTrig1.Width = 186;
                    imgboxTrig2.Width = 186;
                    imgboxTrig3.Width = 0;
                    break;

                case ScanConfig.Trigger_4:
                    windowTrig0.Width = 140;
                    windowTrig1.Width = 140;
                    windowTrig2.Width = 140;
                    windowTrig3.Width = 140;
                    imgboxTrig0.Width = 140;
                    imgboxTrig1.Width = 140;
                    imgboxTrig2.Width = 140;
                    imgboxTrig3.Width = 140;
                    break;
            }
        }

        private void btnSaveImg_Click( object sender, RoutedEventArgs e )
        {
            string savePath = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if ( fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                savePath = fbd.SelectedPath;
                for ( int i = 0; i < TrgImgBoxArr.GetLength( 0 ); i++ )
                {
                    string filename = savePath + "\\" + i.ToString("D3") + ".png";
                    TrgImgBoxArr[i].Image?.Save( filename );
                }
            }
        }
        #endregion

        #region Init

       
        
        void InitCore( )
        {
            foreach ( var item in GD.YXZ )
            {
                StgState[item] = StageEnableState.Enabled;
            }
            Core.evtTrgImg        += new TferTrgImgArr( DisplayTrgImg );
            Core.evtSV            += new TferNumber( DisplayAF );
            Core.evtFedBckPos     += new TferFeedBackPos( DisplayPos );
            Core.evtScanEnd       += new TferScanStatus( ( ) => { this.BeginInvoke( () => Mouse.OverrideCursor = null ); } );
            Core.evtScanEnd       += new TferScanStatus( ( ) => 
            {
                //if ( scancounter < 200 )
                //{
                //    scancounter++;
                //    serilacom.Send( "ssf " + (startlinerate + linerateStep * scancounter).ToString() );
                //    Thread.Sleep( 100 );
                //    Core.StartTrigScan( ScanConfig.Trigger_1 );
                //
                //}
            } );


            Core.evtDummyTrgImg += new TferImgandVar( ( img, vari ) =>
            {
                //string linerate = (startlinerate + linerateStep* scancounter).ToString();
                //string name = vari.ToString() + ".bmp";
                //string basepath = @"D:\TDI45Deg_LinerateTest";
                //img.Save( basepath + "\\" + linerate +"_"+ name );
            } );


            Task.Run(()=>Core.GetFeedbackPos());
            InitViewWin();
        }

     
        void InitImgBox()
        {
            TrgImgBoxArr = new ImageBox[4];
            TrgImgBoxArr[0] = imgboxTrig0;
            TrgImgBoxArr[1] = imgboxTrig1;
            TrgImgBoxArr[2] = imgboxTrig2;
            TrgImgBoxArr[3] = imgboxTrig3;

            foreach ( var item in TrgImgBoxArr )
            {
                item.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        void InitViewWin( )
        {
            nudExtime.Value = 2400;
            nudlinerate.Value = 400;
            nudScanSpeed.Value = 1;
            nudGoXPos.Value = 100;
            nudGoYPos.Value = 50;
            nudGoZPos.Value = 26.190;
        }

        void InitLocalData() {
            StgState = new Dictionary<string , StageEnableState>();
            StgState.Add("Y", new StageEnableState());
            StgState.Add("X", new StageEnableState());
            StgState.Add("Z", new StageEnableState());

            SampleConfig = new Dictionary<ScanConfig , System.Windows.Controls.RadioButton>();
            SampleConfig.Add( ScanConfig.Trigger_1  ,rdb1inch);
            SampleConfig.Add( ScanConfig.Trigger_2  ,rdb2inch);
            SampleConfig.Add( ScanConfig.Trigger_4  ,rdb4inch);
        }

        void DisplayPos(double[] inputPos)
        {
            Task.Run( ( ) => lblXpos.BeginInvoke( (Action)(( ) => lblXpos.Content = inputPos[0].ToString("N4")) ) );
            Task.Run( ( ) => lblYpos.BeginInvoke( (Action)(( ) => lblYpos.Content = inputPos[1].ToString("N4")) ) );
            Task.Run( ( ) => lblZpos.BeginInvoke( (Action)(( ) => lblZpos.Content = inputPos[2].ToString("N4")) ) );
        }
        #endregion

        #region MainWindowEvent
        void ScanStart( ) { Mouse.OverrideCursor = Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;}
        void ScanEnd( ) { Mouse.OverrideCursor = null; }

        #region Camera
        private void btnGrap_Click(object sender, RoutedEventArgs e)
        {
            if ( Core == null ) Console.WriteLine( "null ");
            Core.Grab();
        }
        private void btnFreeze_Click( object sender, RoutedEventArgs e )
        {
            Core.Freeze();
        }
        #endregion

        #region Stage
        // common //
        private void btnOrigin_Click( object sender, RoutedEventArgs e ) {

            Core.Stg.Home( "X" )();
            Core.Stg.Home( "Y" )();
            //foreach ( var item in GD.YXZ ) Core.Stg.Home( item )();
        }

        // XYZStage //
        private void btnYMove_Click( object sender, RoutedEventArgs e )
        {
            if ( StgState["Y"] == StageEnableState.Enabled ) Core.MoveXYstg( "Y" , ( double ) nudGoYPos.Value );
        }
        private void btnXMove_Click( object sender, RoutedEventArgs e )
        {
            if ( StgState["X"] == StageEnableState.Enabled ) Core.MoveXYstg( "X" , ( double ) nudGoXPos.Value );
        }
        private void btnZMove_Click( object sender, RoutedEventArgs e )
        {
            if( StgState["Z"] == StageEnableState.Enabled) Core.MoveZstg( ( double ) nudGoZPos.Value );
        }

      

        // R Stage //
        private void btnRMove_Click( object sender, RoutedEventArgs e )
        {
            double pulse = (double)nudGoRPos.Value * 400;
            
        }
        private void btnROrigin_Click( object sender, RoutedEventArgs e )
        {
           
        }
        private void btnRForceStop_Click( object sender, RoutedEventArgs e )
        {
            
        }
        #endregion

        #endregion

        #region Motor Enable / Disable // Done
        private void ckbYDisa_Checked( object sender , RoutedEventArgs e )
        {
            Core.Stg.Disable( "Y" )();
            StgState["Y"] = StageEnableState.Disabled;
        }
        private void ckbXDisa_Checked( object sender, RoutedEventArgs e ) {
            Core.Stg.Disable("X")();
            StgState["X"] = StageEnableState.Disabled;
        }
        private void ckbZDisa_Checked( object sender, RoutedEventArgs e ) {
            Core.Stg.Disable( "Z" )();
            StgState["Z"] = StageEnableState.Disabled;
        }
        private void ckbYDisa_Unchecked( object sender , RoutedEventArgs e )
        {
            Core.Stg.Enable( "Y" )();
            StgState["Y"] = StageEnableState.Enabled;
        }
        private void ckbXDisa_Unchecked( object sender , RoutedEventArgs e )
        {
            Core.Stg.Enable( "X" )();
            StgState["X"] = StageEnableState.Enabled;
        }
        private void ckbZDisa_Unchecked( object sender, RoutedEventArgs e ) {
            Core.Stg.Enable( "Z" )();
            StgState["Z"] = StageEnableState.Enabled;
        }

        
        #endregion

        #region Sscan data Setting 

        private void nudlinerate_KeyUp( object sender, System.Windows.Input.KeyEventArgs e ) {
            if ( e.Key != System.Windows.Input.Key.Enter ) return;
            Core.LineRate( (int)nudlinerate.Value );
        }

        private void nudExtime_KeyUp( object sender , System.Windows.Input.KeyEventArgs e )
        {
            if ( e.Key != System.Windows.Input.Key.Enter ) return;
            Core.Exposure( ( int ) nudExtime.Value );
        }

        #endregion

        #region window Event 
        private void MetroWindow_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            foreach ( var item in GD.YXZ )
            {
                Core.Stg.Disable( item )();
                Core.Stg.Disconnect();
            }
            Core.Cam.Freeze();
            Core.Cam.Disconnect();
        }

        private void TabItem_Selected( object sender, RoutedEventArgs e )
        {

        }

        private void TabItem_Unselected( object sender, RoutedEventArgs e )
        {

        }
        #endregion

        #region Tab Select Event

        #endregion

        ////////////////////////////////////////////////////////////////////////

        public void ConnectCamParm()
        {

        }

        private void btnSyncTest_Click( object sender, RoutedEventArgs e )
        {

        }

        private void btnArea_Click( object sender, RoutedEventArgs e )
        {

        }

        private void btnLine_Click( object sender, RoutedEventArgs e )
        {

        }
    }
}
