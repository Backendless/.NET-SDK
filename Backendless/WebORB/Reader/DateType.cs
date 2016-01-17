using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Types;

namespace Weborb.Reader
  {
  public class DateType : IAdaptingType
    {
    //private static Type longType = typeof( Int64 );
    //private static Type dateTimeType = typeof( DateTime );

    private DateTime dateObj;

    public DateType( DateTime dateObj )
      {
      this.dateObj = dateObj;
      }

    #region IAdaptingType Members

    public Type getDefaultType()
      {
      return typeof( DateTime );
      }

    public object defaultAdapt()
      {
      return dateObj;
      }

    public object adapt( Type type )
      {
      if ( type.Equals( typeof( IAdaptingType ) ) )
        return this;
      else if ( typeof( DateTime ).IsAssignableFrom( type ) )
        return dateObj;
      else if ( typeof( DateTimeOffset ).IsAssignableFrom( type ) )
        return new DateTimeOffset(dateObj);
      else if ( typeof( Int64 ).IsAssignableFrom( type ) )
        return dateObj.Ticks;
      else if ( typeof( UInt64 ).IsAssignableFrom( type ) )
        return dateObj.Ticks;
      else if ( typeof( DateTime? ).IsAssignableFrom( type ) )
        return dateObj;
      else if ( typeof( Int64? ).IsAssignableFrom( type ) )
        return dateObj.Ticks;
      else if ( typeof( UInt64? ).IsAssignableFrom( type ) )
        return dateObj.Ticks;
      else if ( typeof( Object ).Equals( type ) )
        return dateObj;
      else
        throw new Exception( "unable to adapt date object to type " + type );
      }

    public bool canAdaptTo( Type formalArg )
      {
      return typeof( IAdaptingType ).IsAssignableFrom( formalArg ) ||
          typeof( DateTime ).IsAssignableFrom( formalArg ) ||
          typeof( DateTimeOffset ).IsAssignableFrom( formalArg ) ||
          typeof( Int64 ).IsAssignableFrom( formalArg ) ||
          typeof( UInt64 ).IsAssignableFrom( formalArg ) ||
          typeof( DateTime? ).IsAssignableFrom( formalArg ) ||
          typeof( Object ).Equals( formalArg ) ||
          typeof( int? ).IsAssignableFrom( formalArg ) ||
          typeof( uint? ).IsAssignableFrom( formalArg );
      }

    #endregion

    public override string ToString()
      {
      return "Date/time type. Value - " + dateObj.ToString();
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return Equals( _obj );
      }

    public override bool Equals( object _obj )
      {
      DateType obj = _obj as DateType;

      if ( obj == null )
        return false;

      return obj.dateObj.Equals( dateObj );
      }

    public override int GetHashCode()
      {
      return dateObj.GetHashCode();
      }
    }
  }
