using System;

namespace BackendlessAPI.Test.PersistenceService.Entities
{
  public class UniqueWPPerson : WPPerson
  {
    public DateTime Birthday { get; set; }

    protected bool Equals( UniqueWPPerson other )
    {
      return Birthday.Equals( other.Birthday );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (UniqueWPPerson) obj );
    }

    public override int GetHashCode()
    {
      return Birthday.GetHashCode();
    }
  }
}