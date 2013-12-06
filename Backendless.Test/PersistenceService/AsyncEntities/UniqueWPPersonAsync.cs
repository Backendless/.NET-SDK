using System;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities
{
  public class UniqueWPPersonAsync : WPPersonAsync
  {
    public DateTime Birthday { get; set; }

    public bool Equals( UniqueWPPersonAsync other )
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
      return Equals( (UniqueWPPersonAsync) obj );
    }

    public override int GetHashCode()
    {
      return Birthday.GetHashCode();
    }
  }
}