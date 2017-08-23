using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUtilTool;

namespace testconsole
{
    class Program
    {
        static void Main( string [ ] args )
        {
            iniSimple ini = new iniSimple("");
            ini.Writeini();
            ini.Loadini();

        }
    }
}
