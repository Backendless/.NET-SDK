using Weborb.Service;

namespace BackendlessAPI.Property
{
  public class UserProperty : AbstractProperty
  {
    [SetClientClassMemberName( "identity" )]
    public bool IsIdentity { get; set; }
  }
}