using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
	class Program
	{
		static void Main( string [ ] args )
		{
			string path = @"E:\Temp\test.txt";
			string w = "ghahahahahah";

			File.AppendAllText( path , "\r\n" + w );

		}
	}
}
