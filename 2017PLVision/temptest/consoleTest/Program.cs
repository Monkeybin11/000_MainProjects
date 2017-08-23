using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using static SpeedyCoding.SpeedyCoding_Reflection;
using SpeedyCoding;

namespace consoleTest
{
    class Program
    {
        static void Main( string [ ] args )
        {

            //var w1 = new Writer<int,logc>(1,new logc());

            var l1 = new Just<int,Logman,string>(4);
            var l2 = new Just<int,Logman,string>(4);

            
        }
    }

    public class logc : IWriterLog<string>
    {
        StringBuilder CurrnetMethodName = new StringBuilder();

        public string Log { get; set; }

        public string Combine( string logA , string logB )
        {
            return logA + Environment.NewLine + logB;
        }

        public void WriteLog()
        {
            CurrnetMethodName.Append(GetCurrentMethod());
            CurrnetMethodName.Append(Environment.NewLine);
        }

    }

    public class Logman : ILogger<string>
    {
        public List<string> LogList { get; set; }
    }

}
