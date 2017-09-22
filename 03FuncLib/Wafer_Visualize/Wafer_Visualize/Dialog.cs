using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wafer_Visualize
{
	public static class Dialog
	{
		public static string OpenFileDia()
		{
			OpenFileDialog ofd = new OpenFileDialog();

			if ( ofd.ShowDialog() == DialogResult.OK )
			{
				return ofd.FileName;
			}
			return "";
		}
	}
}
