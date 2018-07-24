using System;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.PrimitiveEntities
{
  public class BooleanEntityAsync: CreatedEntityAsync 
  {
    public bool BooleanField { get; set; }

    protected bool Equals( BooleanEntityAsync other )
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
      return Equals( (BooleanEntityAsync) obj );
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
