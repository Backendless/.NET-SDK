using System;
using System.Collections.Generic;
using Weborb.Service;

namespace BackendlessAPI.Geo.Fence
{
  public class GeoFence
  {
    [SetClientClassMemberName( "objectId" )]
    public String ObjectId { get; set; }

    [SetClientClassMemberName( "geofenceName" )]
    public String GeofenceName { get; set; }

    [SetClientClassMemberName( "onStayDuration" )]
    public long OnStayDuration { get; set; }

    [SetClientClassMemberName( "type" )]
    public FenceType Type { get; set; }

    [SetClientClassMemberName( "nodes" )]
    public List<GeoPoint> Nodes { get; set; }

    [SetClientClassMemberName( "nwPoint" )]
    public GeoPoint NWPoint { get; set; }

    [SetClientClassMemberName( "sePoint" )]
    public GeoPoint SEPoint { get; set; }

    public GeoFence()
    {
    }

    internal GeoFence( String geoFenceName )
    {
      GeofenceName = geoFenceName;
    }
  }
}
