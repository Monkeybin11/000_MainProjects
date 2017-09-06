using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using ModelLib.TypeClass;
using ModelLib.ClassInstance;

namespace ModelLib.ClassInstance
{
	public static class PorpertyDescription // NonTimeNumSignal
	{
		public static NonTimeNumSignal<Maybe<A>> fmap<A>(
		this NonTimeNumSignal<Maybe<A>> src ,
		Func<A,A> func
		) 
		{
			var xlist = src.xData.Select<Maybe<A>,Maybe<A>>( x =>
			{
				var xjust = x as Just<A>;
				if( xjust == null)
				{
					return new Nothing<A>();
				}
				else
				{
					return new Just<A>( func(xjust.Value) );
				}
			}).ToList<Maybe<A>>();
			return new NTimeSg<A>( src.yData , xlist );
		}


		public static Maybe<A> GetY<A>(
		this NonTimeNumSignal<Maybe<A>> x , 
		A xdata )
		{
			//var mx = x as NTimeSg<A>;	
			//return mx == null ? new Nothing<A>
			//			      : new Just<A>( x.yData.IndexOf(x )
			return null;
		}

	}
}
