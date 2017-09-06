using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.Monad
{
    public interface IFunctor<FA,FB,A,B>
    {
        T Value<T>();
    }

    public interface Functor<T> 
    {
        T Fmap<T>( Func<T,T> f, T a );
        
    }

    
}
