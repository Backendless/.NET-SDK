using System;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.PrimitiveEntities
{
  public class DateEntityAsync: CreatedEntityAsync
  {
    public DateTime DateField { get; set; }

    protected bool Equals( DateEntityAsync other )
    {
      return base.Equals( other ) && DateField.Equals( other.DateField );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (DateEntityAsync) obj );
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ DateField.GetHashCode();
      }
    }
  }
}
