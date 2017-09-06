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

namespace PLImagingMachine
{
    /// <summary>
    /// Interaction logic for UC_ScanWindow.xaml
    /// </summary>
    public partial class UC_ScanWindow : UserControl
    {
        public UC_ScanWindow()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler evtScanMenuBtnClick;

        private void btnScanMenu_Click( object sender , RoutedEventArgs e )
        {
            evtScanMenuBtnClick( sender , e );
        }
    }
}
