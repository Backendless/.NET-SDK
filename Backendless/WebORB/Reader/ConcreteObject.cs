using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class ConcreteObject : IAdaptingType
    {
    private object obj;

    public ConcreteObject( object obj )
      {
      this.obj = obj;
      }

    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return obj.GetType();
      }

    public object defaultAdapt()
      {
      return obj;
      }

    public object adapt( Type type )
      {
      return obj;
      }

    public bool canAdaptTo( Type formalArg )
      {
      return obj.GetType().IsAssignableFrom( formalArg );
      }

    #endregion

    public override string ToString()
      {
      return "Concrete object - " + obj;
      }

    public override bool Equals( object _obj2 )
      {
      ConcreteObject obj2 = _obj2 as ConcreteObject;

      if ( obj2 == null )
        return false;

      if ( Object.ReferenceEquals( this, _obj2 ) )
        return true;

      return obj2.obj.Equals( obj );
      
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return Equals( _obj );
      }

    public override int GetHashCode()
      {
      if ( obj == null )
        return 0;

      return obj.GetHashCode();
      }
    }
  }