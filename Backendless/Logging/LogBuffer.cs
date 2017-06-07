using System;
using System.Threading;
using System.Collections.Generic;

namespace BackendlessAPI.Logging
{
  internal class LogBuffer
  {
    private int numOfMessages;
    private int timeFrequency;
    private LinkedList<LogMessage> logBatch;
#if !UNITY && !PURE_CLIENT_LIB   
    private Mutex mutex;
#endif
    private Timer timer;
    private TimerCallback timerCallback;

    internal LogBuffer()
    {
#if !UNITY && !PURE_CLIENT_LIB
      mutex = new Mutex( false, "LogBufferMutex" );
#endif
      numOfMessages = 100;
      timeFrequency = 1000 * 60 * 5; // 5 minutes
      logBatch = new LinkedList<LogMessage>();
      setupTimer();
    }

    private void setupTimer()
    {
      if( timer != null )
      {
        timer.Dispose();
        timer = null;
      }

      if( timerCallback == null )
        timerCallback = new TimerCallback( FlushMessages );

      if( numOfMessages > 1 )
        timer = new Timer( timerCallback, null, 0, timeFrequency ); 
    }

    public void FlushMessages( Object stateInfo )
    {
      if( logBatch.Count == 0 )
        return;
#if !UNITY && !PURE_CLIENT_LIB
      mutex.WaitOne();
      Flush();
      mutex.ReleaseMutex();
#else
      Flush();
#endif
      }

    internal void ResetTimer()
    {
      timer.Change( timeFrequency, timeFrequency );
    }

    public void SetLogReportingPolicy( int numOfMessages, int timeFrequency )
    {
      if( numOfMessages > 1 && timeFrequency <= 0 )
       throw new System.Exception( "the time frequency argument must be greater than zero" );

      this.numOfMessages = numOfMessages;
      this.timeFrequency = timeFrequency;
      setupTimer();
    }

    internal void Enqueue( String logger, LogLevel logLevel, String message, System.Exception error )
    {
      if( numOfMessages == 1 )
      {
        Backendless.Logging.ReportSingleLogMessage( logger, logLevel, message, error );
      }
      else
      {
        String logLevelStr = logLevel.ToString();

#if !UNITY && !PURE_CLIENT_LIB
        mutex.WaitOne();
#endif
        logBatch.AddLast( new LogMessage( logger, logLevel, DateTime.Now, message, error == null ? null : error.StackTrace ) );

        if( logBatch.Count == numOfMessages )
        {
          Flush();
          ResetTimer();
        }
#if !UNITY && !PURE_CLIENT_LIB
        mutex.ReleaseMutex();
#endif
      }
    }

    private void Flush()
    {
      LogMessage[] messages = new LogMessage[ logBatch.Count ];
      logBatch.CopyTo( messages, 0 );
      logBatch.Clear();
      Backendless.Logging.ReportBatch( messages );
    }
  }
}
