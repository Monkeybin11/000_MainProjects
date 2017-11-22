﻿using Microsoft.Win32;
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


	/// <summary>
	/// Interaction logic for Win_ResultAnalysis.xaml
	/// </summary>
	public partial class Win_ResultAnalysis : Window
	{
		AnalysisState StateLib;
		public event Action evtClose;
		Func < AnalysisState , Maybe<AnalysisState>> UpdateState;


		public Win_ResultAnalysis( BitmapSource img , Maybe<IPSResult> result ) 
		{
			InitializeComponent();

			Loaded += delegate
			{
				var defualtImg = new Image<Gray,byte>(100,100, new Gray(100) );

				ucAnalysisMap.evtClickedIndex += InOutUpdate; // Update Event

				result.Match(
						() => Just( defualtImg.ToBitmapSource() )
								.ForEach( ucAnalysisMap.SetImage ),

						res => 
								Just( res )
								.Lift( SetStateLib )
								.ForEach( ucAnalysisMap.SetBtnTag )
								.Lift( x => CreateMapImg( img )( res ) )
								.ForEach( ucAnalysisMap.SetImage ) );
			};
		
			ucIntensityChart.SetExtractor( ExtractInten );
			ucReflectivityChart.SetExtractor( ExtractRflct );

			ucIntensityChart.lblTitle.Content = "Intensity";
			ucIntensityChart.axisY.Title = "Intensity";
			ucIntensityChart.Ysprtor.Step = 5000;

			ucReflectivityChart.lblTitle.Content = "Reflectivity";
			ucReflectivityChart.axisY.Title = "Reflectivity";
			ucReflectivityChart.Ysprtor.Step = 10;
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
					//CreateState( res.Value.ToDictionary( x => i++ ) )

					ucIntensityChart.Reset();
					ucReflectivityChart.Reset();
					

					CreateState( res.Value.ToDictionary( x => i++ ))
						.Map( SetStateLib )
						.Map( Adaptor.ToIPSResult )
						.Act( x => ucAnalysisMap.SetBtnTag(x) )
						.Map( x => CreateMapandBar( x , 6 ) )
						.Map( x => x.Item1[0].ToBitmapSource() )
						.Act( x => ucAnalysisMap.SetImage(x) );
				}
				else
				{
					MessageBox.Show( "File is not valid. please select other file" );
				}
			}
		}

		private void btnAlyBound_Click( object sender , RoutedEventArgs e )
		{
			var min = nudDown.Value.ToNonNullable();
			var max = nudUp.Value.ToNonNullable();

			InOutUpdate( MsgType.ChangeWav , 
						 minmax: new double [ ] { min , max } );
		}


		#region Initialize

		void SetChart()
		{
			ucReflectivityChart.axisY.MaxValue = 100;
			ucReflectivityChart.axisY.MinValue = 1;

			ucIntensityChart.axisY.MaxValue = 60000;
			ucIntensityChart.axisY.MinValue = 0;
		}

		#endregion

		#region Main Function

		void InOutUpdate( MsgType msg , string idx = null , double [ ] minmax = null )
		{
			UpdateState = state => msg == MsgType.ChangeWav
								? ChangeState( state , msg  , minmax : minmax )
								: ChangeState( state , msg  , idx );

			Just( StateLib )
				.Bind( UpdateState ) //Need to Change this. Neew Apply partial apply for pure 
				.Bind( RefreshState )
				.ForEach( SendState ); // 상태 전파
		}

		void SendState( AnalysisState state ) // side func
			=> state.Act( x => ucIntensityChart.UpdateSeries( x ) )
					.Act( x => ucReflectivityChart.UpdateSeries( x ) );
		#endregion

		#region Sub Function
		AnalysisState ChangeState( AnalysisState state , MsgType msg , string idx = null, double[] minmax = null)
		{
			
			int index = -1;
			if ( idx != null && !int.TryParse( idx , out index ) )
			{
				MessageBox.Show( " Loaded File is Worng Index " );
				return StateLib;
			}

			// Change State
			switch ( msg )
			{
				case MsgType.Add:
					return Add( state , StateLib.State [ index ] , index );

				case MsgType.Remove:
					return Pop(state , index );

				case MsgType.ChangeWav:
					return ChangeWaveLen( state , minmax );

				default:
					return default( AnalysisState );
			}
		}

		Maybe<AnalysisState> RefreshState( AnalysisState newstate )
		{
			StateLib = newstate;
			return StateLib;
		}

		Func<IPSResult , BitmapSource> CreateMapImg( Maybe<BitmapSource> img )
			=> res => img.Match(
						() => CreateMapandBar( res , 6 ).Item1 [ 0 ].ToBitmapSource() ,
						thisimg => thisimg );

		AnalysisState SetStateLib( AnalysisState state )
			=> StateLib = state;

		IPSResult SetStateLib( IPSResult result )
		{
			StateLib = result.ToState();
			return result;
		} 

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
					x.Value.DThickness ,
					x.Value.DIntenList.ToArray() ,
					x.Value.DReflectivity.ToArray() )).ToList();

			var res = new IPSResult(WaveLen) { SpotDataList = spotlist  };
			return res;
		}

		public static AnalysisState ToState(
			this IPSResult self )
		{
			var resState = new Dictionary<int, IPSResultData>();
			var wave = self.WaveLen;
			var count = self.SpotDataList.Count();


			foreach ( var spot in self.SpotDataList )
			{
				var dictdata = new IPSResultData(
					spot.IntenList,
					spot.Reflectivity,
					wave,
					spot.Thickness,
					new mCrtCrd( Just(spot.CrtPos.X) , Just(spot.CrtPos.Y) ));

				resState.Add( count++ , dictdata );
			}
			return CreateState(resState);
		}



	}

}
