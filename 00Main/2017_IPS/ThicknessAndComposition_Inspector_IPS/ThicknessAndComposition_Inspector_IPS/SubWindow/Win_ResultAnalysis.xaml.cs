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
using ModelLib.AmplifiedType;
using SpeedyCoding;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;

namespace ThicknessAndComposition_Inspector_IPS
{
	using IPSAnalysis;

	using static Core_Helper;
	using static System.IO.Path;
	using static StateCreator;
	using static IPSAnalysis.Handler;
	using static ModelLib.AmplifiedType.Handler;
	using ModelLib.Data;
	using static IPSAnalysis.AnalysisFunc;
	using static Adaptor;


	/// <summary>
	/// Interaction logic for Win_ResultAnalysis.xaml
	/// </summary>
	public partial class Win_ResultAnalysis : Window
	{
		AnalysisState State;
		public event Action evtClose;
		IPSResultData[] ResultDataLib;
		Func < AnalysisState , Maybe<AnalysisState>> UpdateState;


		public Win_ResultAnalysis( BitmapSource img , Maybe<IPSResult> result ) 
		{
			InitializeComponent();

			var defualtImg = new Image<Gray,byte>(100,100, new Gray(100) );

			ucAnalysisMap.evtClickedIndex += InOutUpdate; // Update Event

			result.Match(
					() =>  Just( defualtImg.ToBitmapSource() )
							.ForEach( ucAnalysisMap.SetImage ) ,

					res => Just( res )
							.ForEach( ucAnalysisMap.SetBtnTag )
							.Lift( x => CreateMapImg( img )( res ) )
							.ForEach( ucAnalysisMap.SetImage ));

			ResultDataLib = null; // result.ToResultData();
		}

		private void btnLoad_Click( object sender , RoutedEventArgs e )
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Csv Files (.csv)|*.csv|All Files (*.*)|*.*";
			if ( ofd.ShowDialog() == true )
			{
				var res = ResultDataFrom( ofd.FileName ); //  여기부터 체크를 해야한다. 
				if ( res.isJust ) // Error Handle. Change this Flow to railroad o style
				{
					int i = 0;

					// Not handle Error Value. Need Change Flow for Error in future
					var ipsRes = CreateState( res.Value.ToDictionary( x => i++ ) )
										.Act( x => State = x )
										.Map( Adaptor.ToIPSResult )
										.Act( x => ucAnalysisMap.SetBtnTag(x))
										.Map( x => CreateMap( x , 6 ) )
										.Map( x => x.Item1[0].ToBitmapSource() )
										.Act( x => ucAnalysisMap.SetImage(x) );
				}
				else
				{
					MessageBox.Show( "File is not valid. please select other file" );
				}
			}
		}

		#region Main Function

		void InOutUpdate( string idx , MsgType msg )
		{
			int index;
			if ( !int.TryParse( idx , out index ) )
			{
				MessageBox.Show( " Loaded File is Worng Index " );
				return;
			}

			UpdateState = state => ChangeState( state , index , msg );

			Just( State )
				.Bind( UpdateState ) //Need to Change this. Neew Apply partial apply for pure 
				.Bind( RefreshState )
				.ForEach( SendState ); // 상태 전파
		}



		#endregion

		#region Sub Function
		AnalysisState ChangeState( AnalysisState state , int idx , MsgType msg , double[] minmax = null)
		{
			// Change State
			switch ( msg )
			{
				case MsgType.Add:
					return Add( state , ResultDataLib [ idx ] , idx );

				case MsgType.Remove:
					return Pop(state , idx);

				case MsgType.ChangeWav:
					return ChangeWaveLen( state , minmax );

				default:
					return default( AnalysisState );
			}
		}


		Maybe<AnalysisState> RefreshState( AnalysisState newstate )
		{
			State = newstate;
			return State;
		}

		void SendState( AnalysisState state ) // side func
		{
			State = state;
			
			//여기에서 갱신된 상태를 보내야 한다. <<<<<<<<<<<<<

			// send to reflect graph 
			// send to inten graph 
		
		}

		// Load Data -> State


		Func<IPSResult , BitmapSource> CreateMapImg( Maybe<BitmapSource> img )
			=> res => img.Match(
						() => CreateMap( res , 6 ).Item1 [ 0 ].ToBitmapSource() ,
						thisimg => thisimg );


		#endregion

		private void Window_Closing( object sender , System.ComponentModel.CancelEventArgs e )
			 =>evtClose();
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
