using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThicknessAndComposition_Inspector_IPS_Data;
using static System.Linq.Enumerable;
using SpeedyCoding;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using ModelLib.Data;
	using ModelLib.Data.NewType;
	using ModelLib.AmplifiedType;
	using static System.IO.Path;
	using System.IO;
	using static IPSDataHandler.Handler;
	using static BaseFunc_Analysis;

	public class Core_Analysis
	{
		public enum ResultType { Thckness, Intensity, Reflectivity }
		List<IPSResultData> ResultData;
		Dictionary<int,IPSResultData> SelectedData;


		public Core_Analysis()
		{
			ResultData = new List<IPSResultData>();
			SelectedData = new Dictionary<int,IPSResultData>();
			// Reset
		}

		public List<IPSResultData> ScarpIpsResult(string path)
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
				var posThickness = File.ReadAllLines( resPath )
										 .ResultRefine(0)
										 .ToPosThickness();
			
				var rftList  = File.ReadAllLines( rftPath )
							    .ResultRefine(1)
							    .ToReflectivity();

				var rawList  = File.ReadAllLines( rawPath )
								.ResultRefine(4)
								.ToIntensity();

				var scanResult = Range( 0 , posThickness.Count() )
								.Map( i => new IPSResultData
								{
									Position = posThickness[i].Item1 ,
									Thickness = posThickness[i].Item2,
									IntenList = rawList[i] ,
									Reflectivity = rftList[i]
								} ).ToList();

				return scanResult; 

			}
			return null;

		
		}

		private void SelectionTaker(int idx)
		{
			SelectedData [ idx ] = ResultData [ idx ];




			// extract each data With Data

			//Display 


		}

		private void DeselectionTaker( int idx )
		{
			SelectedData.Remove ( idx );


			// extract each data With Data

			//Display 


		}

		public bool IsValid( mCrtCrd pos )
			=> pos.Match(
				() => false ,
				X => true );
		public bool Invalid( WaveLength [ ] wlen )
			=> wlen.Where( x => x.Value == None )




		// 이 창은 만들어 질때, 1. 웨이퍼 그림 2. 위치마크 + 이벤트 3.업데이트 result 로 태그 그리기 . 
		// UpdateResult( IPSResult ) => 이게 실행시 태그의 값들이 바뀐다
		// 각각의 로우 데이터는 IPSResult에 담겨 있다.
		/*
		 *  |
		 * |
		 * 
		 * 
		 * 
		 */
	}

	public struct IPSResultData
	{
		public Maybe<Intensity[]> IntenList;
		public Maybe<Reflectivity[]> Reflectivity;
		public Maybe<Thickness> Thickness;
		public mCrtCrd Position;

		private static WaveLength[] _WaveLegth;

		public WaveLength [ ] WaveLegth {
			get
			{
				if ( _WaveLegth == null ) _WaveLegth = new WaveLength [ ] { };
				return _WaveLegth;
			}
			set
			{
				if ( _WaveLegth == null ) _WaveLegth = value;
			} }
	}

	public static class AnalysisHelper
	{

	}



}
