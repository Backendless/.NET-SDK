using System;
using System.IO;
using System.Collections;

using Weborb.Message;
using Weborb.Util;
#if FULL_BUILD
using Weborb.Util.Cache;
#endif
using Weborb.Util.Logging;

namespace Weborb.Writer
  {
  public class AMFMessageWriter : AbstractUnreferenceableTypeWriter
    {
    #region ITypeWriter Members

    public override void write( object obj, IProtocolFormatter writer )
      {
      Request message = (Request)obj;
      writer.BeginWriteMessage( message );
      writer.WriteMessageVersion( (int)message.getVersion() );

      Header[] headers = message.getResponseHeaders();
      Body[] bodies = message.getResponseBodies();

      writer.DirectWriteShort( headers.Length );

      if ( Log.isLogging( LoggingConstants.DEBUG ) )
        {
        Log.log( LoggingConstants.DEBUG, "got headers " + headers.Length );
        Log.log( LoggingConstants.DEBUG, "got bodies " + bodies.Length );
        Log.log( LoggingConstants.DEBUG, "AMFMessageWriter.write - message version: " + message.getVersion() + " header length: " + headers.Length );
        }

      for ( int i = 0; i < headers.Length; i++ )
        MessageWriter.writeObject( headers[ i ], writer );

      writer.DirectWriteShort( bodies.Length );

#if FULL_BUILD
      bool isAMF3 = ThreadContext.currentHttpContext() != null && ThreadContext.currentHttpContext().Items.Contains( "v3_request" );
#endif

      for ( int i = 0; i < bodies.Length; i++ )
        {
        // if message is amf0 then body of the message is invocation result and if should be 
        // processed via cache managment
#if FULL_BUILD
        if ( !isAMF3 )
          Cache.WriteAndSave( bodies[ i ], writer );
        else
#endif
          MessageWriter.writeObject( bodies[ i ], writer );
        }

      writer.EndWriteMessage();
      }

    #endregion
    }
  }
