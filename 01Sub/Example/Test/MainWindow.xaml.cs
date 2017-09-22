using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Test
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public double[] curData = new double[] { };
		public ChartValues<double> ChartDatas { get; set; }
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			
		}


		private void btnstart_Click( object sender , RoutedEventArgs e )
		{
			var r = new Random();
			ChartDatas = new ChartValues<double>();
			//chart.ChartDatas = new ChartValues<double>();
			Task.Run( () =>
			{
				for ( int i = 0 ; i < 10 ; i++ )
				{
					ChartDatas.Clear();
					curData = new double [ ] { r.NextDouble() , r.NextDouble() , r.NextDouble() , r.NextDouble() , i };
					ChartDatas.AddRange( curData );
					Thread.Sleep( 700 );
					Console.WriteLine( "Chart Set Data Real time" );
				}
			} );
			
			Task.Run( ( Action )chart.SetDatasReadlTime );
			chart.ContinusInsert = true;
			chart.CurrentData = curData;
		}
	}
}
