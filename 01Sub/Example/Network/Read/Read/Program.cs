using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Read
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Temp\test.csv";

            var res = File.ReadAllLines(path);

            var splited = res.Select(x => x.Split(',')).ToList();

            Console.WriteLine();


        }
    }
}
