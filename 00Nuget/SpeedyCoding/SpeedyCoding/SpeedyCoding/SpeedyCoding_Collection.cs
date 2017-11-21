using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class SpeedyCoding_Collection
    {
        public static Dictionary<Tk , Tv> Append<Tk, Tv>( 
            this Dictionary<Tk , Tv> src , 
            Tk key , 
            Tv value)
        {
            src.Add(key, value);
            return src;
        }

        public static List<Tv> Append<Tv>(
            this List<Tv> src ,
            Tv value )
        {
            src.Add( value );
            return src; 
        }


		/// <summary>
		/// Index of element that satisfy condition
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <param name="cond"></param>
		/// <returns></returns>
        public static List<int> IndicesOf<T>(
           this IEnumerable<T> src ,
           Func<T , bool> cond )
        {
            var reslist = src.Select(x => cond(x) ? 0 : 1 );
            var res = reslist.ToArray();

            var output = new List<int>();
            reslist.Aggregate( ( f , s ) => s != 0 
                                            ? f + s 
                                            : f + 1.Act( x => output.Add( f ) ) );
            return output;
        }
		
		public static IEnumerable<int> xRange(
		this int start ,
		int count )
		{
			return Enumerable.Range( start , count );
		}

		public static IEnumerable<double> xRange(
			this double start ,
			int count ,
			double step )
		{
			List<double> output = new List<double>();
			for ( int i = 0 ; i < count ; i ++ )
			{
				output.Add( start + i*step );
			}
			return output;	
		}

		public static IEnumerable<int> xRange(
			this int start ,
			int count ,
			int step )
		{
			return Enumerable.Range( start , count ).Select( x => x * step );
		}

		/// <summary>
		/// Pick only element that in range for prevent outofindex exception
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public static T Pick<T>(
			this T [ ] [ ] self ,
			int row ,
			int col )
		{
			var rowlimit = self.Length; 
			var collimit = self[0].Length;

			if ( row >= rowlimit || col >= collimit )
			{
				return default(T);
			}
			return self [ row ] [ col ];
		}

		public static T Pick<T>(
			this T [ , ] self ,
			int row ,
			int col )
		{
			var rowlimit = self.GetLength(0);
			var collimit = self.GetLength(1);

			if ( row >= rowlimit || col >= collimit )
			{
				return default( T );
			}
			return self [ row , col ];
		}

		public static T Pick<T>(
			this T [ ] [ ] self ,
			int[] rowcol  ) 
		{
			var rowlimit = self.Length;
			var collimit = self[0].Length;

			if ( rowcol[0] >= rowlimit 
				|| rowcol[1] >= collimit 
				|| rowcol [ 0 ]  < 0
				|| rowcol [ 1 ]  < 0)
			{
				var de = default(T);
				return default(T);
			}
			var temp = self [ rowcol[0] ] [ rowcol[1] ];
			return self [ rowcol[0] ] [ rowcol[1] ];
		}

		public static T Pick<T>(
			this T [ , ] self ,
			int [ ] rowcol )
		{
			var rowlimit = self.GetLength(0);
			var collimit = self.GetLength(1);

			if ( rowcol [ 0 ] >= rowlimit || rowcol [ 1 ] >= collimit )
			{
				return default( T );
			}
			return self [ rowcol [ 0 ] , rowcol [ 1 ] ];
		}



	}
}
