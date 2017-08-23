using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSample
{
    class Program
    {
        static void Main( string [ ] args )
        {
            //XmlReadWrite xml = new XmlReadWrite();
            //xml.WriteXml();
            //xml.ReadXml();
            //xml.XmlDocCreate();
            //xml.XmlDocRead();
            //xml.XmlNavi();

            XmlSerialization xs = new XmlSerialization();
            //xs.XmlSerializationWrite();
            xs.XmlSerializationRead();


            Console.ReadLine();
        }
    }
}
