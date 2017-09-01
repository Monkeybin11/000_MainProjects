using Accord.Statistics.Models.Regression.Linear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using VisualizeLib.TypeClass;


namespace Test
{
	class Program
	{
		static void Main( string [ ] args )
		{
			byte[,] temp = new byte[2,2];


			test1();
			double[] inputs = { 80, 60, 10, 20, 30 };
			double[] outputs = { 20, 40, 30, 50, 60 };
			OrdinaryLeastSquares ols = new OrdinaryLeastSquares();
			SimpleLinearRegression regression = ols.Learn(inputs, outputs);


		}


		public static void test1()
		{
			double r = 3;
			double the = 30.0.ToRadian();
			var pol1 = new PlrCrd(r,the);

			double r2 = 3;
			double the2 = 240.0.ToRadian();
			var pol2 = new PlrCrd(r2,the2);

			var c1 = pol1.ToCartesian();
			var c2 = pol2.ToCartesian();

		}

	}
}
