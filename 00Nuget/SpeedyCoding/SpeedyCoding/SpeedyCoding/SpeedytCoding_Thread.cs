using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class SpeedytCoding_Thread
    {
        public static T Delay50<T>(
            this T src )
        {
            Thread.Sleep( 50 );
            return src;
        }

        public static T Delay100<T>(
            this T src )
        {
            Thread.Sleep( 100 );
            return src;
        }

        public static T Delay300<T>(
            this T src )
        {
            Thread.Sleep( 300 );
            return src;
        }

        public static T Delay500<T>(
            this T src )
        {
            Thread.Sleep( 500 );
            return src;
        }

        public static T Delay1000<T>(
            this T src )
        {
            Thread.Sleep( 1000 );
            return src;
        }
    }
}
