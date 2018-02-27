using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpeedyCoding
{
    public static class SpeedyCoding_Base
    {
        #region Debug
        public static T Print<T>( this T src )
        {
            Console.WriteLine( src?.ToString() );
            return src;
        }

        public static T Print<T>( this T src , string msg )
        {
            Console.WriteLine( msg + " : " + src?.ToString() );
            return src;
        }

        public static T [ ] Print<T>( this T [ ] src )
        {
            if ( src == null ) return null;
            foreach ( var item in src )
            {
                Console.Write( item.ToString() + " " );
            }
            Console.WriteLine();
            return src;
        }

        public static T [ ] Print<T>( this T [ ] src , string msg )
        {
            if ( src == null ) return null;
            Console.Write( msg + " : " );
            foreach ( var item in src )
            {
                Console.Write( item.ToString() + " " );
            }
            Console.WriteLine();
            return src;
        }

		public static void Show(
			this bool src ,
			string trueMsg , 
			string failMsg)
		{
			if ( src ) MessageBox.Show( trueMsg );
			else MessageBox.Show( failMsg );
		}

		public static void FailShow(
				this bool src ,
				string failMsg )
		{
			if ( !src ) MessageBox.Show( failMsg );
		}


		#endregion



        #region Length
        public static int Len<TSrc>(
            this IEnumerable<TSrc> src )
        {
            return src.Count();
        }

        public static int Len<TSrc>(
            this IList<TSrc> src )
        {
            return src.Count();
        }

        public static int Len<TSrc>(
            this ICollection<TSrc> src )
        {
            return src.Count();
        }

        public static int Len<TSrc>(
           this TSrc [ ] src ,
           int order = 0 )
        {
            return src.GetLength( 0 );
        }

        public static int Len<TSrc>(
          this TSrc [ ] [ ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src [ 0 ].GetLength( 0 );
            else return src [ 0 ].GetLength( 0 );
        }

        public static int Len<TSrc>(
          this TSrc [ , ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src.GetLength( 1 );
            else return src.GetLength( 0 );
        }


        public static int Len<TSrc>(
          this TSrc [ ] [ ] [ ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src [ 0 ].GetLength( 0 );
            if ( order == 2 ) return src [ 0 ] [ 0 ].GetLength( 0 );
            else return src [ 0 ] [ 0 ].GetLength( 0 );
        }

        public static int Len<TSrc>(
          this TSrc [ , , ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src.GetLength( 1 );
            if ( order == 2 ) return src.GetLength( 2 );
            else return src.GetLength( 0 );
        }
		#endregion
	}
}

