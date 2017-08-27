using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using SpeedyCoding;
using System.Diagnostics;

namespace consoleTest
{
    class Program
    {
        static void Main( string [ ] args )
        {
           
            var wr1 = 1.ToWriter( "" );
            Adder logger = new Adder();

            wr1.Bind( "+1" , logger , x => x.ToString() )
                .Bind(" add hi" , logger , x => x + "hi")
                .Bind( "none" , logger , x => null);
         
          
            
        }
    }


    public interface IWriter<T>
    { }

   //public class Writer<T> where T : Maybe<T>
   //{
   //    public T Value { get; }
   //    public List<string> Logs;
   //}

    public class Writer<T> 
    {
        public Maybe<T> Value { get; private set; }
        public List<string> Logs;

        public static Writer<T> Nothing()
        => new Writer<T>();
        
        public Writer()
        {
            Value = new Nothing<T>();
        }

        public static Writer<T> Nothing(string initlog)
        =>  new Writer<T>();
        
        public Writer( string initlog )
        {
            Value = new Nothing<T>();
            Logs = new List<string>();
            Logs.Add( initlog );
        }


        public static Writer<T> Nothing( List<string> logs )
            => new Writer<T>(logs);
        
        public Writer( List<string> logs )
        {
            Value = new Nothing<T>();
            Logs = logs;
        }

        public static Writer<T> Just( T val , string log )
        => new Writer<T>( val , log );
        
        public Writer(T val,  string initlog )
        {
            Value = new Just<T>(val);
            Logs = new List<string>();
            Logs.Add( initlog );
        }

        public static Writer<T> Just( T val , List<string> logs )
        => new Writer<T>(val,logs);
        
        public Writer( T val , List<string> logs )
        {
            Value = new Just<T>( val );
            Logs = logs;
        }
    }


    public static class WriterExt
    {
        public static Writer<A> ToWriter<A>(
            this A val ,
            string initLog)
        {
            return Writer<A>.Just( val , initLog );
        }

        public static Writer<B> Bind<A,B, Tlog>(
            this Writer<A> src ,
            string log ,
            Tlog logger ,
            Func<A , Maybe<B>> func)
            where Tlog : ILogger
        {
            var justa = src.Value as Just<A>;
            if ( justa == null ) return Writer<B>.Nothing( logger.AddError( src.Logs ) );
            else
            {
                var result = func( justa.Value );
                return result == null
                    ? Writer<B>.Nothing( logger.AddError( src.Logs ) )
                    : Writer<B>.Just( result.Value , logger.AddLog( src.Logs , log ) );
                //저 함수는 maybe 리턴, just 는
            }
        }
    }

    public interface ILogger
    {
        List<string> AddLog( List<string> logs , string log = null);
        List<string> AddError( List<string> logs , string log = null);
    }

    public class Adder : ILogger
    {
        public List<string> AddLog( List<string> logs , string log = null )
            => log == null
                ? logs
                : logs.Append( "Log : " + log ); 

        public List<string> AddError( List<string> logs , string log = null )
        {
            StackTrace stackTrace = new StackTrace();
            var na = stackTrace.GetFrame( 1 ).GetMethod().Name;

            return log == null
                ? logs.Append( "Error : " + na )
                : logs.Append( $"Error ( {log} ) : " + na );
        }
    }



}
