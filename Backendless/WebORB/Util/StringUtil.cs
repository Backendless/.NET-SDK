using System;
using System.Text;

namespace Weborb.Util
{
  public class StringUtil
  {
    private static Type stringType = typeof( string );
    private static Type charArrayType = typeof( char[] );
    private static Type charType = typeof( char );
    private static Type stringBuilderType = typeof( StringBuilder );

    public static bool IsStringType( Type type )
    {
      if( stringType.IsAssignableFrom( type ) )
        return true;

      if( charArrayType.IsAssignableFrom( type ) )
        return true;

      if( charType.IsAssignableFrom( type ) )
        return true;

      if( stringBuilderType.IsAssignableFrom( type ) )
        return true;

      return false;
      //return obj is string || obj is char[] || obj is Char || obj is StringBuilder;
    }

    public static bool IsString( object obj )
    {
      if( stringType.IsInstanceOfType( obj ) )
        return true;

      if( charArrayType.IsInstanceOfType( obj ) )
        return true;

      if( charType.IsInstanceOfType( obj ) )
        return true;

      if( stringBuilderType.IsInstanceOfType( obj ) )
        return true;

      return false;
      //return obj is string || obj is char[] || obj is Char || obj is StringBuilder;
    }

    public static bool IsEmptyString( object obj )
    {
      string str = obj as string;

      if( str != null )
        return str.Length == 0;

      StringBuilder sb = obj as StringBuilder;

      if( sb != null )
        return sb.Length == 0;

      char[] charArr = obj as char[];

      if( charArr != null )
        return charArr.Length == 0;

      return false;
    }

    public static bool IsTrueConfigValue( String val )
    {
      if( val == null )
        return false;

      val = val.Trim().ToLower();

      if( val.Length == 0 )
        return false;

      return val.Equals( "true" ) || val.Equals( "yes" ) || val.Equals( "1" );
    }
  }
}
