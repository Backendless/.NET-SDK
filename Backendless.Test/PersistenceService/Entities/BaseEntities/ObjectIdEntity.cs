using Weborb.Service;

namespace BackendlessAPI.Test.PersistenceService.Entities.BaseEntities
{
  public abstract class ObjectIdEntity
  {
    [SetClientClassMemberName("objectId")]
    public string ObjectId { get; set; }

    protected bool Equals(ObjectIdEntity other)
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
      return Equals((ObjectIdEntity)obj);
    }

    public override int GetHashCode()
    {
      return (ObjectId != null ? ObjectId.GetHashCode() : 0);
    }
  }
}