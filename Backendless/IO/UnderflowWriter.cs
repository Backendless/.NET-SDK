using System;
using System.Collections;
using System.Collections.Generic;
using BackendlessAPI.Utils;
using Weborb.Writer;

namespace BackendlessAPI.IO
{
  public class UnderflowWriter : ObjectWriter
  {
    protected override void onWriteObject( object obj, string className, IDictionary objectFields, IProtocolFormatter writer )
    {
      IDictionary<string, object> underflowData = UnderflowStore.GetObjectUnderflow( obj );

      if( underflowData != null )
      {
        int lastIndex = className.LastIndexOf( '.' );

        if( lastIndex > -1 )
          className = className.Substring( lastIndex + 1 );

        foreach( string key in underflowData.Keys )
          if( !objectFields.Contains( key ) && underflowData[ key ] != null )
            objectFields[ key ] = underflowData[ key ];
      }

      if( !className.StartsWith( "flex.messaging.messages" ) && !className.StartsWith( "com.backendless" ) )
      {
        if( objectFields.Contains( "___class" ) )
          className = (String) objectFields[ "___class" ];
        else
        {
          int dotIndex = className.LastIndexOf( '+' );

          if( dotIndex == -1 )
            dotIndex = className.LastIndexOf( '.' );

          if( dotIndex > -1 )
            className = className.Substring( dotIndex + 1 );

          objectFields.Add( "___class", className );
        }

        className = null;
      }

      writer.GetObjectSerializer().WriteObject( className, objectFields, writer );
    }
  }
}
