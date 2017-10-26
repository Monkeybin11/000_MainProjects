using Microsoft.Win32;
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
using ThicknessAndComposition_Inspector_IPS_Core;
using ThicknessAndComposition_Inspector_IPS_Data;
using SpeedyCoding;
using System.Threading;
using System.IO;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		IPSCore Core { get; set; }
		Win_Config WinConfig;
		Win_SpctDisplay WinSpct;
		WIn_SinglePointAnalysis WinSingleScan;
		bool CoreRunning;

		#region Load Close
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
		}
		private void Window_Loaded( object sender , RoutedEventArgs e )
		{
			ucSpectrum.Title = "Raw Spectrum";
			ucRefelectivity.Title = "Refelectivity";

			WinConfig = new Win_Config();
			WinSpct = new Win_SpctDisplay();
			WinSingleScan = new WIn_SinglePointAnalysis();
			ucLSMenu.evtBtn += new BtnEvt( LeftSideBtn );

			Core = new IPSCore()
				.Act( x => x.evtConnection += new Action<bool , bool>( ucLSStatus.DisplayConnection ) )
				.Act( x => x.evtSpectrum += new Action<IEnumerable<double> , IEnumerable<double>>( ucSpectrum.UpdateSeries ) )
				.Act( x => x.evtRefelectivity += new Action<IEnumerable<double> , IEnumerable<double>>( ucRefelectivity.UpdateSeries ) )
				.Act( x => x.evtSngSignal += new Action<IEnumerable<double> , IEnumerable<double> , IEnumerable<double> , double , int>( WinSingleScan.DrawSignal ) )
				.Act( x => x.evtSingleMeasureComplete += new Action( WinSingleScan.ToReadyState ) )
				.Act( x => x.Connect());

			WinConfig
				.Act( x => x.evtStgSpeedSetChange += new StgSpeedEvent( Core.SetHWInternalParm ) );

			WinSpct 
				.Act( x => x.evtCloseWin += new Action( () => { Core.FlgAutoUpdate = false; FlgSpctDisplay = false; } ) );

			WinSingleScan 
				.Act( x => x.evtScanStart += new Action<double[], int, int>( Core.StartManualRunEvent ) )
				.Act( x => x.evtStopSingleScan += new Action ( () => Core.FlgCoreSingleScan = false) );
			

			Config2UI( Core.Config );
			if(!Core.OpLoadAbsReflecDatas() 
				|| !Core.OpPickWaveIdx() 
				|| !Core.OpPickReflectFactorIdx())
				MessageBox.Show("Spectrometer Problem. Transfer wavelength fail.  Please restart spectrometer ") ;
			Core.StartBackgroundTask();


		}
		private void Window_Closed( object sender , EventArgs e )
		{
			Core.Act( x => x.Config = UI2IpsConfig() )
				.Act( x => x.SaveConfig( x.ConfigFullPath ) )
				.Act( x => x.SavePrcConfig( x.PrcConfigFullPath ) );
			WinConfig.Close();
			Environment.Exit( Environment.ExitCode );
		}

		void UpDownLimit()
		{
			//ToDo : UpDown Limit Setting  
		}


		#endregion

		bool FlgSpctDisplay;
		public void FileMenuClick( object sender , RoutedEventArgs e )
		{
			SaveFileDialog ofd = new SaveFileDialog();
			if ( CoreRunning ) return;
			CoreRunning = true;
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuSaveResultonly":
					
					break;
				case "menuSaveRawonly":

					break;
				case "menuSaveImageonly":

					break;
				case "menuViewSpct":

					break;
				case "menuSaveConfig":
					ofd.Filter = "Data Files (*.xml)|*.xml";
					ofd.DefaultExt = "xml";
					ofd.AddExtension = true;
					if ( ofd.ShowDialog() == true )
					{
						Core.SaveConfig(ofd .FileName);
					}
					
					break;

			
					

					break;

				case "menuExit":
					Environment.Exit( Environment.ExitCode );
					break;
			}

		}

		public void AnalysisMenuClick( object sender , RoutedEventArgs e )
		{
			if ( CoreRunning ) return;
			CoreRunning = true;
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuSinglePosScan":
					WinSingleScan.Visibility = Visibility.Visible;
					break;

				default:
					break;
			}
			CoreRunning = false;
		}

		public void OptionMenuClick( object sender , RoutedEventArgs e )
		{
			
			if ( CoreRunning ) return;
			CoreRunning = true;
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuViewSpct":
					Core.FlgAutoUpdate = true;
					FlgSpctDisplay = true;
					WinSpct.Show();
					Task.Run( () =>
					 {
						 while ( FlgSpctDisplay )
						 {
							 Console.WriteLine( "Trs Data" );
							 WinSpct.ucSpctShart.UpdateSeries( Core.BkD_Spctrm , Core.SelectedWaves );
							 Console.WriteLine( Core.BkD_Spctrm.Count() < 1 ? "NO data" : Core.BkD_Spctrm.Last().ToString() );
							 Thread.Sleep( Core.SpectrometerDelayTime );
						 }
					 } );
					Console.WriteLine( "closed" );
					break;

				case "menuSetSpecStg":
					WinConfig.Show();
					break;


				case "menuShowConfig":
						Core.ShowSetting();
					break;

				case "menuSetDefualtConfig":
					Core.Config = new IPSDefualtSetting().ToConfig();
					Config2UI(Core.Config);
					break;

				default:
					break;
			}
			CoreRunning = false;
		}

		bool changer = false;

		public async void LeftSideBtn( string name )
		{
			if ( CoreRunning ) return;
			//CoreRunning = true;
			OpenFileDialog ofd = new OpenFileDialog();
			Mouse.OverrideCursor = Cursors.Wait;
			switch ( name )
			{
				case "btnSaveResult":
					SaveFileDialog sfd = new SaveFileDialog();
					if ( sfd.ShowDialog() == true )
					{
						Core.SaveResult( sfd.FileName + "_Result.csv" , Core.ResultData );
						Core.SaveRaw( 
							sfd.FileName + "_Raw.csv" , 
							Core.ResultData , 
							Core.SelectedWaves , 
							Core.Darks ,
							Core.Refs ,
							Core.SelectedReflctFactors );
						Core.SaveRawReflectivity(
							sfd.FileName + "_Refelctivity.csv" ,
							Core.ResultData ,
							Core.SelectedWaves );
						Core.SaveImage( sfd.FileName + "_MapImage.png" );
					}

					break;
				case "btnLoadConfig":
					if ( ofd.ShowDialog() == true )
					{
						Core.LoadConfig( ofd.FileName );
						Config2UI( Core.Config );
					}
				break;

				case "btnRefScan":
					Core.OpReady( IPSCore.ScanReadyMode.Ref );
					break;
				case "btnDarkScan":
					Core.OpReady( IPSCore.ScanReadyMode.Dark );
					break;
				case "btnSetWave":
					Core.OpReady( IPSCore.ScanReadyMode.WaveLen );
					break;
				case "btnLoadReflection":
					Core.OpReady( IPSCore.ScanReadyMode.Refelct );
					break;
				case "btnHome":
					var data = Enumerable.Range(0,50).Select( x => (double)x).ToArray();
					Random rnd = new Random();
					double[] data2 = new double[4] { rnd.Next(10) *10 , rnd.Next(10) * 10 , rnd.Next(10) * 10 ,  rnd.Next(10) *10};
					//ucHisto.CreateHistogram2( data2 );
					//ucHisto.CreateHistogram( );
					//Core.OnePointScan();
					//Core.OpHome();
					break;
				case "btnScanReady":
					Core.OpReady(IPSCore.ScanReadyMode.All);
					break;
				case "btnReconnect":
					Core.DrawTest();
					ucMapDisplay.DrawImg( Core.ImgScanned , Core.ImgScaleBar );
					//Window_Loaded( null , null );
					break;

				case "btnStart":
					//Core.ScanRun();
					
					Core.Config = UI2IpsConfig();
					ucLSStatus.lblProgress.Content = "InProgress";
					Core.ScanPos = new ScanPosData();
					Core.ScanPos.RhoList [ 3 ] = ucLSMenu.nudOuterLength.Value.ToNonNullable();

					var result = await Task<bool>.Run(()=> Core.ScanAutoRun());

					if ( result )
					{
						ucLSStatus.lblProgress.Content = "Ready";
						ucDataGrid.UpdateGrid( Core.ResultData.SpotDataList.Select( x =>
																x.ToGridResult() ).ToList() );
						ucMapDisplay.DrawImg( Core.ImgScanned , Core.ImgScaleBar );
						//ucHisto.DrawHistogram( Core.ResultData.SpotDataList.Select( x => x.Thickness).ToArray() );
					}
					else ucLSStatus.lblProgress.Content = ( "Ready (Interruped)" );
					
					break;

				case "btnSaveCurrentConfig":
					if ( ofd.ShowDialog() == true )
					{
						Core.Config = UI2IpsConfig();
						Core.SaveConfig( ofd.FileName );
					}
					break;

				case "btnLoadData":
					//if ( ofd.ShowDialog() == true )
					//{
					//	Core.LoadConfig( ofd.FileName );
					//	Config2UI( Core.Config );
					//}
					break;

				
				default:
					break;
			}
			Mouse.OverrideCursor = null;
			CoreRunning = false;

			// Load config 
			// Save config
			// Save Raw Data
			// ToDo : 결과 저장 기능. 결과 디스플레이 , 두께 찾는기능 ( 스펙트럼데이터 -> 두께)
			// ToDo : 서브기능 = 1. 레전드 표시 , 2. 스테이지 실시간 포지션  3. 스펙트로미터 실시가나 표시 

		}
	}
	
}
