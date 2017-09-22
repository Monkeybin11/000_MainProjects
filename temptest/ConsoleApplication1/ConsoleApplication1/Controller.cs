using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Controller
    {
        bool isconnected = false;
        string Name;

        public Controller(string name)
        {
            Name = name;
        }

        public Controller go1(int pos)
        {
            Console.WriteLine( "go1 pos" );  
            return this;
        }

        public Controller go2( int pos )
        {
            Console.WriteLine( "go2 pos" );
            return this;
        }

        public Controller go3( int pos )
        {
            Console.WriteLine( "go3 pos" );
            return this;
        }




    }
}
