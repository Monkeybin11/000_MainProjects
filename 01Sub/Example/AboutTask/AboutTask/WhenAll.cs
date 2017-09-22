using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AboutTask
{
	public class WhenAll
	{
		public void main()
		{
			try
			{

				Task<int>[] list = new Task<int>[2];

				list [ 0 ] = Task.Run<int>( () =>
					{
						Thread.Sleep( 2000 );
						return 1;
					} );


				list [ 1 ] = Task.Run<int>( () =>
					{
						Thread.Sleep( 3000 );
						return 2;
					} );

				var res = Task<int>.WhenAll( list).Result;
				Console.WriteLine( res [ 0 ] );
				Console.WriteLine( res [ 1 ] );


			}
			catch ( Exception ex )
			{
				Console.WriteLine( ex.ToString() );
			}

			Console.ReadLine();


		}

		

	}
}
