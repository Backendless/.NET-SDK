﻿using System;
using System.Collections.Generic;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util;

namespace BackendlessAPI.Geo
{
  class BackendlessGeometryFactory : IArgumentObjectFactory
  {
    public Object createObject( IAdaptingType iAdaptingType)
    {
      if( iAdaptingType is NamedObject)
        iAdaptingType = ((NamedObject) iAdaptingType).TypedObject;

      if ( iAdaptingType.GetType() == typeof( NullType ) )
        return null;

      if ( iAdaptingType is AnonymousObject )
      {
        Dictionary<string, object> properties = ( Dictionary<string, object> )iAdaptingType.defaultAdapt();
        String geoJson = ( String )properties["geoJson"];

        if ( geoJson == null )
          return null;

        String geomClass = ( String )properties["geomClass"];
        int srsId = ( int )properties["srsId"];

        Geometry geometry = new GeometryDTO( geomClass, srsId, geoJson ).ToGeometry<Geometry>();

        return geometry;
      }
        else
          throw new System.Exception( "Uknown type" );
    }
    public bool canAdapt( IAdaptingType iAdaptingType )
    {
      return false;
    }
  }
}