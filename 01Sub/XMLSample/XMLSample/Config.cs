using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLSample
{
    public class XmlSerialization
    {
        public void XmlSerializationWrite()
        {
            Employee emp = new Employee
            {
                Seq = 1,
                Id = 1001,
                Name = "Tim",
                Dept = "Sales"
            };

            using ( StreamWriter wr = new StreamWriter( @"C:\000_ProjectCode\01Sub\XMLSample\data\serial1.xml" ) )
            {
                XmlSerializer xs = new XmlSerializer( typeof(Employee));
                //XmlSerializer xs = new XmlSerializer(emp.GetType()); 
                xs.Serialize( wr , emp );
            }

            {
                XmlSerializer xs = new XmlSerializer(typeof(Employee));
                //객체로부터 타입을 얻는 다른 방법
                //XmlSerializer xs = new XmlSerializer(emp.GetType()); 

                xs.Serialize( Console.Out , emp );
            }
        }

        public void XmlSerializationRead()
        {
            using ( var reader = new StreamReader( @"C:\000_ProjectCode\01Sub\XMLSample\data\serial1.xml" ) )
            {
                XmlSerializer xs = new XmlSerializer(typeof(Employee));
                Employee emp2 = xs.Deserialize(reader) as Employee;

                Console.WriteLine( "{0}, {1}" , emp2.Id , emp2.Name );
            }
        }



    }

    public class Employee
    {
        public int Seq;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Dept { get; set; }
    }

    public class Config
    {
		public double parm1;
		public int parm2;
		public string name;
    }
}
