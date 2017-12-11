using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;


namespace EvalExample2_Lib
{
	public static class P1
	{
		public static async void main()
		{
			var res = await CSharpScript.EvaluateAsync("5+5");
			Console.WriteLine( res );


			res = await CSharpScript.EvaluateAsync( @"""Sample""" );
			Console.WriteLine( res );

			res = await CSharpScript.EvaluateAsync( "int x = 5; int y = 3; y-x" );
			Console.WriteLine( res );
			Console.ReadLine();
		}

		public static void main2()
		{
			var state = CSharpScript.RunAsync(@"int x = 5; int y = 3 ; int z = x+y;");
			ScriptVariable x = state.Result.GetVariable("z");
			var intx = x.Value;
			Console.WriteLine( intx );
			Console.WriteLine( x.Name );

			Console.ReadLine();
		}





	}
}
