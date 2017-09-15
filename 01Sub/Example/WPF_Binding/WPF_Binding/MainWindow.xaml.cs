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
using BindingTestLib;
using Xceed.Wpf.Toolkit;
using System.ComponentModel;

namespace WPF_Binding
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window 
	{
		public Core CR { get; set; }
		public int mainspeed { get; set; }
		public string haha = "aha";
		public MainWindow()
		{
			
			InitializeComponent();
			DataContext = this;
			CR = new Core();



		}

		private void Check_Click( object sender , RoutedEventArgs e )
		{
			Console.WriteLine( "" );
			Console.WriteLine( "Main" );
			Console.WriteLine( mainspeed );
			Console.WriteLine( "----Second---");
			Console.WriteLine( CR.secondspeed );
			Console.WriteLine( "----Last---" );
			Console.WriteLine( CR.TestData.Speed );
			Console.WriteLine( "" );
		}

		int counter = 0;

		private void btnspeed1_Click( object sender , RoutedEventArgs e )
		{
			mainspeed = counter++;
		}

		private void btnspeedmain_Click( object sender , RoutedEventArgs e )
		{
			CR.TestData.Speed = counter++;
		}
	}
}
