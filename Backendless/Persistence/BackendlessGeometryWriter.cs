using System;
using Weborb.Writer;


namespace BackendlessAPI.Geo
{
  public class BackendlessGeometryWriter : ITypeWriter
  {
    public void write( Object obj, IProtocolFormatter iProtocolFormatter)
    {
      try
      {
        Geometry geometry = ( Geometry )obj;
        MessageWriter.writeObject( geometry.AsWKT(), iProtocolFormatter );
      }
      catch(System.Exception ex)
      {
        Console.WriteLine( ex.Message );
      }
    }

    public ITypeWriter getReferenceWriter()
    {
      return null;
    }
  }
}
