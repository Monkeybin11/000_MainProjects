using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLib.DeviceLib
{
	public class Maya_Spectrometer_Virtual : IMaya_Spectrometer
	{
		public IMaya_Spectrometer BoxCar( int width )
		{
			return this;
		}

		public bool Connect()
		{
			return true;
		}

		public double [ ] GetSpectrum()
		{
			Random rnd = new Random();
			return Enumerable.Range(0,2068).Select( x => (double)rnd.Next(0,3000)).ToArray<double>();
		}

		public double [ ] GetWaveLen()
		{
			double w = (1120 - 200) / 2068.0;
			return Enumerable.Range( 0 , 2068 ).Select( x => x * w + 200 ).ToArray<double>();
		}

		public IMaya_Spectrometer IntegrationTime( int time )
		{
			return this;
		}

		public string LastException()
		{
			return "Last Exception";
		}

		public IMaya_Spectrometer RemoveDark()
		{
			return this;
		}

		public IMaya_Spectrometer ScanAvg( int count )
		{
			return this;
		}

		public string SerialNum()
		{
			return "Dummy";
		}

		public IMaya_Spectrometer Timeout( int millisec )
		{
			return this;
		}
	}
}
