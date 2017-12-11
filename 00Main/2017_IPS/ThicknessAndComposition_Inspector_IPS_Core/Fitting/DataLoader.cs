using System.Collections.Generic;
using System.Linq;
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
	using DPosThckRflt = PosThckRflt<double>;


	public static partial class DataLoader
	{
		readonly static string RefFileName = "Reflectivity.csv";
		readonly static string ResFileName = "Result.csv";

		public static Maybe<List<DPosThckRflt>> GetIPSDatas( string path )
		{
			path = Path.GetDirectoryName( path );

			FileNames filenames = Just( path )
							.Bind( GetAllDirs )
							.Map( GetAllFileNames );

			if ( !CheckFiles( filenames ) ) return None;

			var wave     = GetDataWith( ReadWaveLen , RfltFilter , filenames ).First().Datas.First().Value;
			var thckness = GetDataWith( ReadThikness , ThckFilter , filenames );
			var rflts    = GetDataWith( ReadReflectivity , RfltFilter , filenames );
			var Total = thckness.Zip( rflts , (f,s) => ToTuple( f  ,  s ) ).ToList();

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

				if ( newthck.Count != newrflt.Count ) return None;

				List<DPosThckRflt> output  = new List<DPosThckRflt>();

				for ( int j = 0 ; j < newrflt.Count ; j++ )
				{
					var ptrw = ToPosThckRflt( newthck[j][0] , newthck[j][1] , newthck[j][2] , newrflt[j] , wave );
					
					output.Add( ptrw );
				}
				totallist.Add( output );
			}
			return Just(totallist.Flatten().ToList());
		}
	}


}



