using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSample
{
    class Program
    {
        static void Main( string [ ] args )
        {
			string path3 = @"E:\Temp\xmltest\test.xml";

			IPSDefualtSetting temp = new IPSDefualtSetting();
			XmlTool.WriteXmlClass( temp , @"E:\Temp\xmltest\" , "test.xml" );



			/*

			string path = @"E:\Temp\test";
			string path2 = @"E:\Temp\test\.test.txt";

			var res1 = File.Exists( path2 );
			File.Create( path2 );


			//XmlReadWrite xml = new XmlReadWrite();
			//xml.WriteXml();
			//xml.ReadXml();
			//xml.XmlDocCreate();
			//xml.XmlDocRead();
			//xml.XmlNavi();

			//XmlSerialization xs = new XmlSerialization();
			//xs.XmlSerializationWrite();
			//xs.XmlSerializationRead();


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
			*/
			Console.ReadLine();
        }
    }
}
