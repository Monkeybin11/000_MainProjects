using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.TypeClass
{
	interface Either<A,B>
	{
		A Left { get; set; }
		B Right { get; set; }
	}
}
