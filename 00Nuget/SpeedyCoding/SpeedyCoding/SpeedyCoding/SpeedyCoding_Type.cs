using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
	public static partial class Handler
	{
		public static Tuple<A , B> ToTuple<A, B>( A a , B b )
		{ return Tuple.Create( a , b ); }
	}
}
