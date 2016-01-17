using System;
using System.Collections.Generic;
using System.Threading;

using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Core;
using Windows.System.Threading;

namespace BackendlessAPI.Geo.Location
{
  class Win8LocationTrackerEngine : ILocationTrackerEngine
  {
    private IBackgroundTaskRegistration _geolocTask = null;
    private CancellationTokenSource cancellationTokenSource = null;
    private ThreadPoolTimer timer;
    private LocationBackgroundTask task;
    private const string SampleBackgroundTaskName = "BackendlessLocationBackgroundTask";
    private const string SampleBackgroundTaskEntryPoint = "BackendlessAPI.Geo.Location.LocationBackgroundTask";
    private uint updateInterval = 15;
    public uint UpdateInterval
    {
      get { return updateInterval; }
      set { this.updateInterval = value; }
    }

    public void StopLocationTracker()
    {
      if( null != _geolocTask )
      {
        _geolocTask.Unregister( true );
        _geolocTask = null;
      }
    }

    public void StartLocationTracker()
    {
     if( CanRunAsTask() )
      {
        BackgroundTaskBuilder geolocTaskBuilder = new BackgroundTaskBuilder();

        geolocTaskBuilder.Name = SampleBackgroundTaskName;
        geolocTaskBuilder.TaskEntryPoint = SampleBackgroundTaskEntryPoint;

        // Create a new timer triggering at a 15 minute interval
        var trigger = new TimeTrigger( UpdateInterval, true );

        // Associate the timer trigger with the background task builder
        geolocTaskBuilder.SetTrigger( trigger );

        // Register the background task
        _geolocTask = geolocTaskBuilder.Register();

        // Associate an event handler with the new background task
        _geolocTask.Completed += new BackgroundTaskCompletedEventHandler( OnCompleted );
      }
      else
      {
         task = new LocationBackgroundTask();
        timer = ThreadPoolTimer.CreatePeriodicTimer( TimerElapsed, new TimeSpan( TimeSpan.TicksPerMillisecond * 30000 ) );
     }
    }

    private void TimerElapsed( ThreadPoolTimer timer )
    {
     var result = Windows.System.Threading.ThreadPool.RunAsync(
        (workItem) => {
          task.FetchPosition();
          LocationChanged();
        });
    }

    private bool CanRunAsTask()
    {
      try
      {
        BackgroundAccessStatus backgroundAccessStatus = BackgroundExecutionManager.GetAccessStatus();

        switch( backgroundAccessStatus )
        {
          case BackgroundAccessStatus.Unspecified:
          case BackgroundAccessStatus.Denied:
            return false;

          default:
            return true;
        }
      }
      catch( System.Exception )
      {
        return false;
      }
    }

    private void OnCompleted( IBackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs e )
    {

      if( sender != null )
      {
        // If the background task threw an exception, display the exception in
        // the error text box.
        e.CheckResult();

        LocationChanged();

        // if( settings.Values[ "Longitude" ] != null )
        // if( settings.Values[ "Accuracy" ] != null )
      }
    }

    private void LocationChanged()
    {
      var settings = ApplicationData.Current.LocalSettings;
      // if( settings.Values[ "Status" ] != null )
      double latitude = double.Parse( settings.Values[ "Latitude" ].ToString() );
      double longitude = double.Parse( settings.Values[ "Longitude" ].ToString() );
      double accuracy = double.Parse( settings.Values[ "Accuracy" ].ToString() );

      LocationTracker.Instance.LocationChanged( latitude, longitude, accuracy );
    }
  }
}
