using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.Log
{
	public interface Logger
	{
		void Log( string msg , bool addTime = false );
	}
}
