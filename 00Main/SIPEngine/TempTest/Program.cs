using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.AmplifiedType;

namespace TempTest
{
    using static ModelLib.AmplifiedType.Handler;

    class Program
    {
        static void Main(string[] args)
        {
            var writer = new Func<string, string>( x =>"|"+ x+"haha" );


            int temp = 3;

            var val = Accmululatable<int>(temp,"START", writer);

            var f1 = new Func<int, int>(x => x + 10);
            var f2 = new Func<int, int>(x => x * 10);
            var f3 = new Func<int, int>(x => x * 1000);





            var res = val.Lift(f1);

            var res2 = val.Bind(f1,"f1").Bind(f2,"f2").Bind(f3,"f3");

            var res2res = res2.GetLastPaper();
            var valres = res2.GetLastValue();

            var temptemp = res2.PaperHistory;
            var temptemp2 = res2.ValueHistory;

            Console.WriteLine();


        }
    }
}
