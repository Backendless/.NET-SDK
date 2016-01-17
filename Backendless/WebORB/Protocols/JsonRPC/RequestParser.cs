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
      readers[JsonTokenClass.Null] = new NullReader();  
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
      return contentType.Contains( "application/json" );
      }

    public Request Parse( Stream requestStream )
      {
      Header id = new Header();
      id.headerName = "id";
      Header version = new Header();
      version.headerName = "version";
      version.headerValue = new StringType( "1.0" );
      Header notification = new Header();
      notification.headerName = "notification";           
      Header inspection = new Header();
      inspection.headerName = ORBConstants.DESCRIBE_SERVICE;

      Header credentials = null;

      Body body = new Body();
      string methodName = null;

      HttpRequest request = (HttpRequest)ThreadContext.currentRequest();
      string targetName = request.QueryString[ "target" ];

      try
        {        
        JsonTextReader reader = new JsonTextReader( new StreamReader( requestStream )  );
        reader.ReadToken( JsonTokenClass.Object );

        while ( reader.TokenClass != JsonTokenClass.EndObject )
          {
          string memberName = reader.ReadMember();

          switch ( memberName )
            {
            case "version":
            case "jsonrpc":
              {                
              version.headerValue = Read( reader );
              break;
              }

            case "id":
              {                           
              id.headerValue = Read( reader );
              break;
              }

            case "method":
              {
              methodName = reader.ReadString();              
              break;
              }

            case "credentials":
              {
                  credentials = new Header();
                  credentials.headerName = "Credentials";
                  credentials.headerValue = Read(reader);
                 
                  break;
              }

            case "params":
              {
              body.dataObject = ((ArrayType)Read( reader )).getArray();              
              break;
              }

            case "kwparams":
              throw new Exception( "Named parameters (kwparams) are not supported in JSON-RPC" );                

            default:
              {
              reader.SkipItem();
              break;
              }
            }
          }

        reader.Read();   
        }
      catch ( Exception e )
        {
        if ( Log.isLogging( LoggingConstants.EXCEPTION ) )
          Log.log( LoggingConstants.EXCEPTION, "Error while parsing JSON input stream", e );

        throw new Exception( INCORRECT_JSON, e );
        }

      if ( String.IsNullOrEmpty( methodName ) )
        throw new Exception( MISSING_METHOD );

      // prepare header

      bool isNotification = false;
      string ver = (string)version.headerValue.defaultAdapt();
            
      if ( id.headerValue != null )
        {
        if ( ver == "1.0" )
          isNotification = id.headerValue is NullType;          
        }       
      else if ( ver == "2.0" )
        isNotification = true;

      notification.headerValue = new BooleanType( isNotification );

      Header[] headers;

      if ( methodName == "systemDescribe" ) 
        headers = new Header[] { id, version, notification, inspection };
      else
        headers = new Header[] { id, version, notification };

        if(credentials != null)
           headers[headers.Length - 1] = credentials;

      // prepare body

      if ( body.dataObject == null )
        body.dataObject = new object[0];

      if ( String.IsNullOrEmpty( methodName ) )
        methodName = request.QueryString[ "method" ];      

      body.serviceURI = targetName + "." + methodName;

      Request message = new Request( 1, headers , new Body[] { body } );
      message.SetFormatter( new JsonRPCFormatter() );

      return message;
      }

    public static IAdaptingType Read( JsonReader reader )
      {
      return Read( reader.TokenClass, reader );
      }

    public static IAdaptingType Read( JsonTokenClass type, JsonReader reader )
      {
      if ( !readers.ContainsKey( type ) )
        {
        if ( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, string.Format( "Don't know how to import {0} from JSON.", type ) );
        return null;
        }

      IJsonReader jsonReader = readers[ type ];      
      return jsonReader.read( reader );
      } 
    }
  }
