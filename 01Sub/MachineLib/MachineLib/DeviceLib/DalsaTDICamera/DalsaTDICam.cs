using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceCollection;
using ModelLib.Monad;
using System.Runtime.InteropServices;

namespace MachineLib.DeviceLib.DalsaTDICamera
{
    public partial class DalsaTDICam : IDalsaTDICam
    {
        public Maybe<IDalsaTDICam> Connect( string connect )
        {
            // Load Config , Create Object
            return Initialize().Bind( x => ConnectSerialPort( connect ) ) as Maybe<IDalsaTDICam>;
        }

        public Maybe<IDalsaTDICam> Disconnect( )
        {
            try
            {
                if ( Xfer != null )
                {
                    Xfer.Destroy();
                    Xfer.Dispose();
                }

                if ( AcqDevice != null )
                {
                    AcqDevice.Destroy();
                    AcqDevice.Dispose();
                }

                if ( Acquisition != null )
                {
                    Acquisition.Destroy();
                    Acquisition.Dispose();
                }

                if ( Buffers != null )
                {
                    Buffers.Destroy();
                    Buffers.Dispose();
                }
                if ( ServerLocation != null ) ServerLocation.Dispose();
                return this.ToMaybe<IDalsaTDICam>();
            }
            catch ( Exception )
            {

                return new Nothing<IDalsaTDICam>();
            }
        }

        public Maybe<IDalsaTDICam> Direction( DirectionMode direction )
        {
            var obj = SerialCom.SetCamParm( CommandList.scd, ( double )( direction == DirectionMode.Forward ? 0:1 ) );
            return obj != null
                ? this.ToMaybe<IDalsaTDICam>()
                : new Nothing<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> ExposureMode( double value )
        {
            var obj = SerialCom.SetCamParm( CommandList.sem , value ) as Just<DalsaTDICam_SerialCom>;
            return obj != null 
                ? this.ToMaybe<IDalsaTDICam>()
                : new Nothing<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> Freeze()
        {
            try
            {
                if ( Xfer.Grabbing ) Xfer.Freeze();
                return this.ToMaybe<IDalsaTDICam>();
            }
            catch ( Exception )
            {
                return new Nothing<IDalsaTDICam>();
            }
        }

        public Maybe<int[]> GetBufferHW()
        {
            try
            {
                return new int [ Buffers.Width * Buffers.Height ].ToMaybe();
            }
            catch ( Exception )
            {
                return new Nothing<int[]>();
            }
        }

        public Maybe<byte[]> GetFullBuffer()
        {
            try
            {
                byte[] output = new byte[Buffers.Width*Buffers.Height];
                GCHandle outputAddr = GCHandle.Alloc( output, GCHandleType.Pinned); // output 의 주소 만듬
                IntPtr pointer = outputAddr.AddrOfPinnedObject(); // 
                Buffers.ReadRect( 0 , 0 , Buffers.Width , Buffers.Height , pointer );
                Marshal.Copy( pointer , output , 0 , output.Length );
                outputAddr.Free();
                return output.ToMaybe();
            }
            catch ( Exception )
            {
                return new Nothing<byte[]>();
            }
        }

        public Maybe<IDalsaTDICam> Grab()
        {
            try
            {
                if ( !Xfer.Grabbing ) Xfer.Grab();
                return this.ToMaybe<IDalsaTDICam>();
            }
            catch ( Exception )
            {
                return new Nothing<IDalsaTDICam>();
            }
        }
 
        public Maybe<IDalsaTDICam> LineRate( double value )
        {
            var obj = SerialCom.SetCamParm( CommandList.ssf , value ) as Just<DalsaTDICam_SerialCom>;
           return obj != null 
                ? this.ToMaybe<IDalsaTDICam>() 
                : new Nothing<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> RegistBuffGetEvt()
        {
            // sjw 모륵겠다. 
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> TDIMode( TdiMode mode )
        {
            var obj = SerialCom.SetCamParm( CommandList.tdi , mode ==  TdiMode.Tdi ? 1 : 0) as Just<DalsaTDICam_SerialCom>;
            return obj != null
                ? this.ToMaybe<IDalsaTDICam>()
                : new Nothing<IDalsaTDICam>();
        }
    }
}
