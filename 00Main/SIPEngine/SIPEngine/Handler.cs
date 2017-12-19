using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace SIPEngine
{
	using Img = Image<Gray , byte>;
	public static partial class Handler
	{

		public static Img PreProcessing( Img src )
			=> src;

		//  여기서 프로세싱 과정 진행해야한다. 

	}
}
