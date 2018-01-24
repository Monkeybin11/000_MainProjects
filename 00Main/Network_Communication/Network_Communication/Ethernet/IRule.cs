using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network_Communication.Ethernet
{
    public interface IRule
    {
        bool CheckMsg( string msg );
        Action<string> DoAct { get; set; } 
    }
}




