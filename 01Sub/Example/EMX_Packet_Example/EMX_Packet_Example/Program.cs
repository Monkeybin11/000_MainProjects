using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMX_Packet_Example
{
    using static System.Console;

    public enum CmdType { Error }
    class Program
    {
        static void Main( string[] args )
        {
            string serverpath = "*IDN?";

            int bytelength  = Encoding.UTF8.GetByteCount( serverpath);

            WriteLine( bytelength );

            string wholestr = bytelength.ToString() + "|" + serverpath;

            WriteLine( wholestr );


            var err = CmdType.Error;

            var str = err.ToString();
            WriteLine( str );

            ReadLine();
        }

        public void test1()
        {
            

        }

    }
}
