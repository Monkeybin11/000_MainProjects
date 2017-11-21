using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
	public static class SpeedyCoding_Conversion
	{
		/// <summary>
		/// Nullable to non-nullable 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <returns></returns>
		public static T ToNonNullable<T>( this Nullable<T> src ) where T : struct
		{
			return src == null ? default( T ) : (T)src;
		}
	}
}
