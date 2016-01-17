using System;
using System.Collections;
using System.Collections.Generic;

using Weborb.Config;
using Weborb.Util;
using Weborb.Types;

namespace Weborb.Writer
{
  public class CollectionWriter : ITypeWriter
  {
    private ITypeWriter referenceWriter = new DynamicReferenceWriter();

    #region ITypeWriter Members

    public void write( object obj, IProtocolFormatter writer )
    {
      ITypeWriter typeWriter;

      if ( obj is IDictionary )
      {
        typeWriter = MessageWriter.getWriter( typeof( IDictionary ), writer, false );
        typeWriter.write( obj, writer );
        return;
      }

      /*
if( obj is Array )
{
  base.write( obj, writer );
  return;
}*/

      object[] array = null;

#if( FULL_BUILD)
      

      if ( VectorUtils.IsVector( obj ) )
      {
        Type objectType = obj.GetType();
        Type elementType = objectType.GetGenericArguments()[0];
        Type vectorType = typeof( V3VectorWriter<> ).MakeGenericType( elementType );
        typeWriter = (ITypeWriter)vectorType.GetConstructor( new Type[0] ).Invoke( new object[0] );
        typeWriter.write( obj, writer );
        return;
      }

      if ( obj is ICollection )
      {
        ICollection coll = (ICollection) obj;
        SerializationConfigHandler serializationConfig = (SerializationConfigHandler) ORBConfig.GetInstance().GetConfig( "weborb/serialization" );
        
        if ( !serializationConfig.LegacyCollectionSerialization && !(obj is IWebORBArray ) )
        {
          typeWriter = MessageWriter.getWriter( typeof( IWebORBArrayCollection ), writer, false );
          typeWriter.write( new WebORBArrayCollection( coll ), writer );
          return;
        }
        else
        {
          array = new object[ coll.Count ];
          coll.CopyTo( array, 0 );
        }
      }
      else
      {
#endif
        IEnumerable collection = (IEnumerable) obj;
        IEnumerator enumerator = collection.GetEnumerator();
        List<Object> arrayList = new List<Object>();

        while ( enumerator.MoveNext() )
          arrayList.Add( enumerator.Current );

        array = arrayList.ToArray();
#if( FULL_BUILD)
      }
#endif

      typeWriter = MessageWriter.getWriter( array.GetType(), writer, false );
      typeWriter.write( array, writer );
      //base.write( array, writer );
    }



    public ITypeWriter getReferenceWriter()
    {
      return referenceWriter;
    }

    #endregion
  }
}
