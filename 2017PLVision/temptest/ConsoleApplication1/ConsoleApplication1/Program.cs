using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main( string [ ] args )
        {
            Controller cd = new Controller("main");
            cd.go1(1)
              .go2(2)
              .go3(3);
        }
    }
}
