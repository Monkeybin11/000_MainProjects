using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using System.Threading;


namespace ApplicationUtilTool.Communication
{
    public class RS232<TcommandList> where TcommandList : struct, IComparable, IConvertible , IFormattable
    {
        public SerialPort Port;
        public event Action<string> evtReadDone; 

        public RS232( SerialPort port )
        {
            Port = port;
            Port.DataReceived += new SerialDataReceivedEventHandler( ReadDone );
        }

        public bool? Open()
        {
            return Port.IsOpen ? Port.Act( x => x.Close() )
                                     .Map( x => { x.Open(); return true as bool?; } ) 
                               : null;
        }

        private void ReadDone(object sender , SerialDataReceivedEventArgs e)
        {
            try
            {
                evtReadDone( ( sender as SerialPort ).ReadExisting() );
            }
            catch ( Exception )
            {
                evtReadDone(null);
                throw;
            }
        }

        public void Send(string text)
        {
            byte[] Delimiter = new byte[] { 0x0d };
            var arr = Encoding.ASCII.GetBytes(text.Trim());
            Port.Write( arr , 0 , arr.Length );
            Port.Write( Delimiter , 0 , Delimiter.Length );
        }

        public void Send( string text , double value)
        {
            text = text + " " + value.ToString();
            byte[] Delimiter = new byte[] { 0x0d };
            var arr = Encoding.ASCII.GetBytes(text.Trim());
            Port.Write( arr , 0 , arr.Length );
            Port.Write( Delimiter , 0 , Delimiter.Length );
        }

        public void Send( TcommandList command , double value )
        {
            var text = command.ToString() + " " + value.ToString();
            byte[] Delimiter = new byte[] { 0x0d };
            var arr = Encoding.ASCII.GetBytes(text.Trim());
            Port.Write( arr , 0 , arr.Length );
            Port.Write( Delimiter , 0 , Delimiter.Length );
        }

    }
}
