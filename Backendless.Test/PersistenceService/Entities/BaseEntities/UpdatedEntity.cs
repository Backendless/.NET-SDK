using System;
using Weborb.Service;

namespace BackendlessAPI.Test.PersistenceService.Entities.BaseEntities
{
  public abstract class UpdatedEntity : CreatedEntity
  {
    [SetClientClassMemberName("updated")]
    public DateTime? Updated { get; set; }

    protected bool Equals( UpdatedEntity other )
    {
      return base.Equals( other ) && Updated.Equals( other.Updated );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (UpdatedEntity) obj );
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ Updated.GetHashCode();
      }
    }
  }
}