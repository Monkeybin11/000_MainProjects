using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    public interface LogMaybe<T, Tlogger, Tlogtype> : IMonad where Tlogger : ILogger<Tlogtype>
    {
        T Value { get; set; }
        Tlogger Logger {get;set;}
        bool HasValue { get; }
    }

    public class Just<T, Tlogger, Tlogtype> : LogMaybe<T , Tlogger , Tlogtype> , IEnumerable<T> where Tlogger : ILogger<Tlogtype>
    {
        public T Value { get; set; }
        public Tlogger Logger { get; set; }
        
        public bool HasValue { get { return true; } }
        public Just( T value )
        {
            Value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals( object obj )
        {
            var target = obj as Maybe<T>;
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

    public class Nothing<T, Tlogger, Tlogtype> : LogMaybe<T , Tlogger , Tlogtype>, IEnumerable<T> where Tlogger : ILogger<Tlogtype>
    {
        public T Value { get;  set; }
        public Tlogger Logger { get; set; }
        public bool HasValue { get { return false; } }


        public override string ToString()
        {
            return null;
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

    public class FuncNameLog : ILogger<string>
    {
        public List<string> LogList { get; set; }
        
    }
    public static class LogMaybeExt
    {
        public static LogMaybe<T,Tlogger,Tlogtype> ToLogMaybe<T, Tlogger, Tlogtype>
            ( this T value ,
            Tlogger logger ) 
            where Tlogger : ILogger<Tlogtype>
        {
            if ( value == null ) return new Nothing<T , Tlogger , Tlogtype>();
            else                 return new Just<T , Tlogger , Tlogtype>( value );
        }


        public static LogMaybe<B , Tlogger , Tlogtype> Bind<A,B, Tlogger, Tlogtype>(
            this LogMaybe<A , Tlogger , Tlogtype> a ,
            Func< A , LogMaybe<B , Tlogger , Tlogtype>> func )
            where Tlogger : ILogger<Tlogtype>
        {
            var justa = a as Just<A , Tlogger , Tlogtype>;
            return justa == null 
                   ? new Nothing<B , Tlogger , Tlogtype>() 
                   : func( justa.Value );
        }

        

        // This is Currying. 
        // select : M<A> -> A -> B -> C -> M<C> 
        // select 
        //
        public static Maybe<C> SelectMany<A, B, C>(
            this Maybe<A> src ,
            Func<A , Maybe<B>> func ,
            Func<A , B , C> select )
        {
            return src.Bind( a =>
                    func( a ).Bind( b =>
                    select( a , b ).ToMaybe() ) );
        }

        //public static Maybe<A> Else<A>(
        //    this Maybe<A> a ,
        //    Action act )
        //{
        //    var justa = a as Just<A>;
        //    if ( justa == null )
        //    {
        //        act();
        //        return a;
        //    }
        //    return new Nothing<A>();
        //}
        //
        //public static Maybe<int> Div( this int numerator , int denominator )
        //{
        //    return denominator == 0
        //               ? ( Maybe<int> )new Nothing<int>()
        //               : new Just<int>( numerator / denominator );
        //}


    }

}
