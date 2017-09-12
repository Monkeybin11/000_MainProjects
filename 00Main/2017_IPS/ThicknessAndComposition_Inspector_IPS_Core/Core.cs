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


namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public class IPSCore
	{
		
		IPSErrorMsgData	   Err; // 에러 데이터 공통 메세지 클래스 
		Logger			   Lggr; // 같은 로그 파일에 사용하기 위해서 전역 변수로 만들었다. 
		IPSConfig		   Config; // 여기서 
		ISgmaStg_XR        Stg;
		IMaya_Spectrometer Spct;

		string LogTime = "yyyy-MM-dd_HH-mm-ss";

		string ConfigBasePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "config";
		string ConfigName = "SettedConfig.xml";

		string LogDirPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "log";
		string LogName = "IPSLog.txt";

		string CurentSaveTime { get { return DateTime.Now.ToString( LogTime ); } }


		#region Init

		public IPSCore()
		{
			InitCore(); // Field Initialize
			if(ConnectHW()) MessageBox.Show( "Virtual Mode is Activated" );
		}

		public void InitCore()
		{
			Lggr = new TextLogger( LogDirPath , LogName )
					.Act(x => 
			Err = new IPSErrorMsgData( x ) ); 

			var fullpath = Path.Combine(ConfigBasePath , ConfigName);
			if ( !Directory.Exists( ConfigBasePath ) ) Directory.CreateDirectory( ConfigBasePath );
			Config = new XmlTool().ReadXmlClas(
								   new IPSDefualtSetting().ToConfig() ,
								   ConfigBasePath ,
								   ConfigName );
		}

		public bool ConnectHW()
		{
			Stg = new SgmaStg_XR();
			Spct = new Maya_Spectrometer();

			var stg =  !Stg.Open();
			var spt = !Spct.Connect();

			if ( stg ) Err.WriteShowErr(ErrorType.StgConnectErr) ;
			if( stg ) Err.WriteShowErr( ErrorType.SpecConnectErr );
			if ( stg && spt )
			{
				Stg = new SgmaStg_XR_Virtual();
				Spct = new Maya_Spectrometer_Virtual();
			}
			return stg && spt;
			// False 는 버추얼 모드시 반환됨
		}

		#endregion

		#region MainFunction

		public void TestFunction()
		{
			Stg.Send( Stg.Home );
			var res = Spct.GetSpectrum();
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
