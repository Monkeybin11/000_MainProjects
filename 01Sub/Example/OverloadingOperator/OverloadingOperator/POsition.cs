using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadingOperator
{
	public class Position
	{
		public int X;
		public int Y;
		public double Value;

		public Position(int x , int y)
		{
			X = x;
			Y = y;
		}

		//Explicit double[] => Position
		public static explicit operator Position( double [ ] d )
		{
			return new Position( (int)d [ 0 ] , (int)d [ 1 ] );
		}


		// Implicit Position => doublep[]
		public static implicit operator double[] ( Position pos )
		{
			return new double [ ] { pos.X , pos.Y };
		}


		public static Position operator +( Position pos1 , Position pos2 )
		{
			var xres = pos1.X + pos2.X;  
			var yres = pos1.Y + pos2.Y;
			return new Position( xres , yres );
		}


	}

	public struct Position_struct
	{
		public int X;
		public int Y;
		public double Value;

		public Position_struct( int x , int y )
		{
			X = x;
			Y = y;
		}

		//Explicit double[] => Position
		public static explicit operator Position_struct( double [ ] d )
		{
			return new Position_struct( ( int )d [ 0 ] , ( int )d [ 1 ] );
		}


		// Implicit Position => doublep[]
		public static implicit operator double [ ] ( Position_struct pos )
		{
			return new double [ ] { pos.X , pos.Y };
		}


		public static Position operator +( Position_struct pos1 , Position_struct pos2 )
		{
			var xres = pos1.X + pos2.X;
			var yres = pos1.Y + pos2.Y;
			return new Position_struct( xres , yres );
		}


	}

}
