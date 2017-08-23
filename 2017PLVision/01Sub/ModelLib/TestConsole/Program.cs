using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;

namespace TestConsole
{
    class Program
    {
        static void Main( string [ ] args )
        {
            var testc = new testclass();
            var classmaybe = testc.ToMaybe();

            Console.WriteLine( classmaybe.ToString() );
            var te = classmaybe.GetType();
            Console.ReadLine();


        }

        public class testclass
        {

        }
    }
}
