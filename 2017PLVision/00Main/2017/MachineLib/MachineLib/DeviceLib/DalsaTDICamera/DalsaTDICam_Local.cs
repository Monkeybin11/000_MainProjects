using DALSA.SaperaLT.SapClassBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModelLib.Monad;
using InterfaceCollection;

namespace MachineLib.DeviceLib.DalsaTDICamera
{
    public partial class DalsaTDICam
    {
        public static readonly string ServerName     = "Xcelera-HS_PX8_1";
        public static readonly string ConfigFileNameBase = @"C:\Program Files\Teledyne DALSA\Sapera\CamFiles\User\";
        public static readonly int    ResourceIndex  = 0;

        public string LastMsg;

        SapLocation     ServerLocation      ;
        SapAcqDevice    AcqDevice           ;
        SapAcquisition  Acquisition         ;
        public SapBuffer       Buffers      ;
        public SapAcqToBuf     Xfer         ;

        private DalsaTDICam_SerialCom SerialCom;


        public string ConfigFileName        ;
        public string ConfigFile { get { return Path.Combine( ConfigFileNameBase , ConfigFileName ); } }

        private Maybe<IDalsaTDICam> Initialize()
        {
            try
            {
                ServerLocation = new SapLocation( ServerName , ResourceIndex );
                Acquisition = new SapAcquisition( ServerLocation , ConfigFile );

                if ( SapBuffer.IsBufferTypeSupported( ServerLocation , SapBuffer.MemoryType.ScatterGather ) )
                    Buffers = new SapBufferWithTrash( 2 , Acquisition , SapBuffer.MemoryType.ScatterGather );
                else
                    Buffers = new SapBufferWithTrash( 2 , Acquisition , SapBuffer.MemoryType.ScatterGatherPhysical );

                Acquisition.Create();

                Xfer = new SapAcqToBuf( Acquisition , Buffers );
                Xfer.Pairs [ 0 ].EventType = SapXferPair.XferEventType.EndOfFrame;
                return this.ToMaybe<IDalsaTDICam>();
            }
            catch ( Exception )
            {
                MessageBox.Show( "Camera initialization is fail" );
                return new Nothing<IDalsaTDICam>();
            }
        }

        private Maybe<IDalsaTDICam> ConnectSerialPort(string port)
        {
            try
            {
                // constant connect info
                SerialPort Port = new SerialPort();
                Port.PortName = port;
                Port.BaudRate = 115200;
                Port.DataBits = 8;
                Port.Parity = Parity.None;
                Port.Handshake = Handshake.None;
                Port.StopBits = StopBits.One;
                Port.Encoding = Encoding.UTF8;

                SerialCom = new DalsaTDICam_SerialCom( Port );
                SerialCom.evtReadDone += new Action<string>( x => LastMsg = x );
                return this.ToMaybe<IDalsaTDICam>();
            }
            catch ( Exception )
            {
                MessageBox.Show( "Serial Communication with Camera is fail" );
                return new Nothing<IDalsaTDICam>();
            }
        }
        #region curried func

        
        #endregion

    }
}
