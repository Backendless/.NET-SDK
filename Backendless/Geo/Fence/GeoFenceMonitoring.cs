using System;
using System.Collections.Generic;
using System.Threading;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo.Location;

#if !NET20
namespace BackendlessAPI.Geo.Fence
{

  class GeoFenceMonitoring : IBackendlessLocationListener
  {
    public static String NAME = "GeoFenceMonitoring";
    private HashSet<GeoFence> onStaySet = new HashSet<GeoFence>();
    private Dictionary<GeoFence, ICallback> fencesToCallback = new Dictionary<GeoFence, ICallback>();
    private HashSet<GeoFence> pointFences = new HashSet<GeoFence>();
    private static readonly GeoFenceMonitoring instance = new GeoFenceMonitoring();
    private static HashSet<DurationStayTask> tasks = new HashSet<DurationStayTask>();
 
    private volatile GeoPoint location;

    private GeoFenceMonitoring()
    {
    }

    public static GeoFenceMonitoring Instance
    {
      get { return instance; }
    }

    public void OnLocationChanged( double latitude, double longitude, double accuracy )
  {
    HashSet<GeoFence> oldFences, newFences, currFence;

    lock( this )
    {
      this.location = new GeoPoint( latitude, longitude ); ;
      oldFences = pointFences;
      currFence = FindGeoPointsFence( location );
      newFences = new HashSet<GeoFence>( currFence );

      newFences.ExceptWith( oldFences );
      oldFences.ExceptWith( currFence );

      CallOnEnter( newFences );
      CallOnStay( newFences );
      CallOnExit( oldFences );
      CancelOnStay( oldFences );

      pointFences = currFence;
    }
  }

    private void CallOnEnter( HashSet<GeoFence> geoFences )
    {
      foreach( GeoFence geoFence in geoFences )
        fencesToCallback[ geoFence ].CallOnEnter( geoFence, location );
    }

    private void CallOnStay( HashSet<GeoFence> geoFences )
    {
      foreach( GeoFence geoFence in geoFences )
        if( geoFence.OnStayDuration > 0 )
          AddOnStay( geoFence );
    }

    private void CancelOnStay( HashSet<GeoFence> geoFences )
    {
      foreach( GeoFence geoFence in geoFences )
        CancelOnStay( geoFence );
    }

    private void CallOnExit( HashSet<GeoFence> geoFences )
    {
      foreach( GeoFence geoFence in geoFences )
      {
        fencesToCallback[ geoFence ].CallOnExit( geoFence, location );
      }
    }

    public void AddGeoFences( HashSet<GeoFence> geoFences, ICallback callback )
    {
      if( fencesToCallback.Count != 0 )
        throw new BackendlessException( ExceptionMessage.GEOFENCES_MONITORING );

      foreach( GeoFence geoFence in geoFences )
        AddGeoFence( geoFence, callback );
    }

    public void AddGeoFence( GeoFence geoFence, ICallback callback )
    {
      if( fencesToCallback.ContainsKey( geoFence ) )
      {
        if( !fencesToCallback[ geoFence ].EqualCallbackParameter( callback ) )
          throw new BackendlessException( String.Format( ExceptionMessage.GEOFENCE_ALREADY_MONITORING, geoFence.GeofenceName ) );

        return;
      }

      if( !IsDefiniteRect( geoFence.NWPoint, geoFence.SEPoint ) )
        DefiniteRect( geoFence );

      this.fencesToCallback.Add( geoFence, callback );

      if( location != null && IsPointInFence( new GeoPoint( location.Latitude, location.Longitude ), geoFence ) )
      {
        pointFences.Add( geoFence );
        callback.CallOnEnter( geoFence, location );
        AddOnStay( geoFence );
      }
    }

    public void removeGeoFence( String geoFenceName )
    {
      GeoFence removed = new GeoFence( geoFenceName );

      if( fencesToCallback.ContainsKey( removed ) )
      {
        fencesToCallback.Remove( removed );
        CancelOnStay( removed );
        pointFences.Remove( removed );
      }
    }

    public void RemoveGeoFences()
    {
      onStaySet.Clear();
      pointFences.Clear();
      fencesToCallback.Clear();
    }

    public bool IsMonitoring()
    {
      return fencesToCallback.Count != 0;
    }

    private HashSet<GeoFence> FindGeoPointsFence( GeoPoint geoPoint )
    {
      HashSet<GeoFence> pointFences = new HashSet<GeoFence>();

      foreach( GeoFence geoFence in fencesToCallback.Keys )
        if( IsPointInFence( geoPoint, geoFence ) )
          pointFences.Add( geoFence );

      return pointFences;
    }

    private bool IsPointInFence( GeoPoint geoPoint, GeoFence geoFence )
    {
      if( !GeoMath.IsPointInRectangular( geoPoint, geoFence.NWPoint, geoFence.SEPoint ) )
      {
        return false;
      }

      if( geoFence.Type == FenceType.CIRCLE && !GeoMath.IsPointInCircle( geoPoint, geoFence.Nodes[ 0 ], GeoMath.Distance( geoFence.Nodes[ 0 ].Latitude, geoFence.Nodes[ 0 ].Longitude, geoFence.Nodes[ 1 ].Latitude, geoFence.Nodes[ 1 ].Longitude ) ) )
      {
        return false;
      }

      if( geoFence.Type == FenceType.SHAPE && !GeoMath.IsPointInShape( geoPoint, geoFence.Nodes ) )
      {
        return false;
      }

      return true;
    }

    private bool IsDefiniteRect( GeoPoint nwPoint, GeoPoint sePoint )
    {
      return nwPoint != null && sePoint != null;
    }

    private void DefiniteRect( GeoFence geoFence )
    {
      switch( geoFence.Type )
      {
        case FenceType.RECT:
          {
            GeoPoint nwPoint = geoFence.Nodes[ 0 ];
            GeoPoint sePoint = geoFence.Nodes[ 1 ];
            geoFence.NWPoint = nwPoint;
            geoFence.SEPoint = sePoint;
            break;
          }
        case FenceType.CIRCLE:
          {
            double[] outRect = GeoMath.GetOutRectangle( geoFence.Nodes[ 0 ], geoFence.Nodes[ 0 ] );
            geoFence.NWPoint = new GeoPoint( outRect[ 0 ], outRect[ 1 ] );
            geoFence.SEPoint = new GeoPoint( outRect[ 2 ], outRect[ 3 ] );
            break;
          }
        case FenceType.SHAPE:
          {
            double[] outRect = GeoMath.GetOutRectangle( geoFence.Nodes );
            geoFence.NWPoint = new GeoPoint( outRect[ 0 ], outRect[ 1 ] );
            geoFence.SEPoint = new GeoPoint( outRect[ 2 ], outRect[ 3 ] );
            break;
          }
        default:
          break;
      }
    }

    private void AddOnStay( GeoFence geoFence )
    {
      onStaySet.Add( geoFence );
      DurationStayTask task = new DurationStayTask();
      task.geoFence = geoFence;
      task.Schedule();
      tasks.Add( task );
    }

    private void CancelOnStay( GeoFence geoFence )
    {
      onStaySet.Remove( geoFence );
    }

    internal void ExecuteOnStayCallback( DurationStayTask task, GeoFence geoFence )
    {
      tasks.Remove( task );

      if( onStaySet.Contains( geoFence ) )
      {
          fencesToCallback[ geoFence ].CallOnStay( geoFence, location );
          CancelOnStay( geoFence );
      }
    }


    internal class DurationStayTask
    {
      internal Timer timer;
      internal GeoFence geoFence;

      internal void Schedule()
      {
        TimerCallback callback = this.CheckOnStayAfterDuration;
        timer = new Timer( callback, geoFence, geoFence.OnStayDuration * 1000, Timeout.Infinite );
      }

      public void CheckOnStayAfterDuration( Object stateInfo )
      {
        GeoFence geoFence = (GeoFence) stateInfo;
        timer.Dispose();
        GeoFenceMonitoring.Instance.ExecuteOnStayCallback( this, geoFence );
      }
    }
  }
}
#endif
