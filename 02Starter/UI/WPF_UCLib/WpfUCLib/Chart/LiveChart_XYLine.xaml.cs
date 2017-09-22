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
using System.ComponentModel;
using LiveCharts.Configurations;
using System.Threading;

namespace WpfUCLib.Chart
{
	public partial class LiveChart_XYLine : UserControl, INotifyPropertyChanged
	{
		#region ChartValue

		public double yMin;
		public double yMax;
		public double xMin;
		public double xMax;
		public double[] CurrentData;
		public double[] XLabels;
		public Func<double , string> LabelXFormat { get; set; }
		public Func<double , string> LabelYFormat { get; set; }
		//public ChartValues<double> ChartDatas { get; set; }

		#region XY Min Max


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

		public double AxisXStep { get; set; }
		public double AxisYStep { get; set; }

		public bool ContinusInsert;
		#endregion

		public LiveChart_XYLine()
		{
			InitializeComponent();
			//DataContext = this;
			SettingChart();
			CurrentData = new double [ ] { };
			//ChartDatas = new ChartValues<double>(); // Cause of problem
			//XLabels = new double [ ] { };
			//DataContext = this;
		}

		public void SetDatasReadlTime()
		{
			while ( true )
			{
				if ( ContinusInsert )
				{
					SetDatas( CurrentData );
					SetAxisLimit();
				}
				Thread.Sleep( 100 );
				Console.WriteLine( "Chart Set Data Real time" );
			}
		}

		public void SetDatas(double[] data )
		{
			//ChartDatas.Clear();
			//ChartDatas.AddRange( data );
		}

		public void SetAxisLimit()
		{
			

		}

		public void SetXYMampper() // If x values = index of CurrentData, Dont set mapper. 
		{ 
			var mapper = Mappers.Xy<double>()
				.X((x,i) => XLabels[i])     // Xs :: double[] 
                .Y((x,i) => x);             // Ys :: double[]  , Length of X Y is same
			Charting.For<double>( mapper );
		}

		public void SettingChart()
		{
			LabelXFormat = value => value.ToString( "##.##" ); // Xvalue = double => display is string 
			LabelYFormat = value => value.ToString( "##.##" );
			
		}

		public void ResetZoom()
		{
			X.MinValue = double.NaN;
			X.MaxValue = double.NaN;
			Y.MinValue = double.NaN;
			Y.MaxValue = double.NaN;
		}
		

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged( string propertyname = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyname ) );
		}
	}
}
