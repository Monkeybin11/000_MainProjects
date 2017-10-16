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

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for Win_SpctDisplay.xaml
	/// </summary>
	public partial class Win_SpctDisplay : Window
	{
		public event Action evtCloseWin;
		public Win_SpctDisplay()
		{
			InitializeComponent();
		}

		private void Window_Closing( object sender , System.ComponentModel.CancelEventArgs e )
		{
			evtCloseWin();
		}
	}
}
