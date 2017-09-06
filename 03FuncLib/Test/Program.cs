using Accord.Statistics.Models.Regression.Linear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using VisualizeLib.TypeClass;
using Accord.Math.Optimization.Losses;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Interpolation;

namespace Test
{
	class Program
	{
		static void Main( string [ ] args )
		{
			byte[,] temp = new byte[2,2];

			test2();
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

		public static void test2()
		{
			var ols = new OrdinaryLeastSquares()
			{
				UseIntercept = true
			};

			double[][] inputs =
			{
				new double[] { 1, 1 },
				new double[] { 0, 1 },
				new double[] { 1, 0 },
				new double[] { 0, 0 },
			};

			double[] outputs = { 1, 1, 1, 1 };
			MultipleLinearRegression regression = ols.Learn(inputs, outputs);

			double a = regression.Weights[0]; // a = 0
			double b = regression.Weights[1]; // b = 0
			double c = regression.Intercept; // c = 1

			double[] predicted = regression.Transform(inputs);

			double error = new SquareLoss(outputs).Loss(predicted);

		}

		public static void test3()
		{
			LinearSpline tes = new LinearSpline();
		}



	}
}
