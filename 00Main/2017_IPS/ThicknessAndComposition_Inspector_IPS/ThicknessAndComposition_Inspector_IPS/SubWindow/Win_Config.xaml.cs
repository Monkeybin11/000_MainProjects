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

namespace ThicknessAndComposition_Inspector_IPS
{
	public delegate void StgSpeedEvent( 
		double rspeed 
		, double xspeed 
		, double scan2avg 
		, double intetime , double boxcar );

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

		public event StgSpeedEvent evtStgSpeedSetChange;

		public Win_Config()
		{
			InitializeComponent();
		}

		private void btnSettingApply_Click( object sender , RoutedEventArgs e )
		{
			ComPort			= nudXStgPort.Value		    .ToNonNullable(); ;
			RstgSpeed		= nudRStgSpeed.Value		.ToNonNullable();
			XstgSpeed		= nudXStgSpeed.Value		.ToNonNullable();
			Scan2Avg		= nudScan2Avg.Value			.ToNonNullable();
			IntegrationTIme = nudIntegrationTime.Value	.ToNonNullable();
			Boxcar			= nudBoxcar.Value			.ToNonNullable();

			evtStgSpeedSetChange( 
								RstgSpeed		,
								XstgSpeed		,
								Scan2Avg		,
								IntegrationTIme ,
								Boxcar			);

		}

		private void btnCancel_Click( object sender , RoutedEventArgs e )
		{
			nudXStgPort.Value         = ComPort          ;
			nudXStgSpeed.Value		  = XstgSpeed		 ;
			nudRStgSpeed.Value		  = RstgSpeed		 ;
			nudScan2Avg.Value		  = Scan2Avg		 ;
			nudIntegrationTime.Value  = IntegrationTIme  ;
			nudBoxcar.Value			  = Boxcar			 ;
		}
	}
}
