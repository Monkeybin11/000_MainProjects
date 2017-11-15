using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMultiEvent
{
	public class EventClass
	{
		public event EventHandler<BtnEventArg> evtCreate;

		public void RegistEvent()
		{
			

		}


	}

	public class BtnEventArg : EventArgs
	{
		public int Index { get; set; }

		public BtnEventArg( int idx)
		{
			Index = idx;
		}


	}

}
