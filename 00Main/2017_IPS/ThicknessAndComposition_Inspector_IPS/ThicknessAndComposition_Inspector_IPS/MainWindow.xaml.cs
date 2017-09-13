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
			switch ( name )
			{
				case "btnConnect":
					break;
				case "btnDisconnect":
					break;
				case "btnStart":
					Core.TestFunction();
					// Main Set
					// Main Start
					break;

				case "btnLoadConfig":
					OpenFileDialog ofd = new OpenFileDialog();
					if ( ofd.ShowDialog() == true )
					{
						Core.LoadConfig( ofd.FileName );
					}
					break;

				case "btnSaveCurrentConfig":
					OpenFileDialog ofd2 = new OpenFileDialog();
					if ( ofd2.ShowDialog() == true )
					{
						Core.SaveConfig( ofd2.FileName );
					}
					break;

				case "btnLoadData":
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
			// 

		}


	

		#region MidFunc
		IPSConfig UI2IpsConfig()
		{
			var res = new IPSConfig();
			res.AngFirst		= ucLSMenu.nudAngFirst.Value			  .ToNonNullable();
			res.AngStep			= ucLSMenu.nudAngStep.Value				  .ToNonNullable();
			res.RhoFirst		= ucLSMenu.nudRhoFirst.Value			  .ToNonNullable();
			res.RhoStep			= ucLSMenu.nudRhoStep.Value				  .ToNonNullable();
			res.RhoCount		= ucLSMenu.nudRhoCount.Value			  .ToNonNullable();
			res.IntegrationTime = (int)WinConfig.nudIntegrationTime.Value .ToNonNullable();
			res.Scan2Avg		= (int)WinConfig.nudScan2Avg.Value		  .ToNonNullable();
			res.Boxcar			= (int)WinConfig.nudBoxcar.Value		  .ToNonNullable();
			res.XStgSpeed		= (int)WinConfig.nudXStgSpeed.Value		  .ToNonNullable();
			res.RStgSpeed		= (int)WinConfig.nudRStgSpeed.Value		  .ToNonNullable();

			return res;
		}

		void Config2UI(IPSConfig config)
		{
			ucLSMenu.nudAngFirst.Value			= config.AngFirst		 ;
			ucLSMenu.nudAngStep.Value			= config.AngStep		 ;	
			ucLSMenu.nudRhoFirst.Value			= config.RhoFirst		 ;
			ucLSMenu.nudRhoStep.Value			= config.RhoStep		 ;	
			ucLSMenu.nudRhoCount.Value			= config.RhoCount		 ;
			WinConfig.nudIntegrationTime.Value	= config.IntegrationTime ; 
			WinConfig.nudScan2Avg.Value			= config.Scan2Avg		 ;
			WinConfig.nudBoxcar.Value			= config.Boxcar			 ;
			WinConfig.nudXStgSpeed.Value		= config.XStgSpeed		 ;
			WinConfig.nudRStgSpeed.Value		= config.RStgSpeed		 ;			
		}


		#endregion

		
	}
	
}
