using System;
using Weborb.Types;
using Weborb.Reader;
using Weborb.Util.IO;

namespace BackendlessAPI.Utils
{
  public class WeborbSerializationHelper
  {
    internal static Object[] Serialize( Object arg )
    {
      return new object[] { Serializer.ToBytes( arg, Serializer.JSON ) };
    }

    internal static IAdaptingType Deserialize( byte[] arg )
    {
      IAdaptingType adaptingType = (IAdaptingType) Serializer.FromBytes( arg, Serializer.JSON, true );

      if( adaptingType is CacheableAdaptingTypeWrapper )
        return ((CacheableAdaptingTypeWrapper) adaptingType).getType();
      else
        return adaptingType;
    }

    internal static AnonymousObject Cast( IAdaptingType obj )
    {
      AnonymousObject anonymousObject;
      if( obj is AnonymousObject )
      {
        anonymousObject = (AnonymousObject) obj;
      }
      else if( obj is CacheableAdaptingTypeWrapper )
      {
        anonymousObject = (AnonymousObject) ((CacheableAdaptingTypeWrapper) obj).getType();
      }
      else
      {
        throw new System.Exception( "Object must be of or contain the AnonymousObject type" );
      }
      return anonymousObject;
    }

    internal static String AsString( IAdaptingType obj, String key )
    {
      AnonymousObject anonymousObject = Cast( obj );
      return AsString( anonymousObject, key );
    }

    internal static String AsString( AnonymousObject obj, String key )
    {
      IAdaptingType adaptingType = (IAdaptingType) obj.Properties[ key ];
      return adaptingType == null ? null : (String) adaptingType.adapt( typeof( String ) );
    }

    internal static Object AsObject( AnonymousObject obj, String key )
    {
      IAdaptingType adaptingType = (IAdaptingType) obj.Properties[ key ];
      return adaptingType == null ? null : adaptingType.defaultAdapt();
    }

    internal static IAdaptingType AsAdaptingType( AnonymousObject obj, String key )
    {
      return (IAdaptingType) obj.Properties[ key ];
    }

    internal static IAdaptingType AsAdaptingType( IAdaptingType obj, String key )
    {
      AnonymousObject anonymousObject = Cast( obj );
      return (IAdaptingType) anonymousObject.Properties[ key ];
    }
  }
}
