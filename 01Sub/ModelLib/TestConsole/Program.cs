using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using MachineLib.DeviceLib;

namespace TestConsole
{
    class Program
    {
        static void Main( string [ ] args )
        {
			Maya_Spectrometer sp = new Maya_Spectrometer();
			var res1 = sp.Connect();
			var res2 = sp.Timeout(1);
			var res = sp.GetSpectrum();
			
			

		}

        public class testclass
        {

        }
    }
}
