using System;

namespace BackendlessAPI.Geo.Location
{
  interface IBackendlessLocationListener
  {
    void OnLocationChanged( double latitude, double longitude, double accuracy );
  }
}
