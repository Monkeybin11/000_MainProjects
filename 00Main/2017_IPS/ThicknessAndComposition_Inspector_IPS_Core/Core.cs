using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLib.DeviceLib;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public class Core
	{
		IMaya_Spectrometer Spct;

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





		public Core()
		{
			Spct = new Maya_Spectrometer();
		}

		public void Connect()
		{ 
		
		}

		public 


	}
}
