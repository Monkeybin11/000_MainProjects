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
	public static class BaseFunc_Analysis
	{
		public static IEnumerable<string[]> ResultRefine( 
			this IEnumerable<string> src ,
			int skipnum )
			=> src.Skip( 1 ).Map( x => x.Split( ',' ).Skip( skipnum ).ToArray() );

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


	}
}
