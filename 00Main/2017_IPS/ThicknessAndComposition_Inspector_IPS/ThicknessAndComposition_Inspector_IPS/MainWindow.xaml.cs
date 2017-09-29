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

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		IPSCore Core { get; set; }
		Win_Config WinConfig;
		bool CoreRunning;

		#region Load Close
		public MainWindow()
		{
			InitializeComponent();
			//DataContext = this;
		}
		private void Window_Loaded( object sender , RoutedEventArgs e )
		{
			WinConfig = new Win_Config();
			ucLSMenu.evtBtn += new BtnEvt( LeftSideBtn );
			Core = new IPSCore();
			Core.evtConnection += new Action<bool,bool>( ucLSStatus.DisplayConnection );
			//Core.evtScanStatus += new Action<string>( ucLSStatus.DisplayScanStatus );
			Core.Connect();
			WinConfig.evtStgSpeedSetChange += new StgSpeedEvent( Core.SetHWInternalParm );
			Config2UI( Core.Config );
		}
		private void Window_Closed( object sender , EventArgs e )
		{
			Core.Act( x => x.Config = UI2IpsConfig() )
				.Act( x => x.SaveConfig( x.ConfigFullPath ) )
				.Act( x => x.SavePrcConfig( x.PrcConfigFullPath ) );
			WinConfig.Close();
		}

		void UpDownLimit()
		{
			//ToDo : UpDown Limit Setting  
		}


		#endregion

		public void OptionMenuClick( object sender , RoutedEventArgs e )
		{
			if ( CoreRunning ) return;
			CoreRunning = true;
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuLoadConfig":
					break;

				case "menuCreateConfig":
					break;

				case "menuSetSpecStg":
					WinConfig.Show();
					break;

				case "menuShowConfig":
					Core.ShowSetting();
					break;

				default:
					break;
			}
			CoreRunning = false;
		}



		public async void LeftSideBtn( string name )
		{
			if ( CoreRunning ) return;
			CoreRunning = true;
			OpenFileDialog ofd = new OpenFileDialog();
			Mouse.OverrideCursor = Cursors.Wait;
			switch ( name )
			{
				case "btnConnect":
					Window_Loaded( null , null );
					break;
				case "btnDisconnect":
					Core.test();
					break;


				case "btnStart":

					Core.Config = UI2IpsConfig();
					ucLSStatus.lblProgress.Content = "InProgress";
					var result = await Task<bool>.Run(()=> Core.ScanRun());
					if ( result ) ucLSStatus.lblProgress.Content = "Ready";
					else ucLSStatus.lblProgress.Content = ( "Ready (Interruped)" );
					ucDataGrid.UpdateGrid( Core.ResultData.SpotDataList.Select( x => 
																x.ToGridResult()).ToList() );
					ucMapDisplay.DrawImg( Core.ImgScanned , Core.ImgScaleBar);
					break;


				case "btnLoadConfig":
					if ( ofd.ShowDialog() == true )
					{
						Core.LoadConfig( ofd.FileName );
					}
					break;

				case "btnSaveCurrentConfig":
					if ( ofd.ShowDialog() == true )
					{
						Core.Config = UI2IpsConfig();
						Core.SaveConfig( ofd.FileName );
					}
					break;

				case "btnLoadData":
					if ( ofd.ShowDialog() == true )
					{
						Core.LoadConfig( ofd.FileName );
						Config2UI( Core.Config );
					}
					break;

				case "btnSaveRes":
					var temp = new List<double[]>();
					temp.Add( new double [ ] { 300 , 100 } );
					temp.Add( new double [ ] { 310 , 30 } );
					temp.Add( new double [ ] { 320 , 210 } );
					temp.Add( new double [ ] { 330 , 50 } );
					ucHisto.CreateHistogram( temp );

					break;

				case "btnSaveRaw":
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
