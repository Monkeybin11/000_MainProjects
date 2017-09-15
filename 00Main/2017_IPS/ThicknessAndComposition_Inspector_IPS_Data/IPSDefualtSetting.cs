using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class IPSDefualtSetting
	{
		public IPSDefualtSetting() { }

		// -- Configs config --
		public string BaseDirPath { get { return AppDomain.CurrentDomain.BaseDirectory + "\\" + "config"; } set { } }
		public string StartupConfigName { get { return "SettedConfig.xml"; } set { } }

		// -- scan config --
		public int SampleDiameter { get { return 4; } set { } }
		public List<PlrCrd> ScanSpot 
		{
			get 
			{
				// angle 45 (count = 4) , rho = 0.5 , 1 , 1.5  x2 inch (count 6 )
				var output = new List<double[]>();
				output.Add( new double [ ] { 0 , 0 } );
				var output2 = Enumerable.Range( 1 , 3 )
					.SelectMany(
						f => Enumerable.Range( 0 , 4 ).Select( x => (double)x * 45 ) ,
						( f , s ) => new double [ ] { f * 0.5 , s * 45 } );
				return ( output.Concat( output2 ) ).Select( x => new PlrCrd( x [ 0 ] , x [ 1 ] )).ToList();
			}
			set { }
		}
		public double AngFirst { get { return 0; } set { }  }
		public double AngStep { get { return 45; } set { } }
		public double RhoFirst { get { return 0.5; } set { } }
		public double RhoStep { get { return 0.5; } set { } }
		public double RhoCount { get { return 100; } set { } }

		// -- HW Config --
		// Spetrometer
		public int Boxcar { get { return 3; } set { } }
		public int Scan2Avg { get { return 5; } set { } }
		public int IntegrationTime { get { return 100; } set { } }

		// Stage
		public int Port { get { return 4; } set { } }
		public int XStgSpeed { get { return 100; } set { } }
		public int RStgSpeed { get { return 100; } set { } }
		public int RStgStep	{ get { return 2; } set { } }
	}

	public static class ConfigExt
	{
		public static IPSConfig ToConfig(
		this IPSDefualtSetting src )
		{
			var res = new IPSConfig();
			res.BaseDirPath			=	src.BaseDirPath			;	
			res.StartupConfigName	=	src.StartupConfigName	;
			res.SampleDiameter		=	src.SampleDiameter		;
			res.ScanSpot			=	src.ScanSpot			;
			res.Port				=   src.Port			    ;
			res.AngFirst			=   src.AngFirst			;
			res.AngStep				=   src.AngStep 			;
			res.RFirst			=   src.RhoFirst			;
			res.RStep				=   src.RhoStep 			;
			res.RCount			=   src.RhoCount			;
			res.Boxcar				=	src.Boxcar				;
			res.Scan2Avg			=	src.Scan2Avg			;
			res.IntegrationTime		=	src.IntegrationTime		;
			res.XStgSpeed			=	src.XStgSpeed			;
			res.RStgSpeed			=	src.RStgSpeed			;
			res.RStgStep			=	src.RStgStep			;
			return res;
		}
	}
}
