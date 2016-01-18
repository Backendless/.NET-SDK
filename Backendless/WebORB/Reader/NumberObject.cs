using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.Reader
  {
  public class NumberObject : IAdaptingType
    {
    //private static Type stringType = typeof( string );
    //private static Type stringBuilderType = typeof( StringBuilder );

    private object data;

    public NumberObject( double data )
    {
      this.data = data;
    }

    public NumberObject( byte data )
    {
      this.data = data;
    }

    #region IAdaptingType Members

    public Type getDefaultType()
    {
      if (this.data is Byte)
        return typeof (Byte);

      Double data = (Double) this.data;

      if (Math.Round(data) == data)
      {
        if (data >= Int32.MinValue && data <= Int32.MaxValue)
          return typeof (Int32);
        else if (data >= Int64.MinValue && data <= Int64.MaxValue)
          return typeof (Int64);
      }

      return typeof (Double);
    }

    public object defaultAdapt()
    {
      if ( this.data is Byte )
        return this.data;

      Double data = (Double)this.data;

      if (Double.IsNaN(data))
      {
        if (Log.isLogging(LoggingConstants.DEBUG))
          Log.log(LoggingConstants.DEBUG, "Value is NaN, returning -1");

        return -1;
      }

      try
      {
        if (Math.Round(data) == data)
          return Convert.ToInt32(data);
        else
          return Convert.ToDouble(data);
      }
      catch (Exception)
      {
        if (Log.isLogging(LoggingConstants.EXCEPTION))
          Log.log(LoggingConstants.EXCEPTION, "unable to convert data to Int32, attempting Int64. Data is " + data);

        if (Math.Round(data) == data)
          return Convert.ToInt64(data);
        else
          return Convert.ToDouble(data);
      }
    }

    public object adapt( Type type )
    {
      Double data = Convert.ToDouble(this.data);
      object checkedValue = data;

      if (Double.IsNaN(data))
        checkedValue = null;

      if (type.Equals(typeof (IAdaptingType)))
        return this;

      else if (type.Equals(typeof (Byte)))
        return Convert.ToByte(checkedValue);
      else if (type.Equals(typeof (Byte?)))
        return checkedValue == null ? null : (object) Convert.ToByte(data);

      else if (type.Equals(typeof (SByte)))
        return Convert.ToSByte(checkedValue);
      else if (type.Equals(typeof (SByte?)))
        return checkedValue == null ? null : (object) Convert.ToSByte(data);

      else if (type.Equals(typeof (Char)))
        return Convert.ToChar(checkedValue);
      else if (type.Equals(typeof (Char?)))
        return checkedValue == null ? null : (object) Convert.ToChar(data);

      else if (type.Equals(typeof (Int16)))
        return Convert.ToInt16(checkedValue);
      else if (type.Equals(typeof (Int16?)))
        return checkedValue == null ? null : (object) Convert.ToInt16(data);

      else if (type.Equals(typeof (Int32)))
        return Convert.ToInt32(checkedValue);
      else if (type.Equals(typeof (Int32?)))
        return checkedValue == null ? null : (object) Convert.ToInt32(data);

      else if (type.Equals(typeof (Int64)))
        return Convert.ToInt64(checkedValue);
      else if (type.Equals(typeof (Int64?)))
        return checkedValue == null ? null : (object) Convert.ToInt64(data);

      else if (type.Equals(typeof (UInt16)))
        return Convert.ToUInt16(checkedValue);
      else if (type.Equals(typeof (UInt16?)))
        return checkedValue == null ? null : (object) Convert.ToUInt16(data);

      else if (type.Equals(typeof (UInt32)))
        return Convert.ToUInt32(checkedValue);
      else if (type.Equals(typeof (UInt32?)))
        return checkedValue == null ? null : (object) Convert.ToUInt32(data);

      else if (type.Equals(typeof (UInt64)))
        return Convert.ToUInt64(checkedValue);
      else if (type.Equals(typeof (UInt64?)))
        return checkedValue == null ? null : (object) Convert.ToUInt64(data);

      else if (type.Equals(typeof (Decimal)))
        return Convert.ToDecimal(checkedValue);
      else if (type.Equals(typeof (Decimal?)))
        return checkedValue == null ? null : (object) Convert.ToDecimal(data);

      else if (type.Equals(typeof (Single)))
        return Convert.ToSingle(checkedValue);
      else if (type.Equals(typeof (Single?)))
        return checkedValue == null ? null : (object) Convert.ToSingle(data);

      else if (type.Equals(typeof (Double)))
        return data;
      else if (type.Equals(typeof (Double?)))
        return checkedValue == null ? null : (object) data;

      else if (typeof (string).IsAssignableFrom(type))
      {
        if (checkedValue == null)
          return null;

        if (data - Convert.ToInt32(data) == 0.0d)
          return Convert.ToInt32(data).ToString();
        else
          return data.ToString();
      }
      else if (typeof (StringBuilder).IsAssignableFrom(type))
        return checkedValue == null ? new StringBuilder() : new StringBuilder(data.ToString());

      else if (type.Equals(typeof (Boolean)))
        return Convert.ToBoolean(checkedValue);
      else if (type.Equals(typeof (DateTime)))
      {
        long ticks = (new DateTime(1970, 1, 1)).Ticks;
          // Intervals that have elapsed since 12:00:00 midnight, January 1, 0001 
        return new DateTime(Convert.ToInt64(checkedValue)*10000 + ticks); //There are 10,000 ticks in a millisecond
      }
      else if (type.Equals(typeof (Boolean?)))
        return checkedValue == null ? null : (object) Convert.ToBoolean(data);

      else if (type.BaseType == typeof (Enum))
      {
        Type enumUnderlyingType = Enum.GetUnderlyingType(type);
        object adaptedNumber = adapt(enumUnderlyingType);
        string enumItemName = Enum.GetName(type, adaptedNumber);
        return type.GetField(enumItemName).GetValue(null);
      }

      else if (type.Equals(typeof (TimeSpan)))
        return TimeSpan.FromMilliseconds(checkedValue == null ? 0 : data);

      return data;
    }

    public bool canAdaptTo( Type formalArg )
      {
      return typeof( Byte ).IsAssignableFrom( formalArg ) ||
        typeof( Int16 ).IsAssignableFrom( formalArg ) ||
        typeof( Int32 ).IsAssignableFrom( formalArg ) ||
        typeof( Int64 ).IsAssignableFrom( formalArg ) ||
        typeof( UInt16 ).IsAssignableFrom( formalArg ) ||
        typeof( UInt32 ).IsAssignableFrom( formalArg ) ||
        typeof( UInt64 ).IsAssignableFrom( formalArg ) ||
        typeof( Single ).IsAssignableFrom( formalArg ) ||
        typeof( Double ).IsAssignableFrom( formalArg ) ||
        typeof( string ).IsAssignableFrom( formalArg ) ||
        typeof( Boolean ).IsAssignableFrom( formalArg ) ||
        typeof( DateTime ).IsAssignableFrom( formalArg ) ||
                typeof( Decimal ).IsAssignableFrom( formalArg ) ||
                typeof( TimeSpan ).IsAssignableFrom( formalArg ) ||
        typeof( StringBuilder ).IsAssignableFrom( formalArg ) ||
                formalArg.IsGenericType && formalArg.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) ||
        typeof( IAdaptingType ).IsAssignableFrom( formalArg );
      }

    #endregion

    public override string ToString()
      {
      return "Number type. Value - " + data;
      }

    public override bool Equals( object _obj )
      {
      NumberObject obj = _obj as NumberObject;

      if ( obj == null )
        return false;

      return obj.data.Equals( data );   
      }

    public bool Equals( object _obj, Dictionary<DictionaryEntry, bool> visitedPairs )
      {
      return Equals( _obj );
      }

    public override int GetHashCode()
      {
      return data.GetHashCode();
      }
    }
  }
