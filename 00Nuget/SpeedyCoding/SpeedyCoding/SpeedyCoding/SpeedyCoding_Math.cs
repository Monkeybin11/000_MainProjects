using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
	public static class SpeedyCoding_Math
	{
		public static double Integral(
			this IEnumerable<double> self ,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx 
				|| startidx > self.Count()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0 
						? 0 
						: startidx;

			var end  = endidx ;

			return self.Where( ( _ , i ) => i >= start && i <= end ).Sum();
		}

		public static double Integral(
			this IEnumerable<double> self ,
			IEnumerable<double> indices ,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx
				|| startidx > self.Count()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			return self.Where( ( _ , i ) => indices.ElementAt(i) >= start && indices.ElementAt( i ) <= end ).Sum();
		}

		public static double Integral(
			this IEnumerable<double> self ,
			IEnumerable<double> indices ,
			IEnumerable<int> pickIdx,
			List<double> dark ,
			List<double> refs ,
			List<double> reffactor,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx
				|| startidx > self.Count()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			var targetInten = self.ToArray();
			var targetWave = indices.ToArray();
			var pickedinten = pickIdx.Select( x  => targetInten[x] ).ToArray();
			var pickedWave  = pickIdx.Select( x  => targetWave[x] ).ToArray();

			var refspectrum = Enumerable.Range(0,pickedWave.Count())
								.Select( i => reffactor[i] * ( pickedinten[i] - dark[i] )/( refs[i] - dark[i]  ) ).ToList();
			return refspectrum.Where( ( _ , i ) => pickedWave.ElementAt( i ) >= start && pickedWave.ElementAt( i ) <= end ).Sum();
		}



	}
}
