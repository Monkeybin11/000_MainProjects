using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterfaceCollection
{
    public enum TdiMode { Tdi , Area}
    public enum DirectionMode { Forward , Backward}
    public interface ITDICameraAPI<TModel,TbufType,TtransInfo,TconnectInfoType>
    {
        TModel Connect( TconnectInfoType connect );
        TModel Disconnect();
        TModel Grab();
        TModel Freeze();
        TbufType GetFullBuffer();
        TModel RegistBuffGetEvt();
        TtransInfo GetBufferHW();
        TModel LineRate(double value);
        TModel ExposureMode( double value );
        TModel Direction( DirectionMode direction );
        TModel TDIMode( TdiMode mode);
    }
}
