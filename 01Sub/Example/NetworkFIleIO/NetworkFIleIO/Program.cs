using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;

namespace NetworkFIleIO
{
	class Program
	{
		static void Main( string [ ] args )
		{
			string path = @"\\192.168.10.10\임시폴더\s소재우\netio";

			var res = NetReader.GetUNCPath( path );

			var imgpath = Path.Combine(res , "blue_Done_Test.png");


			Image<Gray,byte> img;

			img = new Image<Gray , byte>( imgpath );


			Console.WriteLine( img.Size.ToString() );


		}
	}
}
