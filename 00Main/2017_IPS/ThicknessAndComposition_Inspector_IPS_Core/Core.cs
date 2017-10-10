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
using System.Windows.Input;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCvExtension;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public partial class IPSCore
	{
		public IPSConfig Config { get; set; }
		public IPSProcessingConfig PrcConfig { get; set; }
		IPSErrorMsgData    Err; // 에러 데이터 공통 메세지 클래스 
		Logger             Lggr; // 같은 로그 파일에 사용하기 위해서 전역 변수로 만들었다. 
		ISgmaStg_XR        Stg;
		IMaya_Spectrometer Spctr;
		Image<Bgr,byte> ImgScanResult;
		Image<Bgr,byte> Imgscalebar;
		public System.Windows.Media.Imaging.BitmapSource ImgScanned { get { return ImgScanResult.ToBitmapSource(); } }
		public System.Windows.Media.Imaging.BitmapSource ImgScaleBar { get { return Imgscalebar.ToBitmapSource(); } }

		public List<int> PickedIdx = new List<int>();
		public List<double> Wave = new List<double>();
		public List<double> Refs = new List<double>();
		public List<double> Darks = new List<double>();
		public List<int> SDWaves = new List<int>();
		public List<double> ReflctFactors = new List<double>();


		#region Init
		public IPSCore()
		{
			InitCore(); // Field Initialize

		}

		public void Connect()
		{
			ConnectHW( "COM" + Config.Port.ToString() ).FailShow( "Virtual Mode is Activated" );
		}


		public string TodayLogPath()
		=> DateTime.Now.ToString( "yyyyMMdd" ) + "_IPSLog.txt";

		public void InitCore()
		{
			LogName = TodayLogPath();
			Lggr = new TextLogger( LogDirPath , LogName )
					.Act( x => Err = new IPSErrorMsgData( x ) );

			Config = XmlTool.ReadXmlClas(
						new IPSDefualtSetting().ToConfig() , // Defulat Setting
						ConfigBasePath.CheckAndCreateDir() ,
						ConfigName.CheckAndCreateFile() );


			var defualt = new IPSProcessingConfig()
			{
				IntglStart = 440 ,
				IntglEnd = 470 ,
				a = -0.5277 ,
				b = 1838.9
			};

			PrcConfig = XmlTool.ReadXmlClas(
							defualt ,
							PrcConfigBasePath.CheckAndCreateDir() ,
							PrcConfigName.CheckAndCreateFile() );

			Task.Run( () =>
			 {
				 while ( true )
				 {
					 if ( FlgAutoUpdate )
					 {
						 AutoUpdateSpctrm();
					 }
					 Thread.Sleep( 80 );
				 }
			 } );

			//Task.Run( () =>
			//{
			//	while ( true )
			//	{
			//		if ( FlgAutoUpdate )
			//		{
			//			GetPos();
			//		}
			//		Thread.Sleep( 0 );
			//	}
			//} );

		}

		public bool ConnectHW( string comport )
		{
			Stg = new SgmaStg_XR( comport , false );
			Spctr = new Maya_Spectrometer();

			var stg =  Stg.Open() == true
						? true
						: false.Act( x =>  Err.WriteShowErr( ErrorType.StgConnectErr ));

			var spt = Spctr.Connect()  == true
						? true
						: false.Act( x =>  Err.WriteShowErr( ErrorType.SpecConnectErr ));

			if ( !stg || !spt )
			{
				Stg.Close();
				Stg = new SgmaStg_XR_Virtual();
				Spctr = new Maya_Spectrometer_Virtual();
			}

			evtConnection( stg , spt );

			return stg && spt;
		}

		#endregion

		#region MainFunction
		int counter = 0;
		public Func<LEither<double [ ]> , double [ ] , PlrCrd ,
						Tuple<PlrCrd , LEither<double>>> CalcPorce =>
			( inten , wave , plrcrd ) =>
			{
				double[] intendata = new double[] { };
				double[] wav = new double[] { };
		
				PlrCrd crd = new PlrCrd(0,0);
				double res = 355.46848;

				try
				{
					if ( plrcrd.Rho != 0 )
					{
						var intentemp = LoadTestData();
						var postemp = CreatedefualtPos();

						intendata = intentemp [ counter ];
						crd = postemp [ counter ];
						wav = LoadWavelen();

						var integ = intendata.Integral(wav , PrcConfig.IntglStart , PrcConfig.IntglEnd);
						res = integ * PrcConfig.a + PrcConfig.b;

						//Console.WriteLine( "Coordinate : " + crd.ToString() );
						//Console.WriteLine( "Integral : " + integ );

						updatecounter();
						"updated".Print( counter.ToString() );
					}
					else
					{
						"Not updated".Print( counter.ToString() );
						plrcrd.ToString().Print( "Current POs" );
					}
				}
				catch ( Exception )
				{
				}
				return Tuple.Create( crd , res.ToLEither() );
			};


		//public Func<LEither<double [ ]> , double [ ] , PlrCrd ,
		//				Tuple<PlrCrd , LEither<double>>> CalcPorce =>
		//	( inten , wave , plrcrd ) =>
		//	{
		//		return Tuple.Create( plrcrd,
		//							 inten.Bind( x => x.Integral(wave , PickedIdx ,Darks,Refs, ReflctFactors , PrcConfig.IntglStart , PrcConfig.IntglEnd), "Integral Fail" )
		//								  .Bind( x => ( x*PrcConfig.a + PrcConfig.b).Print("result") ));
		//	};

		
		List<Tuple<PlrCrd,LEither<double>>> TempRes = new List<Tuple<PlrCrd, LEither<double>>>();
		public bool ScanRun1()
		{
			counter = 0;
			FlgAutoUpdate = true;

			var calcTaskList = new Task< Tuple<PlrCrd , LEither<double> >>[ Config.ScanSpot.Len() ];
			var wavelength = Bkd_WaveLen = Spctr.GetWaveLen();

			SetHWInternalParm(
		   Config.RStgSpeed ,
		   Config.XStgSpeed ,
		   Config.Scan2Avg ,
		   Config.IntegrationTime ,
		   Config.Boxcar
		   );

			var res1 = 0.ToPos( Axis.X );
			var res = new TEither( Stg as IStgCtrl , 12)
						//.Bind( x => x.Act( f =>
						//	f.SendAndReady( f.Home + Axis.W.ToIdx() ) ).ToTEither( 12 ) , "R or X Home Fail" )
						.Bind( x => x.Act( f =>
									f.SendAndReady( f.GoAbs
													+ 0.ToOffPos(Axis.X).Print() ) )
											 .ToTEither( 1 ) , "Opposit Movement Fail" )
								.Bind( x => x.Act( f =>
									f.SendAndReady( f.Go ) ).ToTEither( 1 ) , "Stage Movement Fail" );

			return true;
		}


		public bool ScanRun() // Use Internal Config , not get config from method parameter
		{
			counter = 0;
			FlgAutoUpdate = true;

			var calcTaskList = new Task< Tuple<PlrCrd , LEither<double> >>[ Config.ScanSpot.Len() ];
			var wavelength = Bkd_WaveLen = Spctr.GetWaveLen();

			SetHWInternalParm(
							  Config.RStgSpeed ,
							  Config.XStgSpeed ,
							  Config.Scan2Avg ,
							  Config.IntegrationTime ,
							  Config.Boxcar
							  );

			// Home And Check --
			if ( !FlgHomeDone ) if ( !OpHome() ) return false;
			FlgHomeDone = true;

			// Scan Ready And Check -- 
			if ( FlgScanReady == false ) if ( !OpReady(ScanReadyMode.All) ) return false;
			FlgScanReady = true;

			var res = new TEither( Stg as IStgCtrl , 12);
						//.Bind( x => x.Act( f =>
						//	f.SendAndReady( f.Home + Axis.W.ToIdx() ) ).ToTEither( 12 ) , "R or X Home Fail" )
						//.Bind( x => x.Act( f =>
						//			f.SendAndReady( f.GoAbs
						//							+ 0.ToOffPos(Axis.X) ) )
						//					 .ToTEither( 1 ) , "Opposit Movement Fail" )
						//		.Bind( x => x.Act( f =>
						//			f.SendAndReady( f.Go ) ).ToTEither( 1 ) , "Stage Movement Fail" );

			var posIntenlist = Config.ScanSpot.Select( ( pos , i) =>
				 {
					 pos.Print("current pos");
					 var logres = res
								.Bind( x => x.Act( f =>
									f.SendAndReady( f.GoAbs
													+ pos.Theta.Degree2Pulse().ToPos( Axis.W )
													+ pos.Rho.mmToPulse().ToOffPos() ) )
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
				 } ).ToList().Duplicate0ToAllTheta().ToList(); // Raw Data duplicate [0,0] datas to [0,45] [0,90] [0,135]....

			// Thickness Result List
			// ( You dont need to use try catch pattern for catch all exception from tasklist )
			var posThicknesses = Task.WhenAll( calcTaskList ).Result.Duplicate0ToAllTheta().ToList();

			// Reset Pos
			Task.Run( () => { 
			res.Bind( x => x.Act( f => Stg.SendAndReady( Stg.SetSpeed
													     + Axis.W.ToIdx()
													     + ( 50000 ).ToSpeed()
													     + ( 200000 ).ToSpeed() )).ToTEither(1))
				.Bind( x => x.Act( f => f.SendAndReady( f.GoAbs + 0.ToPos( Axis.W ) + 0.ToPos() ) ).ToTEither( 1 ) )
			    .Bind( x => x.Act( f => f.SendAndReady( f.Go ) ).ToTEither( 1 ) );
			} );

			FlgAutoUpdate = false;
			//For Simulation 
			var posThickInten = posThicknesses
									.Zip(posIntenlist ,
										(f,s) => new { Pos = f.Item1 ,
													   Thickness = f.Item2 ,
													   Intensity = s.Item2 } );

			// -- Integration Log Result -- // 
			bool isright = posThicknesses.Select( x => x.Item2.IsRight).Aggregate( (f,s) => f && s);

			if ( !isright ) // If Task Error
			{
				var tasklogs = posThickInten.Where( x => x.Thickness.IsRight == false)
											.Select( x => x.Pos.ToString()+" ||" + x.Thickness.Left )
											.ActLoop( x => Lggr.Log(x , true));
				ImgScanResult = new Image<Bgr , byte>( @" C:\Temp\testImg034837.bmp " );
				return false;
			}
			else
			{
				var poses     = posThickInten.Select( x => x.Pos ).ToList();
				var thckneses = posThickInten.Select( x => x.Thickness.Right ).ToList();
				var intens    = posThickInten.Select( x => x.Intensity.Right ).ToList();
				

				ResultData = ToResult( poses , thckneses , intens , Bkd_WaveLen );
				CreateMap( ResultData , 5 )
					.Act( x => ImgScanResult = x [ 0 ] )
					.Act( x => Imgscalebar   = x [ 1 ] );
					;
				// evtScanImg( CreateMap( ResultData , 5 ).ToBitmapSource());
				return true;
			}
		}

		public Image<Bgr,byte>[] CreateMap(IPSResult src , int divide)
		{
			var scalebarTask = new Task<Image<Bgr,byte>>(
									() => CreateScalebar())
							    .Act( x => x.Start());

			var imgshiftoffset = 3;
			var offset = src.SpotDataList.Select( x => x.PlrPos.Rho).Max(); 
			var thlist = src.SpotDataList.Select( x => x.Thickness);
			var min = thlist.Min();
			var max = thlist.Max();
			
			var cm = new ColorMap().RB_cm;

			//var xyCm = src.Result2TRThArr()
			//			   .Interpol_Rho(divide)
			//			   .Interpol_Theta(divide)
			//			   .ToCartesianReslt()
			//			   .Select( x => new
			//						  {
			//							   X  = x[0] ,
			//							   Y  = x[1] ,
			//							   Cm = cm[ (int)(( x[2] -min )/(max - min)*255) ]　//color double[r,g,b]
			//						  });

			var xyCm1 = src.Result2TRThArr().OrderBy( x => x[0]).Select( x =>x).ToList();
			var xyCm2 = xyCm1            .Interpol_Theta(divide).OrderBy( x => x[0]).ThenBy( x => x[1]).Select( x =>x).ToList();
			var xyCm3 = xyCm2             .Interpol_Rho(divide).OrderBy( x => x[0]).ThenBy( x => x[1]).Select( x =>x).ToList();
			var xyCm4 = xyCm3             .ToCartesianReslt().OrderBy( x=> x[0]).ThenBy( x => x[1]).Select( x =>x).ToList();


			var xyCm = xyCm4              .Select( x => new
									  {
										   X  = offset + x[0] ,
										   Y  = offset + x[1] ,
										   Cm = (min - max) == 0 ? cm[127] : cm[ (int)(( x[2] -min )/(max - min)*255) ]　//color double[r,g,b]
									  }).ToList();

			//"-----  Fisrt  -----".Print();
			//xyCm1.ActLoop( x => ( "x : " + x [ 0 ].ToString() + "  y : " + x [ 1 ].ToString() ).Print() );
			//"".Print();
			//"-----  Second After Rho  -----".Print();
			//xyCm2.ActLoop( x => ( "x : " + x [ 0 ].ToString() + "  y : " + x [ 1 ].ToString() ).Print() );
			//"".Print();
			//"-----  third After Theta -----".Print();
			//xyCm3.ActLoop( x => ( "theta : " + x [ 0 ].ToString() + "  rho : " + x [ 1 ].ToString() ).Print() );
			//"".Print();
			//"-----  Fourth After ToCartesianReslt -----".Print();
			//xyCm4.ActLoop( x => ( "x : " + Math.Round(x [ 0 ]).ToString() + "  y : " + Math.Round(x [ 1 ]).ToString() ).Print() );
			//
			//"".Print();
			//"-----  Final -----".Print();
			//xyCm.ActLoop( x => ( "x : " + Math.Round( x.X ).ToString() + "  y : " + Math.Round( x.Y).ToString() ).Print() );
			//
			//
			//"".Print();
			//"-----  Final(original) -----".Print();
			//xyCm.ActLoop( x => ( "x : " +  x.X .ToString() + "  y : " + x.Y .ToString() ).Print() );
			//
			//
			//xyCm.Where( x => x.Y <= 15 ).Count().Print(" Y < 15 Count : ");
			//xyCm.Where( x => x.Y >= 15 ).Count().Print(" Y > 15 Count : ");

			//xyCm.Select( x => x.X.ToString() + " , " + x.Y.ToString()).ActLoop( x => x.Print() );
			//"************".Print();
			//"************".Print();


			var imgsize = Math.Max(
				 xyCm.Select( x => x.X).Max(),
				 xyCm.Select( x => x.Y).Max()
				);

			var imgData = new byte[(int)(imgsize*10 + imgshiftoffset*2.0) ,(int)(imgsize*10+imgshiftoffset*2.0),3];
			Image<Bgr,byte> img = new Image<Bgr, byte>(imgData);
		
			var circleLst = xyCm.Select(x =>
									new
									{
										pos = new System.Drawing.Point(
																			(int)(x.X*10)+imgshiftoffset,
																			(int)(x.Y*10)+imgshiftoffset),
														
										color = new MCvScalar(x.Cm[2]*255 , x.Cm[1]*255 , x.Cm[0]*255)
									});



			circleLst.ActLoop( x => CvInvoke.Circle( img , x.pos , 3 , x.color, -1 , Emgu.CV.CvEnum.LineType.EightConnected  ) );

			@"c:\temp\".CheckAndCreateDir();
			img.Save( @"c:\temp\testImg" + DateTime.Now.ToString("hhmmss") + ".bmp" );

			return new Image<Bgr , byte> [ ] { img , scalebarTask.Result };
		}

		public Image<Bgr , byte> CreateScalebar()
		{

			var scalebar = new byte[255,2,3];
			var cm = new ColorMap().RB_cm;
			for ( int i = 0 ; i < 255 ; i++ )
			{
				scalebar [ i , 0 , 0 ] = ( byte )( cm [ i ] [ 0 ] * 255 );
				scalebar [ i , 0 , 1 ] = ( byte )( cm [ i ] [ 1 ] * 255 );
				scalebar [ i , 0 , 2 ] = ( byte )( cm [ i ] [ 2 ] * 255 );
				scalebar [ i , 1 , 0 ] = ( byte )( cm [ i ] [ 0 ] * 255 );
				scalebar [ i , 1 , 1 ] = ( byte )( cm [ i ] [ 1 ] * 255 );
				scalebar [ i , 1 , 2 ] = ( byte )( cm [ i ] [ 2 ] * 255 );
			}
			return new Image<Bgr , byte>( scalebar );
		}

		public void test()
		{
			Mapping.LoadMapdata();
		}


		#endregion

		#region OpFun

		public bool OpReady( ScanReadyMode mode )
		{
			Stg.SendAndReady( Stg.SetSpeed + Axis.W.ToIdx()
										   + ( 50000 ).ToSpeed()
										   + ( 200000 ).ToSpeed() );
			LEither<bool> result = new LEither<bool>(true);
			switch ( mode )
			{
				case ScanReadyMode.Dark:
					result.Bind( x => OpSetdark() , "Dark Scan is Fail" );
					break;

				case ScanReadyMode.Ref:
					result.Bind( x => OpSetRef() , "Referance Scan is Fail" );
					break;

				case ScanReadyMode.Refelct:
					result.Bind( x => OpLoadReflecDatas() , "Refelct Scan is Fail" );
					break;

				case ScanReadyMode.WaveLen:
					result.Bind( x => OpPickWaveIdx() , "WaveLength Scan is Fail" );
					break;
				case ScanReadyMode.All:
					result.Bind( x => OpScanReady() , "Scan Ready is Fail" );
					break;
			}
			return result.IsRight
				? true
				: false;
		}

		public bool OpScanReady()
		{
			return new LEither<bool>( true )
					.Bind( x => OpSetdark() , "Dark Scan is Fail" )
					.Bind( x => OpSetRef() , "Referance Scan is Fail" )
					.IsRight
				? true.Act( x => FlgScanReady = true )
				: false.Act( x => FlgScanReady = false );
		}

		public bool OpSetdark() //sjw
		{
			FlgAutoUpdate = true;
			Stg.SendAndReady( Stg.GoAbs + 0.ToPos( Axis.X ) );
			Stg.SendAndReady( Stg.Go );
			Thread.Sleep( 500 );
			var darkraw = BkD_Spctrm;
			Darks = PickedIdx.Select( x => darkraw [ x ] ).ToList();
			FlgAutoUpdate = false;
			return true;
		}

		public bool OpSetRef() //sjw
		{
			FlgAutoUpdate = true;
			Stg.SendAndReady( Stg.GoAbs + 0.ToOffPos( Axis.X ) );
			Stg.SendAndReady( Stg.Go );
			Thread.Sleep( 500 );
			var refraw = BkD_Spctrm;
			Refs = PickedIdx.Select( x => refraw [ x ] ).ToList();
			FlgAutoUpdate = false;
			return true;
		}

		public bool OpPickWaveIdx() //sjw
		{
			Bkd_WaveLen = Spctr.GetWaveLen();
			var wave = Bkd_WaveLen.Select( x => (int)x).ToArray();
			List<int> idxlist = new List<int>();
			foreach ( var item in SDWaves )
			{
				var idx = Array.IndexOf( wave , item);
				if ( idx >= 0 ) idxlist.Add( idx );
			}
			PickedIdx = idxlist;
			return true;
		}

		public bool OpHome()
		{
			var resHome = new TEither( Stg as IStgCtrl , 12)
						.Bind( x => x.Act( f =>
							f.SendAndReady( f.Home + Axis.W.ToIdx() ) ).ToTEither( 12 ) , "R or X Home Fail" );
			//.Bind( x => x.Act( f =>
			//			f.SendAndReady( f.GoAbs
			//							+ 0.ToOffPos(Axis.X) ) )
			//					 .ToTEither( 1 ) , "Opposit Movement Fail" )
			//.Bind( x => x.Act( f =>
			//	f.SendAndReady( f.Go ) ).ToTEither( 1 ) , "Stage Movement Fail" );
			if ( !resHome.IsRight ) return false.Act( x => FlgHomeDone = false );
			return true.Act( x => FlgHomeDone = true );
		}

		public bool OpLoadReflecDatas() //sjw
		{
			//string path = @"C:\000_MainProjects\00Main\2017_IPS\ThicknessAndComposition_Inspector_IPS\ThicknessAndComposition_Inspector_IPS\bin\x64\Debug\10d_avg.csv";
			string path = AppDomain.CurrentDomain.BaseDirectory + "10d_avg.csv";
			CsvTool cv = new CsvTool();
			var relec = cv.ReadCsv2String( path );
			SDWaves = relec.Select( x => Convert.ToInt32( x [ 0 ] ) ).ToList();
			ReflctFactors = relec.Select( x => Convert.ToDouble( x [ 1 ] ) ).ToList();
			return true;
		}
		#endregion

		#region IO

		public void LoadConfig( string path )
		{
			Config = XmlTool.ReadXmlClas(
						Config ,
						Path.GetDirectoryName( path ) ,
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
		public IPSResult ToResult( List<PlrCrd> pos , List<double> thckness , List<double [ ]> intens , double [ ] wavelen )
		{
			var res = new IPSResult(wavelen );
			for ( int i = 0 ; i < pos.Count ; i++ )
			{
				res.SpotDataList.Add( new SpotData( pos [ i ] , thckness [ i ] , intens [ i ] ) );
			}
			return res;
		}
		#endregion

		#region SubFuntion
		public void ShowSetting()
		{
			StringBuilder configlog = new StringBuilder();
			Type ty = Config.GetType();
			foreach ( PropertyInfo config in ty.GetProperties() ) // pick one property from class
			{
				var condition = config.GetValue( Config ) // get property value
									  .GetType() // type of value
									  .GetInterfaces() // interface of type
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

		public void SetHWInternalParm( double rspeed , double xspeed , double scan2avg , double intetime , double boxcar )
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
		public void updatecounter()
		{
			lock ( key )
			{
				counter++;
			}
		}

		
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
				return res [ 0 ].Select( x => Convert.ToDouble( x ) ).ToArray();
			}
		}


		public List<PlrCrd> CreatedefualtPos()
		{
			lock ( key )
			{
				CsvTool ct = new CsvTool();
				var path = @"D:\03JobPro\2017\011.ISP\CsvData\TLA talko _new Sample\1pos\pos.csv";
				var datas = ct.ReadCsv2String( path , ',' , false);

				List<PlrCrd> crtnlist = new List<PlrCrd>();

				for ( int i = 0 ; i < datas.Len( 2 ) ; i++ )
				{

					for ( int j = 0 ; j < datas.Len( 0 ) / 2 ; j++ )
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
