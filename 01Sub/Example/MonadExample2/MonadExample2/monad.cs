using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonadExample2
{
    public class Identity<T>
    {
        public T Value { get; private set; }

        public Identity( T value )
        {
            Value = value;
        }
    }


}
