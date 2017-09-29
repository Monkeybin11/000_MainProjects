using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class IPSResult
	{
		public List<SpotData> SpotDataList;
		public double[] WaveLen;

		public IPSResult( double[] wavelen )
		{
			WaveLen = wavelen;
			SpotDataList = new List<SpotData>();
		}

	}

	public class SpotData
	{
		// For Genrelize  
		//public CrtnCrd CrtnPos;
		public PlrCrd	PlrPos;
		public double	Thickness;
		public double[] IntenList;

		public SpotData( PlrCrd pos , double thckness , double [ ] intens )
		{
			PlrPos		= pos;
			Thickness	= thckness;
			IntenList	= intens;
		}
	}

	public class IPSResult_ForGrid
	{
		public string Theta { get; set; }
		public string Rho { get; set; }
		public string Thickness { get; set; }
	}


	public class DisplayDaya
	{
		public double X;
		public double Y;
		public double Theta;
		public double Rho;
		public double Thickness;
	}

	public static class DataExt
	{
		public static IPSResult_ForGrid ToGridResult(
			this SpotData self )
		{
			return new IPSResult_ForGrid()
			{
				Theta = Math.Round(self.PlrPos.Theta).ToString() ,
				Rho = Math.Round( self.PlrPos.Rho).ToString(  ) ,
				Thickness = self.Thickness.ToString( "N4" )
			};
		}
	}
}
