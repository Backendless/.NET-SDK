using System;

namespace BackendlessAPI.Logging
{
  public class Logger
  {
    private String loggerName;
    private LogBuffer logBuffer;

    internal Logger( String loggerName, LogBuffer logBuffer )
    {
      this.loggerName = loggerName;
      this.logBuffer = logBuffer;
    }

    public void Debug( String message )
    {
      logBuffer.Enqueue( loggerName, LogLevel.DEBUG, message, null );
    }

    public void Info( String message )
    {
      logBuffer.Enqueue( loggerName, LogLevel.INFO, message, null );
    }

    public void Warn( String message )
    {
      logBuffer.Enqueue( loggerName, LogLevel.WARN, message, null );
    }

    public void Warn( String message, System.Exception exception )
    {
      logBuffer.Enqueue( loggerName, LogLevel.WARN, message, exception );
    }

    public void Error( String message )
    {
      logBuffer.Enqueue( loggerName, LogLevel.ERROR, message, null );
    }

    public void Error( String message, System.Exception exception )
    {
      logBuffer.Enqueue( loggerName, LogLevel.ERROR, message, exception );
    }

    public void Fatal( String message )
    {
      logBuffer.Enqueue( loggerName, LogLevel.FATAL, message, null );
    }

    public void Fatal( String message, System.Exception exception )
    {
      logBuffer.Enqueue( loggerName, LogLevel.FATAL, message, exception );
    }

    public void Trace( String message )
    {
      logBuffer.Enqueue( loggerName, LogLevel.TRACE, message, null );
    }
  }
}
