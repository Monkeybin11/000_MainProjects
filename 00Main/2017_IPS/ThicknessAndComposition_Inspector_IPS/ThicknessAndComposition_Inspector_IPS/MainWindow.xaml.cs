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

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}


		public void OptionMenuClick( object sender , RoutedEventArgs e )
		{
			var master = sender as Button;
			switch ( master.Name )
			{
				case "menuLoadConfig":
					break;

				case "menuCreateConfig":
					break;

				default:
					break;
			}
		}

	}
}
