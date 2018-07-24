using System;
using Weborb.Service;

namespace BackendlessAPI.Test.PersistenceService.Entities.BaseEntities
{
  public abstract class CreatedEntity : ObjectIdEntity
  {
    [SetClientClassMemberName("created")]
    public DateTime? Created { get; set; }

    protected bool Equals( CreatedEntity other )
    {
      return base.Equals( other ) && Created.Equals( other.Created );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (CreatedEntity) obj );
    }

    public override int GetHashCode()
    {
      return Created.GetHashCode();
    }
  }
}