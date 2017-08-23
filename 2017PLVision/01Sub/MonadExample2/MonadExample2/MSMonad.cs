using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonadExample2.MS
{
    public class MSMonad
    {
        public void main()
        {
            Func<int,double> g = x => 0.2*(double)x;
            Func<double,string> f = x => x.ToString();


            var r1 = f(g(4));
            var r2 = f.Compose(g)(4);

            Identity<int> t = new Identity<int>(3);

            var r3 = Bind(5.ToIdentity() , x => Bind(6.ToIdentity(), y => (x+y).ToIdentity()));

            var r4 = 5.ToIdentity().SelectMany(
                x => 6.ToIdentity().SelectMany(
                y => (x+y).ToIdentity()));
            
                  
        }

        static Identity<U> Bind<T, U>( Identity<T> id , Func<T , Identity<U>> k )
        {
            return k( id.Value );
        }


    }

    public static class MSextension
    {
        public static Func<T,V> Compose<T,U,V> (
            this Func<U,V> f,
            Func<T,U> g)
        {
            return x => f( g( x ) );
        }

        public static T Identity<T>( this T value )
        {
            return value;
        }

        public static Identity<T> ToIdentity<T>(
            this T value)
        {
            return new Identity<T>( value );
        }

        public static Identity<U> SelectMany<T, U>(
            this Identity<T> id ,
            Func<T , Identity<U>> k )
        {
            return k( id.Value );
        }

     

       // public static Identity<V> SelectMany<T, U, V>( this Identity<T> id , Func<T , Identity<U>> k , Func<T , U , V> s )
       // {
       //     return s( id.Value , k( id.Value ).Value ).ToIdentity();
       // }


    }

    public class Identity<T>
    {
        public T Value { get; private set; }
        public Identity(T value)
        {
            Value = value;
        }
    }
}
