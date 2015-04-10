﻿using System;
using System.Collections.Generic;
using BackendlessAPI.Engine;
using BackendlessAPI.Async;

namespace BackendlessAPI.Logging
{
  public class LoggingService
  {
    private const string LOGGING_SERVICE_ALIAS = "com.backendless.services.logging.LogService";
    private LogBuffer buffer;
    private Dictionary<String, Logger> loggers;

    public LoggingService()
    {
      buffer = new LogBuffer();
      loggers = new Dictionary<string, Logger>();
    }

    public void SetLogReportingPolicy( int numOfMessages, int timeFrequencyMS )
    {
      buffer.SetLogReportingPolicy( numOfMessages, timeFrequencyMS );
    }

    public Logger GetLogger( Type loggerType )
    {
      return GetLogger( loggerType.Name );
    }

    public Logger GetLogger( String loggerName )
    {
      if( loggers.ContainsKey( loggerName ) )
        return loggers[ loggerName ];

      Logger logger = new Logger( loggerName );
      loggers[ loggerName ] = logger;
      return logger;
    }

    internal LogBuffer Buffer
    {
      get
      { 
       return buffer; 
      }
    }

    internal void ReportSingleLogMessage( String logger, String loglevel, String message, System.Exception error )
    {
      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, loglevel, logger, message, error != null ? error.StackTrace : null };
      AsyncCallback<Object> callback = new AsyncCallback<Object>(
       result =>
       {
        
       },
       fault =>
       {
        
       } );
      Invoker.InvokeAsync<Object>( LOGGING_SERVICE_ALIAS, "log", args, callback );
    }

    internal void ReportBatch( LogBatch[] batch )
    {
      Object[] args = new Object[] { Backendless.AppId, Backendless.VersionNum, batch };
      AsyncCallback<Object> callback = new AsyncCallback<Object>(
       result =>
       {
        
       },
       fault =>
       {
        
       } );
      Invoker.InvokeAsync<Object>( LOGGING_SERVICE_ALIAS, "batchLog", args, callback );
    }
  }
}
