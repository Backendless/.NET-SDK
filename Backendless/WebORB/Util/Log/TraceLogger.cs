using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Permissions;

namespace Weborb.Util.Logging
  {
  /// <summary>
  /// 
  /// </summary>
  public class TraceLogger : AbstractLogger, IDisposable
    {
    protected bool _disposed;
    protected string fileName;
    protected string fileNamePattern;
    protected FileStream stream;
    protected TextWriterTraceListener listener;
    private bool initialized = false;


    public TraceLogger() : base()
      {
      initialized = true;
      _disposed = false;
      }

    public TraceLogger( string fileName ) : base()
      {
      this.fileName = fileName;
      fileNamePattern = fileName;
      }

    private bool initialize()
      {
      lock( this )
        {
        if( initialized )
          return initialized;

        if( fileName.IndexOf( Path.DirectorySeparatorChar ) == -1 )
          fileName = Path.Combine( Path.Combine( Paths.GetWebORBPath(), "logs" ), fileName );

        try
          {
          FileInfo file = new FileInfo( fileName );
          DirectoryInfo parent = file.Directory;

          if( !parent.Exists )
            parent.Create();

          Trace.AutoFlush = true;
          stream = file.Open( FileMode.Append, FileAccess.Write, FileShare.Read );
          }
          catch( IOException )
          {
            try
            {
              FileInfo busyFile = new FileInfo( fileName );
              fileName = busyFile.Directory.FullName + Path.DirectorySeparatorChar + Process.GetCurrentProcess().Id + "_" + busyFile.Name;

              FileInfo file = new FileInfo( fileName );
              stream = file.Open( FileMode.Append, FileAccess.Write, FileShare.Read );

            }
            catch (Exception)
            {
              LogToEventLog();
              //This means typically means the process doesn't have rights to log!
              return false;
            }
          }
        catch( Exception e )
        {
          LogToEventLog();
          //This means typically means the process doesn't have rights to log!
          return false;
        }

          listener = new TextWriterTraceListener( stream );
        Trace.Listeners.Add( listener );
        return ( initialized = true );
        }
      }

    private void LogToEventLog()
    {
      try
      {
        if( !EventLogger.IsLogging )
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
      }
      catch
      {
      }
    }

    public List<String> getFileNames()
      {
      if( fileNamePattern == null )
        return null;
      DirectoryInfo di = new DirectoryInfo( Path.Combine( Paths.GetWebORBPath(), "logs" ) );
      FileInfo[] rgFiles = di.GetFiles();

      Array.Sort( rgFiles, new FileSorter() );

      List<String> files = new List<String>();

      foreach( FileInfo fileInfo in rgFiles )
        {
        Match m = Regex.Match( fileInfo.Name, fileNamePattern );
        if( m.Success )
          files.Add( fileInfo.Name );
        }
      return files;
      }

    public override void fireEvent( string category, object eventObject, DateTime timestamp )
    {
      if (!initialize())
        return;

      if (eventObject is ExceptionHolder)
        eventObject = ((ExceptionHolder) eventObject).ExceptionObject.ToString();

      StringBuilder sb = new StringBuilder();

      if (logThreadName)
        sb.Append("[Thread-").Append(Thread.CurrentThread.ManagedThreadId).Append("] ");

      sb.Append(category);

      if (dateFormatter != null)
        sb.Append(":").Append(timestamp.ToString(dateFormatter));

      sb.Append(":").Append(eventObject);
      Trace.WriteLine(sb.ToString());
      Trace.Flush();
      listener.Flush();
      stream.Flush();

    }

    public void Dispose()
      {
      Dispose( true );

      // Use SupressFinalize in case a subclass
      // of this type implements a finalizer.
      GC.SuppressFinalize( this );
      }

    protected virtual void Dispose( bool disposing )
    {
      lock (this)
      {
        if (!_disposed)
        {
          if (disposing)
          {
            if ( listener != null )
            {
              Trace.Listeners.Remove(listener);
              listener.Close();
            }
            if (stream != null)
              stream.Close();
          }

          stream = null;
          listener = null;

          // Indicate that the instance has been disposed.        
          _disposed = true;
        }
      }
    }

    ~TraceLogger()
      {
      Dispose( false );
      }
    }

  internal class FileSorter : IComparer
    {
    public int Compare( object x, object y )
      {
      FileInfo f1 = (FileInfo)x;
      FileInfo f2 = (FileInfo)y;

      if( f1.LastWriteTime > f2.LastWriteTime )
        return -1;
      else if( f1.LastWriteTime == f2.LastWriteTime )
        return 0;
      else
        return 1;
      }
    }
  }