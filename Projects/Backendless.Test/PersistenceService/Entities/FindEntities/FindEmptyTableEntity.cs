using BackendlessAPI.Test.PersistenceService.AsyncEntities.FindEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.FindEntities
{
  public class FindEmptyTableEntity : BaseFindEntity
  {
    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (FindEmptyTableEntity) obj );
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}