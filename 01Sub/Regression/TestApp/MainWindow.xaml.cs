using Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        double TestFunction(double[] factor, double x)
        {

            return factor[0] * Math.Sin(x) + Math.Cos(factor[1]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var source = Regex.Split(tbxSource.Text, "\r\n");
            List<double> xlist = new List<double>();
            List<double> ylist = new List<double>();


            foreach (var item in source)
            {
                var singlerow = item.Split(' ', '\t', ',');
                if (singlerow.Length >= 2)
                {
                    double x;
                    double y;
                    if (double.TryParse(singlerow[0], out x) && double.TryParse(singlerow[1], out y))
                    {
                        xlist.Add(x);
                        ylist.Add(y);
                    }
                }
            }

            var max = ylist.Max();

            // normalize
            ylist = ylist.ConvertAll(x => x / ylist.Max());

            GetGaussinResult(xlist, ylist);
            GetSechResult(xlist, ylist);

        }

        private void GetGaussinResult(List<double> xlist, List<double> ylist)
        {
            double[] factor = new double[2] { 50, 0 };

            double[] resultylist = new double[xlist.Count];
            double[] resultFactor = new double[2];

            RegressionFactory regression = new LevenbergMarquardtRegression();
            regression.DoRegression(xlist.ToArray(), ylist.ToArray()
                , GetGaussian, factor, out resultylist, out resultFactor, 0.0001);
            chartGauss.DisplayChart(xlist, ylist, "source", true);
            chartGauss.DisplayChart(xlist, resultylist.ToList(), "gaussian", true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factor">H</param>
        /// <param name="theta"></param>
        /// <returns></returns>
        double GetGaussian(double[] factor, double theta)
        {
            var GH = factor[0];
            var deltaGH = factor[1];

            var radian = theta / 180 * Math.PI;

            double d = -1 * (4 * Math.Log(2) * Math.Pow(theta - deltaGH, 2)) / (GH * GH);
            return Math.Exp(d);
        }

        private void GetSechResult(List<double> xlist, List<double> ylist)
        {
            double[] factor = new double[2] { 50, 0 };

            double[] resultylist = new double[xlist.Count];
            double[] resultFactor = new double[2];

            RegressionFactory regression = new LevenbergMarquardtRegression();
            regression.DoRegression(xlist.ToArray(), ylist.ToArray()
                , GetSech, factor, out resultylist, out resultFactor, 0.0001);
            chartSech.DisplayChart(xlist, ylist, "source", true);
            chartSech.DisplayChart(xlist, resultylist.ToList(), "Sech", true);
        }


        double GetSech(double[] factor, double theta)
        {
            var GH = factor[0];
            var deltaGH = factor[1];

            var radian = theta / 180 * Math.PI;

            double s = Math.Pow(sech((theta - deltaGH), (0.567 * GH)), 2);

            return s;
        }

        double sech(double t, double tau)
        {
            return 1 / Math.Pow(Math.Cosh(t / tau), 2);
        }
        
    }
}
