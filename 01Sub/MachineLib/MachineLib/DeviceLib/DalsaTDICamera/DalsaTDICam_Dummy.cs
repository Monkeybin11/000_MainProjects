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
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> Direction( DirectionMode direction )
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> Disconnect()
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> ExposureMode( double value )
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> Freeze()
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<int [ ]> GetBufferHW()
        {
            CallerName(1).Print();
            return new int [ ] { 1600 , 1200 }.ToMaybe();
        }

        public Maybe<byte [ ]> GetFullBuffer()
        {
            CallerName(1).Print();
            return new byte [ ] { 255 , 1 }.ToMaybe();
        }

        public Maybe<IDalsaTDICam> Grab()
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> LineRate( double value )
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> RegistBuffGetEvt()
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }

        public Maybe<IDalsaTDICam> TDIMode( TdiMode mode )
        {
            CallerName(1).Print();
            return this.ToMaybe<IDalsaTDICam>();
        }
    }
}
