using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingTestLib
{
	public class Core
	{
		public Data TestData { get; set; }
		public int secondspeed { get; set; }


		public Core()
		{
			TestData = new Data();
			TestData.Height = 999;
			TestData.Speed = 999;
			TestData.Name = "999";
		}

		public void Showresult()
		{
			Console.WriteLine(TestData.Name);
			Console.WriteLine(TestData.Speed);
			Console.WriteLine(TestData.Height);

		}

	}
}
