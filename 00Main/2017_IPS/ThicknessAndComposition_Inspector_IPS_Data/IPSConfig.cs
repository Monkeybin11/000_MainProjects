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
		public string BaseDirPath;
		public string StartupConfigName;

		// -- scan config --
		public int SampleDiameter;
		public List<double[]> ScanSpot; // Polar

		// -- HW Config --
		// Spetrometer
		public int Boxcar;
		public int Scan2Avg;
		public int IntegrationTime;

		// Stage
		public int XStgSpeed;
		public int RStgSpeed;
		public int RStgStep;
	}

	public static class IPSConfigExt
	{
		public static IPSConfig SaveConfig(
			this IPSConfig src ,
			string dirpaht,
			string name)
		{
			new XmlTool().WriteXmlClass( src , dirpaht , name );
			return src;
		}


	}

}
