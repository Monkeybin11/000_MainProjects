using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ValueType;
namespace VisionTotalTestTool
{
    public class TestM
    {




    }



    public interface Maybe<T> { }
    public class Nothing<T> : Maybe<T>
    {
        public T Value { get; private set;  }

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

    }

    public class Just<T> : Maybe<T>
    {
        public T Value { get; private set; }
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

    }

    public static class MExt
    {
        public static Maybe<T> ToMaybe<T>
            (this T value)
        {
            return new Just<T>( value );
        }

        public static Maybe<B> Bind<A, B>(
            this Maybe<A> a ,
            Func<A , Maybe<B>> func )
        {
            var justa = a as Just<A>;
            return justa == null ?
                   new Nothing<B>() :
                   func( justa.Value ); 
        }

        // This is Currying. 
        // select : M<A> -> ( M<B> -> M<C> )
        // select 
        //
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

       

    }

}
