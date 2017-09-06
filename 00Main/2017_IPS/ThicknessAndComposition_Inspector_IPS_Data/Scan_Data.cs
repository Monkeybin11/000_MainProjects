using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Data;

namespace ThicknessAndComposition_Inspector_IPS_Data
{
	public class Scan_Data
	{
		public List<PlrCrd> Coordinate;
		public Scan_Data( List<PlrCrd> cord )
		{
			Coordinate = cord;
		}
	}
}
