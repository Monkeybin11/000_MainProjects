using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointerPractice
{
	class Program
	{
		static void Main( string [ ] args )
		{
			//testendian();
			new MarchalCopyEx().main();





		}



		public static void testcopy()
		{
			byte[] test = {
				1 ,0,0,0,
				1 ,0,0,0,
				0 ,1,0,0,
				0,1,0,0};

			int[] output = new int[test.Length/4];


			Stopwatch ste=  new Stopwatch();
			ste.Start();
			for ( int i = 0 ; i < 50000000 ; i++ )
			{
				var temp = new SimplePointer().convert(output , test);
			}
			Debug.WriteLine( ste.ElapsedMilliseconds );
			ste.Stop();
			ste.Start();

			for ( int i = 0 ; i < 50000000 ; i++ )
			{
				var temp2  = new SimplePointer().byteArr2intArr(test);
			}
			Debug.WriteLine( ste.ElapsedMilliseconds );
			ste.Stop();
		}

		public static void testendian()
		{
			short[] src  = { 258, 259, 260, 261, 262, 263, 264,
						  265, 266, 267, 268, 269, 270 };
			long[] dest = { 17, 18, 19, 20 };

			// Display the initial value of the arrays in memory.
			Console.WriteLine( "Initial values of arrays:" );
			Console.WriteLine( "   Array values as Bytes:" );
			DisplayArray( src , "src" );
			DisplayArray( dest , "dest" );
			Console.WriteLine( "   Array values:" );
			DisplayArrayValues( src , "src" );
			DisplayArrayValues( dest , "dest" );
			Console.WriteLine();

			// Copy bytes 5-10 from source to index 7 in destination and display the result.
			Buffer.BlockCopy( src , 5 , dest , 7 , 6 );
			Console.WriteLine( "Buffer.BlockCopy(src, 5, dest, 7, 6 )" );
			Console.WriteLine( "   Array values as Bytes:" );
			DisplayArray( src , "src" );
			DisplayArray( dest , "dest" );
			Console.WriteLine( "   Array values:" );
			DisplayArrayValues( src , "src" );
			DisplayArrayValues( dest , "dest" );
			Console.WriteLine();

			// Copy bytes 16-20 from source to index 22 in destination and display the result. 
			Buffer.BlockCopy( src , 16 , dest , 22 , 5 );
			Console.WriteLine( "Buffer.BlockCopy(src, 16, dest, 22, 5)" );
			Console.WriteLine( "   Array values as Bytes:" );
			DisplayArray( src , "src" );
			DisplayArray( dest , "dest" );
			Console.WriteLine( "   Array values:" );
			DisplayArrayValues( src , "src" );
			DisplayArrayValues( dest , "dest" );
			Console.WriteLine();

			// Copy overlapping range of bytes 4-10 to index 5 in source.
			Buffer.BlockCopy( src , 4 , src , 5 , 7 );
			Console.WriteLine( "Buffer.BlockCopy( src, 4, src, 5, 7)" );
			Console.WriteLine( "   Array values as Bytes:" );
			DisplayArray( src , "src" );
			DisplayArray( dest , "dest" );
			Console.WriteLine( "   Array values:" );
			DisplayArrayValues( src , "src" );
			DisplayArrayValues( dest , "dest" );
			Console.WriteLine();

			// Copy overlapping range of bytes 16-22 to index 15 in source. 
			Buffer.BlockCopy( src , 16 , src , 15 , 7 );
			Console.WriteLine( "Buffer.BlockCopy( src, 16, src, 15, 7)" );
			Console.WriteLine( "   Array values as Bytes:" );
			DisplayArray( src , "src" );
			DisplayArray( dest , "dest" );
			Console.WriteLine( "   Array values:" );
			DisplayArrayValues( src , "src" );
			DisplayArrayValues( dest , "dest" );



		}

		public static void DisplayArray( Array arr , string name )
		{
			Console.WindowWidth = 120;
			Console.Write( "{0,11}:" , name );
			for ( int ctr = 0 ; ctr < arr.Length ; ctr++ )
			{
				byte[] bytes;
				if ( arr is long [ ] )
					bytes = BitConverter.GetBytes( ( long )arr.GetValue( ctr ) );
				else
					bytes = BitConverter.GetBytes( ( short )arr.GetValue( ctr ) );

				foreach ( byte byteValue in bytes )
					Console.Write( " {0:X2}" , byteValue );
			}
		}

		public static void DisplayArrayValues( Array arr , string name )
		{
			// Get the length of one element in the array.
			int elementLength = Buffer.ByteLength(arr) / arr.Length;
			string formatString = String.Format(" {{0:X{0}}}", 2 * elementLength);
			Console.Write( "{0,11}:" , name );
			for ( int ctr = 0 ; ctr < arr.Length ; ctr++ )
				Console.Write( formatString , arr.GetValue( ctr ) );
			Console.WriteLine();
		}





	}
}
