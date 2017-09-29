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

namespace WPFDataGridExample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		List<test> dds = new List<test>();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnstart_Click( object sender , RoutedEventArgs e )
		{
			dgd.ItemsSource = Load();
		}

		List<test> Load()
		{
			List<test> datas = new List<test>();
			datas.Add( new test()
			{
				ID = 101 ,
				Name = "Mahesh Chand" ,
				BookTitle = "Graphics Programming with GDI+" ,
				DOB = new DateTime( 1975 , 2 , 23 ) ,
				IsMVP = false
			} );
			datas.Add( new test()
			{
				ID = 201 ,
				Name = "Mike Gold" ,
				BookTitle = "Programming C#" ,
				DOB = new DateTime( 1982 , 4 , 12 ) ,
				IsMVP = true
			} );
			datas.Add( new test()
			{
				ID = 244 ,
				Name = "Mathew Cochran" ,
				BookTitle = "LINQ in Vista" ,
				DOB = new DateTime( 1985 , 9 , 11 ) ,
				IsMVP = true
			} );
			return datas;
		}

	}


	public class test
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public DateTime DOB { get; set; }
		public string BookTitle { get; set; }
		public bool IsMVP { get; set; }
	}

}
