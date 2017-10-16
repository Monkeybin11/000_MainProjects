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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveCharts_Example
{
	/// <summary>
	/// Interaction logic for UC_ColumChart.xaml
	/// </summary>
	public partial class UC_ColumChart : UserControl, INotifyPropertyChanged
	{
		public string [ ] Labels
		{
			get { return _Labels; }
			set
			{
				//_Labels = value.Select( x => x.ToString() );
				OnPropertyChanged( "Labels" );
			}
		}
		public string [ ] _Labels;
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
		public UC_ColumChart()
		{
			InitializeComponent();
			DataContext = this;

			ChartValues = new ChartValues<double>();
			DisFormatter = value => Math.Round( value ).ToString();
		}

		int counter = 0;

		public void CreateHistogram( ) // 
		{
			//var repVal = RepresentativeValueAndFrequency
			//				.Select( x => x[0]);
			//var fre = RepresentativeValueAndFrequency
			//				.Select( x => x[1]);
			//
			
			ChartValues.Clear();
			//ChartValues.AddRange( fre );
			var r = new Random();
			ChartValues.AddRange( Enumerable.Range( counter , 4 ).Select( x => ( double )( x + r.Next( 0 , 50 ) ) ) );
			Labels = new string [ ] { "300","310","300","222" };
			lbX.Labels = new [ ] { "300" , "310" , "300" , "222" }; 
			//Labels = repVal.Select( x => Math.Round(x).ToString() ).ToArray();
		}

		public void CreateHistogram2( double[] src) // 
		{
			//var repVal = RepresentativeValueAndFrequency
			//				.Select( x => x[0]);
			//var fre = RepresentativeValueAndFrequency
			//				.Select( x => x[1]);
			//
			//ChartValues.Clear();
			//ChartValues.AddRange( fre );
			var r = new Random();
			ChartValues.AddRange( src );
			Labels = new string [ ] { "300,310,300,222" };
			//Labels = repVal.Select( x => Math.Round(x).ToString() ).ToArray();
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged( string propertyName = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
		}

	}
}
