using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceCollection;
using ModelLib.Monad;
using SpeedyCoding;
using static SpeedyCoding.SpeedyCoding_Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;


namespace MachineLib.DeviceLib.DalsaTDICamera
{
    public class DalsaTDICam_Dummy : IDalsaTDICam
    {
        public string ConfigFile
        {
            get { return "Loaded"; }
        }

        public Maybe<IDalsaTDICam> Connect( string connect )
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> Direction( DirectionMode direction )
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> Disconnect()
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> ExposureMode( double value )
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> Freeze()
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<int [ ]> GetBufferHW()
        {
            GetCurrentMethod().Print();
            return new int [ ] { 1600 , 1200 }.ToMaybe();
        }

        public Maybe<byte [ ]> GetFullBuffer()
        {
            GetCurrentMethod().Print();
            return new byte [ ] { 255 , 1 }.ToMaybe();
        }

        public Maybe<IDalsaTDICam> Grab()
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> LineRate( double value )
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> RegistBuffGetEvt()
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> TDIMode( TdiMode mode )
        {
            GetCurrentMethod().Print();
            return this.ToMaybe<IDalsaTDICam>();
        }
    }
}
