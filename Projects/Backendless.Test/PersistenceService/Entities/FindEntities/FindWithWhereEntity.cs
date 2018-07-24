using BackendlessAPI.Test.PersistenceService.AsyncEntities.FindEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.FindEntities
{
  public class FindWithWhereEntity : BaseFindEntity
  {
    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (FindWithWhereEntity) obj );
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
