using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Expressions;
using static System.Console;

using Emgu.CV;
using Emgu.CV.Structure;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;

namespace EvalExample2_Lib
{
	class Program
	{
		static void Main( string [ ] args )
		{
			var value = CSharpScript.EvaluateAsync("1+2");
			WriteLine( value.Result );


			// state preserve
			ScriptState state = CSharpScript.RunAsync(" int c = 1 + 12; ").Result;
			int val2 = (int)state.GetVariable("c").Value;
			WriteLine( val2 );


			ScriptOptions options = ScriptOptions.Default
										.AddReferences(Assembly.GetAssembly(typeof(Path)))
										.AddImports("System.IO");

			var val3 = CSharpScript.EvaluateAsync<string>(@"Path.Combine(""A"",""B"");" , options).Result;
			WriteLine( val3 );

										



			var script = @"int Add(int x, int y) 
								{
									return x+y;
								}
							var temp = new Program();
								

							temp.Getimg(""asd"")";
			//note: we block here, because we are in Main method, normally we could await as scripting APIs are async
			var result = CSharpScript.EvaluateAsync<string>(script , ScriptOptions.Default.WithImports("EvalExample2_Lib")).Result;

			//result is now 5
			Console.WriteLine( result );
			Console.ReadLine();
		}

		static Func<string,string> Getimg =
			Path => "Image";
	}

	class Animal
	{
		public string GetDogName => "Mike";
	}

}
