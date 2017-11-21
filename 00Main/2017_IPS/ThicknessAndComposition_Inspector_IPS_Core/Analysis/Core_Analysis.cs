using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ThicknessAndComposition_Inspector_IPS_Data;
using static System.Linq.Enumerable;
using Unit = System.ValueTuple;
using static System.ValueTuple;
using SpeedyCoding;


/* State Manipulation Part  */

namespace IPSAnalysis
{
	using ThicknessAndComposition_Inspector_IPS_Core;
	using static IPSAnalysis.Handler;

	public static partial class Handler
	{
		public static AnalysisState CreateState
			( Dictionary<int , IPSResultData> dict , 
			  double [ ] waveMinMax = null )
			=> new AnalysisState( dict , waveMinMax );
	}

	public class AnalysisState
	{
		public Dictionary<int,IPSResultData> State;
		public static double[] WaveMinMax;
		private double[] _waveMinMax;

		public AnalysisState( Dictionary<int , IPSResultData> dict , double [ ] waveMinMax = null )
		{
			State = dict;
			_waveMinMax = WaveMinMax == null
							? waveMinMax
							: WaveMinMax;
		}
	}

	public static class AnalysisFunc
	{
		public static AnalysisState Add( AnalysisState state , IPSResultData data , int idx )
			=> state.State.ContainsKey( idx )
				? state
				: state.Insert( data , idx );

		public static AnalysisState Pop( AnalysisState state , int idx )
			=> state.State.ContainsKey( idx )
					? state.Remove( idx )
					: state;

		public static AnalysisState ChangeWaveLen( AnalysisState state , double [ ] minmax )
			=> CreateState( state.State , minmax );

		public static AnalysisState Insert(
			this AnalysisState self , IPSResultData data , int idx )
		{
			self.State [ idx ] = data;
			return self;
		}

		public static AnalysisState Remove(
			this AnalysisState self , int idx )
		{
			self.State.Remove( idx );
			return self;
		}

		#region IO

		public static Intensity [ ] OfIntensity
			( this AnalysisState self , int idx )
			=> self.State.ContainsKey( idx )
				? self.State [ idx ].IntenList
				: null;

		public static Reflectivity [ ] OfRefelctivity
			( this AnalysisState self , int idx )
			=> self.State.ContainsKey( idx )
				? self.State [ idx ].Reflectivity
				: null;

		public static Thickness OfThickness
		( this AnalysisState self , int idx )
		=> self.State.ContainsKey( idx )
			? self.State [ idx ].Thickness
			: null;

		public static mCrtCrd OfPosition
		( this AnalysisState self , int idx )
		=> self.State.ContainsKey( idx )
			? self.State [ idx ].Position
			: null;

		#endregion
	}
}

