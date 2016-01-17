using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using Weborb.Util;
using Weborb.Util.Cache;
using Weborb.Util.Logging;
using Weborb.Message;

namespace Weborb.Protocols
{
  public class ProtocolRegistry
  {
    private ArrayList factories = new ArrayList();


    public ArrayList Factories
    {
      get { return factories; }
    }

    public void AddMessageFactory( IMessageFactory messageFactory )
    {
      factories.Add( messageFactory );
    }

    public bool IsKnownContentType( string contentType )
    {
      foreach( IMessageFactory messageFactory in factories )
        if( messageFactory.CanParse( contentType ) )
          return true;

      return false;
    }

    public Request BuildMessage( string contentType, Stream requestStream, NameValueCollection headers )
    {
      Request message = null;

      foreach( IMessageFactory messageFactory in factories )
      {
        if( messageFactory.CanParse( contentType ) )
          try
          {
            message = messageFactory.Parse( requestStream );
            ThreadContext.getProperties()[ Cache.CURRENT_PROTOCOL ] = messageFactory.GetProtocolName( message );
            break;
          }
          catch( Exception exception )
          {
            if( Log.isLogging( LoggingConstants.EXCEPTION ) )
              Log.log( LoggingConstants.EXCEPTION, "exception while parsing request", exception );
          }
      }

      if( message == null )
        throw new UnknownRequestFormatException( "cannot parse request. possible reasons: malformed request or protocol formatter is not registered " );

      /*
      Hashtable hashtable = new Hashtable();
      string[] keys = headers.AllKeys;

      for ( int i = 0; i< keys.Length; i++) 
      {
          ArrayList list = new ArrayList();
          string[] values = headers.GetValues( keys[ i ] );

          foreach( string headerValue in values )
              list.Add( headerValue );

          hashtable.Add( keys[ i ], list );
      }

      message.getCall().setHeaders( hashtable );
       */
      return message;
    }
  }
}
