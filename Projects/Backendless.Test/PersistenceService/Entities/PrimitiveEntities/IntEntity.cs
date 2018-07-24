using BackendlessAPI.Test.PersistenceService.Entities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.PrimitiveEntities
{
  public class IntEntity: CreatedEntity
  {
    public int IntField { get; set; }

    protected bool Equals(IntEntity other)
    {
      return base.Equals( other ) && IntField == other.IntField;
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals((IntEntity)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ IntField;
      }
    }
  }
}
