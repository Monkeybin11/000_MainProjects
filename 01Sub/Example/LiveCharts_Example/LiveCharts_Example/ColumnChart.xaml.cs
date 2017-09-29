using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LiveCharts_Example
{
	/// <summary>
	/// Interaction logic for ColumnChart.xaml
	/// </summary>
	public partial class ColumnChart : Window, INotifyPropertyChanged
	{
		private double _axisMax;
		private double _axisMin;
		private double _trend;

		public ChartValues<double> ChartValues { get; set; }
		public Func<double , string> DisFormatter { get; set; }
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


		public ColumnChart()
		{
			InitializeComponent();
			DataContext = this;

			ChartValues = new ChartValues<double>();
			DisFormatter = value => value.ToString( "##.##" );
		}




		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged( string propertyName = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
		}

		int counter = 0;
		private void btnadd_Click( object sender , RoutedEventArgs e )
		{
			var r = new Random();
			ChartValues.AddRange( Enumerable.Range( counter , 20 ).Select( x => ( double )( x + r.Next( 0 , 50 ) ) ) );
		}
	}

}
