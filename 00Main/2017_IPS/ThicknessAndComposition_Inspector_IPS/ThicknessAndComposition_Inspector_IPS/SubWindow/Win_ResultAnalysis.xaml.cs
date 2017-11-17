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
	using IPSAnalysis;
	using ModelLib.AmplifiedType;
	using static Core_Helper;
	using static System.IO.Path;
	using static StateCreator;
	using static IPSAnalysis.Handler;
	using static ModelLib.AmplifiedType.Handler;
	using ModelLib.Data;

	/// <summary>
	/// Interaction logic for Win_ResultAnalysis.xaml
	/// </summary>
	public partial class Win_ResultAnalysis : Window
	{
		AnalysisState State;
		public event Action evtClose;
		IPSResult ResultData;

		public Win_ResultAnalysis( Maybe<BitmapSource> img , Maybe<IPSResult> result ) 
		{
			InitializeComponent();
			var defualtImg = new Image<Gray,byte>(100,100, new Gray(100) );

			result.Match(
					() =>  Just( defualtImg.ToBitmapSource() )
							.ForEach( ucAnalysisMap.SetImage ) ,

					res => Just( res )
							.ForEach( ucAnalysisMap.SetBtnTag )
							.Map( x => CreateMapImg( img )( res ) )
							.ForEach( ucAnalysisMap.SetImage ));
		}

		Func<IPSResult , BitmapSource> CreateMapImg( Maybe<BitmapSource> img )
			=> res => img.Match(
						() => CreateMap( res , 6 ).Item1 [ 0 ].ToBitmapSource() ,
						thisimg => thisimg );

		void InitializeState()
		{
			// 그림 있는지
			// 그림 
		}

		void Update( MsgType msg , int idx )
		{
			AnalysisState newState;
			switch ( msg )
			{
				case MsgType.Add:

					break;

				case MsgType.Remove:


					break;


				case MsgType.ChangeWav:


					break;
			}

			// Trans New State
		}


		public void UpdateResultData()
		{

		}

		// Load Data -> State
		private void btnLoad_Click( object sender , RoutedEventArgs e )
		{
			try
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.Filter = "Csv Files (.csv)|*.csv|All Files (*.*)|*.*";
				if ( ofd.ShowDialog() == true )
				{
					var res = StateFrom( ofd.FileName ); //  여기부터 체크를 해야한다. 
					if ( res.isJust )
					{
						int i = 0;
						State = CreateState( res.Value.ToDictionary( x => i++ ) ); // Set State
						var ipsRes = State.ToIPSResult();
						// DrawMap
						CreateMap( ipsRes , 6 ).Item1 [ 0 ].ToBitmapSource()
							//.Act( x => ucAnalysisMap.SetImage( x ) )
							.Act( x => ucAnalysisMap.SetBtnTag(ipsRes)); // 여기서부터 버튼 태그 생성 이벤트 발생 
					}
					else
					{
						MessageBox.Show( "File is not valid. please select other file" );
					}
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show( ex.ToString() );
			}
			
		}

		private void Window_Closing( object sender , System.ComponentModel.CancelEventArgs e )
		{
			evtClose();
		}
	}

	public static class Adaptor
	{
		public static IPSResult ToIPSResult(
			this AnalysisState self )
		{
			List<SpotData> spotlist;
			var temp = self.State[0].WaveLegth.ToList();
			List<double> WaveLen = self.State[0].WaveLegth.Select( x => (double)x).ToList();

			spotlist = self.State.Select( x =>

				new SpotData
				(   x.Value.Position.Pos.Value.ToPolar() as PlrCrd,
					x.Value.Thickness.Value.Value ,
					x.Value.IntenList.Select( f => (double)f ).ToArray() ,
					x.Value.Reflectivity.Select( f => ( double )f ).ToArray() ) ).ToList();

			var res = new IPSResult(WaveLen) { SpotDataList = spotlist  };
			return res;
		}

	}

}
