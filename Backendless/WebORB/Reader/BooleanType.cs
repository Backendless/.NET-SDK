using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class BooleanType : IAdaptingType
    {
    private bool boolean;
    //private Type booleanType = typeof( Boolean );
    //private static Type stringType = typeof( string );

    public BooleanType( bool boolean )
      {
      this.boolean = boolean;
      }

    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return typeof( Boolean );
      }

    public object defaultAdapt()
      {
      return boolean;
      }

    public object adapt( Type type )
      {
      if ( type.Equals( typeof( IAdaptingType ) ) )
        return this;
      else if ( typeof( Boolean ).IsAssignableFrom( type ) )
        return boolean;
      else if ( typeof( string ).IsAssignableFrom( type ) )
        return boolean.ToString();
      else if ( typeof( Boolean? ).IsAssignableFrom( type ) )
        return boolean;
      else if ( type.Equals( typeof( Object ) ) )
        return boolean;
      else
        throw new Exception( "unable to adapt boolean to type " + type );
      }

    public bool canAdaptTo( Type formalArg )
      {
      return typeof( IAdaptingType ).IsAssignableFrom( formalArg ) ||
             typeof( Boolean ).IsAssignableFrom( formalArg ) ||
             typeof( string ).IsAssignableFrom( formalArg ) ||
             typeof( Boolean? ).IsAssignableFrom( formalArg ) ||
             formalArg.Equals( typeof( Object ) );
      }

    #endregion

    public override string ToString()
      {
      return "Boolean type. Value - " + boolean;
      }

    public override bool Equals( object _obj )
      {
      BooleanType obj = _obj as BooleanType;

      if ( obj == null )
        return false;

      return obj.boolean.Equals( boolean );
      
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return Equals( _obj );
      }

    public override int GetHashCode()
      {
      return boolean.GetHashCode();
      }
    }
  }
