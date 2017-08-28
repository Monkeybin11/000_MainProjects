using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class SpeedyCoding_Reflection
    {
        public static string CallerName( int higher ) => 
            new StackFrame(higher).GetMethod().Name;
    }
}
