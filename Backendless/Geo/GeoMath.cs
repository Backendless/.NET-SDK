using System;
using System.Collections.Generic;

namespace BackendlessAPI.Geo
{
  class GeoMath
  {
    public const double EARTH_RADIUS = 6378100.0; //meters

    public static double Distance( double lat1, double lon1, double lat2, double lon2 )
    {
      double deltaLon = lon1 - lon2;

      deltaLon = ( deltaLon * Math.PI ) / 180;
      lat1 = ( lat1 * Math.PI ) / 180;
      lat2 = ( lat2 * Math.PI ) / 180;

      return EARTH_RADIUS * Math.Acos( Math.Sin( lat1 ) * Math.Sin( lat2 ) + Math.Cos( lat1 ) * Math.Cos( lat2 ) * Math.Cos( deltaLon ) );
    }

    // for circle
    public static double[] GetOutRectangle( double latitude, double longitude, double r )
    {
      double boundLat = latitude + ( 180 * r ) / ( Math.PI * EARTH_RADIUS ) * ( latitude > 0 ? 1 : -1 );
      double littleRadius = CountLittleRadius( boundLat );
      double westLong, eastLong, northLat, southLat;

      if( littleRadius > r )
      {
        westLong = longitude - ( 180 * r ) / littleRadius;
        eastLong = 2 * longitude - westLong;

        westLong = UpdateDegree( westLong );
        eastLong = eastLong % 360 == 180 ? 180 : UpdateDegree( eastLong );
      }
      else
      {
        westLong = -180;
        eastLong = 180;
      }

      if( latitude > 0 )
      {
        northLat = boundLat;
        southLat = 2 * latitude - boundLat;
      }
      else
      {
        southLat = boundLat;
        northLat = 2 * latitude - boundLat;
      }

      return new double[] { Math.Min( northLat, 90 ), westLong, Math.Max( southLat, -90 ), eastLong };
    }

    private static double CountLittleRadius( double latitude )
    {
      double h = Math.Abs( latitude ) / 180 * EARTH_RADIUS;
      double diametre = 2 * EARTH_RADIUS;
      double l_2 = ( Math.Pow( diametre, 2 ) - diametre * Math.Sqrt( Math.Pow( diametre, 2 ) - 4 * Math.Pow( h, 2 ) ) ) / 2;
      double littleRadius = diametre / 2 - Math.Sqrt( l_2 - Math.Pow( h, 2 ) );

      return littleRadius;
    }

    public static double[] GetOutRectangle( GeoPoint center, GeoPoint bounded )
    {
      return GetOutRectangle( center.Latitude, center.Longitude, Distance( center.Latitude, center.Longitude, bounded.Latitude, bounded.Longitude ) );
    }

    // for shape
    public static double[] GetOutRectangle( List<GeoPoint> geoPoints )
    {
      GeoPoint geoPoint = geoPoints[ 0 ];
      double nwLat = geoPoint.Latitude;
      double nwLon = geoPoint.Longitude;
      double seLat = geoPoint.Latitude;
      double seLon = geoPoint.Longitude;
      double minLon = 0, maxLon = 0, lon = 0;

      for( int i = 1; i < geoPoints.Count; i++ )
      {
        if( geoPoints[ i ].Latitude > nwLat )
          nwLat = geoPoints[ i ].Latitude;

        if( geoPoints[ i ].Latitude < seLat )
          seLat = geoPoints[ i ].Latitude;

        double deltaLon = geoPoints[ i ].Longitude - geoPoints[ i - 1 ].Longitude;

        if( deltaLon < 0 && deltaLon > -180 || deltaLon > 270 )
        {
          if( deltaLon > 270 )
            deltaLon -= 360;
          lon += deltaLon;

          if( lon < minLon )
            minLon = lon;
        }
        else if( deltaLon > 0 && deltaLon <= 180 || deltaLon <= -270 )
        {
          if( deltaLon <= -270 )
            deltaLon += 360;
          lon += deltaLon;

          if( lon > maxLon )
            maxLon = lon;
        }
      }
      nwLon += minLon;
      seLon += maxLon;

      if( seLon - nwLon >= 360 )
      {
        seLon = 180;
        nwLon = -180;
      }
      else
      {
        seLon = UpdateDegree( seLon );
        nwLon = UpdateDegree( nwLon );
      }

      return new double[] { nwLat, nwLon, seLat, seLon };
    }

    public static double UpdateDegree( double degree )
    {
      degree += 180;
      while( degree < 0 )
      {
        degree += 360;
      }
      return degree == 0 ? 180 : degree % 360 - 180;
    }

    public static bool IsPointInCircle( GeoPoint point, GeoPoint center, double radius )
    {
      return Distance( point.Latitude, point.Longitude, center.Latitude, center.Longitude ) <= radius;
    }

    public static bool IsPointInRectangular( GeoPoint point, GeoPoint nwPoint, GeoPoint sePoint )
    {
      if( point.Latitude > nwPoint.Latitude || point.Latitude < sePoint.Latitude )
        return false;

      if( nwPoint.Longitude > sePoint.Longitude )
        return point.Longitude >= nwPoint.Longitude || point.Longitude <= sePoint.Longitude;
      else
        return point.Longitude >= nwPoint.Longitude && point.Longitude <= sePoint.Longitude;
    }

    public static bool IsPointInShape( GeoPoint point, List<GeoPoint> shape )
    {
      int count = 0;

      for( int i = 0; i < shape.Count; i++ )
      {
        PointPosition position = GetPointPosition( point, shape[ i ], shape[ ( i + 1 ) % shape.Count ] );
        switch( position )
        {
          case PointPosition.INTERSECT:
            count++;
            break;

          case PointPosition.ON_LINE:
          case PointPosition.NO_INTERSECT:
          default:
            break;
        }
      }

      return count % 2 == 1;
    }

    private static PointPosition GetPointPosition( GeoPoint point, GeoPoint first, GeoPoint second )
    {
      double delta = second.Longitude - first.Longitude;
      if( delta < 0 && delta > -180 || delta > 180 )
      {
        GeoPoint tmp = first;
        first = second;
        second = tmp;
      }

      if( point.Latitude < first.Latitude == point.Latitude < second.Latitude )
        return PointPosition.NO_INTERSECT;

      double x = point.Longitude - first.Longitude;

      if( x < 0 && x > -180 || x > 180 )
        x = ( x - 360 ) % 360;

      double x2 = ( second.Longitude - first.Longitude + 360 ) % 360;
      double result = x2 * ( point.Latitude - first.Latitude ) / ( second.Latitude - first.Latitude ) - x;

      if( result > 0 )
        return PointPosition.INTERSECT;

      return PointPosition.NO_INTERSECT;
    }

    private enum PointPosition
    {
      ON_LINE, INTERSECT, NO_INTERSECT
    }
  }
}
