using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;

namespace ModelLib.Monad
{
    public interface ILogger
    {
        void AddNormal( string log );
        void AddError( object log );
    }
}
