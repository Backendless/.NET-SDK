using Weborb.Service;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities
{
  public abstract class ObjectIdEntityAsync 
  {
    [SetClientClassMemberName("objectId")]
    public string ObjectId { get; set; }

    protected bool Equals( ObjectIdEntityAsync other )
    {
      return string.Equals( ObjectId, other.ObjectId );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (ObjectIdEntityAsync) obj );
    }

    public override int GetHashCode()
    {
      return (ObjectId != null ? ObjectId.GetHashCode() : 0);
    }
  }
}