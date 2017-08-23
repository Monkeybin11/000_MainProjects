using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;

namespace ModelLib.Monad
{
    public interface mLogWriter<T>
    {
        ILogData Log { get; set; }
        //ILogger Logger { get; set; }
    }

    public class Normal<T> : mLogWriter<T> , IEnumerable<T> 
    {
        public T Value { get; private set; }

        public ILogData Log { get; set; }
        //public ILogger Logger { get; set; }

        public Normal(T val , ILogData log )
        {
            Value = val;
            Log = log;
        }

        public bool HasValue { get { return true; } }
        public Normal( T value )
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals( object obj )
        {
            var target = obj as mLogWriter<T>;
            return target == null ? false : true;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public IEnumerable<T> ToEnumerable()
        {
            yield return Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Value;
        }

    }


    public class Error<T> : mLogWriter<T> , IEnumerable<T>
    {
        public T Value { get; private set; }
        public ILogData Log { get; set; }
        //public List<Td> Log { get; set; }

        //public ILogger<Td> Logger = StringLogger.GetInstance();
        public bool HasValue { get { return false; } }

        public Error( ILogData log )
        {
            Log = log;
        }


        public override string ToString()
        {
            return "";
        }

        public override bool Equals( object obj )
        {
            return obj == null ? true : false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public IEnumerable<T> ToEnumerable()
        {
            yield return Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return null;
        }


    }

    public static class mLogWriterExt
    {
        public static mLogWriter<B> Bind<A, B>(
            this mLogWriter<A> a ,
            ILogger logger ,
            Func<A , mLogWriter<B>> func )
        {
            var obj = a as Normal<A>;
            if ( obj == null )
            {
                var error = a as Error<A>;
                


                return new Error<B>( logger.AddError( obj.Log ));
            }
            else
            {
                return func( obj.Value );
            }
        }

        public static mLogWriter<T> TomLogWriter<T, Tlog>(
            this T value ,
            Tlog log) 
            where Tlog : ILogData
        {
            return new Normal<T>( value , log );
        }

    }

    public class test
    {
        public void main()
        {
            var log = new LogData();
            log.Logs.Add( "log" );

            var mw = new Normal<int>(3 , log);

            mw.Bind( x => 4.ToMaybe<int>( log ) );
               

        }
    }
}
