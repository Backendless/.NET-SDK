using System;
using Weborb.Service;

namespace BackendlessAPI.Geo
{
    public class SearchMatchesResult
    {
        [SetClientClassMemberName("matches")]
        public Double Matches { get; set; }

        [SetClientClassMemberName("geoPoint")]
        public GeoPoint GeoPoint { get; set; }
    }
}
