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
using System.ComponentModel;

namespace LiveCharts_Example
{
	/// <summary>
	/// Interaction logic for BindingExample1.xaml
	/// </summary>
	public partial class BindingExample1 : Window , INotifyPropertyChanged
	{
		TestData _Tdata;

		public TestData Tdata {
			get { return _Tdata; }
			set {
				_Tdata = value;
				OnPropertyChanged( "Tdata" );
			} }

		public string _Counter;
		public string Counter {
			get { return _Counter; }
			set
			{
				_Counter = value;
				OnPropertyChanged( "Counter" );
			} }

		int i;

		

		public BindingExample1()
		{
			InitializeComponent();

			Loaded += delegate
			{
				Tdata = new TestData();
			};
			

			
			DataContext = this;
		}

		private void btnTest_Click( object sender , RoutedEventArgs e )
		{
			Counter = i++.ToString();
			Tdata.RandomNum = DateTime.Now.ToString();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged( string propertyname = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyname ) );
		}


	}


	public class TestData : INotifyPropertyChanged
	{
		public string RandomNum {

			get {

				Random rnd = new Random();
				return rnd.Next().ToString(); }

			set
			{
				OnPropertyChanged( "RandomNum" );
			} }

	public event PropertyChangedEventHandler PropertyChanged;

	public void OnPropertyChanged( string propertyname = null )
	{
		if ( PropertyChanged != null )
			PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyname ) );
	}
}

}
