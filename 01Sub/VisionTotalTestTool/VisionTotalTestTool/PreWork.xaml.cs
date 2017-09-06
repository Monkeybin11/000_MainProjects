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

namespace VisionTotalTestTool
{
    public partial class PreWork : UserControl
    {
        public event RoutedEventHandler evtAddImage;
        public event RoutedEventHandler evtResizeAll;
        public event RoutedEventHandler evtHStack;
        public event RoutedEventHandler evtVStack;

        public PreWork()
        {
            InitializeComponent();
        }

        private void btnAddImage_Click( object sender , RoutedEventArgs e )
        {
            evtAddImage(sender,e);
        }

        private void btnResizeAll_Click( object sender , RoutedEventArgs e )
        {
            evtResizeAll( sender , e );
        }

        private void btnHStack_Click( object sender , RoutedEventArgs e )
        {
            evtHStack( sender , e );
        }

        private void btnVStack_Click( object sender , RoutedEventArgs e )
        {
            evtVStack( sender , e );
        }
    }
}
