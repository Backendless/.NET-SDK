using System;
using Weborb.Service;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities
{
  public abstract class UpdatedEntityAsync : CreatedEntityAsync
  {
    [SetClientClassMemberName("updated")]
    public DateTime? Updated { get; set; }

    protected bool Equals( UpdatedEntityAsync other )
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
      return Equals( (UpdatedEntityAsync) obj );
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