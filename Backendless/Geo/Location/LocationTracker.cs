using System;
using System.Collections.Generic;
using System.Threading;

namespace BackendlessAPI.Geo.Location
{
  class LocationTracker
  {
    private static readonly LocationTracker instance = new LocationTracker();
    private Dictionary<String, IBackendlessLocationListener> listeners = new Dictionary<string, IBackendlessLocationListener>();

     #if UNIVERSALW8
    private ILocationTrackerEngine locationTrackerEngine = new Win8LocationTrackerEngine();
#else
    private ILocationTrackerEngine locationTrackerEngine = new NoOpLocationTrackingEngine();
#endif


    private LocationTracker()
    {
      UpdateInterval = 5;
    }

    internal static LocationTracker Instance
    {
      get { return instance; }
    }

    uint UpdateInterval { get; set; }


    public bool ContainsListener( String name )
    {
      return listeners.ContainsKey( name );
    }

    internal void AddListener( String name, IBackendlessLocationListener listener )
    {
      if( ContainsListener( name ) )
        throw new System.Exception( String.Format( "Cannot add location listener. Listener with the name {0} already exists", name ) );

      listeners.Add( name, listener );

      if( listeners.Count == 1 )
        StartLocationTracker();
    }

    internal void RemoveListener( String name )
    {
      listeners.Remove( name );

      if( listeners.Count == 0 )
        StopLocationTracker();
    }

    internal void LocationChanged( double latitude, double longitude, double accuracy )
    {
      foreach( IBackendlessLocationListener listener in listeners.Values )
        listener.OnLocationChanged( latitude, longitude, accuracy );
    }

    private void StopLocationTracker()
    {
      locationTrackerEngine.StopLocationTracker();
    }

    private void StartLocationTracker()
    {
      locationTrackerEngine.StartLocationTracker();
    }
  }
}
