using System;
using System.Collections.Generic;
using System.Text;

namespace BackendlessAPI.Geo.Location
{
  interface ILocationTrackerEngine
  {
    void StopLocationTracker();
    void StartLocationTracker();
  }
}
