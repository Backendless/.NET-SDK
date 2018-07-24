
using BackendlessAPI.Test.PersistenceService.Entities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.PrimitiveEntities
{
  public class BooleanEntity: CreatedEntity
  {
    public bool BooleanField { get; set; }

    protected bool Equals( BooleanEntity other )
    {
      return base.Equals( other ) && BooleanField.Equals( other.BooleanField );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (BooleanEntity) obj );
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ BooleanField.GetHashCode();
      }
    }
  }
}
