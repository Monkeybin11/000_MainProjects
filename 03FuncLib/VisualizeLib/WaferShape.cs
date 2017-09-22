using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using VisualizeLib.TypeClass;

namespace VisualizeLib
{
	public class WaferShape
	{
		public void CreateWfMatrix( int inchsize )
		{ 
		
		
		}

		// 먼저 각 지점의 점들로 f(x,y) 를 구한다. 
		// f(x,y)를 이용해 그리드에 대한 z 값을 구한다. 
		// 완료된 그리드에 웨이퍼 마스크를 곱한다. 

		public byte [ , , ] CreateWaferMask( int dia, int rStep, int rhoDegreestep )
		{
			double rhoRadinaStep = rhoDegreestep * Math.PI / 180;

			var rlist   =  rStep.xRange( (int)(dia / 2 / rStep) , rStep );
			var rholist =  (0.0).xRange( (int)(2*Math.PI / rhoRadinaStep) , rhoRadinaStep );

			var crtnMx = rlist.SelectMany(
									f => rholist ,
									(f,s) => new PlrCrd(f,s).ToCartesian()  )
							  .ToArray();

			byte[,,] mask = new byte[(int)dia,(int)dia,3];

			crtnMx.ActLoop( p =>
				0.xRange( 3 ).ActLoop( x => mask [ (int)Math.Round(p.Y) , ( int )Math.Round( p.X ) , x ] = 1 )
			);
			return mask; // 웨이퍼 모양은 1 , 아닌부분은 0으로 되있는 어레이 이다. 
		}


	}
}
