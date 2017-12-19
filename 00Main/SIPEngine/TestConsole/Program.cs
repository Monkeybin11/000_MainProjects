using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;


namespace TestConsole
{


	class Program
	{
		static void Main( string [ ] args )
		{
			EvalCSCode temp = new EvalCSCode();

			string src = "Console.WriteLine( \"hi\" );";

			
			var res = EvalCSCode.Eval( src );

			Console.WriteLine( "Done" );
		}
	}
}
