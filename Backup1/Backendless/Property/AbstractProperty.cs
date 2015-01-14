using Weborb.Service;

namespace BackendlessAPI.Property
{
  public abstract class AbstractProperty
  {
    [SetClientClassMemberName( "name" )]
    public string Name { get; set; }

    [SetClientClassMemberName( "required" )]
    public bool IsRequired { get; set; }

    [SetClientClassMemberName( "selected" )]
    public bool IsSelected { get; set; }

    [SetClientClassMemberName( "type" )]
    public DateTypeEnum Type { get; set; }

    [SetClientClassMemberName( "defaultValue" )]
    public object DefaultValue { get; set; }
  }
}
