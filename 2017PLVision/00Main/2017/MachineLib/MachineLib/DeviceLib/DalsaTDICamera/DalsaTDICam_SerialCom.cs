using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUtilTool.Communication;
using ModelLib.Monad;
using SpeedyCoding;

namespace MachineLib.DeviceLib.DalsaTDICamera
{
    // This enum is directly pasrsed to string  
    public enum CommandList { ssf , tdi , scd , sem /*, roi*/}
    public class DalsaTDICam_SerialCom : RS232<CommandList>
    {
        // need to be same order with CommanListt 
        private string[] CommandListExplain = new string[4]
            {
                "Linerate",
                "TdiMode",
                "Direction",
                "ExposureMode"
                //"Roi"
            };

        public DalsaTDICam_SerialCom( SerialPort port ) 
            : base( port  )
        {
        
        }

        public Maybe<DalsaTDICam_SerialCom> SetCamParm( CommandList command, double value )
        {
            try
            {
                Send( command , value );
                return this.ToMaybe();
            }
            catch ( Exception )
            {
                Send( command , value );
                return new Nothing<DalsaTDICam_SerialCom>();
            }
            
        }

    }
}
