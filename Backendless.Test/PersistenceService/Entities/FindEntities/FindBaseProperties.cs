namespace BackendlessAPI.Test.PersistenceService.Entities.FindEntities
{
  public class FindBaseProperties : BaseFindEntity
  {
    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return base.Equals((FindBaseProperties)obj);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}