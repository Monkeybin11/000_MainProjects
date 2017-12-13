using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;

namespace ApplicationUtilTool.Log
{
	public class TextLogger : Logger
	{
		string BaseDir;
		string FileName;
		string FullPath { get { return Path.Combine( BaseDir , FileName ); }  }
		object key = new object();

		public TextLogger( string dir , string name )
		{
			BaseDir = dir.CheckAndCreateDir();
			FileName = name;
			FullPath.CheckAndCreateFile();
		}

		public void Log( string msg , bool addTime = false)
		{
			if ( addTime ) msg = DateTime.Now.ToString( "yyyy-MM-dd_HH-mm-ss" ) + " | Error : " + msg;
			lock ( key )
			{
				File.AppendAllText( FullPath , "\r\n" + msg );
			}
		}
	}
}
