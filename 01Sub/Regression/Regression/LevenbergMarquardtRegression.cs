using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Regression
{
    public class LevenbergMarquardtRegression : RegressionFactory
    {
        public bool DoRegression(double[] sourceX, double[] sourceY, Func<double[], double, double> function, double[] initialFactor, out double[] fittedY, out double[] fittedFactor, double exitcondition = 0)
        {
            // validate

            if (sourceX.Length != sourceY.Length)
            {
                throw new Exception("x와 y의 길이가 다릅니다.");
            }

            if (sourceX.Length == 0)
            {
                throw new Exception("데이터가 없네요");
            }
            
            fittedY = sourceY;
            fittedFactor = initialFactor;

            var result = GetLM(sourceX, sourceY, function, initialFactor, exitcondition);
            fittedFactor = result.FindValue;

            
            for (int i = 0; i < fittedY.Length; i++)
            {
                fittedY[i] = function(fittedFactor, sourceX[i]);
            }

            return true;
        }

        public static double dot(Matrix<double> a, Matrix<double> b)
        {
            double result = 0;
            for (int i = 0; i < a.RowCount; i++)
            {
                result += (a[i, 0] * b[i, 0]);
            }
            
            return Math.Sqrt(result);
        }
        
        public static LMResult<double[]> GetLM(double[] sourceX, double[] sourceY, Func<double[], double, double> function, double[] initialFactor, double exitcondition = 0)
        {
            try
            {

                LMResult<double[]> lmResult = new LMResult<double[]>();
                lmResult.DiffDelta = 1E-10;
                lmResult.InitLambda = 1;
                lmResult.LambdaFactor = 2;
                lmResult.IterationCount = 50;
                lmResult.InitValue = initialFactor;

                /// 팩터가 몇개니?
                var factorCount = lmResult.InitValue.Length;

                int xCount = sourceX.Length;

                Matrix<double> jacobian = new DenseMatrix(xCount, factorCount);

                Matrix<double> hessian;
                Matrix<double> H_lm;
                double[,] _d = new double[xCount, 1];

                Matrix<double> d = Matrix.Build.DenseOfArray(_d);
                Matrix<double> d_lm = Matrix.Build.DenseOfArray(_d);

                double y_init = function(initialFactor, sourceX[xCount / 2]);

                int n_iters = lmResult.IterationCount;
                double lambda = lmResult.InitLambda;

                bool needToUpdateJMatrix = false;

                double[] currentFactor = lmResult.InitValue;
                double[] lmEstimatedFactor = new double[factorCount];

                double currentDot = 0;
                double newDot = 0;

                /// init value
                {
                    for (int k = 0; k < xCount; k++)
                    {
                        double delta = lmResult.DiffDelta;

                        for (int factorItrator = 0; factorItrator < factorCount; factorItrator++)
                        {
                            double[] modarrPos = GetDeepCopy(currentFactor);
                            double[] modarrNeg = GetDeepCopy(currentFactor);
                            modarrPos[factorItrator] += delta;
                            modarrNeg[factorItrator] -= delta;
                            double deltaValue = function(modarrPos, sourceX[k]) - function(modarrNeg, sourceX[k]) / (delta * 2);
                        }

                        d[k, 0] = sourceY[k] - function(currentFactor, sourceX[k]);
                    }
                    currentDot = dot(d, d);
                }

                for (int i = 0; i < n_iters; i++)
                {
                    jacobian.Clear();
                    for (int k = 0; k < xCount; k++)
                    {
                        double delta = lmResult.DiffDelta;

                        for (int factorItrator = 0; factorItrator < factorCount; factorItrator++)
                        {
                            double[] modarrPos = GetDeepCopy(currentFactor);
                            double[] modarrNeg = GetDeepCopy(currentFactor);
                            modarrPos[factorItrator] += delta;
                            modarrNeg[factorItrator] -= delta;
                            double deltaValue = (function(modarrPos, sourceX[k]) - function(modarrNeg, sourceX[k])) / (delta * 2);
                            jacobian[k, factorItrator] = deltaValue * -1;
                        }

                        d[k, 0] = sourceY[k] - function(currentFactor, sourceX[k]);
                    }
                    
                    hessian = jacobian.Transpose().Multiply(jacobian);
                    
                    currentDot = dot(d, d);
                    
                    var dd = Matrix.Build.DiagonalIdentity(hessian.RowCount);
                    for (int p = 0; p < hessian.RowCount; p++)
                    {
                        dd[p, p] = hessian[p, p];
                    }

                    // var dd = new DiagonalMatrix(H.RowCount, H.RowCount, H.Diagonal().ToArray());


                    H_lm = hessian + (lambda * hessian * dd); // Levenberg - Marquardt

                    var transpose = jacobian.Transpose();
                    var inversed = H_lm.Inverse();

                    Matrix<double> dp = (-1 * H_lm.Inverse()) * (jacobian.Transpose() * d); // LM

                    // Matrix<double> dp = (H.Add(dd.Multiply(lambda))).Cholesky().Solve(J.Transpose().Multiply(d));

                    //Console.WriteLine(dp[0, 0].ToString() + " " + dp[1, 0].ToString());

                    for (int factorItrator = 0; factorItrator < factorCount; factorItrator++)
                    {
                        lmEstimatedFactor[factorItrator] = currentFactor[factorItrator] + dp[factorItrator, 0];
                    }

                    for (int k = 0; k < xCount; k++)
                    {
                        d_lm[k, 0] = sourceY[k] - function(lmEstimatedFactor, sourceX[k]);
                    }

                    newDot = dot(d_lm, d_lm);

                    LMSpot spot = new LMSpot();
                    spot.LMLegacy = GetDeepCopy(currentFactor);
                    spot.LMEst = GetDeepCopy(lmEstimatedFactor);
                    spot.ELegacy = currentDot;

                    
                    if (newDot < currentDot)
                    {
                        lambda = lambda / lmResult.LambdaFactor;
                        currentFactor = lmEstimatedFactor;
                            
                        needToUpdateJMatrix = true;
                        currentDot = newDot;
                    }
                    else
                    {
                        needToUpdateJMatrix = false;
                        lambda = lambda * lmResult.LambdaFactor;
                    }

                    
                    spot.LMNew = currentFactor;
                    spot.Lambda = lambda;
                    spot.IsIn = needToUpdateJMatrix;
                    spot.ENew = newDot;

                    lmResult.AnalyzeResult.Add(spot);
                    
                    if (newDot < exitcondition)
                    {
                        Console.WriteLine(string.Format("{0} 회만에 빠져나왔습니다.", i + 1));
                        break;
                    }
                }

                lmResult.FindValue = currentFactor;
                Console.WriteLine(lmResult.ToString());
                return lmResult;


            }
            catch (Exception except)
            {
                Console.WriteLine(except.ToString());
                return null;
            }
        }

        private static double[] GetDeepCopy(double[] InitValue)
        {
            double[] modarr = new double[InitValue.Length];
            for (int ppp = 0; ppp < modarr.Length; ppp++)
            {
                modarr[ppp] = InitValue[ppp];
            }

            return modarr;
        }
    }

    public class LMResult<T>
    {
        public List<double> X { get; set; }
        public List<double> Y { get; set; }
        public T InitValue { get; set; }
        public T FindValue { get; set; }
        public double DiffDelta { get; set; }
        public double LambdaFactor { get; set; }
        public int IterationCount { get; set; }
        public double InitLambda { get; set; }
        public List<LMSpot> AnalyzeResult { get; set; }

        public LMResult()
        {
            X = new List<double>();
            Y = new List<double>();
            DiffDelta = 0.001;
            LambdaFactor = 2;
            IterationCount = 20;
            InitLambda = 1;
            AnalyzeResult = new List<LMSpot>();

        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < AnalyzeResult.Count; i++)
            {
                str += AnalyzeResult[i].ToString() + Environment.NewLine;
            }
            return str;
        }
    }

    public class LMSpot
    {
        public double[] LMNew;
        public double[] LMEst;
        public double[] LMLegacy;
        public double ELegacy;
        public double ENew;
        public double Lambda;
        public bool IsIn;

        public LMSpot()
        {

        }

        public override string ToString()
        {
            return string.Format(
            "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}]\t{10}"
            , "IsIn : ", IsIn
            , "LMLEG : ", LMLegacy.ArrayToString("0.00")
            , "LMEST : ", LMEst.ArrayToString("0.00")
            , "LMNEW : ", LMNew.ArrayToString("0.00")
            , ELegacy, ENew, Lambda
            );
        }
    }

    public static class StringExtension
    {
        public static string ArrayToString<T>(this T[] source, string format = "", string seperator = " ")
        {
            var str = "";
            foreach (var item in source)
            {
                str += string.Format("{0:" + format + "}", item) + seperator;
            }

            return str;
        }
    }
    
}
