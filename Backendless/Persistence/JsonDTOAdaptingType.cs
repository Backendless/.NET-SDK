using BackendlessAPI.Utils;
using System;
using Weborb.Reader;
using System.Collections.Generic;
using Weborb.Types;
using System.Collections;
using System.Text;
using Weborb.Service;

namespace BackendlessAPI.Persistence
{
  public class JsonDTOAdaptingType : ICacheableAdaptingType
  {
    [SetClientClassMemberName("rawJsonString")]
    public String RawJsonString { get; set; }
    public bool IsAdapting { get; set; }

    public JsonDTOAdaptingType()
    {
    }

    public JsonDTOAdaptingType( String rawJsonString )
    {
      RawJsonString = rawJsonString;
    }

    public override Int32 GetHashCode() => RawJsonString.GetHashCode();

    public Object defaultAdapt()
    {
      return defaultAdapt( null );
    }

    public Object defaultAdapt( ReferenceCache refCache )
    {
      try
      {
        return adapt( typeof( Dictionary<String, Object> ), refCache );
      }
      catch
      {
        throw new System.Exception( "Unable to adapt JSON value to Dictionary" );
      }
    }

    public Object adapt( Type type, ReferenceCache refCache )
    {
      if( RawJsonString == null )
        return null;

      //Object result = JsonConvert.DeserializeObject<Dictionary<String, Object>>( RawJsonString );
      Object result = Weborb.Util.IO.Serializer.FromBytes( Encoding.UTF8.GetBytes( RawJsonString ), Weborb.Util.IO.Serializer.JSON, true );
      return ((IAdaptingType) result).adapt( type );
    }

    public IAdaptingType getCacheKey()
    {
      return this;
    }

    public Type getDefaultType()
    {
      return typeof( Dictionary<String, Object> );
    }

    public object adapt( Type type )
    {
      return adapt( type, null );
    }


    public bool canAdaptTo( Type formalArg )
    {
      return true;
    }

    public bool Equals( object obj, Dictionary<DictionaryEntry, bool> visitedPairs )
    {
      if( this == obj )
        return true;

      if( !( obj is Json ) )
        return false;

      JsonDTOAdaptingType jsonDTO = (JsonDTOAdaptingType) obj;
      return Object.Equals( RawJsonString, jsonDTO.RawJsonString );
    }
  }
}
