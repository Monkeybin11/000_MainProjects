using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.TypeClass;
using ModelLib.Monad;

namespace ModelLib.ClassInstance
{
	public class NTimeSg<T> : NonTimeNumSignal<Maybe<T>>
	{

		public List<Maybe<T>> xData { get; set; }

		public List<Maybe<T>> yData { get; set; }

		public NTimeSg( List<Maybe<T>> ylist , List<Maybe<T>> xlist )
		{
			xData = xlist;
			yData = ylist;
		}

	}
}
