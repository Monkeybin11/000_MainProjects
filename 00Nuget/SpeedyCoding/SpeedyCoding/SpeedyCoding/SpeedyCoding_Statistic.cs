using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
	public static class SpeedyCoding_Statistic
	{
		#region statistical parameter
		public static IEnumerable<double> ToZScore(
			this IEnumerable<double> self )
		{
			double avg = self.Average();
			double sd;
			if ( self.Count() > 1000000 )
			{
				sd = self.AsParallel().Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average();
				return self.AsParallel().Select( x => ( x - avg ) / sd );
			}
			else
			{
				sd = self.Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average();
				return self.Select( x => ( x - avg ) / sd );
			}
		}

		public static double SD(
			this IEnumerable<double> self )
		{
			var avg = self.Average();
			if ( self.Count() > 1000000 )
				return self.AsParallel().Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average(); 
			else
				return self.Select( x => Math.Pow( ( x - avg ) , 2 ) ).Average();
		}

		public static double Cov(
			this IEnumerable<double> self ,
			IEnumerable<double> trg )
		{
			double ux =  self.Average(); 
			double uy =  trg.Average();
			var ydatas = trg.ToArray();
			if ( self.Count() > 1000000 )
				return self.AsParallel().Select( ( x , i ) => ( x - ux ) * ( ydatas [ i ] - uy ) ).Average();
			else
				return self.Select( ( x , i ) => ( x - ux ) * ( ydatas [ i ] - uy ) ).Average();
		}

		public static double PearsonCorrelation(
			this IEnumerable<double> self ,
			IEnumerable<double> trg )
		=> self.Cov( trg ) / ( self.SD() * trg.SD() );
		#endregion
	}
}
