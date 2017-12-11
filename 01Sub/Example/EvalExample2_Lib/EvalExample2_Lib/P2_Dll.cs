using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;


using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using System.IO;



namespace EvalExample2_Lib
{
	public static class P2_Dll
	{
		public static async void main()
		{
			ScriptOptions op = ScriptOptions.Default;

			var mscorlib = typeof(System.Object).Assembly;
			var systemCore = typeof(System.Linq.Enumerable).Assembly;
			op = op.AddReferences( mscorlib , systemCore );
			op = op.AddNamespaces( "System" );
			op = op.AddNamespaces( "System.Linq" );
			op = op.AddNamespaces( "System.Collections.Generic" );

			var state = await CSharpScript.RunAsync(@"var x = new List<int>(){1,2,3,4,5};",op);
			state = await state.ContinueWithAsync( "var y = x.Take(3).ToList();" );

			var y = state.GetVariable("y");
			var ylist = (List<int>)y.Value;
			foreach ( var item in ylist )
			{
				Console.WriteLine( item );
			}

			Console.ReadLine();

		}

		public static async void main2()
		{
			ScriptOptions op = ScriptOptions.Default;

			var mscorlib = typeof(System.Object).Assembly;
			var systemCore = typeof(System.Linq.Enumerable).Assembly;
			op = op.AddReferences( mscorlib , systemCore );
			op = op.AddImports( "System" );
			//op = op.AddImports( "System.Linq" );
			//op = op.AddImports( "System.Collections.Generic" );

			var emgucvimg = typeof(Emgu.CV.Image<Gray,byte>).Assembly;
			var emgucvstr = typeof(Emgu.CV.Structure.Gray).Assembly;
			op = op.AddReferences( emgucvimg );
			op = op.AddImports( "Emgu.CV.Image" );
			op = op.AddImports( "Emgu.CV.Structure" );

			var state = await CSharpScript.RunAsync(@"var x = ""E:\Temp\al.png"" ; ",op);
			//state = await state.ContinueWithAsync( "var y = new Image<Gray,byte>(x); " );

			var x = state.GetVariable("x");
			Console.WriteLine( x.Value );
			//var y = state.GetVariable("y");
			//var img = (Image<Gray,byte>)y.Value;
		

			Console.ReadLine();

		}

		public static async void main3()
		{
			var engine = new CSharpScript();

		}



	}
}
