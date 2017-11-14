using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModelLib.Handler;
using ModelLib.Data;


namespace TestConsole
{
	using static Console;
	public class ValPosTest
	{
		public static void main()
		{
			var pos2 = ValPosCrt(1,2,0);
			var pos3 = ValPosCrt(3,2,0);
			var res1 = pos2 > pos3;
			var res2 = pos2 + 1;
			var res3 = pos2.AddPos(pos3);

			var plrpos1 = ValPosPlr(10,360,100);
			var plrpos2 = ValPosPlr(10,180,200);

			var pres1 = plrpos1 + 10;
			var pres2 = plrpos1 *100;

			WriteLine( pres1.Value );
			WriteLine( pres2.Value );


		}
	}
}
