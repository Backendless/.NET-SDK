using BackendlessAPI.Test.PersistenceService.AsyncEntities.FindEntities;

namespace BackendlessAPI.Test.PersistenceService.Entities.FindEntities
{
  public class FindWithPageSizeAndOffsetEntity : BaseFindEntity
  {
    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (FindWithPageSizeAndOffsetEntity) obj );
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
