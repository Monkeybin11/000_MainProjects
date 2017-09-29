using LiveCharts;
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
using LiveCharts.Defaults;
using LiveCharts.Configurations;
using System.ComponentModel;
using System.Threading;


namespace LiveCharts_Example
{
	/// <summary>
	/// Interaction logic for Example2_Observable.xaml
	/// </summary>
	public partial class Example2_Observable : Window , INotifyPropertyChanged
	{
		public Example2_Observable()
		{
			InitializeComponent();
			DataContext = this;

			//var mapper = Mappers.Xy<double>()
			//	.X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
			//    .Y(model => model.Value);
			//Charting.For<double>( mapper );
			ChartValues = new ChartValues<double>();
			DateTimeFormatter = value => value.ToString( "##.##" );
			AxisStep = 10;
			AxisUnit = 100;
			AxisMin = 0;
			AxisMax = 60;
		}

		private double _axisMax;
		private double _axisMin;
		private double _trend;

		public ChartValues<double> ChartValues { get; set; }
		public Func<double , string> DateTimeFormatter { get; set; }
		public double AxisStep { get; set; }
		public double AxisUnit { get; set; }

		public double AxisMax
		{
			get { return _axisMax; }
			set
			{
				_axisMax = value;
				OnPropertyChanged( "AxisMax" );
			}
		}
		public double AxisMin
		{
			get { return _axisMin; }
			set
			{
				_axisMin = value;
				OnPropertyChanged( "AxisMin" );
			}
		}

		public bool IsReading { get; set; }

		int counter = 0;
		private void Read()
		{
			var r = new Random();
			Thread.Sleep( 150 );
			var now = DateTime.Now;
			//_trend += r.Next( 0 , 50 );
			ChartValues.AddRange( Enumerable.Range( counter , 20 ).Select( x => ( double )( x + r.Next( 0 , 50 ) ) ) );

			//lets only use the last 150 values
			if ( ChartValues.Count > 150 ) ChartValues.Skip( ChartValues.Count - 150 ).Take( 150 );

			AxisMin = 0;
			AxisMax = AxisMax == 100 ? 70 : 100;
			counter += 20;

		}

		private void btnchange_Click( object sender , RoutedEventArgs e )
		{

		}




		public event PropertyChangedEventHandler PropertyChanged;
		public event Action PointChanged;

		protected void OnPropertyChanged( string propertyName = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
		}

		private void btnstart_Click( object sender , RoutedEventArgs e )
		{
			Task.Run( ( Action )Read );
		}

		private void btnstop_Click( object sender , RoutedEventArgs e )
		{

		}
	}
}

