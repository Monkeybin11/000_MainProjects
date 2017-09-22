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
using VisualizeLib;
using SpeedyCoding;
using ApplicationUtilTool.FileIO;
using Accord.Statistics;
using Accord.Statistics.Models.Regression.Linear;
using Accord.Math.Optimization.Losses;
using System.IO;

namespace Wafer_Visualize
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

		private void btnLoadCsv_Click( object sender , RoutedEventArgs e )
		{
			try
			{

			
			var filepath = Dialog.OpenFileDia();
			if ( filepath == "" ) return;

			CsvTool cv = new CsvTool();

			var res  = cv.ReadCsv2String( filepath , ',' );
			double head = 0;
			if ( !Double.TryParse( res [ 0 ] [ 0 ] , out head ) ) res = res.Skip( 1 ).ToArray();

			var numberList = res.Select( x => x.Select( f => f.ToDouble() ).ToArray() ).ToArray();
			if ( numberList.SelectMany(
										x => x.Select( k => k ) ,
										( f , s ) => s )
							.Where( x => x == 123456789 )
							.Count() > 0 )
			{
				Console.WriteLine( "Invalid string number is contained." );
				return;
			}

			var inputs = numberList.Select( x => new double[] { x[0] , x[1] } ).ToArray();
			var outputs = numberList.Select( x => x[2] ).ToArray();


				//  LS방식으로는 잘 안되는것 같다. ......  어코드에서는 다른 알고리즘은 지원을 안하기 때문에, 일단 테스트를 해보고, 잘 맞는 알고리즘을 c#으로 구현을 해야 될 것 같다. 

			var ols = new OrdinaryLeastSquares()
			{
				UseIntercept = true
			};
			MultipleLinearRegression regression = ols.Learn(inputs,outputs);

			double[] predicted = regression.Transform(inputs);

			double error = new SquareLoss(outputs).Loss(predicted);


			var testdata = GridZ(300 , 15);
			var zdata = regression.Transform(testdata);

			var z2ddata = zdata.Reshape(300 , 300) ;

			var jagged = z2ddata.ToJagged();

			StringBuilder sb = new StringBuilder();

			foreach ( double[] item in jagged )
			{
				foreach ( var el in item )
				{
					sb.Append( el.ToString() );
					sb.Append( ",");
				}
				sb.Append( Environment.NewLine );
			}


			File.WriteAllText( @"E:\Temp\haha2.csv" , sb.ToString() );


			Console.WriteLine("done");



			}
			catch ( Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}



		}

		private void btnSaveImg_Click( object sender , RoutedEventArgs e )
		{

		}



		double [ ] [ ] GridZ( int count , int max)
		{
			var step = max / count;
			return Enumerable.Range( 0 , count ).SelectMany(
				x => Enumerable.Range( 0 , count ) ,
				( f , s ) => new double [ ] { f , s } ).ToArray();
		}

	}

	public static class ext
	{
		public static double ToDouble( this string src )
		{
			double output = 0;
			if ( src == "" ) return 0;
			return double.TryParse( src , out output ) ? output : 123456789;

		}

	}
}
