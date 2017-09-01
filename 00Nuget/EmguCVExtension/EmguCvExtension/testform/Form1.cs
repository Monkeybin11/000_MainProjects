using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmguCvExtension;
using static EmguCvExtension.Processing;
using Emgu.CV.Structure;
using Emgu.CV;

namespace testform
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			byte[,,] temp = new byte[200,200,3];


			Image<Bgr,byte> img = new Image<Bgr, byte>(temp);

			var wf = FnDrawWafer(100 , 0 ,new Emgu.CV.Structure.Bgr(100,20,140) , 1);
			Image<Bgr,byte> res = wf(img);
			pictureBox1.Image = res.ToBitmap();




		}



	}
}
