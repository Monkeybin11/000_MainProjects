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
			ChartValues<double> ChartDatas = new ChartValues<double>();

			var rnd = new Random();
			var datas = Enumerable.Range( 0 , 100 ).Select( x => ( double )( x + rnd.Next( 10 ) ));
			ChartDatas.AddRange( datas );
			srsMain.Values = ChartDatas;

		}
	}
}
