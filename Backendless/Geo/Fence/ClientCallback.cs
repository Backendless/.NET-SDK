using System;

namespace BackendlessAPI.Geo.Fence
{
    class ClientCallback : ICallback
    {
        private GeofenceCallback geofenceCallback;

        internal ClientCallback( GeofenceCallback geofenceCallback )
        {
            this.geofenceCallback = geofenceCallback;
        }

        public void CallOnEnter( GeoFence geoFence, GeoPoint location )
        {
          LocationInfo locationInfo = new LocationInfo( geoFence.GeofenceName, geoFence.ObjectId, location.Latitude, location.Longitude );
          geofenceCallback.OnEnterHandler( locationInfo );
        }

        public void CallOnStay( GeoFence geoFence, GeoPoint location )
        {
          LocationInfo locationInfo = new LocationInfo( geoFence.GeofenceName, geoFence.ObjectId, location.Latitude, location.Longitude );
          geofenceCallback.OnStayHandler( locationInfo );
        }

        public void CallOnExit( GeoFence geoFence, GeoPoint location )
        {
          LocationInfo locationInfo = new LocationInfo( geoFence.GeofenceName, geoFence.ObjectId, location.Latitude, location.Longitude );
          geofenceCallback.OnExitHandler( locationInfo );
        }

        public bool EqualCallbackParameter( object obj )
        {
            if( !(obj is GeofenceCallback) )
                return false;

            return this.geofenceCallback.Equals( obj );
        }
    }
}
