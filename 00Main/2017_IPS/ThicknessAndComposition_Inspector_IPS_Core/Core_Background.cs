using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpeedyCoding;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public partial class IPSCore
	{
		
		public string ConfigBasePath = AppDomain.CurrentDomain.BaseDirectory + "config";
		public string ConfigName = "SettedConfig.xml";
		public string PrcConfigBasePath = AppDomain.CurrentDomain.BaseDirectory + "Prcconfig";
		public string PrcConfigName = "PrcSettedConfig.xml";

		public string ConfigFullPath { get { return Path.Combine( ConfigBasePath , ConfigName ); } }
		public string PrcConfigFullPath { get { return Path.Combine( PrcConfigBasePath , PrcConfigName ); } }
		string LogTime = "yyyy-MM-dd_HH-mm-ss";
		string CurrentSaveTime { get { return DateTime.Now.ToString( LogTime ); } }
		string LogDirPath = AppDomain.CurrentDomain.BaseDirectory + "log";
		string LogName = "";

		double[] BkD_Spctrm = new double[] { }; // Background Data = Bkd_
		double[] Bkd_WaveLen = new double[] { }; 
		double [ ] SpctrmDeciles {
			get { return BkD_Spctrm
					.Map( x =>
							200.xRange( 40 , 20 )
							.Select( i => Bkd_WaveLen [ i ] ) )
					.ToArray();} }
		double [ ] WaveLenDeciles {
			get { return Bkd_WaveLen
					.Map( x =>
							200.xRange( 40 , 20 )
							.Select( i => Bkd_WaveLen [ i ] ))
					.ToArray(); } }

		bool FlgAutoUpdate;

		Action AutoUpdateSpctrm =>
			() => BkD_Spctrm = Spctr.GetSpectrum();
	}
}
