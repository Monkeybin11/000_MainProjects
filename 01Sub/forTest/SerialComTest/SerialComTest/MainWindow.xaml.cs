using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace SerialComTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SerialPort Port;
		public MainWindow()
		{
			InitializeComponent();
		}


		public void Connect()
		{
			Port = new SerialPort();
			Port.PortName = "COM4";
			Port.BaudRate = 38400;
			Port.DataBits = 8;
			Port.Parity = Parity.None;
			Port.StopBits = StopBits.One;
			Port.Handshake = Handshake.RequestToSend;

		}
	}
}
