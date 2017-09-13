using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUtilTool.FileIO;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class IPSConfig
	{
		// -- Configs config --
		public string BaseDirPath { get; set; }
		public string StartupConfigName { get; set; }

		// -- scan config --
		public int SampleDiameter { get; set; }
		public List<double[]> ScanSpot { get; set; } // Polar 
		public double AngFirst { get; set; }
		public double AngStep { get; set; }
		public double RhoFirst { get; set; }
		public double RhoStep { get; set; }
		public double RhoCount { get; set; }

		// -- HW Config Hidden--
		// Spetrometer
		public int Boxcar { get; set; }
		public int Scan2Avg { get; set; }
		public int IntegrationTime { get; set; }

		// Stage
		public int XStgSpeed { get; set; }
		public int RStgSpeed { get; set; }
		public int RStgStep { get; set; }
	}

	public static class IPSConfigExt
	{
		public static IPSConfig SaveConfig(
			this IPSConfig src ,
			string dirpaht,
			string name)
		{
			XmlTool.WriteXmlClass( src , dirpaht , name );
			return src;
		}
	}

}
