using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointerPractice
{
	unsafe class A
	{
		public int a = 100;
		public int b = 200;

	}

	public unsafe class SimplePointer
	{
		public void main()
		{
			byte[] rawdata = new byte[1024];

			fixed ( byte* bptr = rawdata )
			{
				int* ptr=(int*)bptr;
			}

		}

		public unsafe int [ ] byteArr2intArr( byte [ ] src)
		{
			int* intptr;
			int len = src.Length / 4;
			int[] output = new int[len];
			fixed ( byte* srcptr = src)
			{
				intptr = ( int* )srcptr;
				for ( int i = 0 ; i < output.Length ; i++ )
				{
					output [ i ] = *( intptr++ );
				}
			}
			return output;
		}

		public int[] convert( int [ ] num , byte[] by )
		{
			Buffer.BlockCopy( by , 0 , num , 0 , by.Length );
			return num;
		}

		//public ushort[] litte2big( byte [ ] littlebytes )
		//{
		//	int* intptr;
		//	int len = littlebytes.Length / 4;
		//	int[] output = new int[len];
		//	fixed ( byte* srcptr = littlebytes )
		//	{
		//		intptr = ( int* )srcptr;
		//		for ( int i = 0 ; i < output.Length ; i++ )
		//		{
		//			output [ i ] = *( intptr++ );
		//		}
		//	}
		//	return output;
		//
		//
		//}

	}
}
