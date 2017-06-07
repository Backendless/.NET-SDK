using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class StringType : IAdaptingType
    {
    private string stringValue;

    public StringType( string stringValue )
      {
      this.stringValue = stringValue;
      }

    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return typeof( string );
      }

    public object defaultAdapt()
      {
      return stringValue;
      }

    public object adapt( Type type )
      {
      if ( typeof( IAdaptingType ).IsAssignableFrom( type ) )
        return this;
      else if ( typeof( string ).IsAssignableFrom( type ) ||
          ( type.BaseType == null && !type.IsPrimitive ) )
        return stringValue;
      else if ( typeof( StringBuilder ).IsAssignableFrom( type ) )
        return new StringBuilder( stringValue );
      else if ( ( new byte[ 0 ] ).GetType().IsAssignableFrom( type ) )
        return new UTF8Encoding().GetBytes( stringValue );
      else if ( ( new char[ 0 ] ).GetType().IsAssignableFrom( type ) )
        return stringValue.ToCharArray();
      else if ( typeof( Char ).IsAssignableFrom( type ) )
        return stringValue.ToCharArray()[ 0 ];
      else if ( typeof( Boolean ).IsAssignableFrom( type ) && isBooleanString() )
        return Convert.ToBoolean( stringValue );
      else if ( type.BaseType == typeof( Enum ) )
        return type.GetField( stringValue ).GetValue( null );
      else if ( typeof( IConvertible ).IsAssignableFrom( type ) )
        try
        {
          return Convert.ChangeType( stringValue == "" ? "0" : stringValue, type, CultureInfo.InvariantCulture );
        }
        catch( Exception )
        {
          throw new Exception( String.Format( "unable to adapt string value of '{0}' to type '{1}'. Make sure the property type in your class matches the one in the database", stringValue, type.Name ) );
        }
      else if ( typeof( Guid ).IsAssignableFrom( type ) )
        return new Guid( stringValue );
      else if ( type.IsGenericType && type.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) )
      {
        if ( stringValue == null || stringValue == "" )
          return null;

        return adapt( type.GetGenericArguments()[0] );
      }
      else if ( typeof( DateTime ).IsAssignableFrom( type ) )
        return DateTime.Parse( stringValue );
      else if ( typeof( TimeSpan ).IsAssignableFrom( type ) )
        return TimeSpan.FromMilliseconds( long.Parse( stringValue ) );
      else
        throw new Exception( "unable to adapt string to " + type );
      }

    public bool canAdaptTo( Type formalArg )
      {
      if ( formalArg.IsGenericType && formalArg.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) )
        return canAdaptTo( formalArg.GetGenericArguments()[ 0 ] );

      return typeof( string ).IsAssignableFrom( formalArg ) ||
        typeof( StringBuilder ).IsAssignableFrom( formalArg ) ||
        ( new byte[ 0 ] ).GetType().IsAssignableFrom( formalArg ) ||
        ( new char[ 0 ] ).GetType().IsAssignableFrom( formalArg ) ||
        typeof( Char ).IsAssignableFrom( formalArg ) ||
        ( typeof( Boolean ).IsAssignableFrom( formalArg ) && isBooleanString() ) ||
                typeof( Enum ).IsAssignableFrom( formalArg ) ||
                typeof( Guid ).IsAssignableFrom( formalArg ) ||
                ( typeof( IConvertible ).IsAssignableFrom( formalArg ) && !typeof( Boolean ).IsAssignableFrom( formalArg ) ) ||
        typeof( IAdaptingType ).IsAssignableFrom( formalArg ) ||
                ( typeof( TimeSpan ).IsAssignableFrom( formalArg ) && isNumber() ) ||
                ( typeof( DateTime ).IsAssignableFrom( formalArg ) && canParseAsDate() ) ||
        formalArg.BaseType == null;
      }

    #endregion

    private bool canParseAsDate()
      {
      if ( stringValue == null && stringValue == "" )
        return true;

      try
        {
        DateTime.Parse( stringValue );
        return true;
        }
      catch ( Exception )
        {
        return false;
        }
      }

    private bool isNumber()
      {
      try
        {
        long.Parse( stringValue );
        return true;
        }
      catch ( Exception )
        {
        return false;
        }
      }

    private bool isBooleanString()
      {
      string testStr = stringValue.ToLower();
      return testStr.Equals( "yes" ) ||
        testStr.Equals( "no" ) ||
        testStr.Equals( "0" ) ||
        testStr.Equals( "1" ) ||
        testStr.Equals( "true" ) ||
        testStr.Equals( "false" );
      }

    public override string ToString()
      {
      return "String type. Value - " + stringValue;
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return Equals( _obj );
      }

    public override bool Equals( object _obj )
      {
      StringType obj = _obj as StringType;

      if ( obj == null )
        return false;

      return obj.stringValue.Equals( stringValue );
      }

    public override int GetHashCode()
      {
      return stringValue.GetHashCode();
      }
    }
  }
