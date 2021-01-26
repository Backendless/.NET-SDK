using System;
using System.Collections.Generic;

namespace TestProject
{
  public class Person
  {
    public String name;
    public Int32? age;
    public String objectId;
    public List<Order> Surname;
  }

  public class Order
  {
    public String objectId;
    public String LastName{ get; set; }
  }

  public class Area
  {
    public int UserId { get; set; }
    public string AreaA { get; set; }
    public bool Categories { get; set; }
  }

  public class Human
  {
    public List<Area> Related { get; set; }
    public String name { get; set; }
    public Int32? age { get; set; }
    public String objectId { get; set; }
  }

  public static class Comparer
  {
    public static Boolean IsEqual( Object a, Object b )
    {
      if( a == null && b == null ) //both are null
        return true;
      if( a == null || b == null ) //one is null, the other isn't
        return false;
      
      if( IsNumber( a ) && IsNumber( b ) )
      {
        if( IsFloatingPoint( a ) || IsFloatingPoint( b ) )
        {
          double da, db;
          if( Double.TryParse( a.ToString(), out da ) && Double.TryParse( b.ToString(), out db ) )
            return Math.Abs( da - db ) < 0.000001;
        }
        else
        {
          if( a.ToString().StartsWith( "-" ) || b.ToString().StartsWith( "-" ) )
            return Convert.ToInt64( a ) == Convert.ToInt64( b );
          else
            return Convert.ToUInt64( a ) == Convert.ToUInt64( b );
        }
      }

      return a.Equals( b );
    }

    private static Boolean IsFloatingPoint( Object value )
    {
      if( value is Single )
        return true;
      if( value is Double )
        return true;
      if( value is Decimal )
        return true;
      return false;
    }

    private static Boolean IsNumber( Object value )
    {
      if( value is SByte )
        return true;
      if( value is Byte )
        return true;
      if( value is Int16 )
        return true;
      if( value is UInt16 )
        return true;
      if( value is Int32 )
        return true;
      if( value is UInt32 )
        return true;
      if( value is Int64 )
        return true;
      if( value is UInt64 )
        return true;
      if( value is Single )
        return true;
      if( value is Double )
        return true;
      if( value is Decimal )
        return true;
      return false;
    }
  }
}