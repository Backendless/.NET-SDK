using BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities;
using BackendlessAPI.Test.PersistenceService.Entities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.PrimitiveEntities
{
  public class DoubleEntity: CreatedEntity
  {
    public double DoubleField { get; set; }

    protected bool Equals( DoubleEntity other )
    {
      return base.Equals( other ) && DoubleField.Equals( other.DoubleField );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (DoubleEntity) obj );
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ DoubleField.GetHashCode();
      }
    }
  }
}
