﻿using System;
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
using VisualizeLib;
using SpeedyCoding;

namespace Wafer_Visualize
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void btnLoadCsv_Click( object sender , RoutedEventArgs e )
		{
			var res = Dialog.OpenFileDia();
			if ( res == "" ) return;



		}

		private void btnSaveImg_Click( object sender , RoutedEventArgs e )
		{

		}
	}
}