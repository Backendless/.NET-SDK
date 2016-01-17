using System;
using System.IO;
using System.Diagnostics;

namespace Weborb.Util.Logging
  {
  /// <summary>
  /// 
  /// </summary>
  public class DateLogger : TraceLogger
    {
    private static string date;

    private static string dateFormat = "D";
    private DateTime today;

    static DateLogger()
      {
      date = DateTime.Now.ToString( dateFormat );
      }

    public DateLogger() : base( date + ".log" )
      {
      today = DateTime.Now;
      fileNamePattern = "^[A-Z]([a-z]*), [A-Z]([a-z]*) ([0-9]*), ([0-9]{4})";
      }

    public override void fireEvent( string category, object eventObject, DateTime timestamp )
      {
      updateListener();
      base.fireEvent( category, eventObject, timestamp );
      }

    private void updateListener()
      {
      if( sameDay() )
        return;

      today = DateTime.Now;

      string fileName = Path.Combine( Paths.GetWebORBPath(), "logs" ) + Path.DirectorySeparatorChar +
                        today.ToString( dateFormat ) + ".log";
      FileInfo file = new FileInfo( fileName );
      if ( stream != null )
        stream.Close();
      stream = file.Open( FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite );
      if(listener != null)
      {
        Trace.Listeners.Remove(listener);
        listener.Close();
      }
      listener = new TextWriterTraceListener( stream );
      Trace.Listeners.Add( listener );
      }

    private bool sameDay()
      {
      DateTime now = DateTime.Now;
      return today.Year.Equals( now.Year ) && today.Month.Equals( now.Month ) && today.Day.Equals( now.Day );
      }
    }
  }