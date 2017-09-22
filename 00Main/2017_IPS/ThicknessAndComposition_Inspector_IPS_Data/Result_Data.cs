using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class Result_Data
	{
		public List<SpotData> SpotDataList;
	}

	public class SpotData
	{
		// For Genrelize  
		public CrtnCrd CrtnPos;
		public PlrCrd PlrPos;
		public double Thickness;
	}

	public class DisplayDaya
	{
		public double X;
		public double Y;
		public double Rho;
		public double R;
		public double Thickness;
	}
}
