using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUG_Protocol
{
    public class CONSTANTS
    {
        public const uint REQ_FILE_SEND  = 0x01;
        public const uint REP_FILE_SEND  = 0x02;
        public const uint FILE_SEND_DATA = 0x03;
        public const uint FILE_SEND_RES  = 0x04;

        public const byte NOT_FRAGMENT  = 0x00;
        public const byte FRAGMENTED    = 0x01;

        public const byte NOT_LASTMSG   = 0x00;
        public const byte LASTMSG       = 0x01;

        public const byte ACCEPTED      = 0x00;
        public const byte DENIED        = 0x01;

        public const byte FAIL          = 0x00;
        public const byte SUCCESS       = 0x01;
    }

    public interface ISerializable
    {
        byte[] GetByte();
        int GetSize();
    }

    public class Message : ISerializable
    {
        public Header Header { get; set; }
        public ISerializable Body { get; set; }


        public byte[] GetByte()
        {
            byte[] bytes = new byte[GetSize()];

            Header.GetByte().CopyTo( bytes, 0 );
            Body.GetByte().CopyTo( bytes, Header.GetSize() );
            return bytes;
        }

        public int GetSize()
        {
            return Header.GetSize() + Body.GetSize();
        }
    }
}
