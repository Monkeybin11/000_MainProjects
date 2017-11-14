using Microsoft.Win32;
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
using ThicknessAndComposition_Inspector_IPS_Core;
using ThicknessAndComposition_Inspector_IPS_Data;


namespace ThicknessAndComposition_Inspector_IPS
{
	
	/// <summary>
	/// Interaction logic for Win_ResultAnalysis.xaml
	/// </summary>
	public partial class Win_ResultAnalysis : Window
	{
		IPSResult ResultData;

		public Win_ResultAnalysis()
		{
			Console.WriteLine( "In1" );
			InitializeComponent();

			//AnalysisCore ACore=  new AnalysisCore();

		}

		public Win_ResultAnalysis( System.Drawing.Bitmap img , IPSResult result )
		{
			Console.WriteLine( "In1" );

			InitializeComponent();

			var src = img.BitmapToBitmapSource(); // Uc로 
			
			ResultData = result; // UC로 -> 버튼 



		}

		public void UpdateResultData()
		{

		}






		private void btnLoad_Click( object sender , RoutedEventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if ( ofd.ShowDialog() == true )
			{


			}
		}
	}
}
