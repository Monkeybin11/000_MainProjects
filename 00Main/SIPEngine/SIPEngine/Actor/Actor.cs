using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using ModelLib.AmplifiedType;
using SpeedyCoding;

namespace SIPEngine
{
	using static ModelLib.AmplifiedType.Handler;
	using Img = Image<Gray , byte>;

	public static partial class Handler 
	{
		public static Func< Img, IEnumerable<Func<Img, Img>> , Img> RunProcessing
            => (  src , funcList )
			=> funcList.FoldL( src );
	}

	




}
