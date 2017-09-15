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

		#region Load Close
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
		}
		private void Window_Loaded( object sender , RoutedEventArgs e )
		{
			WinConfig = new Win_Config();
			WinConfig.evtStgSpeedSetChange += new StgSpeedEvent(Core.SetHWInternalParm);

			ucLSMenu.evtBtn += new BtnEvt( LeftSideBtn );
			Core = new IPSCore();
			Config2UI( Core.Config );
		}
		private void Window_Closed( object sender , EventArgs e )
		{
			Core.Act( x => x.Config = UI2IpsConfig() )
				.Act( x => x.SaveConfig( x.ConfigFullPath ) );
			WinConfig.Close();
		}


		#endregion

		public void OptionMenuClick( object sender , RoutedEventArgs e )
		{
			var master = sender as MenuItem;
			switch ( master.Name )
			{
				case "menuLoadConfig":
					break;

				case "menuCreateConfig":
					break;

				case "menuSetSpec":
					break;

				case "menuSetStage":
					break;

				default:
					break;
			}
		}



		public void LeftSideBtn( string name )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			switch ( name )
			{
				case "btnConnect":
					break;
				case "btnDisconnect":
					break;
				case "btnStart":
					Core.Config = UI2IpsConfig();
					Core.TestFunction();
					//Core.ScanRun();

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
					break;

				case "btnSaveRaw":
					break;

				default:
					break;
			}


			// Load config 
			// Save config
			// Save Raw Data
			// ToDo : 결과 저장 기능. 결과 디스플레이 , 두께 찾는기능 ( 스펙트럼데이터 -> 두께)
			// ToDo : 서브기능 = 1. 레전드 표시 , 2. 스테이지 실시간 포지션  3. 스펙트로미터 실시가나 표시 

		}

		

		
	}
	
}
