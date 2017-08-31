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
        enum testenum { test1,test2,haha2,hah3}
        static void Main( string [ ] args )
        {
            CurrentPath = AppDomain.CurrentDomain.BaseDirectory;
            iniTool ini = new iniTool(CurrentPath + "test.ini");


            //var val1 = Enum.GetNames( typeof( testenum ) );
            //var val2 = val1.Select( x => Tuple.Create( x , DateTime.Now.ToString("ss") ) ).ToList();
            //ini.WriteValue( "sec1" , "test1" , "22" );

            ini.WriteValues( "sec1"
                , Enum.GetNames( typeof( testenum ) ).Select( x => Tuple.Create( x , "22" ) ) );


            var res1 = ini.GetValues( "sec1"
                , Enum.GetNames( typeof( testenum ) ) );

            Console.WriteLine( CurrentPath );
            Console.ReadLine();

        }


        public static string CurrentPath; 
        static void TestMethod1()
        {

        }
    }
}
