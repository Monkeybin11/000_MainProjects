using InterfaceCollection;
using ModelLib.Monad;

namespace MachineLib.DeviceLib.DalsaTDICamera
{
    public interface IDalsaTDICam : ITDICameraAPI<Maybe<IDalsaTDICam> , Maybe<byte [ ]> , Maybe<int [ ]> , string>
    {
        string ConfigFile { get; }

        Maybe<IDalsaTDICam> Connect( string connect );
        Maybe<IDalsaTDICam> Direction( DirectionMode direction );
        Maybe<IDalsaTDICam> Disconnect();
        Maybe<IDalsaTDICam> ExposureMode( double value );
        Maybe<IDalsaTDICam> Freeze();
        Maybe<int [ ]> GetBufferHW();
        Maybe<byte [ ]> GetFullBuffer();
        Maybe<IDalsaTDICam> Grab();
        Maybe<IDalsaTDICam> LineRate( double value );
        Maybe<IDalsaTDICam> RegistBuffGetEvt();
        Maybe<IDalsaTDICam> TDIMode( TdiMode mode );
    }
}