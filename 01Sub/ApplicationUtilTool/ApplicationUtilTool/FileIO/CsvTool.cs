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
		public string [] [] ReadCsv2String(string path , Char delimiter = ',' , bool rowDirction = true )
		{
			if ( rowDirction ) return File.ReadLines( path ).Select( lines => lines.Split( delimiter ) ).ToArray();
			else
			{
				var res = File.ReadLines( path ).Select( lines => lines.Split( delimiter ) ).ToArray();

				List<string[]> output = new List<string[]>();
				for ( int i = 0 ; i < res[0].GetLength(0) ; i++ )
				{
					List<string> column = new List<string>();
					for ( int j = 0 ; j < res.GetLength(0) ; j++ )
					{
						column.Add( res[j][i] );
					}
					output.Add( column.ToArray() );
				}
				return output.ToArray();
			}
		}
	}
}
