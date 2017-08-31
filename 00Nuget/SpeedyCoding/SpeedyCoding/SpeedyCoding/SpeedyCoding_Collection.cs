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

    }
}
