using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Util
{
  //public static class ExtensionMethods
  //{
  //  // for generic interface IEnumerable<T>
  //  public static string ToString<T>( this IEnumerable<T> source, string separator )
  //  {
  //    if( source == null )
  //      throw new ArgumentException( "Parameter source can not be null." );

  //    if( string.IsNullOrEmpty( separator ) )
  //      throw new ArgumentException( "Parameter separator can not be null or empty." );

  //    string[] array = source.Where( n => n != null ).Select( n => n.ToString() ).ToArray();

  //    return string.Join( separator, array );
  //  }

  //  // for interface IEnumerable
  //  public static string ToString( this IEnumerable source, string separator )
  //  {
  //    if( source == null )
  //      throw new ArgumentException( "Parameter source can not be null." );

  //    if( string.IsNullOrEmpty( separator ) )
  //      throw new ArgumentException( "Parameter separator can not be null or empty." );

  //    string[] array = source.Cast<object>().Where( n => n != null ).Select( n => n.ToString() ).ToArray();

  //    return string.Join( separator, array );
  //  }
  //}
}
