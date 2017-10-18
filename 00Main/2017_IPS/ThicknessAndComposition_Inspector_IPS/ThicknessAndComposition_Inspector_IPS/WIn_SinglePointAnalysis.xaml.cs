using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for WIn_SinglePointAnalysis.xaml
	/// </summary>
	
	public partial class WIn_SinglePointAnalysis : Window
	{
		public event Action<double[],int,int> evtScanStart;
		int Counter;
		List<double[]> Spectruns = new List<double[]>();
		List<double[]> Reflectivitys = new List<double[]>();
		List<double> Thicknesses = new List<double>();
		double[] Waves = new double[] { } ;
		List<string> Time = new List<string>();
		bool waveSetted;
		public WIn_SinglePointAnalysis()
		{
			InitializeComponent();
			//ucIntensitiychart.chtLiveLine.
			ucReflectivityChart.axisY.MaxValue = 100;

		}

		private void btnSinglePosStart_Click( object sender , RoutedEventArgs e )
		{
			evtScanStart(
				new double [ ]
				{
					(double)nudXposSingle.Value,
					(double)nudYposSingle.Value
				} ,
				( int )nudIntervalSingle.Value,
				( int )nudCountSingel.Value
				);
			Counter = 0;
		}

		public void DrawSignal( IEnumerable<double> spct , IEnumerable<double> reflect , IEnumerable<double> wave ,double thckn )
		{
			Thicknesses.Add( thckn );

			Time.Add( DateTime.Now.ToString( "yyMMdd__HH_mm_ss" ) );
			ucIntensitiychart.AddNewSeries( spct , wave , thckn.ToString("N2") , Counter);
			Spectruns.Add( spct.ToArray() );
			if ( !waveSetted ) Waves = wave.ToArray();

			ucReflectivityChart.AddNewSeries( reflect , wave , thckn.ToString( "N2" ) , Counter );
			lblSingleScanStatus.Dispatcher.BeginInvoke( ( Action )( () => lblSingleScanStatus.Content = Counter.ToString() ) );
			Reflectivitys.Add( reflect.ToArray() );
			Counter++;
		}

		

		private void btnSeriesClear_Click( object sender , RoutedEventArgs e )
		{
			ucReflectivityChart.ClearSeries();
			ucIntensitiychart.ClearSeries();
		}

		private void Window_Closing( object sender , System.ComponentModel.CancelEventArgs e )
		{
			e.Cancel = true;
			this.Visibility = Visibility.Hidden;
		}

		private void btnSaveSingleScan_Click( object sender , RoutedEventArgs e )
		{
			SaveFileDialog sfd = new SaveFileDialog();
			if (sfd.ShowDialog() == true)
			{
				var spctpath = sfd.FileName + "_Spectrum.csv";
				var rflctpath = sfd.FileName + "_Reflectivity.csv";

				StringBuilder stbspcts = new StringBuilder();
				StringBuilder stbrflct = new StringBuilder();
				stbspcts.Append( "WaveLen," + Time.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );
				stbrflct.Append( "WaveLen," + Time.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );

				stbspcts.Append( "Thickness," + Thicknesses.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );
				stbrflct.Append( "Thickness," + Thicknesses.Select( x => x.ToString() ).Aggregate( ( f , s ) => f + ',' + s ) + Environment.NewLine );

				var lines = Spectruns [ 0 ].GetLength( 0 );

				for ( int i = 0 ; i < lines ; i++ )
				{
					stbspcts.Append(
						Waves [ i ].ToString() + ',' +
						Spectruns.Select( x => x [ i ].ToString() ).Aggregate( ( f , s ) => f + "," + s ) +
						Environment.NewLine );

					stbrflct.Append(
						Waves [ i ].ToString() + ',' +
						Reflectivitys.Select( x => x [ i ].ToString() ).Aggregate( ( f , s ) => f + "," + s ) +
						Environment.NewLine );
				}
				File.WriteAllText( spctpath,stbspcts.ToString() );
				File.WriteAllText( rflctpath,stbrflct.ToString() );
			}
		}
	}
}
