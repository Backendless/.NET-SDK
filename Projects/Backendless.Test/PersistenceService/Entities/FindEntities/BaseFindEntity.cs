namespace BackendlessAPI.Test.PersistenceService.Entities.FindEntities
{
  public class BaseFindEntity : WPPerson
  {
    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals((BaseFindEntity)obj);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}