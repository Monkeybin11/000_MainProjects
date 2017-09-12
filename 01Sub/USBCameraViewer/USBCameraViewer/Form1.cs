using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using DirectShowLib;

namespace USBCameraViewer
{
	public partial class Form1 : Form
	{
		VideoCapture Cam;
		bool Progress;
		string[] WebCams;

		public Form1()
		{
			InitializeComponent();

		}

		private void btnconnet_Click( object sender , EventArgs e )
		{
			Cam = new VideoCapture( Emgu.CV.CvEnum.CaptureType.Any );
			Cam.ImageGrabbed += new EventHandler( GrabEvt );
		}

		private void btnlose_Click( object sender , EventArgs e )
		{
			Cam.Stop();
		}

		public void GrabEvt( object o , EventArgs e )
		{
			Image<Gray,byte> img = null;
			var suc = Cam.Retrieve(img);
			if ( suc )
			{
				this.BeginInvoke( ( Action )( () => pictureBox1.Image = img.ToBitmap() ) );
			}
		}
	}
}
