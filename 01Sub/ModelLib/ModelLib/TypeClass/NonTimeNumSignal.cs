using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.TypeClass
{
	public interface NonTimeNumSignal<T>
	{
		List<T> yData { get; set; }
		List<T> xData { get; set; }
	}
}
