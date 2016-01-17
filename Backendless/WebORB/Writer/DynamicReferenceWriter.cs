using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Weborb.Types;
using Weborb.Util;
using Weborb.Writer.Amf;

namespace Weborb.Writer
{
  class DynamicReferenceWriter : ITypeWriter
  {
    public void write( object obj, IProtocolFormatter formatter )
    {
      ReferenceCache referenceCache = formatter.GetReferenceCache();
      int refId = referenceCache.GetObjectId( obj );

      if( refId != -1 )
      {
#if FULL_BUILD
        if( VectorUtils.IsVector( obj ) && formatter is AmfV3Formatter )
        {
          Type collectionType = obj.GetType();
          AmfV3Formatter amfV3Formatter = (AmfV3Formatter) formatter;

          if( VectorUtils.isIntType( collectionType ) )
            amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.INT_VECTOR_V3 );
          else if( VectorUtils.isUIntType( collectionType ) )
            amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.UINT_VECTOR_V3 );
          else if( VectorUtils.isNumberType( collectionType ) )
            amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.DOUBLE_VECTOR_V3 );
          else
            amfV3Formatter.WriteVarIntWithoutMarker( Datatypes.OBJECT_VECTOR_V3 );

          amfV3Formatter.WriteVarIntWithoutMarker( refId << 1 );
        }
        else 
#endif
        if( obj is IWebORBArrayCollection || obj is IDictionary )
        {
          formatter.WriteObjectReference( refId );
        }
        else if( obj is ICollection || obj is Array || obj is IWebORBArray )
        {
          formatter.WriteArrayReference( refId );
        }
      }
      else
      {
        referenceCache.AddObject( obj );
        formatter.getContextWriter().write( obj, formatter );
      }
    }

    public ITypeWriter getReferenceWriter()
    {
      return null;
    }
  }
}
