using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SpeedyCoding;
using ModelLib.AmplifiedType;
using static ModelLib.AmplifiedType.Handler;
using FittingDataStruct;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using static FittingDataStruct.Handler;
	using static SpeedyCoding.Handler;
	using FileNames = IEnumerable<string>;
	using MssData = DatasAnMissing<List<double>>;
	using DPosThckRflt = PosThckRflt<double>;


	public static partial class DataLoader
	{
		static string RefFileName = "Reflectivity.csv";
		static string ResFileName = "Result.csv";

		public static void main( string path )
		{
			path = Path.GetDirectoryName( path );

			FileNames filenames = Just( path )
							.Bind( GetAllDirs )
							.Map( GetAllFileNames );

			if ( !CheckFiles( filenames ) ) return;

			var wave     = GetDataWith( ReadWaveLen , RfltFilter , filenames ).First().Datas.First().Value;
			var thckness = GetDataWith( ReadThikness , ThckFilter , filenames );
			var rflts    = GetDataWith( ReadReflectivity , RfltFilter , filenames );

			var Total = thckness.Zip( rflts , (f,s) => ToTuple( f  ,  s ) ).ToList();

			Func< Tuple<MssData,MssData> , List< PosThckData > > test;

			// 이 아래부터는 최적화 안된 코드. 나중에 고쳐야 한다. 

			List<List< DPosThckRflt >> totallist = new List<List<PosThckRflt<double>>>(); 

			for ( int i = 0 ; i < Total.Count ; i++ )
			{
				var item = Total[i];
				var thmiss = item.Item1.MissingIndex;
				var rfmiss = item.Item2.MissingIndex;

				var missing = thmiss.Concat(rfmiss).Distinct().OrderBy( x => x).Reverse();

				foreach ( var idx in missing )
				{
					item.Item1.Datas.RemoveAt( idx );
					item.Item2.Datas.RemoveAt( idx );
				}

				var newthck = item.Item1.Datas.Select( x => x.Value).ToList();
				var newrflt = item.Item2.Datas.Select( x => x.Value).ToList();

				var thckcount = newthck.Count();
				var rfltcount = newrflt.Count();

				if ( thckcount != rfltcount ) return;


				List<DPosThckRflt> output  = new List<DPosThckRflt>();

				for ( int j = 0 ; j < rfltcount ; j++ )
				{
					var ptrw = ToPosThckRflt<double>( newthck[j][0] , newthck[j][1] , newthck[j][2] , newrflt[j] , wave );
					
					output.Add( ptrw );
				}
				totallist.Add( output );
			}

			var flat = totallist.Flatten().ToList();

			// 포지션 두께 인텐시티 웨이브렝스 정보 정리됨. 
			// 이걸로 모델 트레이닝에 사용하면 됨. 

			Console.WriteLine();

		}
	}


}



