using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using BackendlessAPI.Utils;

namespace BackendlessAPI.Persistence
{
  public class WKTParser
  {
    private static String EMPTY = "";
    private static String COMMA = ",";
    private static String L_PAREN = "(";
    private static String R_PAREN = ")";
    private static String NAN_SYMBOL = "NaN";

    private ReferenceSystemEnum srs;

    public WKTParser( ReferenceSystemEnum srs )
    {
      this.srs = srs;
    }
    public WKTParser() : this( SpatialReferenceSystem.DEFAULT )
    {
    }

    public Geometry Read( String wellKnownText )
    {
      StringReader reader = new StringReader( wellKnownText );
      StreamTokenizer tokenizer = CreateTokenizer( reader );
      String type = GetNextWord( tokenizer ).ToUpper();
      try
      {
        return ReadGeometryTaggedText( tokenizer, type );
      }
      catch( IOException ex )
      {
        throw new WKTParseException( ex );
      }
    }

    private static StreamTokenizer CreateTokenizer( TextReader reader )
    {
      StreamTokenizer tokenizer = new StreamTokenizer( reader );
      tokenizer.ResetSyntax();
      tokenizer.WordChars( 'a', 'z' );
      tokenizer.WordChars( 'A', 'Z' );
      tokenizer.WordChars( 128+32, 255 );
      tokenizer.WordChars( '0', '9' );
      tokenizer.WordChars( '-', '-' );
      tokenizer.WordChars( '+', '+' );
      tokenizer.WordChars( '.', '.' );
      tokenizer.WhitespaceChars( 0, ' ' );
      tokenizer.CommentChar( '#' );

      return tokenizer;
    }

    private static String GetNextWord( StreamTokenizer tokenizer )
    {
      try
      {
        int type = tokenizer.NextToken();

        switch ( type )
        {
          case StreamTokenizer.TT_WORD:
            String word = tokenizer.StringValue;

            if ( word.Equals( EMPTY ) )
              return EMPTY;
            return word;
          case '(':
            return L_PAREN;
          case ')':
            return R_PAREN;
          case ',':
            return COMMA;
        }

        throw new WKTParseException( $"Uknown type: '{( char )type}'" );
      }
      catch ( IOException ex )
      {
        throw new WKTParseException( ex );
      }
    }

    private String TokenString( StreamTokenizer tokenizer )
    {
      switch ( tokenizer.ttype )
      {
        case StreamTokenizer.TT_NUMBER:
          return "<NUMBER>";
        case StreamTokenizer.TT_EOL:
          return "End-of-Line";
        case StreamTokenizer.TT_EOF:
          return "End-of-Stream";
        case StreamTokenizer.TT_WORD:
          return $"'{tokenizer.StringValue}'";
      }

      return $"'{(char) tokenizer.ttype}'";
    }

    private static String GetNextEmptyOrOpener( StreamTokenizer tokenizer )
    {
      String nextWord = GetNextWord( tokenizer );

      if( nextWord.Equals( EMPTY ) || nextWord.Equals( L_PAREN ) )
        return nextWord;

      throw new WKTParseException( $"Excepted: {EMPTY} or {L_PAREN}" );
    }

    private static String GetNextCloserOrComma( StreamTokenizer tokenizer )
    {
      String nextWord = GetNextWord( tokenizer );

      if( nextWord.Equals( COMMA ) || nextWord.Equals( R_PAREN ) )
        return nextWord;

      throw new WKTParseException( $"Excepted: {COMMA} or {R_PAREN}" );
    }

    private String GetNextCloser( StreamTokenizer tokenizer )
    {
      String nextWord = GetNextWord( tokenizer );

      if( nextWord.Equals( R_PAREN ) )
        return nextWord;

      throw new WKTParseException( $"Excepted: {R_PAREN}" );
    }

    private static bool IsOpenerNext( StreamTokenizer tokenizer )
    {
      try
      {
        int type = tokenizer.NextToken();
        tokenizer.PushBack();
        return type == '(';
      }
      catch( IOException ex )
      {
        throw new WKTParseException( ex );
      }
    }

    private static bool IsNumberText( StreamTokenizer tokenizer )
    {
      try
      {
        int type = tokenizer.NextToken();
        tokenizer.PushBack();
        return type == StreamTokenizer.TT_WORD;
      }
      catch ( IOException ex )
      {
        throw new WKTParseException( ex );
      }
    }

    private double GetNextNumber( StreamTokenizer tokenizer )
    {
      int type = tokenizer.NextToken();

      if( type == StreamTokenizer.TT_WORD )
      {
        if( tokenizer.StringValue.Equals( NAN_SYMBOL ) )
          return Double.NaN;
        else
        {
          try
          {
            NumberFormatInfo NFI = new NumberFormatInfo()
            {
              NumberDecimalSeparator = "."
            };
            return Double.Parse( tokenizer.StringValue, NFI );
          }
          catch ( FormatException ex )
          {
            throw new WKTParseException( ex );
          }
        }
      }

      throw new WKTParseException( "Excepted: number" );
    }
    private List<double[]> GetCoordinateSequence( StreamTokenizer tokenizer, bool tryParen )
    {
      String nextWord = GetNextEmptyOrOpener( tokenizer );

      if( nextWord.Equals( EMPTY ) )
        return null;

      List<double[]> coordinates = new List<double[]>();

      do
      {
        coordinates.Add( GetCoordinate( tokenizer, tryParen ) );
      }
      while( GetNextCloserOrComma( tokenizer ).Equals( COMMA ) );

      return coordinates;
    }

    private double[] GetCoordinate( StreamTokenizer tokenizer, bool tryParen )
    {
      bool opened;

      if( opened = tryParen && IsOpenerNext( tokenizer ) )
        tokenizer.NextToken();

      double[] sequence = new double[ 2 ];
      sequence[ 0 ] = GetNextNumber( tokenizer );
      sequence[ 1 ] = GetNextNumber( tokenizer );

      if( opened )
        GetNextCloser( tokenizer );

      return sequence;
    }

    private Geometry ReadGeometryTaggedText( StreamTokenizer tokenizer, String type )
    {
      if( type.StartsWith( Point.WKT_TYPE ) )
        return ReadPointText( tokenizer );

      else if( type.StartsWith( LineString.WKT_TYPE ) )
        return ReadLineStringText( tokenizer );

      else if( type.StartsWith( Polygon.WKT_TYPE ) )
        return ReadPolygonText( tokenizer );

      throw new WKTParseException( $"Uknown geometry type: '{type}'" );
    }

    private Point ReadPointText ( StreamTokenizer tokenizer )
    {
      List<double[]> coordinateSequence = GetCoordinateSequence( tokenizer, false );

      if( coordinateSequence == null )
        return null;

      return new Point( this.srs ).SetX( coordinateSequence.ElementAt( 0 )[ 0 ] ).SetY( coordinateSequence.ElementAt( 0 )[ 1 ] );
    }

    private LineString ReadLineStringText( StreamTokenizer tokenizer )
    {
      List<double[]> coordinateSequence = GetCoordinateSequence( tokenizer, false );

      if( coordinateSequence == null )
        return null;

      List<Point> points = new List<Point>();

      foreach( double[] coordinates in coordinateSequence )
        points.Add( new Point( srs ).SetX( coordinates[ 0 ] ).SetY( coordinates[ 1 ] ) );

      return new LineString( points, this.srs );
    }

    private Polygon ReadPolygonText( StreamTokenizer tokenizer )
    {
      String nextToken = GetNextEmptyOrOpener( tokenizer );

      if( nextToken.Equals( EMPTY ) )
        return null;

      LineString shell = ReadLineStringText( tokenizer );
      List<LineString> holes = new List<LineString>();
      nextToken = GetNextCloserOrComma( tokenizer );

      while( nextToken.Equals( COMMA ) )
      {
        LineString hole = ReadLineStringText( tokenizer );
        holes.Add( hole );
        nextToken = GetNextCloserOrComma( tokenizer );
      }

      return new Polygon( shell, holes, srs );
    }

    private class WKTParseException : System.Exception
    {
      public WKTParseException( String message ) : base( message )
      {
      }

      public WKTParseException( System.Exception exception ) : base( exception.Message )
      {
      }

      public WKTParseException( String message, System.Exception exception ) : base( message, exception )
      {
      }
    }
  }
}
