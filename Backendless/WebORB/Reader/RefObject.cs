using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.Reader
{
  public class RefObject : ICacheableAdaptingType
  {

    public int Id;
    public IAdaptingType Object;

    public RefObject( int refId, IAdaptingType refObject )
    {
      Id = refId;
      Object = refObject;
    }

    public bool IsAdapting { get; set; }

    #region IAdaptingType Members

    public Type getDefaultType()
    {
      return Object != null? Object.getDefaultType() : null;
    }

    public object defaultAdapt()
    {
      return Object != null? Object.defaultAdapt() : null;
    }

    public object adapt( Type type )
    {
      return Object != null? Object.adapt( type ) : null;
    }

    public object defaultAdapt( ReferenceCache refCache )
    {
      if( Object != null )
      {
        bool isCachable = Object is ICacheableAdaptingType;
        IAdaptingType cacheKey = isCachable ? ( Object as ICacheableAdaptingType ).getCacheKey() : Object;

        if( refCache.HasObject( cacheKey ) )
          return refCache.GetObject( cacheKey );

        return isCachable? (Object as ICacheableAdaptingType).defaultAdapt(refCache) : Object.defaultAdapt();
      }

      return null;
    }

    public object adapt( Type type, ReferenceCache refCache )
    {
      if( Object != null )
      {
        bool isCachable = Object is ICacheableAdaptingType;
        IAdaptingType cacheKey = isCachable ? ( Object as ICacheableAdaptingType ).getCacheKey() : Object;

        if( refCache.HasObject( cacheKey, type ) )
          return refCache.GetObject( cacheKey, type );

        return isCachable ? ( Object as ICacheableAdaptingType ).adapt( type, refCache ) : Object.adapt( type );
      }
      
      return null;
    }

    public IAdaptingType getCacheKey()
    {
      return Object;
    }

    public bool canAdaptTo( Type formalArg )
    {
      return Object != null? Object.canAdaptTo( formalArg ) : false;
    }

    #endregion

    public override string ToString()
    {
      return string.Concat( "Ref type. ID - ", Id, ", Type - " + getDefaultType() );
    }

    public override bool Equals( object _obj )
    {
      return Object != null? Object.Equals( _obj ) : _obj == null;
    }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
    {
      return Equals( _obj );
    }

    public override int GetHashCode()
    {
      return Object != null? Object.GetHashCode() : 0;
    }
  }
}
