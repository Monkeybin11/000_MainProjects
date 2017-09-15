using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Console;
namespace TaskControl
{
	public static class ThreadCancelation
	{
		public static void main()
		{

		}

		static void AstncOperarion1( CancellationToken token )
		{
			Write("Stating the first task");
			for ( int i = 0 ; i < 5 ; i++ )
			{
				if ( token.IsCancellationRequested )
				{
					WriteLine( "Thre First task has been canceled" );
					return;
				}
				Thread.Sleep( 1000 );
			}
			WriteLine("First task is completed");
		}


		





	}
}
