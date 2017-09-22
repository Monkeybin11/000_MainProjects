using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImg_V2
{
    public class DalsaTDISerial : SerialComHelper
    {
        string TDI = "tdi ";
        string Linerate = "ssf ";

        public Action SetArea => () => Send( TDI + "0" );
        public Action SetTDI => () => Send( TDI + "1" );
        public Action<int> SetLineRate => x => Send( Linerate + x.ToString() );
    }
}
