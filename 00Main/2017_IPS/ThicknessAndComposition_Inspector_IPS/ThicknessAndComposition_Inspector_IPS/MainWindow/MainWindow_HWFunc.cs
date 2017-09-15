using SpeedyCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThicknessAndComposition_Inspector_IPS_Data;

namespace ThicknessAndComposition_Inspector_IPS
{
	public partial class MainWindow
	{


		#region MidFunc
		IPSConfig UI2IpsConfig()
		{
			var res = new IPSConfig();
			res.AngFirst = ucLSMenu.nudAngFirst.Value.ToNonNullable();
			res.AngStep = ucLSMenu.nudAngStep.Value.ToNonNullable();
			res.RFirst = ucLSMenu.nudRFirst.Value.ToNonNullable();
			res.RStep = ucLSMenu.nudRStep.Value.ToNonNullable();
			res.RCount = ucLSMenu.nudRCount.Value.ToNonNullable();
			res.Port = ( int )WinConfig.nudXStgPort.Value.ToNonNullable();
			res.IntegrationTime = ( int )WinConfig.nudIntegrationTime.Value.ToNonNullable();
			res.Scan2Avg = ( int )WinConfig.nudScan2Avg.Value.ToNonNullable();
			res.Boxcar = ( int )WinConfig.nudBoxcar.Value.ToNonNullable();
			res.XStgSpeed = ( int )WinConfig.nudXStgSpeed.Value.ToNonNullable();
			res.RStgSpeed = ( int )WinConfig.nudRStgSpeed.Value.ToNonNullable();

			res.BaseDirPath = Core.ConfigBasePath;
			res.StartupConfigName = Core.ConfigName;
			res.ScanSpot = Core.Config.ScanSpot;
			return res;
		}

		void Config2UI( IPSConfig config )
		{
			ucLSMenu.nudAngFirst.Value = config.AngFirst;
			ucLSMenu.nudAngStep.Value = config.AngStep;
			ucLSMenu.nudRFirst.Value = config.RFirst;
			ucLSMenu.nudRStep.Value = config.RStep;
			ucLSMenu.nudRCount.Value = config.RCount;
			WinConfig.nudXStgPort.Value = config.Port;
			WinConfig.nudIntegrationTime.Value = config.IntegrationTime;
			WinConfig.nudScan2Avg.Value = config.Scan2Avg;
			WinConfig.nudBoxcar.Value = config.Boxcar;
			WinConfig.nudXStgSpeed.Value = config.XStgSpeed;
			WinConfig.nudRStgSpeed.Value = config.RStgSpeed;
		}


		#endregion
	}
}
