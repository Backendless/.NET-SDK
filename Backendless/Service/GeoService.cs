using System;
using System.Collections.Generic;
using BackendlessAPI.Async;
using BackendlessAPI.Data;
using BackendlessAPI.Engine;
using BackendlessAPI.Exception;
using BackendlessAPI.Geo;
using Weborb.Client;
using Weborb.Types;

namespace BackendlessAPI.Service
{
    public class GeoService
    {
        private static string GEO_MANAGER_SERVER_ALIAS = "com.backendless.services.geo.GeoService";
        private static string DEFAULT_CATEGORY_NAME = "Default";

        public GeoService()
        {
            Types.AddClientClassMapping("com.backendless.geo.model.SearchMatchesResult", typeof(SearchMatchesResult));
            Types.AddClientClassMapping("com.backendless.geo.model.GeoPoint", typeof(GeoPoint));
            Types.AddClientClassMapping("com.backendless.geo.model.GeoCategory", typeof(GeoCategory));
            Types.AddClientClassMapping("com.backendless.geo.BackendlessGeoQuery", typeof(BackendlessGeoQuery));
            Types.AddClientClassMapping("com.backendless.geo.Units", typeof(Units));
        }

        public GeoCategory AddCategory(string categoryName)
        {
            CheckCategoryName(categoryName);

            return Invoker.InvokeSync<GeoCategory>(GEO_MANAGER_SERVER_ALIAS, "addCategory",
                                                    new object[] { Backendless.AppId, Backendless.VersionNum, categoryName });
        }

        public void AddCategory(string categoryName, AsyncCallback<GeoCategory> callback)
        {
            try
            {
                CheckCategoryName(categoryName);

                Invoker.InvokeAsync(GEO_MANAGER_SERVER_ALIAS, "addCategory",
                                     new Object[] { Backendless.AppId, Backendless.VersionNum, categoryName }, callback);
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex));
                else
                    throw;
            }
        }

        public bool DeleteCategory(string categoryName)
        {
            CheckCategoryName(categoryName);

            return Invoker.InvokeSync<bool>(GEO_MANAGER_SERVER_ALIAS, "deleteCategory",
                                             new object[] { Backendless.AppId, Backendless.VersionNum, categoryName });
        }

        public void DeleteCategory(string categoryName, AsyncCallback<bool> callback)
        {
            try
            {
                CheckCategoryName(categoryName);

                Invoker.InvokeAsync(GEO_MANAGER_SERVER_ALIAS, "deleteCategory",
                                     new Object[] { Backendless.AppId, Backendless.VersionNum, categoryName }, callback);
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex));
                else
                    throw;
            }
        }

        public GeoPoint SavePoint(double latitude, double longitude, Dictionary<string, string> metadata)
        {
            return SavePoint(latitude, longitude, null, metadata);
        }

        public void SavePoint(double latitude, double longitude, Dictionary<string, string> metadata, AsyncCallback<GeoPoint> callback)
        {
            SavePoint(latitude, longitude, null, metadata, callback);
        }

        public GeoPoint SavePoint(double latitude, double longitude, List<string> categoryNames, Dictionary<string, string> metadata)
        {
            CheckCoordinates(latitude, longitude);
            return Invoker.InvokeSync<GeoPoint>(GEO_MANAGER_SERVER_ALIAS, "addPoint",
                                                 new object[]
                                             {
                                               Backendless.AppId, Backendless.VersionNum,
                                               new GeoPoint( latitude, longitude, categoryNames, metadata )
                                             });
        }

        public void SavePoint(double latitude, double longitude, List<string> categoryNames, Dictionary<string, string> metadata, AsyncCallback<GeoPoint> callback)
        {
            try
            {
                CheckCoordinates(latitude, longitude);

                Invoker.InvokeAsync(GEO_MANAGER_SERVER_ALIAS, "addPoint",
                                     new object[]
                               {
                                 Backendless.AppId, Backendless.VersionNum,
                                 new GeoPoint( latitude, longitude, categoryNames, metadata )
                               }, callback);
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex));
                else
                    throw;
            }
        }

        public GeoPoint AddPoint( GeoPoint geoPoint )
        {
          return SavePoint( geoPoint );
        }

        public GeoPoint SavePoint(GeoPoint geoPoint)
        {
            if (geoPoint == null)
                throw new ArgumentNullException(ExceptionMessage.NULL_GEOPOINT);

            CheckCoordinates(geoPoint.Latitude, geoPoint.Longitude);

            String methodName = geoPoint.ObjectId == null ? "addPoint" : "updatePoint";

            return Invoker.InvokeSync<GeoPoint>(GEO_MANAGER_SERVER_ALIAS, methodName,
                                                 new object[] { Backendless.AppId, Backendless.VersionNum, geoPoint });
        }

        public void AddPoint( GeoPoint geoPoint, AsyncCallback<GeoPoint> callback )
        {
          SavePoint( geoPoint, callback );
        }

        public void SavePoint(GeoPoint geoPoint, AsyncCallback<GeoPoint> callback)
        {
            try
            {
                if (geoPoint == null)
                    throw new ArgumentNullException(ExceptionMessage.NULL_GEOPOINT);

                CheckCoordinates(geoPoint.Latitude, geoPoint.Longitude);
                String methodName = geoPoint.ObjectId == null ? "addPoint" : "updatePoint";
                Invoker.InvokeAsync(GEO_MANAGER_SERVER_ALIAS, methodName,
                                     new object[] { Backendless.AppId, Backendless.VersionNum, geoPoint }, callback);
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex));
                else
                    throw;
            }
        }

        public void removePoint( GeoPoint geoPoint )
        {
          if( geoPoint == null )
            throw new ArgumentNullException( ExceptionMessage.NULL_GEOPOINT );

          CheckCoordinates( geoPoint.Latitude, geoPoint.Longitude );
          Invoker.InvokeSync<GeoPoint>( GEO_MANAGER_SERVER_ALIAS, "removePoint",
                                               new object[] { Backendless.AppId, Backendless.VersionNum, geoPoint.ObjectId } );
        }

        public void RemovePoint( GeoPoint geoPoint, AsyncCallback<GeoPoint> callback )
        {
          try
          {
            if( geoPoint == null )
              throw new ArgumentNullException( ExceptionMessage.NULL_GEOPOINT );

            Invoker.InvokeAsync( GEO_MANAGER_SERVER_ALIAS, "removePoint",
                                 new object[] { Backendless.AppId, Backendless.VersionNum, geoPoint.ObjectId }, callback );
          }
          catch( System.Exception ex )
          {
            if( callback != null )
              callback.ErrorHandler.Invoke( new BackendlessFault( ex ) );
            else
              throw;
          }
        }

        public BackendlessCollection<GeoPoint> GetPoints(BackendlessGeoQuery geoQuery)
        {
            checkGeoQuery(geoQuery);

            var result = Invoker.InvokeSync<BackendlessCollection<GeoPoint>>(GEO_MANAGER_SERVER_ALIAS, "getPoints",
                                                                              new Object[]
                                                                          {
                                                                            Backendless.AppId, Backendless.VersionNum,
                                                                            geoQuery
                                                                          });

            result.Query = geoQuery;

            return result;
        }

        public void GetPoints(BackendlessGeoQuery geoQuery, AsyncCallback<BackendlessCollection<GeoPoint>> callback)
        {
            try
            {
                checkGeoQuery(geoQuery);

                var responder = new AsyncCallback<BackendlessCollection<GeoPoint>>(r =>
                  {
                      r.Query = geoQuery;

                      if (callback != null)
                          callback.ResponseHandler.Invoke(r);
                  }, f =>
                    {
                        if (callback != null)
                            callback.ErrorHandler.Invoke(f);
                        else
                            throw new BackendlessException(f);
                    });

                Invoker.InvokeAsync(GEO_MANAGER_SERVER_ALIAS, "getPoints",
                                     new Object[] { Backendless.AppId, Backendless.VersionNum, geoQuery }, responder);
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex));
                else
                    throw;
            }
        }

        public BackendlessCollection<SearchMatchesResult> RelativeFind(BackendlessGeoQuery geoQuery)
        {
            if (geoQuery == null)
                throw new ArgumentNullException(ExceptionMessage.NULL_GEO_QUERY);

            if (geoQuery.RelativeFindMetadata.Count == 0 || geoQuery.RelativeFindPercentThreshold == 0)
                throw new ArgumentException(ExceptionMessage.INCONSISTENT_GEO_RELATIVE);

            var result = Invoker.InvokeSync<BackendlessCollection<SearchMatchesResult>>(GEO_MANAGER_SERVER_ALIAS, "relativeFind",
                                                                              new Object[]
                                                                          {
                                                                            Backendless.AppId, Backendless.VersionNum,
                                                                            geoQuery
                                                                          });

            result.Query = geoQuery;

            return result;
        }

        public void RelativeFind(BackendlessGeoQuery geoQuery, AsyncCallback<BackendlessCollection<SearchMatchesResult>> callback)
        {
            try
            {
                if (geoQuery == null)
                    throw new ArgumentNullException(ExceptionMessage.NULL_GEO_QUERY);

                if (geoQuery.RelativeFindMetadata.Count == 0 || geoQuery.RelativeFindPercentThreshold == 0)
                    throw new ArgumentException(ExceptionMessage.INCONSISTENT_GEO_RELATIVE);

                var responder = new AsyncCallback<BackendlessCollection<SearchMatchesResult>>(r =>
                {
                    r.Query = geoQuery;

                    if (callback != null)
                        callback.ResponseHandler.Invoke(r);
                }, f =>
                {
                    if (callback != null)
                        callback.ErrorHandler.Invoke(f);
                    else
                        throw new BackendlessException(f);
                });

                Invoker.InvokeAsync(GEO_MANAGER_SERVER_ALIAS, "relativeFind",
                                     new Object[] { Backendless.AppId, Backendless.VersionNum, geoQuery }, responder);
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex));
                else
                    throw;
            }
        }

        public List<GeoCategory> GetCategories()
        {
            return Invoker.InvokeSync<List<GeoCategory>>(GEO_MANAGER_SERVER_ALIAS, "getCategories",
                                                          new object[] { Backendless.AppId, Backendless.VersionNum });
        }

        public void GetCategories(AsyncCallback<List<GeoCategory>> callback)
        {
            try
            {
                Invoker.InvokeAsync(GEO_MANAGER_SERVER_ALIAS, "getCategories",
                                     new object[] { Backendless.AppId, Backendless.VersionNum }, callback);
            }
            catch (System.Exception ex)
            {
                if (callback != null)
                    callback.ErrorHandler.Invoke(new BackendlessFault(ex));
                else
                    throw;
            }
        }

        private void CheckCoordinates(double? latitude, double? longitude)
        {
            if (latitude > 90 || latitude < -90)
                throw new ArgumentException(ExceptionMessage.WRONG_LATITUDE_VALUE);

            if (longitude > 180 || latitude < -180)
                throw new ArgumentException(ExceptionMessage.WRONG_LONGITUDE_VALUE);
        }

        private void CheckCategoryName(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
                throw new ArgumentNullException(ExceptionMessage.NULL_CATEGORY_NAME);

            if (categoryName.Equals(DEFAULT_CATEGORY_NAME))
                throw new ArgumentException(ExceptionMessage.DEFAULT_CATEGORY_NAME);
        }

        private void checkGeoQuery(BackendlessGeoQuery geoQuery)
        {
            if (geoQuery == null)
                throw new ArgumentNullException(ExceptionMessage.NULL_GEO_QUERY);

            if (geoQuery.SearchRectangle != null)
            {
                if (geoQuery.SearchRectangle.Length != 4)
                    throw new ArgumentException(ExceptionMessage.WRONG_SEARCH_RECTANGLE_QUERY);

                if (!Double.IsNaN(geoQuery.Radius))
                    throw new ArgumentException(ExceptionMessage.INCONSISTENT_GEO_QUERY);

                if (!Double.IsNaN(geoQuery.Latitude))
                    throw new ArgumentException(ExceptionMessage.INCONSISTENT_GEO_QUERY);

                if (!Double.IsNaN(geoQuery.Longitude))
                    throw new ArgumentException(ExceptionMessage.INCONSISTENT_GEO_QUERY);
            }
            else if (!Double.IsNaN(geoQuery.Radius))
            {
                if (geoQuery.Radius <= 0)
                    throw new ArgumentException(ExceptionMessage.WRONG_RADIUS);

                if (Double.IsNaN(geoQuery.Latitude))
                    throw new ArgumentNullException(ExceptionMessage.WRONG_LATITUDE_VALUE);

                if (Double.IsNaN(geoQuery.Longitude))
                    throw new ArgumentNullException(ExceptionMessage.WRONG_LONGITUDE_VALUE);

                CheckCoordinates(geoQuery.Latitude, geoQuery.Longitude);

                if (geoQuery.Units == null)
                    throw new ArgumentNullException(ExceptionMessage.NULL_UNIT);
            }
            else if (geoQuery.Categories == null && geoQuery.Metadata == null)
                throw new ArgumentNullException(ExceptionMessage.WRONG_GEO_QUERY);

            if (geoQuery.Categories != null)
                foreach (string categoryName in geoQuery.Categories)
                    CheckCategoryName(categoryName);

            if (geoQuery.Offset < 0)
                throw new ArgumentException(ExceptionMessage.WRONG_OFFSET);

            if (geoQuery.PageSize < 0)
                throw new ArgumentException(ExceptionMessage.WRONG_PAGE_SIZE);
        }
    }
}