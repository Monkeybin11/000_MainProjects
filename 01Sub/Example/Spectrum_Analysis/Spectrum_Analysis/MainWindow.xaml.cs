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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Events;
using LiveCharts.Wpf;
using LiveCharts.Charts;
using System.ComponentModel;
using MachineLib.DeviceLib;
using System.Threading;
using LiveCharts.Configurations;
using SpeedyCoding;

namespace Spectrum_Analysis
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private double yMin;
		private double yMax;
		private double xMin;
		private double xMax;
		private string _status;
		private double[] CurrentData;
		private double[] Wave;

		public event PropertyChangedEventHandler PropertyChanged;
		#region
		public void OnPropertyChanged( string propertyname = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyname ) );
		}

		public double YMin
		{
			get { return yMin; }
			set
			{
				yMin = value;
				OnPropertyChanged( "YMin" );
			}
		}

		public double YMax
		{
			get { return yMax; }
			set
			{
				yMax = value;
				OnPropertyChanged( "YMax" );
			}
		}

		public double XMin
		{
			get { return xMin; }
			set
			{
				xMin = value;
				OnPropertyChanged( "XMin" );
			}
		}

		public double XMax
		{
			get { return xMax; }
			set
			{
				xMax = value;
				OnPropertyChanged( "XMax" );
			}
		}
		#endregion

		public double AxisStep { get; set; }
		public double AxisUnit { get; set; }
		public Func<double , string> LabelFormat {get;set;}
		public ChartValues<double> ChartDatas { get; set; }

		public Maya_Spectrometer Spct;

		public string Status { get { return _status; } set { _status = value; OnPropertyChanged( "Status" ); } }

		public bool run;

		public string[] Labels { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			Setting();
			Task.Run( (Action)Read );
			Task.Run( (Action)GetSpect );
		}

		public void Setting()
		{
			Spct = new Maya_Spectrometer();
			if ( !Spct.Connect() ) MessageBox.Show( "not Connected" );

			// Chart Setting
			ChartDatas = new ChartValues<double>();
			YMin = 0;
			YMax = 10000;
			AxisStep = 100;
			AxisUnit = 5;
			LabelFormat = val => val.ToString(  );
			Wave = Spct.GetWaveLen().Skip( 200 ).Where( ( x , i ) => i % 10 == 0 ).Select( x => x ).ToArray();
			

			var mapper = Mappers.Xy<double>()
				.X((x,i) => Wave[i])   //use DateTime.Ticks as X
                .Y((x,i) => x);           //use the value property as Y
			Charting.For<double>( mapper );
		}

		public void Read()
		{
			while (true)
			{
				if ( !run ) continue;

				Thread.Sleep( 100 );
				ChartDatas.Clear();
				ChartDatas.AddRange( CurrentData );
			}
		}
		
		Action GetSpect => () =>
		{
			while ( true )
			{
				CurrentData = Spct.GetSpectrum().Skip(200).Where( ( x , i ) => i % 10 == 0 ).Select( x => x ).ToArray();
				YMax = CurrentData.Max() + 1000;
				Thread.Sleep( 30 );
			}
		};

		private void btnstart_Click( object sender , RoutedEventArgs e )
		{
			run = run == true ? false : true;
		}

		private void btnreset_Click( object sender , RoutedEventArgs e )
		{
			X.MinValue = double.NaN;
			X.MaxValue = double.NaN;
			Y.MinValue = double.NaN;
			Y.MaxValue = double.NaN;
		}
	}
}
