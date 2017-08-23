using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImg_V2
{
    public class SerialComHelper
    {
        static SerialPort Port = new SerialPort();
        static SerialComHelper instance;
        byte[] Delimiter = new byte[] { 0x0d };
        static object key = new object();


        public static SerialComHelper Instance
        {
            get
            {
                if( instance == null)
                {
                    instance = new SerialComHelper();
                }
                return instance;
            }
        }


        public void SerialInit()
        {
            if ( !Port.IsOpen )
            {
                Port.PortName = "COM7";
                Port.BaudRate = 115200;
                Port.DataBits = 8;
                Port.Parity = Parity.None;
                Port.Handshake = Handshake.None;
                Port.StopBits = StopBits.One;
                Port.Encoding = Encoding.UTF8;
            }
            Port.Open();
        }

        public void Close()
        {
            if(Port.IsOpen)
            { Port.Close(); }
        }

        public void Send( string text )
        {
            var arr = System.Text.Encoding.ASCII.GetBytes(text);
            Port.Write( arr, 0, arr.Length );
            Port.Write( Delimiter, 0, Delimiter.Length );
        }
    }
}
