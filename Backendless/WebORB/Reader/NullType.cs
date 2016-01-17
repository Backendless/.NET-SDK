using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class NullType : IAdaptingType
    {
    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return typeof( object );
      }

    public object defaultAdapt()
      {
      return null;
      }

    public object adapt( Type type )
      {
      if ( type.Equals( typeof( IAdaptingType ) ) )
        return this;
      else if ( typeof( DateTime ).IsAssignableFrom( type ) )
        return DateTime.MinValue;
      else
        return null;
      }

    public bool canAdaptTo( Type formalArg )
      {
      return
        typeof( IAdaptingType ).IsAssignableFrom( formalArg ) ||
        !formalArg.IsValueType ||
        typeof( DateTime ).IsAssignableFrom( formalArg ) ||
                formalArg.IsGenericType && formalArg.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) );
      }

    #endregion

    public override string ToString()
      {
      return "Null type. Value - null";
      }

    public override bool Equals( object _obj )
      {
      return _obj is NullType;
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return _obj is NullType;
      }

    public override int GetHashCode()
      {
      return 0;
      }
    }
  }
