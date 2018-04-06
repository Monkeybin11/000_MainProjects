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

namespace WPF_ContextMenu
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
	
		private void cvsBlack_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			ContextMenu menu = new ContextMenu();

			MenuItem m1 = new MenuItem();
			MenuItem m2 = new MenuItem();

			m1.Header = "M1";
			m2.Header = "M2";

			m1.Click += (ss, ee) =>
			{
				//reset
			};

			m2.Click += (ss, ee) =>
			{
				//back
			};

			menu.Items.Add(m1);
			menu.Items.Add(m2);

			menu.IsOpen = true;


		}

		private void cvsdouble_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			ContextMenu menu = new ContextMenu();

			MenuItem m1 = new MenuItem();
			MenuItem m2 = new MenuItem();
			MenuItem m11 = new MenuItem();
			MenuItem m12 = new MenuItem();

			m1.Header = "M1";
			m2.Header = "M2";
			m11.Header = "M11";
			m12.Header = "M12";

			m1.Click += (ss, ee) =>
			{
				//reset
			};

			m2.Click += (ss, ee) =>
			{
				//back
			};

			m1.Items.Add(m11);
			m1.Items.Add(m12);

			menu.Items.Add(m1);
			menu.Items.Add(m2);

			menu.IsOpen = true;
		}
	}
}
