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
using LiveCharts;
using LiveCharts.Wpf;

namespace LiveCharts_Example
{
	/// <summary>
	/// Interaction logic for FirstBasic.xaml
	/// </summary>
	public partial class FirstBasic : Window
	{
		public SeriesCollection ABC { get; set; }
		
		public FirstBasic()
		{
			
			DataContext = this; // same as elementname , Binding ElementName = 여기에 윈도우 이름 Path = 여기에 프로퍼티 이름 

			ABC = new SeriesCollection()
			{
				new LineSeries()
			 	{ Values = new LiveCharts.ChartValues<double> { 1,2,3,4}  },
				new LineSeries()
			 	{ Values = new LiveCharts.ChartValues<double> { 5,2,3,4}  }
			};

			InitializeComponent();
		}

		
	}
}
