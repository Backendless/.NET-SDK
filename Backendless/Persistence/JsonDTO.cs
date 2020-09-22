using System;

namespace BackendlessAPI.Persistence
{
  public class JsonDTO
  {
    private string rawJsonString;

    public JsonDTO( string rawJsonString )
    {
      this.rawJsonString = rawJsonString;
    }

    public string RawJsonString
    {
      get { return rawJsonString; }
    }

    public override bool Equals( object o )
    {
      if( this == o )
        return true;
      
      if( !(o is JsonDTO) )
      return false;
      
      JsonDTO jsonDTO = (JsonDTO) o;
      
      return Object.Equals( rawJsonString, jsonDTO.rawJsonString );
    }
    
    public override int GetHashCode()
    {
      return rawJsonString.GetHashCode();
    }
  }

}