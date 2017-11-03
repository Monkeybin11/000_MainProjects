using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVTSLib.Data
{
	public interface ResultFormat
	{
	}

	public class LinePLFormat : ResultFormat
	{
		public int Hindex;
		public int Windex;
		public int HindexError;
		public int WindexError;
		public string OKNG;
		public double Intensity;
		public double AreaSize;
		public System.Drawing.Rectangle BoxData;
	}

}
