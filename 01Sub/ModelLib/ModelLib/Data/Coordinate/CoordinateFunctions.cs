using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
	using ModelLib.Data;
	using ModelLib.Data.NewType;
	using ModelLib.AmplifiedType;
	using static ModelLib.Handler;
	public static partial class Handler
	{
		public static Func<ValPosCrt , ValPosCrt> FnReScale
			( Width w0 , Height h0 , Width w1 , Height h1 )
		{
			var wRatio = w1/w0;
			var hRatio = h1/h0;
			return (valpos) => ValPosCrt( valpos.X * wRatio , 
										  valpos.Y * hRatio , 
										  valpos.Value );
		}

	}
}
