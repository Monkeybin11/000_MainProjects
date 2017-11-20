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
using System.Windows.Shapes;
using SpeedyCoding;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.ComponentModel;

namespace ThicknessAndComposition_Inspector_IPS
{
	public delegate void StgSpeedEvent( 
		double rspeed 
		, double xspeed 
		, double scan2avg 
		, double intetime 
		, double boxcar 
		, double waittime );

	/// <summary>
	/// Interaction logic for Win_Config.xaml
	/// </summary>
	/// 

	public partial class Win_Config : Window
	{
		double ComPort         ;
		double XstgSpeed       ;
		double RstgSpeed       ;
		double Scan2Avg        ;
		double IntegrationTIme ;
		double Boxcar          ;
		double SpectrometerDelayTime          ;

		public event StgSpeedEvent evtStgSpeedSetChange;
		public event Action<double> evtSpctWaitTime;

		private const int GWL_STYLE = -16;
		private const int WS_SYSMENU = 0x80000;
		[DllImport( "user32.dll", SetLastError = true )]
		private static extern int GetWindowLong( IntPtr hWnd, int nIndex );
		[DllImport( "user32.dll" )]
		private static extern int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong );

		public Win_Config()
		{
			InitializeComponent();
		}

		//public void SetVisible( bool istrue )
		//{
		//	if ( istrue ) base.Visibility = Visibility.Visible;
		//	else base.Visibility = Visibility.Hidden;
		//}

		private void btnSettingApply_Click( object sender , RoutedEventArgs e )
		{
			Visibility = Visibility.Hidden;
			ComPort			= nudXStgPort.Value		    .ToNonNullable(); 
			RstgSpeed		= nudRStgSpeed.Value		.ToNonNullable();
			XstgSpeed		= nudXStgSpeed.Value		.ToNonNullable();
			Scan2Avg		= nudScan2Avg.Value			.ToNonNullable();
			IntegrationTIme = nudIntegrationTime.Value	.ToNonNullable();
			Boxcar			= nudBoxcar.Value			.ToNonNullable();
			SpectrometerDelayTime = nudSpctWait.Value.ToNonNullable();


			evtStgSpeedSetChange( 
								RstgSpeed		,
								XstgSpeed		,
								Scan2Avg		,
								IntegrationTIme ,
								Boxcar			,
								SpectrometerDelayTime );



		}

		private void btnCancel_Click( object sender , RoutedEventArgs e )
		{
			Visibility = Visibility.Hidden;
			nudXStgPort.Value         = ComPort          ;
			nudXStgSpeed.Value		  = XstgSpeed		 ;
			nudRStgSpeed.Value		  = RstgSpeed		 ;
			nudScan2Avg.Value		  = Scan2Avg		 ;
			nudIntegrationTime.Value  = IntegrationTIme  ;
			nudBoxcar.Value			  = Boxcar			 ;
		}

		private void ScanSettingWindow_Loaded( object sender, RoutedEventArgs e )
		{
			var hwnd = new WindowInteropHelper(this).Handle;
			SetWindowLong( hwnd, GWL_STYLE, GetWindowLong( hwnd, GWL_STYLE ) & ~WS_SYSMENU );
		}
		protected override void OnClosing( CancelEventArgs e )
		{
			base.OnClosing( e );
			e.Cancel = true;
		}

	}

	
}
