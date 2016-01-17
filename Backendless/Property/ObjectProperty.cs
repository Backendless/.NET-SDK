using Weborb.Service;

namespace BackendlessAPI.Property
{
  public class ObjectProperty : AbstractProperty
  {
    public ObjectProperty()
    {
    }

    public ObjectProperty( string name )
    {
      Name = name;
    }

    public ObjectProperty( string name, DateTypeEnum type, bool required )
    {
      Name = name;
      Type = type;
      IsRequired = required;
    }

    [SetClientClassMemberName( "autoLoad" )]
    public bool AutoLoad { get; set; }

    [SetClientClassMemberName( "relatedTable" )]
    public string RelatedTable { get; set; }

    [SetClientClassMemberName( "customRegex" )]
    public string CustomRegex { get; set; }

    [SetClientClassMemberName( "primaryKey" )]
    public bool PrimaryKey { get; set; }
  }
}