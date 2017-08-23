using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonadExample2
{
    public static class Extension
    {
        // maybe identity 
        public static Identity<B> Bind<A, B>( this Identity<A> a , Func<A , Identity<B>> func )
        {
            return func( a.Value );
        }

        public static Identity<T> ToIdentity<T>( this T value )
        {
            return new Identity<T>( value );
        }

       

    }
}
