namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.FindEntities
{
  public class BaseFindEntityAsync : WPPersonAsync
  {
    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (BaseFindEntityAsync) obj );
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}