using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Storage;

namespace BackendlessAPI.Geo.Location
{
    public sealed class LocationBackgroundTask : IBackgroundTask
    {
        CancellationTokenSource cts = null;

        async void IBackgroundTask.Run( IBackgroundTaskInstance taskInstance )
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                // Associate a cancellation handler with the background task.
                taskInstance.Canceled += new BackgroundTaskCanceledEventHandler( OnCanceled );
                FetchPosition();
            }
            catch( UnauthorizedAccessException )
            {
                WriteStatusToAppData( "Disabled" );
                WipeGeolocDataFromAppData();
            }
            catch( System.Exception ex )
            {
#if WINDOWS_APP
                // If there are no location sensors GetGeopositionAsync()
                // will timeout -- that is acceptable.
                const int WaitTimeoutHResult = unchecked((int)0x80070102);

                if (ex.HResult == WaitTimeoutHResult) // WAIT_TIMEOUT
                {
                    WriteStatusToAppData("An operation requiring location sensors timed out. Possibly there are no location sensors.");
                }
                else
#endif
                {
                    WriteStatusToAppData( ex.ToString() );
                }

                WipeGeolocDataFromAppData();
            }
            finally
            {
                cts = null;

                deferral.Complete();
            }
        }

        async internal void FetchPosition()
        {
          if( cts == null )
            cts = new CancellationTokenSource();

          // Create geolocator object
          Geolocator geolocator = new Geolocator();

          // Make the request for the current position
          Geoposition pos = await geolocator.GetGeopositionAsync().AsTask( cts.Token );

          DateTime currentTime = DateTime.Now;

          WriteStatusToAppData( "Time: " + currentTime.ToString() );
          WriteGeolocToAppData( pos );
        }

        private void WriteGeolocToAppData( Geoposition pos )
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[ "Latitude" ] = pos.Coordinate.Point.Position.Latitude.ToString();
            settings.Values[ "Longitude" ] = pos.Coordinate.Point.Position.Longitude.ToString();
            settings.Values[ "Accuracy" ] = pos.Coordinate.Accuracy.ToString();
        }

        private void WipeGeolocDataFromAppData()
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[ "Latitude" ] = "";
            settings.Values[ "Longitude" ] = "";
            settings.Values[ "Accuracy" ] = "";
        }

        private void WriteStatusToAppData( string status )
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values[ "Status" ] = status;
        }

        private void OnCanceled( IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason )
        {
            if( cts != null )
            {
                cts.Cancel();
                cts = null;
            }
        }
    }
}
