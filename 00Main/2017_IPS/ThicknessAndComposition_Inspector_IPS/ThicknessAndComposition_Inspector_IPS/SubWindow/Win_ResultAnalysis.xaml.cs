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
using ThicknessAndComposition_Inspector_IPS_Core;
using ThicknessAndComposition_Inspector_IPS_Data;
using SpeedyCoding;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;

namespace ThicknessAndComposition_Inspector_IPS
{
	using ModelLib.AmplifiedType;
	using static Core_Helper;
	using static System.IO.Path;

	/// <summary>
	/// Interaction logic for Win_ResultAnalysis.xaml
	/// </summary>
	public partial class Win_ResultAnalysis : Window
	{
		public event Action evtClose;
		IPSResult ResultData;

		//public Win_ResultAnalysis()
		//{
		//	Console.WriteLine( "In1" );
		//	InitializeComponent();
		//
		//	//AnalysisCore ACore=  new AnalysisCore();
		//
		//}

		public Win_ResultAnalysis( Maybe<BitmapSource> img , Maybe<IPSResult> result )
		{
			InitializeComponent();


			// result is maybe

			Action<IPSResult> dataExist = 
				res =>
				{
					//var src = img;
					//ResultData = result.Value; // UC로 -> 버튼 //  여기에는 리턴으로 Maybe에 있는 값을 가져와야 한다. 
				};
			var defualtImg = new Image<Gray,byte>(100,100, new Gray(100) );


			result.Match(
						() => defualtImg.ToBitmapSource() ,
						res => res.Act( x => ucAnalysisMap.CrerateScanPosBtn( x ) )
								  .Map( x => img.Match(
							() => CreateMap( res , 6 ).Item1 [ 0 ].ToBitmapSource() ,
							thisimg => thisimg ) ))
					.Act( x => ucAnalysisMap.SetupImage( x ) );

		}

		public void UpdateResultData()
		{

		}

		private void btnLoad_Click( object sender , RoutedEventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			if ( ofd.ShowDialog() == true )
			{
				Core_Analysis core = new Core_Analysis();
				core.ScarpIpsResult( ofd.FileName );
			}
		}

		private void Window_Closing( object sender , System.ComponentModel.CancelEventArgs e )
		{
			evtClose();
		}
	}
}
