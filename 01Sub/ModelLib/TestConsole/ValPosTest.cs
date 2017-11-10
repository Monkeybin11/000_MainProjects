using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.Data.Handler;

namespace TestConsole
{
	public class ValPosTest
	{
		public static void main()
		{
			var pos2 = ValPos(1,2,0);
			var pos3 = ValPos(3,2,0);
			var res1 = pos2 > pos3;
			var res2 = pos2 + pos3;
			var res3 = pos2.ad + pos3.ADDPos;
		}
	}
}
