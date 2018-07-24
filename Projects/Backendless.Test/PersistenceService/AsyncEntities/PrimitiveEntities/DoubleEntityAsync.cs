using System;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.PrimitiveEntities
{
  public class DoubleEntityAsync: CreatedEntityAsync 
  {
    public double DoubleField { get; set; }

    protected bool Equals( DoubleEntityAsync other )
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
      return Equals( (DoubleEntityAsync) obj );
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
