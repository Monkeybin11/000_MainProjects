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


namespace ImageProcessingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core core;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            core = new Core();
            string ip = "192.168.19.176";

            //접속만 해준다. 만약 큐에 입력시 자동으로 프로세싱후 결과를 자동으로 리턴해준다. 
            if (core.Connect("192.168.19.176", 5001)) lblStatus.Content = "True";
            else lblStatus.Content = "Fail";

        }
    }
}
