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
		//string LogName = "IPSLog.txt";

		string CurrentSaveTime { get { return DateTime.Now.ToString( LogTime ); } }


		#region Init

		public IPSCore()
		{
			InitCore(); // Field Initialize
			ConnectHW().FailShow( "Virtual Mode is Activated" );
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

		public bool ConnectHW()
		{
			Stg = new SgmaStg_XR();
			Spct = new Maya_Spectrometer();

			var stg =  Stg.Open() == true 
						? true 
						: false.Act( x =>  Err.WriteShowErr( ErrorType.StgConnectErr ));

			var spt = Spct.Connect()  == true 
						? true 
						: false.Act( x =>  Err.WriteShowErr( ErrorType.SpecConnectErr ));

			if ( !stg || !spt )
			{
				Stg = new SgmaStg_XR_Virtual();
				Spct = new Maya_Spectrometer_Virtual();
			}
			return !stg || !spt;
		}

		#endregion

		#region MainFunction

		public void TestFunction()
		{
			Stg.Send( Stg.Home );
			var res = Spct.GetSpectrum();
			MessageBox.Show( res.GetLength(0).ToString() );

			var temp = Config;

		}

		public bool ScanRun()
		{
			// create Task list. same length with Posittion List 
			// Foreach PositionList 
			//  move  -> measure
			//  data save => create Task and Run task.
			//  
			//  Error
			// if move error , measure error , calc error
			// move error = timeout (Either Left) -> measure (Either Left) -> calc(Either Left)
			// 



			// stage ready position 
			// spectrometer measrue
			// repeate

			// save data 

			// if scan is suceed start mapping map 
			// do event 

			return false;
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


		// TODO : Run
		// TODO : - movestage
		// TODO : - scan 
		// TODO : - scan 결과 가져오기 

		// TODO : Trans Display Data

		// TODO : Log Write

		// TODO : analysis


		// TODO : Log System
		// TODO : Check Spectrometer
		// TODO : Check Stage
		// TODO : Spct get
		// TODO : stage move ( time out )
		// TODO : -- Data --
		// TODO : Create Display Data
		// TODO : -- analysis --
		// TODO : 각각의 작업중에 문제가 생기면 로그를 남기게 한다. 
		// TODO : 일련의 작업을 완료후 로그가 있는지 체크한다. 
		// TODO : 문제가 생겨서 발생한 로그는 앞에 시간을 적는다. 

	}
}
