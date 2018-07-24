using System;
using BackendlessAPI.Test.PersistenceService.Entities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.PrimitiveEntities
{
  public class DateEntity: CreatedEntity
  {
    public DateTime DateField { get; set; }

    protected bool Equals( DateEntity other )
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
      return Equals( (DateEntity) obj );
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
