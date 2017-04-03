using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using BackendlessAPI.Geo.Fence;
using BackendlessAPI.Geo.Location;
using Weborb.Client;
using Weborb.Types;

namespace BackendlessAPI.Service
{
  public class GeoService
  {
    internal static string GEO_MANAGER_SERVER_ALIAS = "com.backendless.services.geo.GeoService";
    private static string DEFAULT_CATEGORY_NAME = "Default";

    public GeoService()
    {
      Types.AddClientClassMapping( "com.backendless.geo.model.SearchMatchesResult", typeof( SearchMatchesResult ) );
      Types.AddClientClassMapping( "com.backendless.geo.model.GeoPoint", typeof( GeoPoint ) );
      Types.AddClientClassMapping( "com.backendless.geo.model.GeoCluster", typeof( GeoCluster ) );
      Types.AddClientClassMapping( "com.backendless.geo.model.GeoCategory", typeof( GeoCategory ) );
      Types.AddClientClassMapping( "com.backendless.geo.BackendlessGeoQuery", typeof( BackendlessGeoQuery ) );
      Types.AddClientClassMapping( "com.backendless.geo.Units", typeof( Units ) );
    }

    #region CATEGORIES
    public GeoCategory AddCategory( string categoryName )
    {
      CheckCategoryName( categoryName );

      return Invoker.InvokeSync<GeoCategory>( GEO_MANAGER_SERVER_ALIAS, "addCategory",
                                              new object[] { categoryName } );
    }

    public void AddCategory( string categoryName, AsyncCallback<GeoCategory> callback )
    {
      try
      {
        CheckCategoryName( categoryName );

        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "addCategory",
                             new Object[] { categoryName }, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }

    public bool DeleteCategory( string categoryName )
    {
      CheckCategoryName( categoryName );

      return Invoker.InvokeSync<bool>( GEO_MANAGER_SERVER_ALIAS, "deleteCategory",
                                       new object[] { categoryName } );
    }

    public void DeleteCategory( string categoryName, AsyncCallback<bool> callback )
    {
      try
      {
        CheckCategoryName( categoryName );

        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "deleteCategory",
                             new Object[] { categoryName }, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }

    public List<GeoCategory> GetCategories()
    {
      return Invoker.InvokeSync<List<GeoCategory>>( GEO_MANAGER_SERVER_ALIAS, "getCategories",
                                                    new object[] { } );
    }

    public void GetCategories( AsyncCallback<List<GeoCategory>> callback )
    {
      try
      {
        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "getCategories",
                             new object[] {}, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }
    #endregion
    #region SAVE POINT
    public GeoPoint SavePoint( double latitude, double longitude, Dictionary<string, string> metadata )
    {
      return SavePoint( latitude, longitude, null, metadata );
    }

    public void SavePoint( double latitude, double longitude, Dictionary<string, string> metadata, AsyncCallback<GeoPoint> callback )
    {
      SavePoint( latitude, longitude, null, metadata, callback );
    }

    public GeoPoint SavePoint( double latitude, double longitude, Dictionary<string, object> metadata )
    {
      return SavePoint( latitude, longitude, null, metadata );
    }

    public void SavePoint( double latitude, double longitude, Dictionary<string, object> metadata, AsyncCallback<GeoPoint> callback )
    {
      SavePoint( latitude, longitude, null, metadata, callback );
    }

    public GeoPoint SavePoint( double latitude, double longitude, List<string> categoryNames, Dictionary<string, string> metadata )
    {
      return SavePoint( new GeoPoint( latitude, longitude, categoryNames, metadata ) );
    }

    public GeoPoint SavePoint( double latitude, double longitude, List<string> categoryNames, Dictionary<string, object> metadata )
    {
      return SavePoint( new GeoPoint( latitude, longitude, categoryNames, metadata ) );
    }

    public void SavePoint( double latitude, double longitude, List<string> categoryNames, Dictionary<string, object> metadata, AsyncCallback<GeoPoint> callback )
    {
      SavePoint( new GeoPoint( latitude, longitude, categoryNames, metadata ), callback );
    }

    public void SavePoint( double latitude, double longitude, List<string> categoryNames, Dictionary<string, string> metadata, AsyncCallback<GeoPoint> callback )
    {
      SavePoint( new GeoPoint( latitude, longitude, categoryNames, metadata ), callback );
    }
    #endregion
    #region ADD and SAVE POINT
    public GeoPoint AddPoint( GeoPoint geoPoint )
    {
      return SavePoint( geoPoint );
    }

    public GeoPoint SavePoint( GeoPoint geoPoint )
    {
      if( geoPoint == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_GEOPOINT );

      CheckCoordinates( geoPoint.Latitude, geoPoint.Longitude );

      String methodName = geoPoint.ObjectId == null ? "addPoint" : "updatePoint";

      return Invoker.InvokeSync<GeoPoint>( GEO_MANAGER_SERVER_ALIAS, methodName,
                                           new object[] { geoPoint } );
    }

    public void AddPoint( GeoPoint geoPoint, AsyncCallback<GeoPoint> callback )
    {
      SavePoint( geoPoint, callback );
    }

    public void SavePoint( GeoPoint geoPoint, AsyncCallback<GeoPoint> callback )
    {
      try
      {
        if( geoPoint == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_GEOPOINT );

        CheckCoordinates( geoPoint.Latitude, geoPoint.Longitude );
        String methodName = geoPoint.ObjectId == null ? "addPoint" : "updatePoint";
        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, methodName,
                             new object[] { geoPoint }, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }
    #endregion
    #region REMOVE POINT
    public void RemovePoint( GeoPoint geoPoint )
    {
      if( geoPoint == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_GEOPOINT );

      CheckCoordinates( geoPoint.Latitude, geoPoint.Longitude );
      Invoker.InvokeSync<GeoPoint>( GEO_MANAGER_SERVER_ALIAS, "removePoint",
                                           new object[] { geoPoint.ObjectId } );
    }

    public void RemovePoint( GeoPoint geoPoint, AsyncCallback<GeoPoint> callback )
    {
      try
      {
        if( geoPoint == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_GEOPOINT );

        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "removePoint",
                             new object[] { geoPoint.ObjectId }, callback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }
    #endregion
    #region GET POINT COUNT
    public int GetGeopointCount( BackendlessGeoQuery query )
    {
      Object[] args = new Object[] { query };
      return Invoker.InvokeSync<int>( GEO_MANAGER_SERVER_ALIAS, "count", args );
    }

    public int GetGeopointCount( String geoFenceName, BackendlessGeoQuery query )
    {
      Object[] args = new Object[] { geoFenceName, query };
      return Invoker.InvokeSync<int>( GEO_MANAGER_SERVER_ALIAS, "count", args );
    }

    public void GetGeopointCount( BackendlessGeoQuery query, AsyncCallback<int> responder )
    {
        Object[] args = new Object[] { query };
        Invoker.InvokeAsync<int>( GEO_MANAGER_SERVER_ALIAS, "count", args, responder );
    }

    public void GetGeopointCount( String geoFenceName, BackendlessGeoQuery query, AsyncCallback<int> responder )
    {
        Object[] args = new Object[] { geoFenceName, query };
        Invoker.InvokeAsync<int>( GEO_MANAGER_SERVER_ALIAS, "count", args, responder );
    }
    #endregion
    #region GET POINTS WITH QUERY
    public IList<GeoPoint> GetPoints( BackendlessGeoQuery geoQuery )
    {
      checkGeoQuery( geoQuery );
      Object[] methodArgs = new Object[] { geoQuery };
      IList<GeoPoint> result = Invoker.InvokeSync<IList<GeoPoint>>( GEO_MANAGER_SERVER_ALIAS, "getPoints", methodArgs );

      foreach( GeoPoint geoPoint in result )
        if( geoPoint is GeoCluster )
          ( (GeoCluster) geoPoint ).GeoQuery = geoQuery;

      return result;
    }

    public void GetPoints( BackendlessGeoQuery geoQuery, AsyncCallback<IList<GeoPoint>> callback )
    {
      try
      {
        checkGeoQuery( geoQuery );

        var responder = new AsyncCallback<IList<GeoPoint>>( r =>
          {
            foreach( GeoPoint geoPoint in r )
              if( geoPoint is GeoCluster )
                ( (GeoCluster) geoPoint ).GeoQuery = geoQuery;

            if( callback != null )
              callback.ResponseHandler.Invoke( r );
          }, f =>
            {
              if( callback != null )
                callback.ErrorHandler.Invoke( f );
              else
                throw new BackendlessException( f );
            } );

        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "getPoints",
                             new Object[] { geoQuery }, responder );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }
    #endregion
    #region GET POINTS - CLUSTER
    public IList<GeoPoint> GetPoints( GeoCluster geoCluster )
    {
      Object[] args = new Object[] { geoCluster.ObjectId, geoCluster.GeoQuery };
      return Invoker.InvokeSync<IList<GeoPoint>>( GEO_MANAGER_SERVER_ALIAS, "loadGeoPoints", args );
    }

    public void GetPoints( GeoCluster geoCluster, AsyncCallback<IList<GeoPoint>> callback )
    {
      try
      {
        var responder = new AsyncCallback<IList<GeoPoint>>( r =>
        {
          foreach( GeoPoint geoPoint in r )
            if( geoPoint is GeoCluster )
              ( (GeoCluster) geoPoint ).GeoQuery = geoCluster.GeoQuery;

          if( callback != null )
            callback.ResponseHandler.Invoke( r );
        }, f =>
        {
          if( callback != null )
            callback.ErrorHandler.Invoke( f );
          else
            throw new BackendlessException( f );
        } );

        Object[] args = new Object[] { geoCluster.ObjectId, geoCluster.GeoQuery };
        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "loadGeoPoints", args, responder );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }
    #endregion
    #region GET POINTS - GEOFENCE
    public IList<GeoPoint> GetPoints( String geofenceName )
    {
      return GetPoints( geofenceName, new BackendlessGeoQuery() );
    }

    public IList<GeoPoint> GetPoints( String geofenceName, BackendlessGeoQuery query )
    {
      checkGeoQuery( query );
      Object[] args = new Object[] { geofenceName, query };
      return Invoker.InvokeSync<IList<GeoPoint>>( GEO_MANAGER_SERVER_ALIAS, "getPoints", args );
    }

    public void GetPoints( String geofenceName, AsyncCallback<IList<GeoPoint>> responder )
    {
      GetPoints( geofenceName, new BackendlessGeoQuery(), responder );
    }

    public void GetPoints( String geofenceName, BackendlessGeoQuery query, AsyncCallback<IList<GeoPoint>> callback )
    {
      checkGeoQuery( query );
      Object[] args = new Object[] { geofenceName, query };
      var responder = new AsyncCallback<IList<GeoPoint>>( r =>
          {
            if( callback != null )
              callback.ResponseHandler.Invoke( r );
          }, f =>
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( f );
            else
              throw new BackendlessException( f );
          } );
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "getPoints", args, responder );
    }

    #endregion
    #region RUN ONENTER - GEOFENCE
    public int RunOnEnterAction( String geoFenceName )
    {
      Object[] args = new Object[] { geoFenceName };
      return Invoker.InvokeSync<int>( GEO_MANAGER_SERVER_ALIAS, "runOnEnterAction", args );
    }

    public void RunOnEnterAction( String geoFenceName, AsyncCallback<int> callback )
    {
      Object[] args = new Object[] { geoFenceName };
      var responder = new AsyncCallback<int>( r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "runOnEnterAction", args, responder );
    }

    public void RunOnEnterAction( String geoFenceName, GeoPoint geoPoint )
    {
      Object[] args = new Object[] { geoFenceName, geoPoint };
      Invoker.InvokeSync<Object>( GEO_MANAGER_SERVER_ALIAS, "runOnEnterAction", args );
    }

    public void RunOnEnterAction( String geoFenceName, GeoPoint geoPoint, AsyncCallback<Object> callback )
    {
      Object[] args = new Object[] { geoFenceName, geoPoint };
      var responder = new AsyncCallback<int>( r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "runOnEnterAction", args, responder );
    }
    #endregion
    #region RUN ONSTAY - GEOFENCE
    public int RunOnStayAction( String geoFenceName )
    {
      Object[] args = new Object[] { geoFenceName };
      return Invoker.InvokeSync<int>( GEO_MANAGER_SERVER_ALIAS, "runOnStayAction", args );
    }

    public void RunOnStayAction( String geoFenceName, AsyncCallback<int> callback )
    {
      Object[] args = new Object[] { geoFenceName };
      var responder = new AsyncCallback<int>( r =>
          {
            if( callback != null )
              callback.ResponseHandler.Invoke( r );
          }, f =>
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( f );
            else
              throw new BackendlessException( f );
          } );
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "runOnStayAction", args, responder );
    }

    public int RunOnStayAction( String geoFenceName, GeoPoint geoPoint )
    {
      Object[] args = new Object[] { geoFenceName, geoPoint };
      return Invoker.InvokeSync<int>( GEO_MANAGER_SERVER_ALIAS, "runOnStayAction", args );
    }

    public void RunOnStayAction( String geoFenceName, GeoPoint geoPoint, AsyncCallback<Object> callback )
    {
      Object[] args = new Object[] { geoFenceName, geoPoint };
      var responder = new AsyncCallback<int>( r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "runOnStayAction", args, responder );
    }
    #endregion
    #region RUN ONEXIT - GEOFENCE
    public int RunOnExitAction( String geoFenceName )
    {
      Object[] args = new Object[] { geoFenceName };
      return Invoker.InvokeSync<int>( GEO_MANAGER_SERVER_ALIAS, "runOnExitAction", args );
    }

    public void RunOnExitAction( String geoFenceName, AsyncCallback<int> callback )
    {
      Object[] args = new Object[] { geoFenceName };
      var responder = new AsyncCallback<int>( r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "runOnExitAction", args, responder );
    }

    public void RunOnExitAction( String geoFenceName, GeoPoint geoPoint )
    {
      Object[] args = new Object[] { geoFenceName, geoPoint };
      Invoker.InvokeSync<Object>( GEO_MANAGER_SERVER_ALIAS, "runOnExitAction", args );
    }

    public void RunOnExitAction( String geoFenceName, GeoPoint geoPoint, AsyncCallback<Object> callback )
    {
      Object[] args = new Object[] { geoFenceName, geoPoint };
      var responder = new AsyncCallback<int>( r =>
      {
        if( callback != null )
          callback.ResponseHandler.Invoke( r );
      }, f =>
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( f );
        else
          throw new BackendlessException( f );
      } );
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "runOnExitAction", args, responder );
    }
    #endregion
#if !NET20
    #region GEOFENCE MONITORING
    public void StartGeofenceMonitoring( GeoPoint geoPoint, AsyncCallback<object> responder )
    {
      ICallback bCallback = new ServerCallback( geoPoint );

      StartGeofenceMonitoring( bCallback, responder );
    }

    public void StartGeofenceMonitoring( GeofenceCallback callback, AsyncCallback<object> responder )
    {
      ICallback bCallback = new ClientCallback( callback );

      StartGeofenceMonitoring( bCallback, responder );
    }

    public void StartGeofenceMonitoring( String geofenceName, GeoPoint geoPoint, AsyncCallback<object> responder )
    {
      ICallback bCallback = new ServerCallback( geoPoint );

      StartGeofenceMonitoring( bCallback, geofenceName, responder );
    }

    public void StartGeofenceMonitoring( String geofenceName, GeofenceCallback callback, AsyncCallback<object> responder )
    {
      ICallback bCallback = new ClientCallback( callback );

      StartGeofenceMonitoring( bCallback, geofenceName, responder );
    }

    public void StopGeofenceMonitoring()
    {
      GeoFenceMonitoring.Instance.RemoveGeoFences();
      LocationTracker.Instance.RemoveListener( GeoFenceMonitoring.NAME );
    }

    public void StopGeofenceMonitoring( String geofenceName )
    {
      GeoFenceMonitoring.Instance.removeGeoFence( geofenceName );

      if( !GeoFenceMonitoring.Instance.IsMonitoring() )
        LocationTracker.Instance.RemoveListener( GeoFenceMonitoring.NAME );
    }

    private void StartGeofenceMonitoring( ICallback callback, AsyncCallback<object> responder )
    {
      var innerResponder = new AsyncCallback<GeoFence[]>(
        r =>
        {
          try
          {
            AddFenceMonitoring( callback, r );
          }
          catch( System.Exception e )
          {
            if( responder != null )
              responder.ErrorHandler( new BackendlessFault( e ) );
          }
        },
        f =>
        {
          if( responder != null )
            responder.ErrorHandler( f );
        } );

      Object[] args = new Object[] {};
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "getFences", args, innerResponder );
    }

    private void StartGeofenceMonitoring( ICallback callback, String geofenceName, AsyncCallback<object> responder )
    {
      var innerResponder = new AsyncCallback<GeoFence>(
        r =>
        {
          try
          {
            AddFenceMonitoring( callback, new GeoFence[] { r } );
          }
          catch( System.Exception e )
          {
            if( responder != null )
              responder.ErrorHandler( new BackendlessFault( e ) );
          }
        },
        f =>
        {
          if( responder != null )
            responder.ErrorHandler( f );
        } );

      Object[] args = new Object[] { geofenceName };
      Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "getFence", args, innerResponder );
    }

    private void AddFenceMonitoring( ICallback callback, GeoFence[] geoFences )
    {
      if( geoFences.Length == 0 )
        return;

      if( geoFences.Length == 1 )
        GeoFenceMonitoring.Instance.AddGeoFence( geoFences[ 0 ], callback );
      else
        GeoFenceMonitoring.Instance.AddGeoFences( new HashSet<GeoFence>( geoFences ), callback );

      if( !LocationTracker.Instance.ContainsListener( GeoFenceMonitoring.NAME ) )
        LocationTracker.Instance.AddListener( GeoFenceMonitoring.NAME, GeoFenceMonitoring.Instance );
    }

    #endregion
#endif
    #region RELATIVE FIND
    public IList<SearchMatchesResult> RelativeFind( BackendlessGeoQuery geoQuery )
    {
      if( geoQuery == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_GEO_QUERY );

      if( geoQuery.RelativeFindMetadata.Count == 0 || geoQuery.RelativeFindPercentThreshold == 0 )
        throw new ArgumentException( ExceptionMessage.INCONSISTENT_GEO_RELATIVE );

      return Invoker.InvokeSync<IList<SearchMatchesResult>>( GEO_MANAGER_SERVER_ALIAS, "relativeFind",
                                                                        new Object[] { geoQuery} );
    }

    public void RelativeFind( BackendlessGeoQuery geoQuery, AsyncCallback<IList<SearchMatchesResult>> callback )
    {
      try
      {
        if( geoQuery == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_GEO_QUERY );

        if( geoQuery.RelativeFindMetadata.Count == 0 || geoQuery.RelativeFindPercentThreshold == 0 )
          throw new ArgumentException( ExceptionMessage.INCONSISTENT_GEO_RELATIVE );

        var responder = new AsyncCallback<IList<SearchMatchesResult>>( r =>
        {
          if( callback != null )
            callback.ResponseHandler.Invoke( r );
        }, f =>
        {
          if( callback != null )
            callback.ErrorHandler.Invoke( f );
          else
            throw new BackendlessException( f );
        } );

        Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "relativeFind",
                             new Object[] { geoQuery }, responder );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }
    #endregion
    #region LOAD METADATA
    public GeoPoint LoadMetadata( GeoPoint point )
    {
      object[] methodArgs = null;

      if( point is GeoCluster )
        methodArgs = new object[] { point.ObjectId, ( (GeoCluster) point ).GeoQuery };
      else
        methodArgs = new object[] { point.ObjectId, null };

      point.Metadata = Invoker.InvokeSync<Dictionary<String, Object>>( GEO_MANAGER_SERVER_ALIAS, "loadMetadata", methodArgs );
      return point;
    }

    public void LoadMetadata( GeoPoint point, AsyncCallback<GeoPoint> callback )
    {
      AsyncCallback<Dictionary<String, Object>> loadMetaCallback = new AsyncCallback<Dictionary<String, Object>>(
       result =>
       {
         point.Metadata = result;

         if( callback != null )
           callback.ResponseHandler.Invoke( point );
       },
       fault =>
       {
         if( callback != null )
           callback.ErrorHandler.Invoke( fault );
       } );

      try
      {
        object[] methodArgs = null;

        if( point is GeoCluster )
          methodArgs = new object[] { point.ObjectId, ( (GeoCluster) point ).GeoQuery };
        else
          methodArgs = new object[] { point.ObjectId, null };

        Invoker.InvokeAsync<Dictionary<String, Object>>( GEO_MANAGER_SERVER_ALIAS, "loadMetadata", methodArgs, loadMetaCallback );
      }
      catch( System.Exception ex )
      {
        if( callback != null )
          callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
        else
          throw;
      }
    }
    #endregion
    #region PRIVATE UTILITY FNs
    private void CheckCoordinates( double? latitude, double? longitude )
    {
      if( latitude > 90 || latitude < -90 )
        throw new ArgumentException( ExceptionMessage.WRONG_LATITUDE_VALUE );

      if( longitude > 180 || latitude < -180 )
        throw new ArgumentException( ExceptionMessage.WRONG_LONGITUDE_VALUE );
    }

    private void CheckCategoryName( string categoryName )
    {
      if( string.IsNullOrEmpty( categoryName ) )
        throw new ArgumentNullException( ExceptionMessage.NULL_CATEGORY_NAME );

      if( categoryName.Equals( DEFAULT_CATEGORY_NAME ) )
        throw new ArgumentException( ExceptionMessage.DEFAULT_CATEGORY_NAME );
    }

    private void checkGeoQuery( BackendlessGeoQuery geoQuery )
    {
      if( geoQuery == null )
        throw new ArgumentNullException( ExceptionMessage.NULL_GEO_QUERY );

      if( geoQuery.SearchRectangle != null )
      {
        if( geoQuery.SearchRectangle.Length != 4 )
          throw new ArgumentException( ExceptionMessage.WRONG_SEARCH_RECTANGLE_QUERY );

        if( !Double.IsNaN( geoQuery.Radius ) )
          throw new ArgumentException( ExceptionMessage.INCONSISTENT_GEO_QUERY );

        if( !Double.IsNaN( geoQuery.Latitude ) )
          throw new ArgumentException( ExceptionMessage.INCONSISTENT_GEO_QUERY );

        if( !Double.IsNaN( geoQuery.Longitude ) )
          throw new ArgumentException( ExceptionMessage.INCONSISTENT_GEO_QUERY );
      }
      else if( !Double.IsNaN( geoQuery.Radius ) )
      {
        if( geoQuery.Radius <= 0 )
          throw new ArgumentException( ExceptionMessage.WRONG_RADIUS );

        if( Double.IsNaN( geoQuery.Latitude ) )
          throw new ArgumentNullException( ExceptionMessage.WRONG_LATITUDE_VALUE );

        if( Double.IsNaN( geoQuery.Longitude ) )
          throw new ArgumentNullException( ExceptionMessage.WRONG_LONGITUDE_VALUE );

        CheckCoordinates( geoQuery.Latitude, geoQuery.Longitude );

        if( geoQuery.Units == null )
          throw new ArgumentNullException( ExceptionMessage.NULL_UNIT );
      }
      else if( geoQuery.Categories == null && geoQuery.Metadata == null )
        throw new ArgumentNullException( ExceptionMessage.WRONG_GEO_QUERY );

      if( geoQuery.Categories != null )
        foreach( string categoryName in geoQuery.Categories )
          CheckCategoryName( categoryName );

      if( geoQuery.Offset < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_OFFSET );

      if( geoQuery.PageSize < 0 )
        throw new ArgumentException( ExceptionMessage.WRONG_PAGE_SIZE );
    }
    #endregion
  }
}