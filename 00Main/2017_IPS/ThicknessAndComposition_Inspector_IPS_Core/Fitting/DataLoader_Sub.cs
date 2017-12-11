using System;
using System.Collections.Generic;
using System.Linq;
using SpeedyCoding;
using ModelLib.AmplifiedType;
using static ModelLib.AmplifiedType.Handler;
using FittingDataStruct;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	using static ApplicationUtilTool.FileIO.CsvTool;
	using static System.IO.Directory;
	using static System.IO.Path;
	using static FittingDataStruct.Handler;

	using Paths = IEnumerable<string>;
	using FileNames = IEnumerable<string>;

	using MList = Maybe<List<double>>;
	using MssData = DatasAnMissing<List<double>>;
	using System.IO;

	public static partial class DataLoader
	{

		static bool CheckFiles( FileNames files )
		{
			var headList = files.Select( x => GetFileName(x).Split( '_' ).First() ).Distinct().ToList();

			foreach ( var head in headList )
			{
				var test = GetFileName(files.First()).Split( '_' ).Last();
				var namelist = files.Where( x=>  head == GetFileName(x).Split( '_' ).First() ).ToList();

					// case : only single files exist
				if ( namelist.Count < 2 ) return false;

				var thcks = ThckFilter(namelist);
				var rflts = RfltFilter(namelist);

					// case : only single rflt and result exist.
					// prevent 1-1_1_Result.csv and  1-1_2_Result.csv 
				if ( thcks.Count() != 1 || rflts.Count() != 1 ) return false;
			}
			return true;
		}

		#region Applied Func

		static List<MssData> GetDataWith
			( Func<string , string [ ] [ ]> type ,
			  Func<Paths , Paths> filter ,
			  FileNames basepaths )
			=> Just( basepaths )
				.Bind( filter )
				.Lift( type )
				.Lift( ToMListTable )
				.Lift( ToWithMissing )
				.ToList();

		static Func<Paths , Paths> ThckFilter
			=> FileFilter.Apply( ResFileName ); //  Filtered Name -> BasPath -> Filtered Names Path
		static Func<Paths , Paths> RfltFilter
			=> FileFilter.Apply( RefFileName );

		#endregion

		#region BaseFunc
		static Func<string , string [ ] [ ]> ReadThikness
		=> file => ReadCsv2String( file , rowend: 25 , colend: 3 , rowskip: 1 , order0Dirction: false );

		static Func<string , string [ ] [ ]> ReadReflectivity
			=> file => ReadCsv2String( file , rowend: 850 , colend: 25 , rowskip: 451 , colskip: 1 , order0Dirction: true );

		static Func<string , string [ ] [ ]> ReadWaveLen
		=> file => ReadCsv2String( file , rowend: 850 , colend: 1 , rowskip: 451 , colskip: 0 , order0Dirction: true );



		/// <summary>
		/// ( Filtered Name , BasPath ) -> Filtered Names Path  
		/// </summary>
		static Func<string , FileNames , FileNames> FileFilter
			=> ( filter , filenames )
			=> filenames.Where( x => x.Split( new char [ ] { '_' } ).Last() == filter );

		static FileNames GetAllFileNames( Paths dirpaths )
			=> dirpaths.Lift( f => GetFiles( f ) ).Flatten();

		static Paths GetAllDirs( string topdir )
		{
			var dir =  GetDirectories( topdir , "*" , System.IO.SearchOption.AllDirectories );
			return dir.Count() == 0
					? Just( topdir ).AsEnumerable()
					: dir;
		}

		#endregion

		#region Helper
		static IEnumerable<MList> ToMListTable // IEnumerable< Maybe<IEnumerable<double>>>;
		( this IEnumerable<IEnumerable<string>> src )
		=> src.Select( x => x.ToDoubleList() );

		static MList ToDoubleList    // Maybe<IEnumerable<double>>;
			( this IEnumerable<string> src )
		{
			var output = new List<double>();
			foreach ( var strdata in src )
			{
				double res;
				if ( !double.TryParse( strdata , out res ) ) return None;
				output.Add( res );
			}
			return output;
		}

		private static Func<MList,bool> IsNothing
			= datas => !datas.isJust;
		#endregion
	}

	internal struct PosThckData
	{
		public double X;
		public double Y;
		public double Thickness;
	}
}
namespace FittingDataStruct
{
	public static class Handler
	{
		public static DatasAnMissing<T> ToWithMissing<T>( IEnumerable<Maybe<T>> src )
			=> new DatasAnMissing<T>()
			{
				Datas = src.ToList()
			};

		public static PosThckRflt<T> ToPosThckRflt<T>
			( T x , T y , T thck , List<T> rflt , List<T> wave )
			=> new PosThckRflt<T>( x , y , thck , rflt , wave );
	}

	public class PosThckRflt<T>
	{
		public readonly T X;
		public readonly T Y;
		public readonly T Thickness;
		public readonly List<T> Reflectivity;
		public List<T> WaveLength => _WaveLength;

		public static List<T>  _WaveLength ;

		public PosThckRflt( T x , T y , T thck , List<T> rflt , List<T> wave )
		{
			if ( _WaveLength == null ) _WaveLength = wave;
			X = x;
			Y = y;
			Thickness = thck;
			Reflectivity = rflt;
		}
	}

	public class DatasAnMissing<T> // For Tracking Missing Datas 
	{
		public List<Maybe<T>> Datas;
		public List<int> MissingIndex
			=> Datas.IndicesOf( x => !x.isJust );
	}

}
