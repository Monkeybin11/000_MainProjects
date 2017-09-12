using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonadExample2
{
    class Program
    {
        static void Main( string [ ] args )
        {
            var r = 1.ToList();
            r.Select()
               


        }
    }

    public static class ext
    {
        public static IEnumerable<T> ToList<T>( this T value )
        {
            yield return value;
        }

    }
}
