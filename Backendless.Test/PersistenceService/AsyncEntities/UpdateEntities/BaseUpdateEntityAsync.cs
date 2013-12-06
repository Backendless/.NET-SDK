using BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.UpdateEntities
{
  public class BaseUpdateEntityAsync : UpdatedEntityAsync
  {
    public int Age { get; set; }
    public string Name { get; set; }

    protected bool Equals( BaseUpdateEntityAsync other )
    {
      return Age == other.Age && string.Equals(Name, other.Name) && string.Equals(ObjectId, other.ObjectId) && string.Equals(Created, other.Created);
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (BaseUpdateEntityAsync) obj );
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int hashCode = base.GetHashCode();
        hashCode = (hashCode*397) ^ Age;
        hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
        return hashCode;
      }
    }
  }
}