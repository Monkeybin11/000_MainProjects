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
using ThicknessAndComposition_Inspector_IPS_Core;
using ThicknessAndComposition_Inspector_IPS_Data;
using Emgu.CV;
using Emgu.CV.Structure;

namespace ThicknessAndComposition_Inspector_IPS
{
	using static ThicknessAndComposition_Inspector_IPS_Core.Core_Helper;
	using static ModelLib.Handler;
	using ModelLib.AmplifiedType;
	using ModelLib.Data;
	/// <summary>
	/// Interaction logic for UC_AnalysisMap.xaml
	/// </summary>
	public partial class UC_AnalysisMap : UserControl
	{
		public UC_AnalysisMap()
		{
			InitializeComponent();
		}


		public void SetupImage()
		{

		}

		public void DrawImage(IPSResult result )
		{
			var res = CreateMap(result , 6);
			var mapimg = res.Item1[0];
			var scalebar = res.Item1[1];

			// need draw 

		}

		public void CrerateScanPosBtn( IPSResult result )
		{
			var w0 = 15;
			var h0 = 15;
			var w1 = this.ActualWidth;
			var h1 = this.ActualHeight;

			var RealToCanvas = FnReScale(w0,h0,w1,h1);

			Func<CrtnCrd , ValPosCrt> toValPos
				= pos => RealToCanvas( ValPosCrt(pos.X, pos.Y) );

			var scaledPosList = result.SpotDataList.Map(x => x.CrtPos)
												   .Map(toValPos)
												   .ToList();


			







		}


	}
}
