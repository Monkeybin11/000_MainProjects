using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLib.DeviceLib;
using ThicknessAndComposition_Inspector_IPS_Data;
using ApplicationUtilTool.FileIO;
using System.IO;
using ModelLib.TypeClass;
using ModelLib.ClassInstance;
using ApplicationUtilTool.Log;
using System.Windows;
using SpeedyCoding;
using System.Threading;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public class IPSCore
	{
		
		IPSErrorMsgData	   Err; // 에러 데이터 공통 메세지 클래스 
		Logger			   Lggr; // 같은 로그 파일에 사용하기 위해서 전역 변수로 만들었다. 
		public IPSConfig   Config { get; set; } // 여기서 
		ISgmaStg_XR        Stg;
		IMaya_Spectrometer Spct;

		string LogTime = "yyyy-MM-dd_HH-mm-ss";

		public string ConfigBasePath = AppDomain.CurrentDomain.BaseDirectory + "config";
		public string ConfigName = "SettedConfig.xml";
		public string ConfigFullPath { get { return Path.Combine( ConfigBasePath , ConfigName ); } }

		string LogDirPath = AppDomain.CurrentDomain.BaseDirectory + "log";
		string LogName = "";

		string CurrentSaveTime { get { return DateTime.Now.ToString( LogTime ); } }


		#region Init

		public IPSCore()
		{
			InitCore(); // Field Initialize
			ConnectHW( "COM" + Config.Port.ToString() ).FailShow( "Virtual Mode is Activated" );
		}

		public string TodayLogPath()
		=> DateTime.Now.ToString( "yyyyMMdd" ) + "_IPSLog.txt";
		

		public void InitCore()
		{
			LogName = TodayLogPath();
			Lggr = new TextLogger( LogDirPath , LogName )
					.Act( x  => Err = new IPSErrorMsgData( x ) );

			Config = XmlTool.ReadXmlClas(
						new IPSDefualtSetting().ToConfig() , // Defulat Setting
						ConfigBasePath.CheckAndCreateDir() ,
						ConfigName.CheckAndCreateFile() );
		}

		public bool ConnectHW(string comport)
		{
			Stg = new SgmaStg_XR( comport );
			Spct = new Maya_Spectrometer();

			var stg =  Stg.Open() == true 
						? true 
						: false.Act( x =>  Err.WriteShowErr( ErrorType.StgConnectErr ));

			var spt = Spct.Connect()  == true 
						? true 
						: false.Act( x =>  Err.WriteShowErr( ErrorType.SpecConnectErr ));

			if ( !stg || !spt )
			{
				Stg.Close();
				Stg = new SgmaStg_XR_Virtual();
				Spct = new Maya_Spectrometer_Virtual();
			}

			//Stg = new SgmaStg_XR_Virtual();

			return stg && spt;
		}

		#endregion

		#region MainFunction

		public void TestFunction()
		{
			//Stg.SendAndReady( Stg.Home + Axis.R.ToIdx() );
			//Stg.SendAndReady( Stg.Home + Axis.X.ToIdx() );
			//Stg.SendAndReady( Stg.GoAbs + 5000.ToPos( Axis.X ) );
			//Stg.SendAndReady( Stg.Go);
			GoAbsPos( Axis.R , 6000 );
			GoAbsPos( Axis.X , 6000 );
			GoAbsPos( Axis.W , -5000 );
			var res = Spct.GetSpectrum();
			//MessageBox.Show( res.GetLength(0).ToString() );
		}

		// Find Thickness Function
		public Func< LEither<double [ ]> , double [ ] , PlrCrd , LEither<double>> CalcPorce =>
			( inten , wave , plrcrd ) =>
			{
				// ToDo : 두께 찾는 기능 구현 
				Console.WriteLine("Processing running");
				Thread.Sleep( 10000 );
				return new LEither<double>( 123 );
			};


		public bool ScanRun() // Use Internal Config , not get config from method parameter
		{
			var calcTaskList = new Task<LEither<double>>[ Config.ScanSpot.Len() ];
			var wavelength = Spct.GetWaveLen();

			var res = new TEither( Stg as IStgCtrl , 12)
						.Bind( x => x.Act( f => f.SendAndReady( f.Home + Axis.R ) ).ToTEither( 12 ) , "R Home Fail" ) 
						.Bind( x => x.Act( f => f.SendAndReady( f.Home + Axis.X ) ).ToTEither( 10 ) , "X Home Fail" );
			
			var posIntenlist = Config.ScanSpot.Select( ( pos , i) => 
			{
				var logres = res
				.Bind( x => x.Act( f => f.SendAndReady( f.GoAbs + pos.Rho.Degree2Pulse().ToPos( Axis.R ) ) ).ToTEither( 1 ) , "R Stage Move Command Fail" )
				.Bind( x => x.Act( f => f.SendAndReady( f.GoAbs + pos.R.ToPos( Axis.X ) ) ).ToTEither( 1 ) , "X Stage Move Command Fail" )
				.Bind( x => x.Act( f => f.SendAndReady( f.Go + Axis.X ) ).ToTEither( 1 ) , "Stage Movement Fail" )
				.ToLEither( default( double[] ) );

				var intenlist = logres.IsRight
									? default(LEither<double[]>).Bind( x => Spct.GetSpectrum() ) 
									: logres.Act( x => Lggr.Log(x.Left , true )); // Logging Error

				calcTaskList[i] = Task.Run<LEither<double>>(  
					() => logres.IsRight ? CalcPorce(intenlist,wavelength,pos)  // Estimate Thickness
										 : new LEither<double>() );


				return Tuple.Create(pos,intenlist);
			} ).ToList(); // Raw Data

			// Thickness Result List
			// ( You dont need to use try catch pattern for catch all exception from tasklist )
			var thicknesses = Task.WhenAll( calcTaskList ).Result;


			// Pos Thickness Inten List 
			var posThickInten = posIntenlist.Zip(thicknesses ,
				(f,s) => new { Pos = f.Item1 , Thckness = s , Intensity = f.Item2 } );


			// -- Integration Result -- // 
			bool isright = thicknesses.Select( x => x.IsRight).Aggregate( (f,s) => f && s);
			if ( isright )
			{
				var tasklogs = posThickInten.Where( x => x.Thckness.IsRight == false)
											.Select( x => x.Pos.ToString()+" ||" + x.Thckness.Left )
											.ActLoop( x => Lggr.Log(x , true));
				return false;
			}
			else
			{
				// Succeed 
				// 여기에서 리턴을 할건지 이벤트로 보낼건지 결정해야함. 
				// 위에서 로깅은 이미 함.
				 
				return true;
			}
		}


		#endregion

		#region IO

		public void LoadConfig(string path)
		{
			Config = XmlTool.ReadXmlClas(
						Config ,
						Path.GetDirectoryName( path ),
						Path.GetFileName( path ) );
		}

		public void SaveConfig( string path )
		{
			XmlTool.WriteXmlClass(
			   Config ,
			   Path.GetDirectoryName( path ) ,
			   Path.GetFileName( path ) );
		}


		#endregion

		#region Trans



		#endregion

		#region Movement
		public void SetHWInternalParm(double rspeed , double xspeed , double scan2avg , double intetime, double boxcar )
		{
			Stg.SendAndReady( Stg.SetSpeed + Axis.R.ToIdx() + rspeed.ToSpeed());
			Stg.SendAndReady( Stg.SetSpeed + Axis.X.ToIdx() + xspeed.ToSpeed());

			Spct.ScanAvg( ( int )scan2avg );
			Spct.IntegrationTime( ( int )intetime );
			Spct.BoxCar( (int)boxcar );

		}

		public bool GoAbsPos( Axis axis , int pos1 , int pos2 = 99999 )
		{
			switch ( axis )
			{
				case Axis.R:
					Stg.SendAndReady( Stg.GoAbs + pos1.ToPos( Axis.R ) );
					break;

				case Axis.X:
					Stg.SendAndReady( Stg.GoAbs + pos1.ToPos( Axis.X ) );
					break;

				case Axis.W:
					Stg.SendAndReady( Stg.GoAbs + pos1.ToPos( Axis.R ) );
					Stg.SendAndReady( Stg.GoAbs + pos2.ToPos( Axis.X ) );
					break;
			}
			Stg.SendAndReady( Stg.Go );
			return true;
		}
		#endregion
	}
}
