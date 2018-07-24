using System;
using BackendlessAPI.Test.PersistenceService.AsyncEntities.BaseEntities;

namespace BackendlessAPI.Test.PersistenceService.AsyncEntities.PrimitiveEntities
{
  public class StringEntityAsync : CreatedEntityAsync
  {
    public string StringField { get; set; }

    protected bool Equals( StringEntityAsync other )
    {
      return base.Equals( other ) && string.Equals( StringField, other.StringField );
    }

    public override bool Equals( object obj )
    {
      if( ReferenceEquals( null, obj ) )
        return false;
      if( ReferenceEquals( this, obj ) )
        return true;
      if( obj.GetType() != this.GetType() )
        return false;
      return Equals( (StringEntityAsync) obj );
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (base.GetHashCode()*397) ^ (StringField != null ? StringField.GetHashCode() : 0);
      }
    }
  }
}
