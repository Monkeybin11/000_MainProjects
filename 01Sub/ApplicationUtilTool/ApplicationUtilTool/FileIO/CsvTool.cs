using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.FileIO
{
	public class CsvTool
	{
		public string [ ] [] ReadCsv2String(string path,Char delimiter )
		{
			return File.ReadLines(path).Select( lines => lines.Split( delimiter ) ).ToArray();
		}

	}
}
