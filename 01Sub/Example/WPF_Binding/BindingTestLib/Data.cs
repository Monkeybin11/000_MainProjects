using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingTestLib
{
    public class Data : INotifyPropertyChanged
	{
		public int Speed { get; set; }
		public int Height;
		public string Name;

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
