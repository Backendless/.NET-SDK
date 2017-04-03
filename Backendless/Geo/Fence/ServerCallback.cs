using System;
using BackendlessAPI.Async;
using BackendlessAPI.Engine;

namespace BackendlessAPI.Geo.Fence
{
    class ServerCallback : ICallback
    {
        private GeoPoint geoPoint;

        internal ServerCallback( GeoPoint geoPoint )
        {
            this.geoPoint = geoPoint;
        }

        public void CallOnEnter( GeoFence geoFence, GeoPoint location )
        {
            geoPoint = location;
            OnGeofenceServerCallback( "onEnterGeofence", geoFence.ObjectId, geoPoint );
        }

        public void CallOnStay( GeoFence geoFence, GeoPoint location )
        {
            geoPoint = location;
            OnGeofenceServerCallback( "onStayGeofence", geoFence.ObjectId, geoPoint );
        }

        public void CallOnExit( GeoFence geoFence, GeoPoint location )
        {
            geoPoint = location;
            OnGeofenceServerCallback( "onExitGeofence", geoFence.ObjectId, geoPoint );
        }

        public bool EqualCallbackParameter( Object obj )
        {
            if( !obj.GetType().Equals( typeof( GeoPoint ) ) )
                return false;

            GeoPoint point = (GeoPoint)obj;
            return this.geoPoint.Metadata.Equals( point.Metadata ) && this.geoPoint.Categories.Equals( point.Categories );
        }

        private void OnGeofenceServerCallback( String method, String geofenceId, GeoPoint geoPoint )
        {
          var responder = new AsyncCallback<object>(
            r =>
            {
            },
            f =>
            {
            } );

          Object[] args = new Object[] { geofenceId, geoPoint };
          Invoker.InvokeAsync( Service.GeoService.GEO_MANAGER_SERVER_ALIAS, method, args, responder );
        }
    }
}
