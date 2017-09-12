using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace XMLSample
{
    public class XmlReadWrite
    {
        public void WriteXml()
        {
            using ( XmlWriter wr = XmlWriter.Create( @"C:\000_ProjectCode\01Sub\XMLSample\data\test.xml" ) )
            {
                wr.WriteStartDocument();
                wr.WriteStartElement( "Employees" );

                // Employee 1
                wr.WriteStartElement( "Employee" );
                wr.WriteAttributeString( "Id" , "1001" );  // attribute 쓰기
                wr.WriteElementString( "Name" , "Tim" );   // Element 쓰기
                wr.WriteElementString( "Dept" , "Sales" );
                wr.WriteEndElement();

                // Employee 2
                wr.WriteStartElement( "Employee" );
                wr.WriteAttributeString( "Id" , "1002" );
                wr.WriteElementString( "Name" , "John" );
                wr.WriteElementString( "Dept" , "HR" );
                wr.WriteEndElement();

                wr.WriteEndElement();
                wr.WriteEndDocument();

                /* 출력파일 Emp.xml
                <?xml version="1.0" encoding="utf-8"?>
                <Employees>
                    <Employee Id="1001">
                        <Name>Tim</Name>
                        <Dept>Sales</Dept>
                    </Employee>
                    <Employee Id="1002">
                        <Name>John</Name>
                        <Dept>HR</Dept>
                    </Employee>
                </Employees>
                
                */
            }
        }

        public void ReadXml()
        {
            using (XmlReader rd = XmlReader.Create( @"C:\000_ProjectCode\01Sub\XMLSample\data\test.xml" ) )
            {
                while ( rd.Read() )
                {
                    if ( rd.IsStartElement() )
                    {
                        if ( rd.Name == "Employees" )
                        {
                            rd.Read();
                            //string id = rd["Id"];
                            string id = rd.GetAttribute("Id");
                            rd.Read();

                            string name = rd.ReadElementContentAsString("Name","");
                            string dept = rd.ReadElementContentAsString("Dept","");
                            Console.WriteLine( id + "," + name + "," + dept );
                        }
                    }
                }

            }

        }

        public void XmlDocCreate()
        {
            //Docment 는 모든 xml의 데이터가 메모리상에 올라가게 된다. 
            XmlDocument xdoc = new XmlDocument();

            XmlNode root = xdoc.CreateElement("Employees");
            xdoc.AppendChild( root );

            //emp1
            XmlNode emp1 = xdoc.CreateElement("Employee");
            XmlAttribute attr = xdoc.CreateAttribute("Id");
            attr.Value = "1001";
            emp1.Attributes.Append( attr );

            XmlNode name1 = xdoc.CreateElement("Name");
            name1.InnerText = "Tim";
            emp1.AppendChild( name1 );

            XmlNode dept1 = xdoc.CreateElement("Dept");
            dept1.InnerText = "Sales";
            emp1.AppendChild( dept1 );

            root.AppendChild( emp1 );


            // emp2
            var emp2 = xdoc.CreateElement("Employee");
            var attr2 = xdoc.CreateAttribute("Id");
            attr2.Value = "1002";
            emp2.Attributes.Append( attr2 );

            var name2 = xdoc.CreateElement("Name");
            name2.InnerText = "John";
            emp2.AppendChild( name2 );

            XmlNode dept2 = xdoc.CreateElement("Dept");
            dept2.InnerText = "HR";
            emp2.AppendChild( dept2 );

            root.AppendChild( emp2 );

            xdoc.Save( @"C:\000_ProjectCode\01Sub\XMLSample\data\tes.xml" );
            
        }

        public void XmlDocRead()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load( @"C:\000_ProjectCode\01Sub\XMLSample\data\tes.xml" );

            XmlNodeList nodes = xdoc.SelectNodes("/Employees/Employee");

            foreach ( XmlNode item in nodes )
            {
                var emp = item as XmlNode;
                string id = emp.Attributes["Id"].Value;

                string name = emp.SelectSingleNode("./Name").InnerText;
                string dept = emp.SelectSingleNode("./Dept").InnerText;
                Console.WriteLine( id + "," + name + "," + dept );

                //employees 안의 employee의 자식 노드들을 loop함
                foreach ( XmlNode child in emp.ChildNodes )
                {
                    Console.WriteLine( "{0}: {1}" , child.Name , child.InnerText );
                }

            }

            XmlNode emp1002 = xdoc.SelectSingleNode("/Employees/Employee[@Id='1002']");
            Console.WriteLine( emp1002.InnerXml );
        }

        public void XmlNavi()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load( @"C:\000_ProjectCode\01Sub\XMLSample\data\tes.xml" );

            XPathNavigator nav = xdoc.CreateNavigator();
            XPathNodeIterator nodes = nav.Select("/Employees/Employee");

            while ( nodes.MoveNext() )
            {
                nodes.Current.MoveToChild("Name","");

                if ( nodes.Current.Value == "Tim" )
                {
                    nodes.Current.SetValue("Timmy");
                }

            }

            using ( XmlWriter wr = XmlWriter.Create( @"C:\000_ProjectCode\01Sub\XMLSample\data\Emp2.xml" ) )
            {
                nav.WriteSubtree( wr );
            }

        }
    }
}
