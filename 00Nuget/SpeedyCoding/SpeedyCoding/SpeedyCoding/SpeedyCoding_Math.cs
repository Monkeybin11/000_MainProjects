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
			this IEnumerable<int> self ,
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
			IEnumerable<int> indices ,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx
				|| startidx > indices.Max()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			var res = self.Where( ( _ , i ) => indices.ElementAt(i) >= start && indices.ElementAt( i ) <= end ).Sum();
			Console.WriteLine( "Integral result : " + res.ToString() );
			return res;
		}



		public static double Integral(
			this IEnumerable<double> self ,
			IEnumerable<double> indices ,
			double startidx ,
			double endidx )
		{
			if ( startidx >= endidx
				|| startidx > indices.Max()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			var res = self.Where( ( _ , i ) => indices.ElementAt(i) >= start && indices.ElementAt( i ) <= end ).Sum();
			return res;
		}


		public static double Integral(
			this IEnumerable<double> self ,
			IEnumerable<double> indices ,
			int startidx ,
			int endidx )
		{
			if ( startidx >= endidx
				|| startidx > indices.Max()
				|| endidx < 0 )
				return 0;

			var start = startidx < 0
						? 0
						: startidx;

			var end  = endidx ;
			var res = self.Where( ( _ , i ) => indices.ElementAt(i) >= start && indices.ElementAt( i ) <= end ).Sum();
			Console.WriteLine( "Integral result : " + res.ToString() );
			return res;
		}


	}
}
