using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonadExample1
{
    class Program
    {
        static void Main( string [ ] args )
        {
            m1.main();

        }

       

    }

    public static class m1
    {
        public static void main()
        {
            Func<int, Identity<int>> add2 = x => new Identity<int>(x + 2);
            Func<int, Identity<int>> mult2 = x => new Identity<int>(x * 2);
            //Func<int, Identity<int>> add2Mult2 = x => mult2(add2(x));
            Func<int, Identity<int>> add2Mult2 = x => add2(x).Bind(mult2);

            var result =
                "Hello World!".ToIdentity().Bind( a =>7.ToIdentity()
                                                        .Bind( b => (new DateTime(2010, 1, 11)).ToIdentity()
                                                                                                .Bind(c => (a + ", " + b.ToString() + ", " + c.ToShortDateString()).ToIdentity())
                                                              )
                                                 );

            var result2 =
                "Hi".ToIdentity().Bind( a =>
                7.ToIdentity().Bind (   b =>
                (new DateTime(2010,1,11)).ToIdentity().Bind( c=>
                (a + "," + b.ToString() + "," + c.ToString())
                .ToIdentity())));

            //Func<int,Identity<string>> secondmethod = input => input.ToString().ToIdentity();

            var result3 =
                7.ToIdentity().Bind( input => input.ToString().ToIdentity() );
        }
    }

    public static class m2
    {
        public static void main()
        {
            var result =
                from a in "Hi".ToIdentity()
                from b in 7.ToIdentity()
                from c in (new DateTime(2010,1,11)).ToIdentity()
                select a + "," + b.ToString() + "," + c.ToString();
        }
    }


    public static class ext
    {
        public static Identity<B> Bind<A, B>( this Identity<A> a , Func<A , Identity<B>> func )
        {
            return func( a.Value );
        }
        public static Identity<T> ToIdentity<T>( this T value )
        {
            return new Identity<T>( value );
        }
        public static Identity<C> SelectMany<A, B, C>(
            this Identity<A> a ,
            Func<A , Identity<B>> func,
            Func<A , B , C> select )
        {
            return select(a.Value, a.Bind(func).Value).ToIdentity();
        } 

    }

    public class Identity<T>
    {
        public T Value { get; private set; }
        public Identity( T value )
        {
            Value = value;
        }
    }

    //------------- Part 5 Maybe ------------------//

    public interface Maybe<T> { }

    public class Nothing<T> : Maybe<T>
    {
        public override string ToString()
        {
            return "Nothing";
        }
    }

    public class Just<T> : Maybe<T>
    {
        public T Value { get; private  set;}
        public Just( T value )
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
