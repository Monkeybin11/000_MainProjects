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
using System.Reflection;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public partial class IPSCore
	{
		public IPSConfig Config { get; set; }
		public IPSProcessingConfig PrcConfig { get; set; }
		IPSErrorMsgData	   Err; // 에러 데이터 공통 메세지 클래스 
		Logger			   Lggr; // 같은 로그 파일에 사용하기 위해서 전역 변수로 만들었다. 
		ISgmaStg_XR        Stg;
		IMaya_Spectrometer Spctr;

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


			var defualt = new IPSProcessingConfig() {
													 IntglStart = 440 ,
													 IntglEnd = 470 ,
													 a = -0.5277 ,
													 b = 1838.9 };

			PrcConfig = XmlTool.ReadXmlClas(
							defualt ,
							PrcConfigBasePath.CheckAndCreateDir() ,
							PrcConfigName.CheckAndCreateFile() );

			Task.Run( () =>
			 {
				 while ( true )
				 {
					 if ( FlgAutoUpdate ) AutoUpdateSpctrm();
					 Thread.Sleep( 80 );
				 }
			 } );
		}

		public bool ConnectHW(string comport)
		{
			Stg = new SgmaStg_XR( comport , false);
			Spctr = new Maya_Spectrometer();

			var stg =  Stg.Open() == true 
						? true 
						: false.Act( x =>  Err.WriteShowErr( ErrorType.StgConnectErr ));

			//var res = Stg.SendAndReady( Stg.Status );

			var spt = Spctr.Connect()  == true 
						? true 
						: false.Act( x =>  Err.WriteShowErr( ErrorType.SpecConnectErr ));

			if ( !stg || !spt )
			{
				Stg.Close();
				Stg = new SgmaStg_XR_Virtual();
				Spctr = new Maya_Spectrometer_Virtual();
			}
			return stg && spt;
		}

		#endregion

		#region MainFunction


		public void TestFunction()
		{
			//GoAbsPos( Axis.R , 6000 );
			//GoAbsPos( Axis.X , 6000 );
			//GoAbsPos( Axis.W , -5000 );
			var res = new TEither( Stg as IStgCtrl , 12)
						.Bind( x => x.Act( f => f.SendAndReady( f.Home + Axis.R.ToIdx() ) ).ToTEither( 12 ) , "R Home Fail" )
						.Bind( x => x.Act( f => f.SendAndReady( f.Home + Axis.X.ToIdx() ) ).ToTEither( 10 ) , "X Home Fail" );
			//Stg.SendAndReady( Stg.Home + Axis.R.ToIdx() );
			//Stg.SendAndReady( Stg.Home + Axis.X.ToIdx() );
			//Stg.SendAndReady( Stg.GoAbs + 5000.ToPos( Axis.X ) );
			//Stg.SendAndReady( Stg.Go);
			
			//MessageBox.Show( res.GetLength(0).ToString() );
		}

		// Find Thickness Function


		int counter = 0;

		public void updatecounter()
		{
			lock ( key )
			{
				counter++;
			}
		}
		public Func< LEither<double [ ]> , double [ ] , PlrCrd , 
						Tuple< PlrCrd , LEither<double>>> CalcPorce =>
			( inten , wave , plrcrd ) =>
			{
				double[] intendata = new double[] { };
				double[] wav = new double[] { };
				PlrCrd crd = new PlrCrd();

				var intentemp = LoadTestData();
				var postemp = CreatedefualtPos();

				intendata = intentemp[counter];
				crd = postemp[counter];
				wav = LoadWavelen();

				var integ = intendata.Integral(wav , PrcConfig.IntglStart , PrcConfig.IntglEnd);
				var res = integ*PrcConfig.a + PrcConfig.b;

				Console.WriteLine( "Coordinate : " + crd.ToString() );
				Console.WriteLine( "Integral : " + integ );

				updatecounter();
				return Tuple.Create( crd , res.ToLEither() );

				
			};

		/* Original
		public Func<LEither<double [ ]> , double [ ] , PlrCrd ,
						Tuple<PlrCrd , LEither<double>>> CalcPorce2 =>
			( inten , wave , plrcrd ) =>
			{
				return Tuple.Create( plrcrd,
									 inten.Bind( x => x.Integral(wave , PrcConfig.IntglStart , PrcConfig.IntglEnd), "Integral Fail" )
										  .Bind( x => ( x*PrcConfig.a + PrcConfig.b).Print("result") ));
			};
		*/

		public bool ScanRun1() {

			CalcPorce( new double [ ] { 1 }.ToLEither() , new double [ ] { 1 } , new PlrCrd() );  // Estimate Thickness
						

			//Stg.SendAndReady( "S:24" );
			//Stg.SendAndReady( "S:15" );
			//
			//Stg.SendAndReady( Stg.SetSpeed
			//				+ Axis.W.ToIdx()
			//				+ ( 50000 ).ToSpeed()
			//				+ ( 50000 ).ToSpeed() );
			//Stg.SendAndReady( Stg.Home + Axis.W.ToIdx() );
			//
			//Stg.SendAndReady( Stg.GoAbs
			//					+ 90.Degree2Pulse().ToPos( Axis.W )
			//					+ 10.mmToPulse().ToPos() );
			//
			//Stg.SendAndReady( Stg.Go );
			return true;
		}


		Func<TEither,Tuple< PlrCrd , LEither<double[]> , Task<LEither<double>>[] >> ScanNode =
			x =>
			{

				return null;
			};
		

		public bool ScanRun() // Use Internal Config , not get config from method parameter
		{
			counter = 0;

			//TODO : 작업들 노드로 다 바꾸기
			//
			// ㅇ -> ㅇ -> ㅇ - > ㅇ ->
			//        | -> ㅇ  - |  
			// Node 클래스를 하나 만들어서 이 노드들 하나하나 정의하자. 
			// 노드의 화살표는 순서를 나타내고, 만약 노드의 인풋이 에러일 경우, 노드작업 안하고 에러를 뒤로 전파한다. 

			FlgAutoUpdate = true;

			var calcTaskList = new Task< Tuple<PlrCrd , LEither<double> >>[ Config.ScanSpot.Len() ];
			var wavelength = Spctr.GetWaveLen();

			SetHWInternalParm(
				Config.RStgSpeed ,
				Config.XStgSpeed,
				Config.Scan2Avg,
				Config.IntegrationTime,
				Config.Boxcar
				);

			var res = new TEither( Stg as IStgCtrl , 12)
						.Bind( x => x.Act( f => 
							f.SendAndReady( f.Home + Axis.W.ToIdx() ) ).ToTEither( 12 ) , "R or X Home Fail" );
			
			var posIntenlist = Config.ScanSpot.Select( ( pos , i) => 
			{
				var logres = res
								.Bind( x => x.Act( f => 
									f.SendAndReady( f.GoAbs 
													+ pos.Theta.Degree2Pulse().ToPos( Axis.W ) 
													+ pos.Rho.mmToPulse().ToPos() ) )
											 .ToTEither( 1 ) , "R or X Stage Move Command Fail" )
								.Bind( x => x.Act( f => 
									f.SendAndReady( f.Go ) ).ToTEither( 1 ) , "Stage Movement Fail" )
								.ToLEither( new double[] { }  );


				var intenlist = logres.IsRight
									? logres.Bind( x => BkD_Spctrm ) 
									: logres.Act( x => Lggr.Log(x.Left , true )); // Logging Error

				calcTaskList[i] = Task.Run<Tuple<PlrCrd , LEither<double> >>(  
					() => logres.IsRight 
							? CalcPorce(intenlist,wavelength,pos )  // Estimate Thickness
							: Tuple.Create( pos , new LEither<double>() ));

				return Tuple.Create(pos,intenlist);
			} ).ToList(); // Raw Data

			// Thickness Result List
			// ( You dont need to use try catch pattern for catch all exception from tasklist )
			var posThicknesses = Task.WhenAll( calcTaskList ).Result;

			// Reset Pos
			res.Bind( x => x.Act( f => f.SendAndReady( f.GoAbs + 0.ToPos(Axis.W) + 0.ToPos() )).ToTEither( 1 ) )
			   .Bind( x => x.Act( f => f.SendAndReady( f.Go ) ).ToTEither( 1 ) );
			

			FlgAutoUpdate = false;

			// Pos Thickness Inten List 
			//var posThickInten = posThicknesses
			//						.Join(posIntenlist ,
			//							o => o.Item1 ,
			//							i => i.Item1 ,
			//							(o , i) => new { Pos = o.Item1 , Thickness = o.Item2 , Intensity = i.Item2 })
			//						.ToArray();	

			//For Simulation 
			var posThickInten = posThicknesses
									.Zip(posIntenlist ,
										(f,s) => new { Pos = f.Item1 , Thickness = f.Item2 , Intensity = s.Item2 } )
									.ToArray();


			// -- Integration Result -- // 
			bool isright = posThicknesses.Select( x => x.Item2.IsRight).Aggregate( (f,s) => f && s);

			if ( !isright ) // If Task Error
			{
				var tasklogs = posThickInten.Where( x => x.Thickness.IsRight == false)
											.Select( x => x.Pos.ToString()+" ||" + x.Thickness.Left )
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

		public void SavePrcConfig( string path )
		{
			XmlTool.WriteXmlClass(
					 PrcConfig ,
					 Path.GetDirectoryName( path ) ,
					 Path.GetFileName( path ) );
		}


		#endregion

		#region Trans



		#endregion

		#region SubFuntion

		public void ShowSetting()
		{
			StringBuilder configlog = new StringBuilder();
			Type ty = Config.GetType();
			foreach ( PropertyInfo config in ty.GetProperties() )
			{
				var condition = config.GetValue( Config )
									  .GetType()
									  .GetInterfaces()
									  .Where( x => x.Name == "ICollection" )
									  .Count();
				"".Print();
				if ( condition > 0 ) continue;

				configlog.Append( " - " + config.Name + " : " + config.GetValue( Config ) );
				configlog.Append( Environment.NewLine );
			}
			MessageBox.Show( configlog.ToString() );
		}

		public void SetComPort( double port ) => Config.Port = ( int )port;

		public void SetHWInternalParm(double rspeed , double xspeed , double scan2avg , double intetime, double boxcar )
		{
			Config.RStgSpeed = ( int )rspeed;
			Config.XStgSpeed = ( int )xspeed;
			Config.Scan2Avg = ( int )scan2avg;
			Config.IntegrationTime = ( int )intetime;
			Config.Boxcar = ( int )boxcar;

			Stg.SendAndReady( "S:24" );
			Stg.SendAndReady( "S:15" );

			Stg.SendAndReady( Stg.SetSpeed
							+ Axis.W.ToIdx()
							+ ( Config.RStgSpeed ).ToSpeed()
							+ ( Config.XStgSpeed ).ToSpeed() );

			Spctr.ScanAvg( Config.Scan2Avg );
			Spctr.IntegrationTime( Config.IntegrationTime );
			Spctr.BoxCar( Config.Boxcar );
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

		#region Test Helper
		object key = new object();
		public List<double [ ]> LoadTestData( List<string> pathlist = null )
		{
			lock ( key )
			{
				try
				{
					List<string> filenamelist = new List<string>();
					List<double[]> output = new List<double[]>();
					if ( pathlist == null )
					{
						string path = @"D:\03JobPro\2017\011.ISP\CsvData\TLA talko _new Sample\1spct";
						filenamelist = Directory.GetFiles( path ).ToList();
					}

					CsvTool ct = new CsvTool();
					foreach ( var item in filenamelist )
					{
						var strdata = ct.ReadCsv2String( item , ',' );
						for ( int i = 1 ; i < strdata.Len( 1 ) ; i++ )
						{
							List<double> res = new List<double>();
							for ( int j = 0 ; j < strdata.Len( 0 ) ; j++ )
							{
								if ( j == 4 ) continue;
								res.Add( Convert.ToDouble( strdata [ j ] [ i ] ) );
							}
							output.Add( res.ToArray() );
						}
					}
					return output;

				}
				catch ( Exception ex )
				{
					Console.WriteLine( ex.ToString() );
					return null;
				}
			}
		}

		public double [ ] LoadWavelen()
		{
			lock ( key )
			{
				string path = @"D:\03JobPro\2017\011.ISP\CsvData\TLA talko _new Sample\1spct\01.csv";
				CsvTool ct = new CsvTool();
				var res = ct.ReadCsv2String(path , rowDirction : false);
				return res [ 0 ].Select( x => Convert.ToDouble(x) ).ToArray();
			}
		}


		public List<PlrCrd> CreatedefualtPos()
		{
			lock(key)
			{
				CsvTool ct = new CsvTool();
				var path = @"D:\03JobPro\2017\011.ISP\CsvData\TLA talko _new Sample\1pos\pos.csv";
				var datas = ct.ReadCsv2String( path , ',' , false);

				List<PlrCrd> crtnlist = new List<PlrCrd>();

				for ( int i = 0 ; i < datas.Len(2) ; i++ )
				{
					
					for ( int j = 0 ; j < datas.Len(0) / 2 ; j++ )
					{
						var crt = new CrtnCrd(Convert.ToDouble( datas[ j * 2 ][i]) ,Convert.ToDouble( datas[ j * 2 + 1 ][i] ));
						var plr = crt.ToPolar();
						crtnlist.Add( plr as PlrCrd );
					}
				}
				return crtnlist;
			}
		}

		#endregion	
	}
}
