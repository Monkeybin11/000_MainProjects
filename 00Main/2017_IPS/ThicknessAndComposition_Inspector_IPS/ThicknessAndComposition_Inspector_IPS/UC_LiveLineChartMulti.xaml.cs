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
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for UC_LiveLineChartMulti.xaml
	/// </summary>
	public partial class UC_LiveLineChartMulti : UserControl , INotifyPropertyChanged
	{
		SeriesCollection SeriesList = new SeriesCollection();
		List<Brush> SeriesColors = new List<Brush>();

		public UC_LiveLineChartMulti()
		{
			InitializeComponent();



			var mapper = Mappers.Xy<double[]>()
				.X(model => model[0])            //use DateTime.Ticks as X
                .Y(model => model[1]);           //use the value property as Y

			Charting.For<double [ ]>( mapper );
			axisY.MaxValue = 60000;
			axisY.MinValue = 0;

			SeriesColors = typeof (Brushes).GetProperties().
												Select(p => p.GetValue(null) as Brush ).
												ToList();
			DataContext = this;

		}

		int ColorCounter = 10;
		public void AddNewSeries( IEnumerable<double> datas , IEnumerable<double> labels )
		{
			chtLiveLine.LegendLocation = LegendLocation.None;
			//ChartDatas.Clear();
			var dts = datas.ToArray();
			var lbls = labels.ToArray();
			dts [ 0 ] = dts [ 2 ];
			dts [ 1 ] = dts [ 2 ];

			ChartValues<double[]> chartDatas = new ChartValues<double[]>();
			chartDatas.AddRange(
				Enumerable.Range( 0 , datas.Count() )
				.Where( ( _ , i ) => i % 10 == 0 )
				.Select( x => new double [ 2 ] { lbls [ x ] , dts [ x ] } )
				);

			this.Dispatcher.BeginInvoke( ( Action )( () =>
			{
				var newseries = new LineSeries();
				newseries.Values = chartDatas;
				newseries.DataLabels = false;
				newseries.PointGeometrySize = 0;
				newseries.Fill = Brushes.Transparent;
				newseries.Foreground = SeriesColors [ ColorCounter++ ];
				var src = new SeriesCollection();

				src.Add( newseries );

				foreach ( var item in SeriesList )
				{
					src.Add( item );
				}
				SeriesList = src;
				this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesList ) );

			} ) );
		}

			public void AddNewSeries( IEnumerable<double> datas , IEnumerable<double> labels , string title , int colorcounter = -1)
		{
			this.Dispatcher.BeginInvoke((Action)(()=> chtLiveLine.LegendLocation = LegendLocation.Right ));
			//ChartDatas.Clear();
			var dts = datas.ToArray();
			var lbls = labels.ToArray();
			dts [ 0 ] = dts [ 2 ];
			dts [ 1 ] = dts [ 2 ];

			ChartValues<double[]> chartDatas = new ChartValues<double[]>();
			chartDatas.AddRange(
				Enumerable.Range( 0 , datas.Count() )
				.Where( ( _ , i ) => i % 10 == 0 )
				.Select( x => new double [ 2 ] { lbls [ x ] , dts [ x ] } )
				);

			this.Dispatcher.BeginInvoke( ( Action )( () => {
				var newseries = new LineSeries();
				newseries.Values = chartDatas;
				newseries.DataLabels = false;
				newseries.PointGeometrySize = 0;
				newseries.Fill = Brushes.Transparent;

				if(colorcounter > 0) newseries.Foreground = SeriesColors [ colorcounter ];
				else newseries.Foreground = SeriesColors [ ColorCounter++ ];
				newseries.Title = title;
				var src = new SeriesCollection();
				//newseries.Foreground = Brushes.BlueViolet;

				src.Add( newseries );

				foreach ( var item in SeriesList )
				{
					src.Add( item );
				}
				SeriesList = src;
				this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesList ) );

			} ) );




		}

		public void ClearSeries()
		{
			SeriesList = new SeriesCollection();
			this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesList ) );
		}


		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged( string propertyName = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
		}

	}
}
