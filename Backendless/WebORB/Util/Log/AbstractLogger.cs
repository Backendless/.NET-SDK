using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Weborb.Util.Logging
  {
  public abstract class AbstractLogger : ILogger
    {
    private long mask;
    private bool enabled = true;
    protected string dateFormatter;
    protected bool logThreadName = false;

    public AbstractLogger() : this( 0 )
      {
      }

    public AbstractLogger( long mask )
      {
      this.mask = mask;
      }

    public bool isLogging( long mask )
      {
      return ( ( this.mask & mask ) != 0 );
      }

    public bool isLogging( string category )
      {
      return isLogging( Log.getCode( category ) );
      }

    public long getMask()
      {
      return mask;
      }

    public void setMask( long mask )
      {
      this.mask = mask;
      Log.recalcMasks();
      }

    public void addMask( long mask )
      {
      this.mask |= mask;
      Log.recalcMasks();
      }

    public void removeMask( long mask )
      {
      this.mask &= ~mask;
      Log.recalcMasks();
      }

    public void startLogging( string category )
      {
      addMask( Log.getCode( category ) );
      }

    public void stopLogging( string category )
      {
      removeMask( Log.getCode( category ) );
      }

    public void setLogDateTime( string format )
      {
      setLogDateTime( true, format );
      }

    public void setLogDateTime( bool isLogDateTime, string format )
      {
      if( !isLogDateTime )
        dateFormatter = null;
      else
        {
        if( format == null )
          format = "g";

        dateFormatter = format;
        }
      }

    public void setLogThreadName( bool logThreadName )
      {
      this.logThreadName = logThreadName;
      }

    public void disable()
      {
      enabled = false;
      }

    public void enable()
      {
      enabled = true;
      }

    public bool isEnabled()
      {
      return enabled;
      }

    protected string format( string category, object eventObject, DateTime timestamp )
      {
        if( eventObject is ExceptionHolder )
        {
          StringBuilder exceptionStringBuilder = new StringBuilder();

          Exception ex = ( (ExceptionHolder) eventObject ).ExceptionObject;

          if( ex.InnerException != null )
            exceptionStringBuilder.Append( "Outer exception: " );

          exceptionStringBuilder.Append( ex.ToString() );

          if( ex.InnerException != null )
          {
            exceptionStringBuilder.Append( "Caused by inner exception: " );
            exceptionStringBuilder.Append( ex.InnerException.ToString() );
          }

          eventObject = exceptionStringBuilder.ToString();
        }

      StringBuilder sb = new StringBuilder();

      if ( logThreadName )
        sb.Append( "[Thread-" ).Append( Thread.CurrentThread.ManagedThreadId ).Append( "] " );

      sb.Append( category );

      if ( dateFormatter != null )
        sb.Append( ":" ).Append( timestamp.ToString( dateFormatter ) );

      sb.Append( ":" ).Append( eventObject );

      return sb.ToString();
      }

    public abstract void fireEvent( String category, Object eventObject, DateTime timestamp );
    }
  }