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
using MachineLib.DeviceLib;
using LiveCharts;
using LiveCharts.Wpf;
using System.Threading;
using System.Diagnostics;

namespace Spectrometer_OceanOptics
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		Maya_Spectrometer SP = new Maya_Spectrometer();
		SeriesCollection SeriesCollection;
		Task tsk;
		bool startflag;
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			//tsk = Task.Run( GetInten );
			Console.WriteLine( "Connection : " + SP.Connect().ToString() );
		}


		public Action GetInten =>
		() =>
		{
			Debug.WriteLine( "s" );
			while ( true )
			{
				if ( startflag )
				{
					Debug.WriteLine( "s" );
					var inten = SP.GetSpectrum().Take(10);
					this.Dispatcher.BeginInvoke( ( Action )( () => {
						Debug.WriteLine( "ssssssssssssss" );
						SeriesCollection = new SeriesCollection
										{
											new LineSeries
											{
												Values = new ChartValues<double>() { 1,2,3,4,5}
											}
										};
						chart.Update();


					} ) );
					
					Thread.Sleep( 500 );
				}
				
			}
			
		};
private void btnSTart_Click( object sender , RoutedEventArgs e )
		{
			startflag = true;
		}

		private void btnSTop_Click( object sender , RoutedEventArgs e )
		{
			startflag = false;
		}

		private void btnADdPOint_Click( object sender , RoutedEventArgs e )
		{
			var chartv = new ChartValues<int>( Enumerable.Range(0,100) );
			var line = new LineSeries(chartv);
			SeriesCollection = new SeriesCollection( line );
		}
	}
}
