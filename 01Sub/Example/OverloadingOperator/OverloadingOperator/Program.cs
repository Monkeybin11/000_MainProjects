using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadingOperator
{
	class Program
	{
		static void Main( string [ ] args )
		{
			var empty = new Position_struct();
			var pos = new Position(1,2);
			double[] temp = new double[] { 3,4};

			double [ ] re1 = pos; // implicit
			var pos2 = (Position)temp; // explicit
			var AddOperation = pos + pos2; // operator overload
		}
	}
}
