using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMxLib.susceptorexport
{
    [Serializable]
    public enum SusceptroExportCommands
    {
        Start,
        LoadSusceptor,
        LoadWafer,
        SetItem,
        SetFixedRange,
        SetAutoRange,
        SetAverageOffsetRange,
        Size,
        Save,
        Close,

        None,
        Error,
    }

    public class SusceptorExportPacket
    {
        public SusceptroExportCommands Commnad { get; set; }
        public string[] Args { get; set; }

        public SusceptorExportPacket()
        {
            Commnad = SusceptroExportCommands.None;
        }

        public bool Parse(string packet)
        {
            var contents = packet.Split('|');
            if (contents.Length > 0)
            {
                SusceptroExportCommands parseCmd;
                if (Enum.TryParse<SusceptroExportCommands>(contents[0], out parseCmd))
                {
                    Console.WriteLine(parseCmd.ToString());
                    Commnad = parseCmd;
                    if (contents.Length > 1)
                    {
                        Args = contents.Skip(1).ToArray();
                    }
                    return true;
                }
            }
            return false;
        }

        public byte[] ToBytes()
        {
            var result = this.ToString() + "\r\n";
            return Encoding.UTF8.GetBytes(result);
        }

        public override string ToString()
        {
            string result = Commnad.ToString();
            foreach (var item in Args)
            {
                result += "|" + item;
            }
            return result;
        }
    }
}
