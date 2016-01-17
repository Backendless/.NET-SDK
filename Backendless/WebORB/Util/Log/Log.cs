using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
#if FULL_BUILD
using Weborb.Messaging.Net.RTMP.Event;
using Weborb.Messaging.Net.RTMP.Message;
using Weborb.Messaging.Util;
#endif

namespace Weborb.Util.Logging
{
	public class Log
	{
		public static readonly string DEFAULTLOGGER = "default";
        private static Dictionary<String, long> stringToCode = new Dictionary<String, long>();
        private static Dictionary<long, String> codeToString = new Dictionary<long, String>();
        private static Dictionary<String, ILogger> nameToLogger = new Dictionary<String, ILogger>();
		private static long masks;

		public static long getCode( String category )
		{
			long code;

			if( stringToCode.ContainsKey( category ) )
			{
				code = (long) stringToCode[ category ];
			}
			else
			{
				code = 1L << stringToCode.Count;
				stringToCode.Add( category, code );
				codeToString.Add( code, category );
			}

			return code;
		}

		public static string getCategory( long code )
		{
			string category;
            codeToString.TryGetValue( code, out category );
            return category;
		}

		public static IEnumerator<String> getCategories()
		{
            return stringToCode.Keys.GetEnumerator();
		}

		public static bool isLogging( string category )
		{
			return isLogging( getCode( category ) );
		}

		public static bool isLogging( long mask )
		{
			return ((masks & mask) != 0 );
		}

		public static void recalcMasks()
		{
			masks = 0;

            lock( nameToLogger )
            {
                Dictionary<string, ILogger>.ValueCollection.Enumerator enumerator = nameToLogger.Values.GetEnumerator();

                while( enumerator.MoveNext() )
                {
                    ILogger logger = enumerator.Current;
                    masks |= logger.getMask();
                }
            }
		}
		
		public static void removeLogger( string name )
		  {
		  lock( nameToLogger )
		    {
#if FULL_BUILD
        // dispose logger to free file(s)
		    if( nameToLogger.ContainsKey( name ) && nameToLogger[ name ] is TraceLogger )
		      ( (TraceLogger)nameToLogger[ name ] ).Dispose();
#endif

		    nameToLogger.Remove( name );
		    }

		  recalcMasks();
		  }

	  public static void addLogger( String name, ILogger logger )
	    {
	    lock( nameToLogger )
	      {
#if FULL_BUILD
        // dispose logger if it exists to free file(s)
        if( nameToLogger.ContainsKey( name ) && nameToLogger[ name ] is TraceLogger )
          ( (TraceLogger) nameToLogger[ name ] ).Dispose();
#endif

	      nameToLogger[ name ] = logger;
	      }

	    //nameToLogger.Add( name, logger );
	    recalcMasks();
	    }

	  public static ILogger getLogger( string name )
		{
			ILogger logger;
            nameToLogger.TryGetValue( name, out logger );
            return logger;
		}

		public static ILogger[] getLoggers()
		{
            lock( nameToLogger )
            {
                ILogger[] loggers = new ILogger[ nameToLogger.Count ];
                nameToLogger.Values.CopyTo( loggers, 0 );
                return loggers;
            }
		}

		public static void startLogging( long code )
		{
			startLogging( getCategory( code ) );
		}

		public static void stopLogging( long code )
		{
			stopLogging( getCategory( code ) );
		}

		public static void startLogging( string category )
		{
            lock( nameToLogger )
            {
                foreach( object logger in nameToLogger.Values )
                    ((ILogger) logger).startLogging( category );
            }
		}

		public static void stopLogging( string category )
		{
            lock( nameToLogger )
            {
                foreach( object logger in nameToLogger.Values )
                    ((ILogger) logger).stopLogging( category );
            }
		}

		public static void log( long code, Exception exception )
		{
			log( code, null, exception );
		}

		public static void log( long code, string message, Exception exception )
		{
			if( (masks & code ) == 0 )
				return;

            ExceptionHolder holder = new ExceptionHolder();
            holder.Message = message;
            holder.ExceptionObject = exception;
			log( code, holder );
		}

        public static void log( String category, object eventObject )
        {
            log( getCode( category ), eventObject );
        }

		public static void log( long codeLong, object eventObject )
		{
			if( (masks & codeLong ) != 0 )
				log( codeLong, getCategory( codeLong ), eventObject );
		}

		public static void log( long code, string category, Object eventObject )
		{
            try
            {
                foreach( object obj in nameToLogger.Values )
                {
                    ILogger logger = (ILogger) obj;

                    if( (logger.getMask() & code) != 0 )
                        try
                        {
                            logger.fireEvent( category, eventObject, DateTime.Now );
                        }
                        catch
                        {
                        }
                }
            }
            catch
            {
            }
    }




#if FULL_BUILD
    //
    // Below methods are used while debugging RTMP
    //

    public static void logBytes( byte[] buf )
      {
      logBytes( buf, "bytes:" );
      }

    public static void logBytes( byte[] buf, string info )
      {
      lock ( typeof( Log ) )
        {
        StreamWriter file = null;
        try
          {
          file = File.AppendText( Path.Combine( Paths.GetWebORBPath(), "weborb_dump.txt" ) );
          file.WriteLine( info );
          file.WriteLine( byteBuffToString( new ByteBuffer( buf, 0, buf.Length ) ) );
          file.WriteLine( "=====================================================" );
          }
        catch ( Exception e )
          {
          if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
            Log.log( LoggingConstants.EXCEPTION, "exception while logging packet ", e );
          }
        finally
          {
          if ( file != null )
            {
            file.Flush();
            file.Close();
            }
          }
        }
      }

    public static void logPacket( Packet buf )
      {
      lock ( typeof( Log ) )
        {
        StreamWriter file = null;
        try
          {
          if ( buf.getMessage() is AudioData || buf.getMessage() is VideoData ||
              buf.getMessage() is Notify || buf.getMessage() is Invoke || buf.getMessage() is ClientBW
              || buf.getMessage() is BytesRead )
            {
            file = File.AppendText( Path.Combine( Paths.GetWebORBPath(), "weborb_dump.txt" ) );


            file.WriteLine( "packet description:" );
            file.WriteLine( buf.getHeader().getChannelId() );
            file.WriteLine( "DataType: " + buf.getHeader().getDataType() );
            file.WriteLine( buf.getHeader().getSize() );
            file.WriteLine( buf.getHeader().getStreamId() );
            file.WriteLine( buf.getHeader().getTimer() );
            file.WriteLine( buf.getHeader().getTimerBase() );
            file.WriteLine( "Body:" );
            file.WriteLine( buf.getMessage().getDataType() );
            file.WriteLine( buf.getMessage().getTimestamp() );
            file.WriteLine( buf.getMessage().getType() );

            if ( buf.getMessage() is AudioData )
              {
              file.WriteLine( ( (AudioData)buf.getMessage() ).getSourceType() );
              file.WriteLine( byteBuffToString( ( (AudioData)buf.getMessage() ).getData() ) );
              }
            else if ( buf.getMessage() is VideoData )
              {
              file.WriteLine( ( (VideoData)buf.getMessage() ).getFrameType() );
              file.WriteLine( ( (VideoData)buf.getMessage() ).getSourceType() );
              file.WriteLine( byteBuffToString( ( (VideoData)buf.getMessage() ).getData() ) );
              }
            else
              if ( buf.getMessage() is Invoke )
                {
                file.WriteLine( dictToString( ( (Invoke)buf.getMessage() ).getConnectionParams() ) );
                if ( ( (Invoke)buf.getMessage() ).getCall() != null )
                  {
                  file.WriteLine( ( (Invoke)buf.getMessage() ).getCall().getServiceName() );
                  file.WriteLine( "Method: " + ( (Invoke)buf.getMessage() ).getCall().getServiceMethodName() );
                  file.WriteLine( ( (Invoke)buf.getMessage() ).getCall().getStatus() );
                  file.WriteLine( ( (Invoke)buf.getMessage() ).getCall().isSuccess() );
                  }
                else
                  {
                  ;
                  }
                file.WriteLine( ( (Invoke)buf.getMessage() ).getInvokeId() );
                file.WriteLine( ( (Invoke)buf.getMessage() ).getSourceType() );
                file.WriteLine( byteBuffToString( ( (Invoke)buf.getMessage() ).getData() ) );
                }
              else if ( buf.getMessage() is Notify )
                {
                file.WriteLine( dictToString( ( (Notify)buf.getMessage() ).getConnectionParams() ) );
                if ( ( (Notify)buf.getMessage() ).getCall() != null )
                  {
                  file.WriteLine( ( (Notify)buf.getMessage() ).getCall().getServiceName() );
                  file.WriteLine( "Method: " + ( (Notify)buf.getMessage() ).getCall().getServiceMethodName() );
                  file.WriteLine( ( (Notify)buf.getMessage() ).getCall().getStatus() );
                  file.WriteLine( ( (Notify)buf.getMessage() ).getCall().isSuccess() );
                  }
                else
                  {
                  ;
                  }
                file.WriteLine( ( (Notify)buf.getMessage() ).getInvokeId() );
                file.WriteLine( ( (Notify)buf.getMessage() ).getSourceType() );
                file.WriteLine( byteBuffToString( ( (Notify)buf.getMessage() ).getData() ) );
                }
              else if ( buf.getMessage() is ClientBW )
                {
                file.WriteLine( ( (ClientBW)buf.getMessage() ).getBandwidth() );
                file.WriteLine( ( (ClientBW)buf.getMessage() ).getValue2() );
                file.WriteLine( ( (ClientBW)buf.getMessage() ).getSourceType() );
                }
              else if ( buf.getMessage() is BytesRead )
                {
                file.WriteLine( ( (BytesRead)buf.getMessage() ).getBytesRead() );
                }
            file.WriteLine( byteBuffToString( buf.getData() ) );
            file.WriteLine( "=====================================================" );

            }
          }
        catch ( Exception e )
          {
          if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
            Log.log( LoggingConstants.EXCEPTION, "exception while logging packet ", e );
          }
        finally
          {
          if ( file != null )
            {
            file.Flush();
            file.Close();
            }
          }
        }
      }


    private static string byteBuffToString( ByteBuffer byteBuffer )
      {
      if ( byteBuffer == null )
        return "";

      StringBuilder sb = new StringBuilder( 500 );

      int i = 0;
      foreach ( byte b in byteBuffer.GetBuffer() )
        {
        //sb.Append( b.ToString("X2") + "," );
        sb.Append( b.ToString() + "," );

        if ( i % 100 == 0 && i != 0 )
          sb.Append( '\n' );
        i++;
        }

      return sb.ToString();
      }

    private static string dictToString( IDictionary iDictionary )
      {
      if ( iDictionary == null )
        return "";

      string res = "";
      foreach ( DictionaryEntry keyValuePair in iDictionary )
        {
        res = keyValuePair.Key + ":" + keyValuePair.Value + "\n ";
        }
      return res;
      }
#endif
  }
}
