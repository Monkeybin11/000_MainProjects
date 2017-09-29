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
	/// Interaction logic for Columnchart_UCTEst.xaml
	/// </summary>
	public partial class Columnchart_UCTEst : Window
	{
		public Columnchart_UCTEst()
		{
			InitializeComponent();
		}

		private void btnstart_Click( object sender , RoutedEventArgs e )
		{
			uctest.CreateHistogram();
		}
	}
}
