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
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace ImageGradetionHandleExample
{
	public partial class Form1 : Form
	{
		Image<Gray,byte> baseGray;
		Image<Gray,byte> baseGrad;
		Image<Gray,byte> ResImg;
		Image<Gray,byte> ScaledImg;

		double normax;
		double gamma;


		public Form1()
		{
			InitializeComponent();
		}

		private void btnLoad_Click( object sender , EventArgs e )
		{
			try
			{


				OpenFileDialog ofd = new OpenFileDialog();
				if ( ofd.ShowDialog() == DialogResult.OK )
				{
					baseGray = new Image<Gray , byte>( ofd.FileName );
					imageBox1.Image = baseGray;
				}
			}
			catch ( Exception )
			{
			}
		}

		private void btnStart_Click( object sender , EventArgs e )
		{
			baseGrad = baseGray.Clone();

			baseGrad = baseGrad.SmoothMedian( 131 );
			baseGrad = baseGrad.SmoothMedian( 131 );
			baseGrad = baseGrad.SmoothMedian( 131 );
			baseGrad = baseGrad.SmoothMedian( 131 );
			baseGrad = baseGrad.SmoothMedian( 131 );

			baseGrad = baseGrad.Not();

			normax = ( double )nudNormal.Value;
			gamma = ( double )nudGamma.Value;


			ResImg = baseGray * 0.5 + baseGrad * 0.5;
			ScaledImg = ResImg.Mul( 255.0 / normax );
			ScaledImg._GammaCorrect( gamma );
			imageBox3.Image = ResImg;


		}

		private void btnShowOriginal_Click( object sender , EventArgs e )
		{
			imageBox1.Image = baseGray;
		}

		private void btnShowGrad_Click( object sender , EventArgs e )
		{
			imageBox1.Image = baseGrad;
		}

		private void btnRes_Click( object sender , EventArgs e )
		{
			imageBox3.Image = ResImg;
		}

		private void btnScale_Click( object sender , EventArgs e )
		{
			imageBox3.Image = ScaledImg;
		}

		private void btnSave_Click( object sender , EventArgs e )
		{

			string path = @"D:\03JobPro\2017\010_Ganju\20171020_광과기 이미지 프로세싱용2\gradation";


			baseGrad.Save( path + "\\grad.png" );
			ResImg.Save( path + "\\res.png" );
			ScaledImg.Save( path + "\\scaled.png" );
		}

		private void btnResscale_Click( object sender , EventArgs e )
		{
			normax = ( double )nudNormal.Value;
			gamma = ( double )nudGamma.Value;


			ResImg = baseGray * 0.5 + baseGrad * 0.5;
			ScaledImg = ResImg.Mul( 255.0 / normax );
			ScaledImg._GammaCorrect( gamma );
		}
	}
}
