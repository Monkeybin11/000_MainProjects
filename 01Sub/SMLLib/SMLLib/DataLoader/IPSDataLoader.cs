using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static ApplicationUtilTool.FileIO.CsvTool;

namespace SMLLib.DataLoader
{
	public class IPSDataLoader
	{
		// Read ALl Files include subdir 

		public string[] GetAllRefctPath(string basepath)
		{
			//return Directory.GetFiles(basepath , ".csv" , SearchOption.AllDirectories)
			//		.Where( x => x.Split('_').Last() == "Refelctivity.csv")
			//		.ToArray();

			var temp1 =  Directory.GetFiles( basepath, "*.csv", SearchOption.AllDirectories);
			var temp2 = temp1.Where( x => x.Split( '_' ).Last() == "Refelctivity.csv" ).ToArray();
			return null;

		}



	}
}
