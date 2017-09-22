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

namespace Binding_UserControl_Example
{
	/// <summary>
	/// Interaction logic for UCTest.xaml
	/// </summary>
	public partial class UCTest : UserControl
	{
		public UCTest()
		{
			this.InitializeComponent();
		}

		public double Value { get; set; }
		public string Text { get; set; }
	}
}
