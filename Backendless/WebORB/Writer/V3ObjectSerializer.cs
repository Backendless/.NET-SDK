using System;
using System.Collections;
using System.IO;
using System.Text;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
  public class V3ObjectSerializer : IObjectSerializer
  {
    #region IObjectSerializer Members

    public void WriteObject( string className, IDictionary objectFields, IProtocolFormatter writer )
    {
      IEnumerator en = objectFields.Keys.GetEnumerator();
      V3ReferenceCache cache = (V3ReferenceCache) writer.GetReferenceCache();
      String traitsClassId = className;

      if( traitsClassId == null )
      {
        StringBuilder sb = new StringBuilder();

        while( en.MoveNext() )
        {
          sb.Append( en.Current.ToString() );
          sb.Append( "-" );
        }

        traitsClassId = sb.ToString();
        en.Reset();
      }

      if( cache.HasTraits( traitsClassId ) )
      {
        writer.DirectWriteBytes( new byte[] { (byte) Datatypes.OBJECT_DATATYPE_V3 } );
        int traitId = (int) cache.GetTraitsId( traitsClassId );
        byte[] bytes = FlashorbBinaryWriter.GetVarIntBytes( 0x1 | traitId << 2 );
        writer.DirectWriteBytes( bytes );
      }
      else
      {
        writer.BeginWriteNamedObject( className, objectFields.Count );

        if( className == null )
          cache.AddToTraitsCache( traitsClassId );

        while( en.MoveNext() )
        {
          string fieldName = en.Current.ToString();

          if( Log.isLogging( LoggingConstants.DEBUG ) )
            Log.log( LoggingConstants.DEBUG, "serializing property/field : " + fieldName );

          writer.WriteFieldName( fieldName );
        }

        en.Reset();
      }

      while( en.MoveNext() )
      {
        Object fieldName = en.Current;

        if( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "serializing property/field : " + fieldName );

        //writer.BeginWriteFieldValue();

        //try
        //{
        //MessageWriter.writeObject( objectFields[ fieldName ], writer );
        //Log.log( "REFCACHE", "WRITING FIELD " + fieldName );
        object obj = objectFields[ fieldName ];
        ITypeWriter typeWriter = MessageWriter.getWriter( obj, writer );
        typeWriter.write( obj, writer );
        //Log.log( "REFCACHE", "DONE WRITING FIELD " + fieldName );
        //}
        //catch( Exception exception )
        //{
        //	if( Log.isLogging( LoggingConstants.ERROR ) )
        //		Log.log( LoggingConstants.ERROR, "unable to serialize object's field " + fieldName, exception );
        //}
        //finally
        //{
        //	writer.EndWriteFieldValue();
        //}
      }

      writer.EndWriteNamedObject();
    }

    #endregion
  }
}
