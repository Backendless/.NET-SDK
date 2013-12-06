using System;
using Weborb.Service;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities
{
  public abstract class CreatedEntityAsync : ObjectIdEntityAsync
  {
    [SetClientClassMemberName("created")]
    public DateTime? Created { get; set; }

    protected bool Equals( CreatedEntityAsync other )
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
      return Equals( (CreatedEntityAsync) obj );
    }

    public override int GetHashCode()
    {
      return Created.GetHashCode();
    }
  }
}