using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUtilTool.FileIO;
using SpeedyCoding;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class IPSConfig 
	{
		public IPSConfig() { } 

		// -- Configs config --
		public string BaseDirPath { get; set; } 
		public string StartupConfigName { get; set; } 

		// -- scan config --
		public int SampleDiameter { get; set; } 
		public List<PlrCrd> ScanSpot { get; set; } // Polar  
		public double AngFirst { get; set; }  
		public double AngStep { get; set; }  
		public double RFirst { get; set; }  
		public double RStep { get; set; } 
		public double RCount { get; set; } 

		// -- HW Config Hidden--
		// Spetrometer
		public int Boxcar { get; set; }
		public int Scan2Avg { get; set; }
		public int IntegrationTime { get; set; }

		// Stage
		public int Port { get; set; }
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

		public static List<PlrCrd> ToSpotList(
			this IPSConfig self)
		{
			var angcount = CalcAngCount( 0 , self.AngFirst , self.AngStep , 360 );
			var res = from r in Enumerable.Range(0, (int)self.RCount)
					  let rho = r*self.RStep + self.RFirst
					  from a in self.AngFirst.xRange( angcount , self.AngStep)
					  select new PlrCrd( r,a);
			return res.ToList();
		}

		#region sub
		public static int CalcAngCount( int counter , double input , double step , double limit ) 
			=> input >= limit 
				? counter
				: CalcAngCount( counter++ , ( input + 1 ) * step , step , limit );

		#endregion	
	}

}
