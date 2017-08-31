using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class SpeedyCoding_IO
    {
        public static string TrimBaseName( this string filepath )
        {
            return Path.GetFileName( filepath );
        }

        public static string TrimFileNameOnly( this string filepath )
        {
            return Path.GetFileName( filepath )
                       .Split( '.' )
                       .First();
        }

        public static string TrimDirPath( this string filepath )
        {
            return Path.GetDirectoryName( filepath );
        }

        #region collection to csv

        public static bool ToCsv<T>(
            this T [] src ,
            string path )
        {
            try
            {
                var sb = new StringBuilder();
                foreach ( var item in src )
                {
                    sb.Append( item );
                    sb.Append( ',' );
                }
                File.WriteAllText( path , sb.ToString() );
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }

        public static bool ToCsv<T>(
      this T [][] src ,
      string path )
        {
            try
            {
                var sb = new StringBuilder();
                foreach ( var items in src )
                {
                    foreach ( var item in items )
                    {
                        sb.Append( item );
                        sb.Append( ',' );
                    }
                    sb.Append( Environment.NewLine );
                   
                }
                File.WriteAllText( path , sb.ToString() );

                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }

        public static bool ToCsv<T>(
      this T [,] src ,
      string path )
        {
            var sb = new StringBuilder();
            try
            {
                for ( int j = 0 ; j < src.GetLength(0) ; j++ )
                {
                    for ( int i = 0 ; i < src.GetLength(1) ; i++ )
                    {
                        sb.Append( src[j,i] );
                        sb.Append( ',' );
                    }
                    sb.Append( Environment.NewLine );
                }
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }


        #endregion

    }
}
