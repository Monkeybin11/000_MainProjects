using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEumerable_Interface
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new test();

            foreach (var item in t)
            {
                Debug.WriteLine(item);
            }

            var res = t.Select((x, i) => i.ToString());


            foreach (var item in res)
            {
                Debug.WriteLine(item);
            }

            Console.ReadLine();

        }
    }

    class test : IEnumerable<string>
    {
        private string[] data = { "ab", "b", "ff" };

        public IEnumerator<string> GetEnumerator()
        {
            int i = 0;
            while (i < data.Length)
            {
                yield return data[i];
                i++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
