using System;

namespace BackendlessAPI.Geo.Fence
{
  interface ICallback
  {
    void CallOnEnter( GeoFence geoFence, GeoPoint location );
    void CallOnStay( GeoFence geoFence, GeoPoint location );
    void CallOnExit( GeoFence geoFence, GeoPoint location );
    bool EqualCallbackParameter( Object obj );
  }
}
