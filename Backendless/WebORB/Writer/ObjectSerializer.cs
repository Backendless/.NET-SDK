using System;
using System.Collections;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
	public class ObjectSerializer : IObjectSerializer
	{
		#region IObjectSerializer Members

		public void WriteObject( string className, IDictionary objectFields, IProtocolFormatter writer )
		{
    if ( objectFields.Contains( "serializeAsArrayMap" ) )
      {
      objectFields.Remove( "serializeAsArrayMap" );
      WriteObjectMap( objectFields, writer );
      return;
      }

		  if( className != null )
				writer.BeginWriteNamedObject( className, objectFields.Count );
			else
				//writer.BeginWriteObjectMap( objectFields.Count );
                writer.BeginWriteObject( objectFields.Count );

			IEnumerator en = objectFields.Keys.GetEnumerator();

			while( en.MoveNext() )
			{
				object fieldName = en.Current;

				if( Log.isLogging( LoggingConstants.SERIALIZATION ) )
                    Log.log( LoggingConstants.SERIALIZATION, "serializing property/field : " + fieldName );

				writer.WriteFieldName( fieldName.ToString() );
				writer.BeginWriteFieldValue();

				try
				{
					MessageWriter.writeObject( objectFields[ fieldName ], writer );
				}
				catch( Exception exception )
				{
					if( Log.isLogging( LoggingConstants.ERROR ) )
						Log.log( LoggingConstants.ERROR, "unable to serialize object's field " + fieldName, exception );
				}
				finally
				{
					writer.EndWriteFieldValue();
				}
			}
            
			if( className != null )
				writer.EndWriteNamedObject();
			else
				//writer.EndWriteObjectMap();
                writer.EndWriteObject();
		}

    private void WriteObjectMap( IDictionary objectFields, IProtocolFormatter writer )
      {
      int maxInt = -1;
      for ( int i = 0; i < objectFields.Count; i++ )
        {
        if ( !objectFields.Contains( i ) )
            break;          

        maxInt = i;
        }

      writer.BeginWriteObjectMap( maxInt + 1 );        

      IEnumerator en = objectFields.Keys.GetEnumerator();

      while ( en.MoveNext() )
        {
        object fieldName = en.Current;

        if ( fieldName.Equals( "length" ) )
          continue;

        if ( Log.isLogging( LoggingConstants.SERIALIZATION ) )
          Log.log( LoggingConstants.SERIALIZATION, "serializing property/field : " + fieldName );

          writer.WriteFieldName( fieldName.ToString() );
          writer.BeginWriteFieldValue();

          MessageWriter.writeObject( objectFields[ fieldName ], writer );

          writer.EndWriteFieldValue();          
        }

        if ( maxInt >= 0 )
          {
          writer.WriteFieldName( "length" );
          MessageWriter.writeObject( maxInt + 1, writer );
          }

        writer.EndWriteObjectMap();        
      }

		#endregion
	}
}
