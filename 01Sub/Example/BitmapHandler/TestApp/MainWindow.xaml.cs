using BitmapHandler;
using kr.etamax.etd32.Gradation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            try
            {
                InitializeComponent();
                var bitmap = BitmapExtension.SourceFromBitmap(BitmapExtension.GetBitmapFromDataAndPallette(1000, 1000, GenerateDummyArray(), new RainbowGradation().GetGradation(256, false)));
                img.Source = bitmap;
            }
            catch (Exception except)
            {
                throw except;
            }
        }

        private double[] GenerateDummyArray()
        {
            Random rand = new Random();
            double[] resultArr = new double[1000 * 1000];

            for (int i = 0; i < resultArr.Length; i++)
            {
                resultArr[i] = rand.Next(0, 1000);
            }
            return resultArr;
        }
    }
}
