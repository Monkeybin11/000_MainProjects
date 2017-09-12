using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FirstConfigLoad
{
	public class XmlFirstLoad
	{
		public string CurrentProgramPath =  AppDomain.CurrentDomain.BaseDirectory;
		public string ConfigDirPath { get { return CurrentProgramPath + "\\" + "config"; } }
		public string FirstConfigname = "CurrnetConfig.xml";


		public void ReadConfig()
		{
			Config con = new Config();
			





		}

		public void WriteConfig()
		{
			Config con = new Config();
			con.parm1 = 999999;
			con.parm2 = 999999;
			con.name = "asd";



			using ( StreamWriter wr = new StreamWriter( @"C:\000_MainProjectData\XMLTestDATa\serial1.xml" ) )
			{
				XmlSerializer xs = new XmlSerializer( typeof(Config));
				//XmlSerializer xs = new XmlSerializer(emp.GetType()); 
				xs.Serialize( wr , con );
			}

		}

		public void CreateFolder( string basepath )
		{
			try
			{
				string outputpath = basepath;
				if ( !Directory.Exists( outputpath ) )
				{
					Directory.CreateDirectory( outputpath );
				}
			}
			catch ( Exception )
			{
				Console.WriteLine( "Access Violation" );
			}
		}


	}

	// Config Class need to be public and no constructor
	public class Config
	{
		public double parm1;
		public int parm2;
		public string name;
	}
}
