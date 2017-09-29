using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;
using SpeedyCoding;

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
				var first = new List<PlrCrd>();
				var counter = RhoFirst == 0
								? 1.Act( x => first.Add( new PlrCrd ( 0 , 0 ) ) )
								: 0.Act( x => first.Add( new PlrCrd ( 0 , 0 ) ) );

				var second = Enumerable.Range( counter, (int)RhoCount)
								.SelectMany( f => Enumerable.Range( 0 , (int)ThetaCount) ,
											(f,s) => new PlrCrd(  
															  ThetaFirst + s*ThetaStep , 
															  RhoFirst + f*RhoStep  ))
								.ToList();

				return first.Act( x => x.AddRange( second ));
			}
			set { }
		}
		public double ThetaFirst { get { return 0; } set { }  }
		public double ThetaStep { get { return 45; } set { } }
		public double ThetaCount
		{
			get
			{
				var count1 = 360 / ThetaStep;
				return Enumerable.Range( 0 , ( int )count1 )
					.Select( x => ThetaFirst + ThetaStep * x )
					.Where( x => x <= 360 )
					.Select( x => 1 )
					.Aggregate( ( f , s ) => f + 1 );
			}
		}
		public double RhoFirst { get { return 1; } set { } }
		public double RhoStep { get { return 20; } set { } }
		public double RhoCount { get { return 2; } set { } }

		// -- HW Config --
		// Spetrometer
		public int Boxcar { get { return 0; } set { } }
		public int Scan2Avg { get { return 1; } set { } }
		public int IntegrationTime { get { return 300; } set { } }

		// Stage
		public int Port { get { return 4; } set { } }
		public int XStgSpeed { get { return 200000; } set { } }
		public int RStgSpeed { get { return 200000; } set { } }
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
			res.ThetaFirst			=   src.ThetaFirst			;
			res.ThetaStep				=   src.ThetaStep 			;
			res.RhoFirst			=   src.RhoFirst			;
			res.RhoStep				=   src.RhoStep 			;
			res.RhoCount			=   src.RhoCount			;
			res.Boxcar				=	src.Boxcar				;
			res.Scan2Avg			=	src.Scan2Avg			;
			res.IntegrationTime		=	src.IntegrationTime		;
			res.XStgSpeed			=	src.XStgSpeed			;
			res.RStgSpeed			=	src.RStgSpeed			;
			res.SetPosition();
			return res;
		}
	}
}
