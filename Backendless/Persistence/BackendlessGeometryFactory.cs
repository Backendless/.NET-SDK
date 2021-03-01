using System;
using System.Collections.Generic;
using System.Linq;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util;

namespace BackendlessAPI.Persistence
{
  class BackendlessGeometryFactory : IArgumentObjectFactory
  {
    public Object createObject( IAdaptingType iAdaptingType )
    {
      if( iAdaptingType is NamedObject )
        iAdaptingType = ((NamedObject) iAdaptingType).TypedObject;

      if( iAdaptingType.GetType() == typeof( NullType ) )
        return null;

      /*ReferenceCache refCache = ReferenceCache.GetInstance();

      if( refCache.HasObject( iAdaptingType, typeof( GeometryDTO ) ) )
        return refCache.GetObject( iAdaptingType, typeof( GeometryDTO ) );*/

      if( iAdaptingType is AnonymousObject )
      {
        Dictionary<Object, Object> properties = (Dictionary<Object, Object>) iAdaptingType.defaultAdapt();
        String geoJson = properties.TryGetValue( "geoJson", out _ ) ? (String) properties[ "geoJson" ] : null;
        String dotnetType = properties.TryGetValue( "___class", out _ ) ? (String) properties[ "___class" ] : null;

        if( geoJson == null )
        {
          if( dotnetType == null )
            return null;
          else
          {
            Dictionary<String, Object> tempProperties = properties.ToDictionary( k => k.Key.ToString(), k => k.Value );
            return new GeoJSONParser<Geometry>().Read( tempProperties );
          }
        }

        String geomClass = (String) properties[ "geomClass" ];
        int srsId = (int) properties[ "srsId" ];
        Geometry geometry = new GeometryDTO( geomClass, srsId, geoJson ).ToGeometry<Geometry>();

        return geometry;
      }
      else if( iAdaptingType is StringType )
      {
        String wkt = ( (StringType) iAdaptingType ).Value;
        return new WKTParser().Read( wkt );
      }
      else
        throw new System.Exception( "Can not create BackendlessGeometry from type " + iAdaptingType.GetType().Name );
    }
    public bool canAdapt( IAdaptingType iAdaptingType )
    {
      return false;
    }
  }
}
