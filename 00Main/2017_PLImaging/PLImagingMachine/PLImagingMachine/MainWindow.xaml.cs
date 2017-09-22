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
using static System.Console;

namespace PLImagingMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Win_ScanSetting winScanSetting = new Win_ScanSetting();
        Win_SaveSetting winSaveSetting = new Win_SaveSetting();


        public MainWindow()
        {
            InitializeComponent();
            ucScanWin.evtScanMenuBtnClick += new RoutedEventHandler( ScanMenuClick );
            
        }

        public void ScanMenuClick( object sender , RoutedEventArgs e )
        {
            var master = sender as Button;
            switch ( master.Name )
            {
                case "btnScan":
                    break;

                case "btnOrigin":
                    break;

                default:
                    WriteLine( "Scan Menu Button is not matched" );
                    break;
            }
        }

        public void OptionMenuClick( object sender , RoutedEventArgs e )
        {
            var master = sender as Button;
            switch ( master.Name )
            {
                case "menuLoadConfig":
                    break;

                case "menuCreateConfig":
                    winScanSetting.Show();
                    break;

                default:
                    WriteLine( "Scan Menu Button is not matched" );
                    break;
            }
        }





    }
}
