using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx_Example.P1
{
	class CustomEventExample
	{
		public void main()
		{
			var temp = new eventOwner();

			temp.myevent += new EventHandler<TextArgs>( afterexe );
		}

		public static void afterexe( object sender , TextArgs t )
		{
			Console.WriteLine( t.Message );
		}


	}

	public class TextArgs : EventArgs
	{
		private string szMessage;

		public TextArgs( string TextMessage )
		{
			szMessage = TextMessage;
		}

		public string Message
		{
			get { return szMessage; }
			set { szMessage = value; }
		}

	}

	public class eventOwner
	{
		public event EventHandler<TextArgs> myevent;
	}


}
