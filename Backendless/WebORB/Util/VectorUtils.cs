using System;
using System.Collections.Generic;
using Weborb.Config;
using Weborb.Types;

namespace Weborb.Util
{
  class VectorUtils
  {
    public static bool IsVector( Object obj )
    {
      if( obj == null )
        return false;

      Type objectType = obj.GetType();
      SerializationConfigHandler serializationConfig = (SerializationConfigHandler) ORBConfig.GetInstance().GetConfig( "weborb/serialization" );

      return objectType.IsGenericType &&
             typeof( ICollection<> ).MakeGenericType( objectType.GetGenericArguments()[ 0 ] ).IsAssignableFrom( objectType ) &&
             ( serializationConfig.SerializeGenericCollectionAsVector ||
               typeof( IWebORBVector<> ).MakeGenericType( objectType.GetGenericArguments()[ 0 ] ).IsAssignableFrom( objectType ) );
    }

    internal static bool isNumberType( Type T )
    {
      return T == typeof( double ) ||
             T == typeof( float ) ||
             T == typeof( long ) ||
             T == typeof( ulong ) ||
             T == typeof( Int64 ) ||
             T == typeof( UInt64 ) ||
             typeof( Nullable<> ).IsAssignableFrom( T ); // TODO: test this
    }

    internal static bool isIntType( Type T )
    {
      return T == typeof( int ) ||
             T == typeof( byte ) ||
             T == typeof( sbyte ) ||
             T == typeof( short ) ||
             T == typeof( ushort );
    }

    internal static bool isUIntType( Type T )
    {
      return T == typeof( uint );
    }
  }
}
