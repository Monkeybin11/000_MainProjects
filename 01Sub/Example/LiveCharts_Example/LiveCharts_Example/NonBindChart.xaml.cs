using LiveCharts;
using System.ComponentModel;
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
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;

namespace LiveCharts_Example
{
	/// <summary>
	/// Interaction logic for NonBindChart.xaml
	/// </summary>
	public partial class NonBindChart : Window
	{
		public NonBindChart()
		{
			InitializeComponent();
		}

		private void btnstart_Click( object sender , RoutedEventArgs e )
		{
			ChartValues<double> chartDatas1 = new ChartValues<double>();
			ChartValues<double> chartDatas2 = new ChartValues<double>();
			var rnd = new Random();
			var datas1 = Enumerable.Range( 0 , 100 ).Select( x => ( double )( x + rnd.Next( 10 ) ));
			var datas2 = Enumerable.Range( 0 , 100 ).Select( x => ( double )( x/2 + rnd.Next( 5 ) ));
			chartDatas1.AddRange( datas1 );
			chartDatas2.AddRange( datas2 );


			SeriesCollection src = new SeriesCollection();

			LineSeries temp1 = new LineSeries();
			LineSeries temp2 = new LineSeries();
			temp1.Values = chartDatas1;
			temp2.Values = chartDatas2;
			temp1.DataLabels = false;
			temp1.PointGeometrySize = 0;

			src.Add( temp1 );
			src.Add( temp2 );
			chartmain.Series = src;

		}
	}
}
