using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace ApplicationUtilTool.FileIO
{
    public class XmlTool
    {
		public bool WriteXmlClass<T>( T clss  , string dirpath , string name ) where T : class
		{
			PathTool.CreateFolder( dirpath );
			try
			{
				using ( StreamWriter wr = new StreamWriter( Path.Combine( dirpath , name ) ) )
				{
					XmlSerializer xs = new XmlSerializer( typeof(T) );
					xs.Serialize( wr , clss );
				}
				return true;
			}
			catch ( Exception ex)
			{
				MessageBox.Show( "Config Folder Access Violation" );
				return false;
			}
		}

		public T ReadXmlClas<T>( T clsDefulat , string dirpath , string name ) where T : class
		{
			try
			{
				string fullpath = Path.Combine( dirpath , name );

				if ( File.Exists( fullpath ) )
				{
					using ( var rd = new StreamReader( fullpath ) )
					{
						XmlSerializer xs = new XmlSerializer( typeof(T) );
						T output = xs.Deserialize(rd) as T;
						return output;
					}
				}
				else
				{
					return clsDefulat;
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show( "Config file is broken. Config is setted with defulat" );
				return clsDefulat;
			}
		}
    }
}
