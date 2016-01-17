using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Weborb.Types;

namespace Weborb.Reader
  {
  public class CacheableAdaptingTypeWrapper : ICacheableAdaptingType
    {
    private IAdaptingType realType;

    public void setType( IAdaptingType type )
      {
      this.realType = type;
      }

    #region ICacheableAdaptingType Members

    public bool IsAdapting
    {
      get;
      set;
    }

    public object defaultAdapt( ReferenceCache refCache )
      {
      if ( realType is ICacheableAdaptingType )
        return ( (ICacheableAdaptingType)realType ).defaultAdapt( refCache );
      else
        return realType.defaultAdapt();
      }

    public object adapt( Type type, ReferenceCache refCache )
      {
      if ( realType is ICacheableAdaptingType )
        return ( (ICacheableAdaptingType)realType ).adapt( type, refCache );
      else
        return realType.adapt( type );
      }

    public IAdaptingType getCacheKey()
    {
      return realType;
    }
    #endregion

    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return realType.getDefaultType();
      }

    public object defaultAdapt()
      {
      return realType.defaultAdapt();
      }

    public object adapt( Type type )
      {
      return realType.adapt( type );
      }

    public bool canAdaptTo( Type formalArg )
      {
      return realType.canAdaptTo( formalArg );
      }

    #endregion

    public override bool Equals( object _obj )
      {
      return Equals( _obj, new Dictionary<DictionaryEntry, bool>() );
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {      
      IAdaptingType obj = _obj as IAdaptingType;

      if ( obj == null )
        return false;

      if ( Object.ReferenceEquals( this, _obj ) )
        return true;

      return obj.Equals( realType, visitedPairs );
      }

    public override int GetHashCode()
      {
      return realType.GetHashCode();
      }
    }
  }