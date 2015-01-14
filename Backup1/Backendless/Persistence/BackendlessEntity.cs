using System;
using Weborb.Service;

namespace BackendlessAPI.Persistence
{
  public abstract class BackendlessEntity
  {
    [SetClientClassMemberName( "objectId" )]
    public string ObjectId { get; set; }

    [SetClientClassMemberName("created")]
    public DateTime? Created { get; set; }

    [SetClientClassMemberName("updated")]
    public DateTime? Updated { get; set; }
  }
}