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
using ThicknessAndComposition_Inspector_IPS_Core;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		IPSCore Core; 


		public MainWindow()
		{
			InitializeComponent();

			ucLSMenu.evtBtn += new BtnEvt( LeftSideBtn );
			Core = new IPSCore();
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

				case "menuSetSpec":
					break;

				case "menuSetStage":
					break;

				default:
					break;
			}
		}



		public void LeftSideBtn( string name )
		{
			switch ( name )
			{
				case "btnStart":
					Core.TestFunction();
					// Main Set
					// Main Start

					break;

				case "btnLoad":
					break;

				case "btnSaveRes":
					break;

				case "btnSaveRaw":
					break;

				default:
					break;
			}

		}





		private void Window_Loaded( object sender , RoutedEventArgs e )
		{
			// Load Config ( This Config include Scan Config Path and File name)
			// Load Scan Config		
		}
	}
}
