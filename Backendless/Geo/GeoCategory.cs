using Weborb.Service;

namespace BackendlessAPI.Geo
{
    public class GeoCategory
    {
      [SetClientClassMemberName( "objectId" )]
      public string Id { get; set; }

      [SetClientClassMemberName( "name" )]
      public string Name { get; set; }

      [SetClientClassMemberName( "size" )]
      public int Size { get; set; }
    }
}
