using System;
using System.Collections.Generic;
using Weborb.Writer;

namespace BackendlessAPI.IO
{
  class BackendlessUserWriter : AbstractUnreferenceableTypeWriter
  {
    public override void write( object obj, IProtocolFormatter writer )
    {
      BackendlessUser user = (BackendlessUser) obj;

      Dictionary<string, object> props = user.Properties;
      props.Add( "___class", "Users" );
      MessageWriter.writeObject( props, writer );
    }
  }
}
