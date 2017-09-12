using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLib.DeviceLib
{
	public class SgmaStg_XR_Virtual : ISgmaStg_XR
	{
		public string Go { get; set; }
		public string GoAbs { get; set; }
		public string Home { get; set; }
		public string Pos { get; set; }
		public string SetSpeed { get; set; }
		public string Status { get; set; }
		public string StatusOK { get; set; }
		public string Stop { get; set; }

		public bool Open()
		{
			return true;
		}

		public string Query( string cmd )
		{
			return "Virtual Mode";
		}

		public bool Send( string cmd )
		{
			return true;
		}
	}
}
