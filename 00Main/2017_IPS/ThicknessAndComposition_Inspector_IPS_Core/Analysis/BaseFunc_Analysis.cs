using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.AmplifiedType.Handler;
using static IPSDataHandler.Handler;
using SpeedyCoding;
using ModelLib.Data;
using ModelLib.Data.NewType;
using ModelLib.AmplifiedType;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using ModelLib.Data;
	using ModelLib.Data.NewType;
	using ModelLib.AmplifiedType;
	using static System.IO.Path;
	using static IPSDataHandler.Handler;
	using static ModelLib.AmplifiedType.Handler;
	using static BaseFunc_Analysis;
	using IPSAnalysis;
	using static IPSAnalysis.Handler;
	using System.IO;
	using static System.Linq.Enumerable;

	public static class StateCreator
	{
		public static event Action<mCrtCrd ,Intensity[] > evtIntenList;
		public static event Action<mCrtCrd , Reflectivity[] > evtRflectList;
		public static event Action<mCrtCrd , Thickness> evtThickness;
		public static event Action<mCrtCrd , IPSResultData> evtResult;
		//public event Action<> evtThickness;

		public static Maybe<List<IPSResultData>> ResultDataFrom( string path )
		{
			var headname = path.Split('_').First();
			var basepath =  GetDirectoryName(path);

			var resPath = headname + "_Result.csv";
			var rftPath = headname + "_Reflectivity.csv";
			var rawPath = headname + "_Raw.csv";

			string[] resStr = new string[] { };
			string[] rftStr = new string[] { };
			string[] rawStr = new string[] { };

			if ( File.Exists( resPath )
				&& File.Exists( rftPath )
				&& File.Exists( rawPath ) )
			{
				// missing Data => Exit Flow


				var posThickness = File.ReadAllLines( resPath )
										 .ResultRefine(0)
										 .ToPosThickness()
										 .ToArray();

				var wavLis = File.ReadAllLines( rftPath )
								.ColumnRead(0)
								.ToWaveLen()
								.ToArray();


				// missing Data => interpolation 

				var rftList  = File.ReadAllLines( rftPath )
								.ResultRefine(1)
								.ToReflectivity()
								.ToArray()
								.Transpose();

				var rawList  = File.ReadAllLines( rawPath )
								.ResultRefine(4)
								.ToIntensity()
								.ToArray()
								.Transpose();

				var scanResult = Range( 0 , posThickness.Count() )
								.Map( i => new IPSResultData
								{
									Position = posThickness[i].Item1 ,
									WaveLegth = wavLis ,
									Thickness = posThickness[i].Item2,
									IntenList = rawList[i] ,
									Reflectivity = rftList[i]
								} ).ToList();

				return Just( scanResult );
			}
			return None;
		}

		static Action<IPSResultData> TransAllData
			=> x =>
			{
				evtThickness( x.Position , x.Thickness );
				evtRflectList( x.Position , x.Reflectivity );
				evtIntenList( x.Position , x.IntenList );
			};

		private static bool IsValid( mCrtCrd pos )
			=> pos.Match(
				() => false ,
				X => true );

		private static bool IsValid( WaveLength [ ] wlen )
			=> wlen.Where( x => x.Value.isJust ).Count() > 0 ? false : true;

		private static bool IsValid( Intensity [ ] wlen )
		=> wlen.Where( x => x.Value.isJust ).Count() > 0 ? false : true;
	}

	public struct IPSResultData
	{
		public Intensity[] IntenList;
		public Reflectivity[] Reflectivity;
		public Thickness Thickness;
		public mCrtCrd Position;

		private static WaveLength[] _WaveLegth;

		public WaveLength [ ] WaveLegth
		{
			get
			{
				if ( _WaveLegth == null ) _WaveLegth = new WaveLength [ ] { };
				return _WaveLegth;
			}
			set
			{
				if ( _WaveLegth == null ) _WaveLegth = value;
			}
		}

		public IEnumerable<double> DInenList
		{
			get
			{
				foreach ( var item in IntenList )
					yield return item;
			}
		}

		public IEnumerable<double> DReflectivity
		{
			get
			{
				foreach ( var item in Reflectivity )
					yield return item;
			}
		}

		public IEnumerable<double> DWaveLength
		{
			get
			{
				foreach ( var item in _WaveLegth )
					yield return item;
			}
		}

		public double DThicckness => Thickness;



	}
	public static class BaseFunc_Analysis
	{
		public static IEnumerable<string> ColumnRead(
			this IEnumerable<string> src ,
			int colNum ,
			int headerSkip = 0)
			=> src.Skip(headerSkip).Map( x => x.Split( ',' ) [ colNum ] ); 

		public static IEnumerable<string[]> ResultRefine( 
			this IEnumerable<string> src ,
			int skipnum )
			=> src.Skip( 1 ).Map( x => x.Split( ',' ).Skip( skipnum ).ToArray() );

		// NewType is Bad Idea on this situation. need to fix this. But explicitivity is good

		public static Tuple<mCrtCrd , Thickness> [ ] ToPosThickness(this IEnumerable<string [ ]> src )
			=> src.Map( x => Tuple.Create(
								mCrtCrd( ParseToDouble( x [ 0 ] ) , ParseToDouble( x [ 1 ] ) ) ,
								Thickness( ParseToDouble( x [ 2 ] ) ) ) )
					.ToArray();

		public static Reflectivity [ ] [ ] ToReflectivity(
			this IEnumerable<string [ ]> self ) 
			=> self.Select( f =>
					f.Select( s => ( Reflectivity ) ParseToDouble( s ) ).ToArray() )
					.ToArray();

		public static Intensity [ ] [ ] ToIntensity(
			this IEnumerable<string [ ]> self )
			=> self.Select( f =>
					f.Select( s => ( Intensity )ParseToDouble( s ) ).ToArray() )
					.ToArray();

		public static WaveLength [ ] ToWaveLen(
			this IEnumerable<string> self )
			=> self.Select( f => ( WaveLength ) ParseToDouble( f ) ).ToArray();


		

	}
}
