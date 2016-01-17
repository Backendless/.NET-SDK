using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Writer.Amf;
using Weborb.Config;
using Weborb.Types;
using Weborb.Util;

namespace Weborb.Writer
{
  class V3VectorWriter<T> : AbstractUnreferenceableTypeWriter
  {
    private ITypeWriter referenceWriter = new ArrayReferenceWriter();

    public override void write( object obj, IProtocolFormatter formatter )
    {
      AmfV3Formatter amfV3Formatter = ( (AmfV3Formatter) formatter );
      ICollection<T> ar = (ICollection<T>) obj;
      Type collectionType = typeof( T );
  
      // write Vector AMF3 marker
      if( VectorUtils.isIntType( collectionType ) )
        amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.INT_VECTOR_V3 );
      else if( VectorUtils.isUIntType( collectionType ) )
        amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.UINT_VECTOR_V3 );
      else if( VectorUtils.isNumberType( collectionType ) )
        amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.DOUBLE_VECTOR_V3 );
      else
        amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.OBJECT_VECTOR_V3 );

      // get length
      int handle = ar.Count;
      handle = handle << 1;
      handle = handle | 1;

      // write vector length and nonmutability
      amfV3Formatter.WriteVarIntWithoutMarker( handle & 0x1fffffff );

      bool readOnly = true;

      try
      {
        readOnly = ar.IsReadOnly;
      }
      catch( Exception )
      {
      }

      amfV3Formatter.WriteVarIntWithoutMarker( readOnly ? 1 : 0 );

      // write element types if necessary
      if( typeof( T ) == typeof( string ) || typeof( T ) == typeof( bool ) )
        amfV3Formatter.WriteString( "", false ); // TODO: ensure this empty string is necessary
      else if( !typeof( T ).IsValueType || ( typeof( T ).IsGenericType &&
                typeof( Nullable<> ).IsAssignableFrom( typeof( T ).GetGenericTypeDefinition() ) ) )
        amfV3Formatter.WriteString( typeof( T ).FullName, false );

      // write elements
      foreach( T element in ar )
      {
        if( VectorUtils.isIntType( collectionType ) )
          amfV3Formatter.WriteUncompressedInteger( (int) Convert.ChangeType( element, typeof( int ) ) );
        else if( VectorUtils.isUIntType( collectionType ) )
          amfV3Formatter.WriteUncompressedUInteger( (uint) Convert.ChangeType( element, typeof( uint ) ) );
        else if( VectorUtils.isNumberType( collectionType ) )
          amfV3Formatter.WriteDouble( (double) Convert.ChangeType( element, typeof( double ) ), false );
        else
          MessageWriter.writeObject( element, amfV3Formatter );
      }
    }
  }
}
