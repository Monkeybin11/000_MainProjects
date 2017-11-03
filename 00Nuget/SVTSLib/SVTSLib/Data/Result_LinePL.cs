using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVTSLib.Data
{
	public class Result_LinePL<F> : ResultOutput where F : ResultFormat
	{


		public List<F> ResultList;
	}
}
