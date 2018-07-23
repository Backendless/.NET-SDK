using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

using Weborb.Message;
using Weborb.Protocols.JsonRPC;
using Weborb.Reader;
using Weborb.Reader.JsonRPC;
using Weborb.Types;
using Weborb.Util;
using Weborb.Util.Logging;
using Weborb.Writer.JsonRPC;
using ArrayReader = Weborb.Reader.JsonRPC.ArrayReader;
using BooleanReader = Weborb.Reader.JsonRPC.BooleanReader;
using NumberReader = Weborb.Reader.JsonRPC.NumberReader;
using NullReader = Weborb.Reader.JsonRPC.NullReader;


namespace Weborb.Protocols.JsonRPC
{
  class RequestParser : IMessageFactory
  {
    public const string MISSING_TARGET = "Target service to invoke wasn't specified in HTTP GET parameters";
    public const string TARGET_WASNOT_FOUND = "Target service wasn't found: ";
    public const string MISSING_METHOD = "Method to invoke wasn't specified";
    public const string INCORRECT_JSON = "Incorrect JSON";

    private static Dictionary<JsonTokenClass, IJsonReader> readers;

    static RequestParser()
    {
      readers = new Dictionary<JsonTokenClass, IJsonReader>();

      readers[ JsonTokenClass.Number ] = new NumberReader();
      readers[ JsonTokenClass.String ] = new Reader.JsonRPC.StringReader();
      readers[ JsonTokenClass.Boolean ] = new BooleanReader();
      readers[ JsonTokenClass.Object ] = new ObjectReader();
      readers[ JsonTokenClass.Array ] = new ArrayReader();
      readers[ JsonTokenClass.Null ] = new NullReader();
    }

    public string GetProtocolName( Request input )
    {
      return "json";
    }

    public string[] GetProtocolNames()
    {
      return new string[] { "json" };
    }

    public bool CanParse( string contentType )
    {
      return contentType.ToLower().Contains( "application/json" );
    }

    public Request Parse( Stream requestStream )
    {
      throw new NotImplementedException( "JSON RPC is not implemented" );
    }

    public static IAdaptingType Read( JsonReader reader )
    {
      return Read( reader.TokenClass, reader, new ParseContext( 3 ) );
    }

    public static IAdaptingType Read( JsonReader reader, ParseContext parseContext )
    {
      return Read( reader.TokenClass, reader, parseContext );
    }

    public static IAdaptingType Read( JsonTokenClass type, JsonReader reader, ParseContext parseContext )
    {
      if( !readers.ContainsKey( type ) )
      {
        if( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, string.Format( "Don't know how to import {0} from JSON.", type ) );
        return null;
      }

      IJsonReader jsonReader = readers[ type ];
      return jsonReader.read( reader, parseContext );
    }
  }
}
