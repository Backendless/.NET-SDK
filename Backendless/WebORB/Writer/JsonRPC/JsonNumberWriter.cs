using System;
using Weborb.Writer;
namespace Weborb.Writer.JsonRPC
{
  public class JsonNumberWriter : AbstractUnreferenceableTypeWriter
  {
    public override void write( object obj, IProtocolFormatter formatter )
    {
      JsonRPCFormatter jsonRPCFormatter = (JsonRPCFormatter) formatter;

      if( obj is int )
        jsonRPCFormatter.WriteInteger( (int) obj );
      else if( obj is long )
        jsonRPCFormatter.WriteLong( (long) obj );
      else
        jsonRPCFormatter.WriteDouble( (double) obj );
    }
  }
}
