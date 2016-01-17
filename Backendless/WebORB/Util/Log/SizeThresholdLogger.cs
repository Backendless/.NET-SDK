using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Weborb.Util.Logging
{
  /// <summary>
  /// 
  /// </summary>
  public class SizeThresholdLogger : TraceLogger
  {
    private FileInfo currentFile;
    private string fileNamePrefix;
    private int fileNumber = 1;
    private long sizeThreshold;
    private bool initialized;

    public SizeThresholdLogger( long threashold, string fileName )
    {
      this.sizeThreshold = threashold * 1024;
      this.fileNamePrefix = fileName;
      this.fileName = fileName;
      fileNamePattern = fileName + "*";
    }

    private void initialize()
    {
      lock ( this )
      {
        if ( this.initialized && stream != null )
          return;

        if ( fileName.IndexOf( Path.DirectorySeparatorChar ) == -1 )
        {
          fileName = Path.Combine( Path.Combine( Paths.GetWebORBPath(), "logs" ), fileName );
        }
        else
        {
          int index1 = fileName.LastIndexOf( Path.DirectorySeparatorChar ) + 1;
          this.fileNamePrefix = fileName.Substring( index1, fileName.Length - index1 );
        }


        fileName = fileName.Replace( "/", Path.DirectorySeparatorChar.ToString() );
        FileInfo logFile = new FileInfo( fileName );

        string logFileName = logFile.Name;
        DirectoryInfo directory = logFile.Directory;
        FileInfo[] logFiles = directory.GetFiles( logFileName + "*.log" );

        foreach ( FileInfo file in logFiles )
          fileNumber = Math.Max( fileNumber, getLogFileNumber( file.Name ) );

        setNewListener();
        this.initialized = true;
      }
    }

    public override void fireEvent( string category, object eventObject, DateTime timestamp )
    {
      initialize(); //Lazy initialization... only do this if we actually use this logger
      rolloverLogFile();

      StringBuilder sb = new StringBuilder();

      if ( eventObject is ExceptionHolder )
        eventObject = ( (ExceptionHolder)eventObject ).ExceptionObject.ToString();

      if ( logThreadName )
        sb.Append( "[Thread-" ).Append( Thread.CurrentThread.ManagedThreadId ).Append( "] " );

      sb.Append( category );

      if ( dateFormatter != null )
        sb.Append( ":" ).Append( timestamp.ToString( dateFormatter ) );

      sb.Append( ":" ).Append( eventObject );
      Trace.WriteLine( sb.ToString() );

      Trace.Flush();
    }

    private int getLogFileNumber( string fileName )
    {
      //This substring feels brittle, the check below at least makes sure there is an extenstion
      if ( fileName.IndexOf( "." ) == -1 )
        return fileNumber;
      else
        return Convert.ToInt32( fileName.Substring( fileNamePrefix.Length, fileName.Length - 4 - fileNamePrefix.Length ) );
    }

    private void setNewListener()
    {
      try
      {
        currentFile = new FileInfo( fileName + fileNumber + ".log" );
        DirectoryInfo directory = currentFile.Directory;

        if ( !directory.Exists )
          directory.Create();
        if ( stream != null )
          stream.Close();

        try
        {
          stream = currentFile.Open(FileMode.Append, FileAccess.Write, FileShare.Read);
        }
        catch(IOException)
        {
          fileNumber++;
          setNewListener();
          return;
        }

        if ( listener != null )
        {
          Trace.Listeners.Remove( listener );
          listener.Close();
        }
        listener = new TextWriterTraceListener( stream );
        Trace.Listeners.Add( listener );
      }
      catch ( Exception exception )
      {
        try
        {
          if ( !EventLogger.IsLogging )
            Log.addLogger( "eventLogger", new EventLogger() );

          Log.stopLogging( LoggingConstants.DEBUG );
          Log.stopLogging( LoggingConstants.INFO );
          Log.stopLogging( LoggingConstants.INSTRUMENTATION );
          Log.stopLogging( LoggingConstants.SERIALIZATION );
          Log.stopLogging( LoggingConstants.SECURITY );
          Log.stopLogging( LoggingConstants.ZIP_DEBUG );

          EventLog eventLog = new EventLog();
          eventLog.Source = "WebORB Event Source";
          eventLog.WriteEntry(
            "An exception was thrown while WebORB.Net was attempting to create a log file.  This typically happens because " +
            "the default ASPNET user that the aspnet_wp process executes in does not have access priviledges to create and write to " +
            "files.  This can be fixed by either 1) modifying the machine.config file to allow the aspnet_wp process to run under a more priviledged user " +
            "OR by 2) giving the ASPNET user more priviledges.  To accomplish #1, change the userName attribute of the processModel element in machine.config " +
            "from 'machine' to 'SYSTEM';  the machine.config file typically located in C:\\WINDOWS\\Microsoft.NET\\Framework\\v1.1.4322\\CONFIG.  The easiest way to accomplish #2 " +
            "is to create a logs directory in your web application and give the ASPNET user full access to that folder.  To do this, right-click the " +
            "logs directory you just created in Windows Explorer and select 'Sharing and Security' from the popup menu.  Under the Security tab, add the ASPNET user and select the Full Control checkbox for " +
            "that user.  Please direct any questions or comments to http://www.themidnightcoders.com/forum/.",
            EventLogEntryType.Error );


          Trace.WriteLine( exception );
        }
        catch
        {
        }
      }
    }

    ~SizeThresholdLogger()
    {
      Dispose( false );
    }

    private void rolloverLogFile()
    {
      if ( stream.Length < sizeThreshold )
        return;

      lock ( this )
      {
        if ( stream.Length < sizeThreshold )
          return;
        fileNumber++;
        setNewListener();
      }
    }
  }
}