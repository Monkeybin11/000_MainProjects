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
using System.Windows.Shapes;

namespace PLImagingMachine
{
    
    public partial class Win_ScanSetting : Window
    {
        public event RoutedEventHandler evtSaveClick;
        public event RoutedEventHandler evtapplyClick; 

        public Win_ScanSetting()
        {
            InitializeComponent();
        }

        private void btnSCanSettingSave_Click( object sender , RoutedEventArgs e )
        {
            evtSaveClick(sender,e);
        }

        private void btnScanSettingApply_Click( object sender , RoutedEventArgs e )
        {
            evtapplyClick(sender,e);
        }
    }
}
