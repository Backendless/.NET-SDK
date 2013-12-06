using BackendlessAPI.Test.PersistenceService.Entities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.PrimitiveEntities
{
  public class StringEntity : CreatedEntity
  {
    public string StringField { get; set; }

    protected bool Equals( StringEntity other )
    {
      return base.Equals( other ) && string.Equals( StringField, other.StringField );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (StringEntity) obj );
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ (StringField != null ? StringField.GetHashCode() : 0);
      }
    }
  }
}
