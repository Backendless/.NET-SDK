using System;
using System.Diagnostics;

namespace Weborb.Util.Logging
{
	/// <summary>
	/// Summary description for EventLogger.
	/// </summary>
	public class EventLogger : AbstractLogger
	{
		private new static bool isLogging = false;

		private EventLog eventLog;

		public EventLogger()
		{
			if( !EventLog.Exists( "WebORB.Net" ) )
				try
				{
					EventLog.CreateEventSource( "WebORB.Net", "Application");
				}
				catch( Exception )
				{
					//Log.log( LoggingConstants.ERROR, "could not register event source", exception );
				}
			
			eventLog = new EventLog();
			eventLog.Source = "WebORB.Net";
			isLogging = true;
		}

		public override void fireEvent( string category, object eventObject, DateTime timestamp )
		{
			if( eventObject is Exception )
				eventLog.WriteEntry( category + ":" + timestamp + ":" + eventObject, EventLogEntryType.Error );
			else
				eventLog.WriteEntry( category + ":" + timestamp + ":" + eventObject );
		}

		public static bool IsLogging
		{
			get
			{
				return isLogging;
			}
		}
	}
}
