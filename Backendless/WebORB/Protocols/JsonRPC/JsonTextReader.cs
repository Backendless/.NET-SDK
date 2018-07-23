using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Weborb.Protocols.JsonRPC
  {
  /// <summary>
  /// Represents a reader that provides fast, non-cached, forward-only 
  /// access to JSON data over JSON text (RFC 4627). 
  /// </summary>

  public sealed class JsonTextReader : JsonReader
    {
    private static readonly char[] _numNonDigitChars = new char[] { '.', 'e', 'E', '+', '-' };

    private BufferedCharReader _reader;
    private Stack _stack;

    private delegate JsonToken Continuation();

    private Continuation _methodParse;
    private Continuation _methodParseArrayFirst;
    private Continuation _methodParseArrayNext;
    private Continuation _methodParseObjectMember;
    private Continuation _methodParseObjectMemberValue;
    private Continuation _methodParseNextMember;

    public JsonTextReader( TextReader reader )
      {
      if ( reader == null )
        throw new ArgumentNullException( "reader" );

      _reader = new BufferedCharReader( reader );
      Push( ParseMethod );
      }

    public int LineNumber { get { return _reader.LineNumber; } }
    public int LinePosition { get { return _reader.LinePosition; } }

    /// <summary>
    /// Reads the next token and returns it.
    /// </summary>

    protected override JsonToken ReadTokenImpl()
      {
      if ( _stack == null )
        {
        return JsonToken.EOF();
        }
      else if ( _stack.Count == 0 )
        {
        _stack = null;
        _reader = null;
        return JsonToken.EOF();
        }
      else
        {
        return Pop()();
        }
      }


    /// <summary>
    /// Parses the next token from the input and returns it.
    /// </summary>

    private JsonToken Parse()
      {
      char ch = NextClean();

      //
      // String
      //

      if ( ch == '"' || ch == '\'' )
        {
        return Yield( JsonToken.String( NextString( ch ) ) );
        }

      //
      // Object
      //

      if ( ch == '{' )
        {
        _reader.Back();
        return ParseObject();
        }

      //
      // Array
      //

      if ( ch == '[' )
        {
        _reader.Back();
        return ParseArray();
        }

      //
      // Handle unquoted text. This could be the values true, false, or
      // null, or it can be a number. An implementation (such as this one)
      // is allowed to also accept non-standard forms.
      //
      // Accumulate characters until we reach the end of the text or a
      // formatting character.
      // 

      StringBuilder sb = new StringBuilder();
      char b = ch;

      while ( ch >= ' ' && ",:]}/\\\"[{;=#".IndexOf( ch ) < 0 )
        {
        sb.Append( ch );
        ch = _reader.Next();
        }

      _reader.Back();

      string s = sb.ToString().Trim();

      if ( s.Length == 0 )
        throw SyntaxError( "Missing value." );


      //
      // Boolean
      //

      if ( s == "true" || s == "false" )
        return Yield( JsonToken.Boolean( s == "true" ) );

      //
      // Null
      //

      if ( s == "null" )
        return Yield( JsonToken.Null() );

      //
      // Number
      //
      // Try converting it. We support the 0- and 0x- conventions. 
      // If a number cannot be produced, then the value will just
      // be a string. Note that the 0-, 0x-, plus, and implied 
      // string conventions are non-standard, but a JSON text parser 
      // is free to accept non-JSON text forms as long as it accepts 
      // all correct JSON text forms.
      //

      if ( ( b >= '0' && b <= '9' ) || b == '.' || b == '-' || b == '+' )
        {
        if ( b == '0' && s.Length > 1 && s.IndexOfAny( _numNonDigitChars ) < 0 )
          {
          if ( s.Length > 2 && ( s[ 1 ] == 'x' || s[ 1 ] == 'X' ) )
            {
            string parsed = TryParseHex( s );
            if ( !ReferenceEquals( parsed, s ) )
              return Yield( JsonToken.Number( parsed ) );
            }
          else
            {
            string parsed = TryParseOctal( s );
            if ( !ReferenceEquals( parsed, s ) )
              return Yield( JsonToken.Number( parsed ) );
            }
          }
        else
          {
          if ( !IsValid( s ) )
            throw SyntaxError( string.Format( "The text '{0}' has the incorrect syntax for a number.", s ) );

          return Yield( JsonToken.Number( s ) );
          }
        }

      //
      // Treat as String in all other cases, e.g. when unquoted.
      //

      return Yield( JsonToken.String( s ) );
      }

    private bool IsValid( string s )
      {
      Regex r = new Regex( "^"
        /*
                number = [ minus ] int [ frac ] [ exp ]
                decimal-point = %x2E       ; .
                digit1-9 = %x31-39         ; 1-9
                e = %x65 / %x45            ; e E
                exp = e [ minus / plus ] 1*DIGIT
                frac = decimal-point 1*DIGIT
                int = zero / ( digit1-9 *DIGIT )
                minus = %x2D               ; -
                plus = %x2B                ; +
                zero = %x30                ; 0
        */
          + @"      -?                # [ minus ]
                                            # int
                        (  0                #   zero
                           | [1-9][0-9]* )  #   / ( digit1-9 *DIGIT )
                                            # [ frac ]
                        ( \.                #   decimal-point
                          [0-9]+ )?         #   1*DIGIT
                                            # [ exp ]
                        ( [eE]              #   e
                          [+-]?             #   [ minus / plus ]
                          [0-9]+ )?         #   1*DIGIT
                  " // NOTE! DO NOT move the closing quote
        // Moving it to the line above change the pattern!
          + "$",
          RegexOptions.IgnorePatternWhitespace
          | RegexOptions.ExplicitCapture
          | RegexOptions.Compiled );

      return r.IsMatch( s );
      }

    private Continuation ParseMethod
      {
      get
        {
        if ( _methodParse == null ) _methodParse = new Continuation( Parse );
        return _methodParse;
        }
      }

    /// <summary>
    /// Parses expecting an array in the source.
    /// </summary>

    private JsonToken ParseArray()
      {
      if ( NextClean() != '[' )
        throw SyntaxError( "An array must start with '['." );

      return Yield( JsonToken.Array(), ParseArrayFirstMethod );
      }

    /// <summary>
    /// Parses the first element of an array or the end of the array if
    /// it is empty.
    /// </summary>

    private JsonToken ParseArrayFirst()
      {
      if ( NextClean() == ']' )
        return Yield( JsonToken.EndArray() );

      _reader.Back();

      Push( ParseArrayNextMethod );
      return Parse();
      }

    private Continuation ParseArrayFirstMethod
      {
      get
        {
        if ( _methodParseArrayFirst == null ) _methodParseArrayFirst = new Continuation( ParseArrayFirst );
        return _methodParseArrayFirst;
        }
      }

    /// <summary>
    /// Parses the next element in the array.
    /// </summary>

    private JsonToken ParseArrayNext()
      {
      switch ( NextClean() )
        {
        case ';':
        case ',':
            {
            if ( NextClean() == ']' )
              return Yield( JsonToken.EndArray() );
            else
              _reader.Back();

            break;
            }

        case ']':
              {
              return Yield( JsonToken.EndArray() );
              }

        default:
              throw SyntaxError( "Expected a ',' or ']'." );
        }

      Push( ParseArrayNextMethod );
      return Parse();
      }

    private Continuation ParseArrayNextMethod
      {
      get
        {
        if ( _methodParseArrayNext == null ) _methodParseArrayNext = new Continuation( ParseArrayNext );
        return _methodParseArrayNext;
        }
      }

    /// <summary>
    /// Parses expecting an object in the source.
    /// </summary>

    private JsonToken ParseObject()
      {
      if ( NextClean() != '{' )
        throw SyntaxError( "An object must begin with '{'." );

      return Yield( JsonToken.Object(), ParseObjectMemberMethod );
      }

    /// <summary>
    /// Parses the first member name of the object or the end of the array
    /// in case of an empty object.
    /// </summary>

    private JsonToken ParseObjectMember()
      {
      char ch = NextClean();

      if ( ch == '}' )
        return Yield( JsonToken.EndObject() );

      if ( ch == BufferedCharReader.EOF )
        throw SyntaxError( "An object must end with '}'." );

      _reader.Back();
      string name = Parse().Text;
      return Yield( JsonToken.Member( name ), ParseObjectMemberValueMethod );
      }

    private Continuation ParseObjectMemberMethod
      {
      get
        {
        if ( _methodParseObjectMember == null ) _methodParseObjectMember = new Continuation( ParseObjectMember );
        return _methodParseObjectMember;
        }
      }

    private JsonToken ParseObjectMemberValue()
      {
      char ch = NextClean();

      if ( ch == '=' )
        {
        if ( _reader.Next() != '>' )
          _reader.Back();
        }
      else if ( ch != ':' )
        throw SyntaxError( "Expected a ':' after a key." );

      Push( ParseNextMemberMethod );
      return Parse();
      }

    private Continuation ParseObjectMemberValueMethod
      {
      get
        {
        if ( _methodParseObjectMemberValue == null ) _methodParseObjectMemberValue = new Continuation( ParseObjectMemberValue );
        return _methodParseObjectMemberValue;
        }
      }

    private JsonToken ParseNextMember()
      {
      switch ( NextClean() )
        {
        case ';':
        case ',':
            {
            if ( NextClean() == '}' )
              return Yield( JsonToken.EndObject() );
            break;
            }

        case '}':
            return Yield( JsonToken.EndObject() );

        default:
            throw SyntaxError( "Expected a ',' or '}'." );
        }

      _reader.Back();
      string name = Parse().Text;
      return Yield( JsonToken.Member( name ), ParseObjectMemberValueMethod );
      }

    private Continuation ParseNextMemberMethod
      {
      get
        {
        if ( _methodParseNextMember == null ) _methodParseNextMember = new Continuation( ParseNextMember );
        return _methodParseNextMember;
        }
      }

    /// <summary> 
    /// Yields control back to the reader's user while updating the
    /// reader with the new found token and its text.
    /// </summary>

    private JsonToken Yield( JsonToken token )
      {
      return Yield( token, null );
      }

    /// <summary> 
    /// Yields control back to the reader's user while updating the
    /// reader with the new found token, its text and the next 
    /// continuation point into the reader.
    /// </summary>
    /// <remarks>
    /// By itself, this method cannot affect the stack such tha control 
    /// is returned back to the reader's user. This must be done by 
    /// Yield's caller by way of explicit return.
    /// </remarks>

    private JsonToken Yield( JsonToken token, Continuation continuation )
      {
      if ( continuation != null )
        Push( continuation );

      return token;
      }

    /// <summary>
    /// Get the next char in the string, skipping whitespace
    /// and comments (slashslash and slashstar).
    /// </summary>
    /// <returns>A character, or 0 if there are no more characters.</returns>

    private char NextClean()
      {
      Debug.Assert( _reader != null );

      while ( true )
        {
        char ch = _reader.Next();

        if ( ch == '/' )
          {
          switch ( _reader.Next() )
            {
            case '/':
                {
                do
                  {
                  ch = _reader.Next();
                  } while ( ch != '\n' && ch != '\r' && ch != BufferedCharReader.EOF );
                break;
                }
            case '*':
                  {
                  while ( true )
                    {
                    ch = _reader.Next();

                    if ( ch == BufferedCharReader.EOF )
                      throw SyntaxError( "Unclosed comment." );

                    if ( ch == '*' )
                      {
                      if ( _reader.Next() == '/' )
                        break;

                      _reader.Back();
                      }
                    }
                  break;
                  }
            default:
                    {
                    _reader.Back();
                    return '/';
                    }
            }
          }
        else if ( ch == '#' )
          {
          do
            {
            ch = _reader.Next();
            }
          while ( ch != '\n' && ch != '\r' && ch != BufferedCharReader.EOF );
          }
        else if ( ch == BufferedCharReader.EOF || ch > ' ' )
          {
          return ch;
          }
        }
      }

    private string NextString( char quote )
      {
      try
        {
        return Dequote( _reader, quote, null ).ToString();
        }
      catch ( FormatException e )
        {
        throw SyntaxError( e.Message, e );
        }
      }

    internal static StringBuilder Dequote( BufferedCharReader input, char quote, StringBuilder output )
      {
      Debug.Assert( input != null );

      if ( output == null )
        output = new StringBuilder();

      char[] hexDigits = null;

      while ( true )
        {
        char ch = input.Next();

        if ( ( ch == BufferedCharReader.EOF ) || ( ch == '\n' ) || ( ch == '\r' ) )
          throw new FormatException( "Unterminated string." );

        if ( ch == '\\' )
          {
          ch = input.Next();

          switch ( ch )
            {
            case 'b': output.Append( '\b' ); break; // Backspace
            case 't': output.Append( '\t' ); break; // Horizontal tab
            case 'n': output.Append( '\n' ); break; // Newline
            case 'f': output.Append( '\f' ); break; // Form feed
            case 'r': output.Append( '\r' ); break; // Carriage return

            case 'u':
                {
                if ( hexDigits == null )
                  hexDigits = new char[ 4 ];

                output.Append( ParseHex( input, hexDigits ) );
                break;
                }

            default:
                output.Append( ch );
                break;
            }
          }
        else
          {
          if ( ch == quote )
            return output;

          output.Append( ch );
          }
        }
      }

    private static char ParseHex( BufferedCharReader input, char[] hexDigits )
      {
      Debug.Assert( input != null );
      Debug.Assert( hexDigits != null );
      Debug.Assert( hexDigits.Length == 4 );

      hexDigits[ 0 ] = input.Next();
      hexDigits[ 1 ] = input.Next();
      hexDigits[ 2 ] = input.Next();
      hexDigits[ 3 ] = input.Next();

      return (char)ushort.Parse( new string( hexDigits ), NumberStyles.HexNumber );
      }

    private void Push( Continuation continuation )
      {
      Debug.Assert( continuation != null );

      if ( _stack == null )
        _stack = new Stack( 6 );

      _stack.Push( continuation );
      }

    private Continuation Pop()
      {
      Debug.Assert( _stack != null );
      return (Continuation)_stack.Pop();
      }

    private Exception SyntaxError( string message )
      {
      return SyntaxError( message, null );
      }

    private Exception SyntaxError( string message, Exception inner )
      {
      if ( LineNumber > 0 )
        {
        message = string.Format(
            "{0} See line {1}, position {2}.",
            message, LineNumber.ToString( "N0" ), LinePosition.ToString( "N0" ) );
        }

      return new Exception( message, inner );
      }

    private static string TryParseOctal( string s )
      {
      Debug.Assert( s != null );
      Debug.Assert( s[ 0 ] == '0' );
      Debug.Assert( s.Length > 1 );

      long num = 0;

      try
        {
        for ( int i = 1; i < s.Length; i++ )
          {
          char ch = s[ i ];

          if ( ch < '0' || ch > '8' )
            return s;

          num = checked( num * 8 ) | ( (uint)ch - 0x30 );
          }
        }
      catch ( OverflowException )
        {
        return s;
        }

      return num.ToString( CultureInfo.InvariantCulture );
      }

    private static string TryParseHex( string s )
      {
      Debug.Assert( s != null );
      Debug.Assert( s.Length > 1 );

      try
        {
        long num = long.Parse( s.Substring( 2 ), NumberStyles.HexNumber, CultureInfo.InvariantCulture );
        return num.ToString( CultureInfo.InvariantCulture );
        }
      catch ( OverflowException )
        {
        return s;
        }
      catch ( FormatException )
        {
        return s;
        }
      }
    }
  }