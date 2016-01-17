using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class UndefinedType : IAdaptingType
    {
    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return null;
      }

    public object defaultAdapt()
      {
      return null;
      }

    public object adapt( Type type )
      {
      if ( typeof( IAdaptingType ).IsAssignableFrom( type ) )
        return this;
      if ( typeof( DateTime ).IsAssignableFrom( type ) )
        return DateTime.MinValue;
      else
        return null;
      }

    public bool canAdaptTo( Type formalArg )
      {
      return
        typeof( IAdaptingType ).IsAssignableFrom( formalArg ) ||
        !formalArg.IsValueType ||
        typeof( DateTime ).IsAssignableFrom( formalArg );
      }

    #endregion

    public override string ToString()
      {
      return "Undefined type. Value - null";
      }

    public override bool Equals( object _obj )
      {
      return _obj is UndefinedType;
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return _obj is UndefinedType;
      }

    public override int GetHashCode()
      {
      return 0;
      }
    }
  }
