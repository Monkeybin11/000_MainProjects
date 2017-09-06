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
    /// <summary>
    /// Interaction logic for Win_SaveSetting.xaml
    /// </summary>
    public partial class Win_SaveSetting : Window
    {
        public event RoutedEventHandler evtSaveOK;

        public Win_SaveSetting()
        {
            InitializeComponent();
        }

        private void btnSaveSCanSetting_Click( object sender , RoutedEventArgs e )
        {
            evtSaveOK(sender,e);
        }

        private void btnCancelSCanSetting_Click( object sender , RoutedEventArgs e )
        {
            this.Close();
        }
    }
}
