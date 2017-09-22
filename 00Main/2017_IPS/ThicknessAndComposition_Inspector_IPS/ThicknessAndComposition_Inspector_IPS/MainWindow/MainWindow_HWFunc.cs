﻿using SpeedyCoding;
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
			res.ThetaFirst = ucLSMenu.nudThetaFirst.Value.ToNonNullable();
			res.ThetaStep = ucLSMenu.nudThetaStep.Value.ToNonNullable();
			res.RhoFirst = ucLSMenu.nudRhoFirst.Value.ToNonNullable();
			res.RhoStep = ucLSMenu.nudRhoStep.Value.ToNonNullable();
			res.RhoCount = ucLSMenu.nudRhoCount.Value.ToNonNullable();
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
			ucLSMenu.nudThetaFirst.Value = config.ThetaFirst;
			ucLSMenu.nudThetaStep.Value = config.ThetaStep;
			ucLSMenu.nudRhoFirst.Value = config.RhoFirst;
			ucLSMenu.nudRhoStep.Value = config.RhoStep;
			ucLSMenu.nudRhoCount.Value = config.RhoCount;
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
