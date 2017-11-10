using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMLLib.DataLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMLLib.DataLoader.Tests
{
	[TestClass()]
	public class IPSDataLoaderTests
	{
		[TestMethod()]
		public void GetAllRefctPathTest()
		{
			IPSDataLoader loader = new IPSDataLoader();
			string path = @"F:\IPSData2";
			var res = loader.GetAllRefctPath( path );

			Assert.Fail();
		}
	}
}