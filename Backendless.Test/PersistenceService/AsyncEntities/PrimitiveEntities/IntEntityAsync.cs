using System;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.PrimitiveEntities
{
  public class IntEntityAsync: CreatedEntityAsync
  {
    public int IntField { get; set; }

    protected bool Equals( IntEntityAsync other )
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
      return Equals( (IntEntityAsync) obj );
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
