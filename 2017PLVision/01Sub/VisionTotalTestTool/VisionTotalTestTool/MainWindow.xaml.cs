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
using MahApps.Metro.Controls;

namespace VisionTotalTestTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitEventRegist();
        }

        public void InitEventRegist()
        {
            UcPrework.evtAddImage  += new RoutedEventHandler(UcPreworkbtn_Click);
            UcPrework.evtResizeAll += new RoutedEventHandler(UcPreworkbtn_Click) ;
            UcPrework.evtHStack    += new RoutedEventHandler(UcPreworkbtn_Click) ;
            UcPrework.evtVStack    += new RoutedEventHandler(UcPreworkbtn_Click) ;
        }

        private void UcPreworkbtn_Click( object sender , RoutedEventArgs e )
        {
            var sendername = (sender as Button)?.Name;

            switch ( sendername )
            {
                case "btnAddImage":
                    UcPrework.txbAddedImg.Text = UcPrework.txbAddedImg.Text + Environment.NewLine + DateTime.Now.ToLongDateString();
                    break;

                case "btnResizeAll":

                    break;

                case "btnHStack":

                    break;

                case "btnVStack":

                    break;


            }

        }
    }
}
