using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLSample
{
	class Class1
	{
	}
	public class PlrCrd
	{
		public double Rho { get; set; }
		public double R { get; set; }

		private PlrCrd(  )
		{
			
		}
		private PlrCrd( double rho , double r )
		{
			R = r;
			Rho = rho;
		}

		public static PlrCrd CreatePlrCrd( double rho , double r )
		{
			return new PlrCrd( rho , r );
		}
	}
	public class IPSDefualtSetting
	{
		public IPSDefualtSetting()
		{ }

		// -- Configs config --
		public string BaseDirPath { get { return AppDomain.CurrentDomain.BaseDirectory + "\\" + "config"; } }
		public string StartupConfigName { get { return "SettedConfig.xml"; } }

		// -- scan config --
		public int SampleDiameter { get { return 4; }  set { } }
		//public List<PlrCrd> ScanSpot
		//{
		//	get
		//	{
		//		// angle 45 (count = 4) , rho = 0.5 , 1 , 1.5  x2 inch (count 6 )
		//		var output = new List<double[]>();
		//		output.Add( new double [ ] { 0 , 0 } );
		//		var output2 = Enumerable.Range( 1 , 3 )
		//			.SelectMany(
		//				f => Enumerable.Range( 0 , 4 ).Select( x => (double)x * 45 ) ,
		//				( f , s ) => new double [ ] { f * 0.5 , s * 45 } );
		//		return ( output.Concat( output2 ) ).Select( x => PlrCrd.CreatePlrCrd( x [ 0 ] , x [ 1 ] ) ).ToList();
		//	}
		//}
		public double AngFirst { get { return 0; } }
		public double AngStep { get { return 45; } }
		public double RhoFirst { get { return 0.5; } }
		public double RhoStep { get { return 0.5; } }
		public double RhoCount { get { return 100; } }

		// -- HW Config --
		// Spetrometer
		public int Boxcar { get { return 3; } }
		public int Scan2Avg { get { return 5; } }
		public int IntegrationTime { get { return 100; } }

		// Stage
		public int Port { get { return 4; } }
		public int XStgSpeed { get { return 100; } }
		public int RStgSpeed { get { return 100; } }
		public int RStgStep { get { return 2; } }
	}

}
