namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.FindEntities
{
  public class FindEmptyTableEntityAsync : BaseFindEntityAsync
  {
    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (FindEmptyTableEntityAsync) obj );
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}