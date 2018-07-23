using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Weborb.Writer;
using Weborb.Writer.JsonRPC;
using Weborb.Protocols.Amf;
#if( !UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
using Weborb.Protocols.Wolf;
using Weborb.Writer.Wolf;
#endif
using Weborb.Writer.Amf;
using Weborb.Reader;
using Weborb.Protocols.JsonRPC;
using Weborb.Types;
using Weborb.Message;

namespace Weborb.Util.IO
{
  public class Serializer
  {
    public const int AMF0 = 0;
    public const int AMF3 = 1;
    public const int WOLF = 2;
    public const int JSON = 3;

    public static byte[] ToBytes( Object obj, int type )
    {
      IProtocolFormatter formatter = null;

      switch( type )
      {
        case AMF0:
          formatter = new AmfFormatter();
          break;

        case AMF3:
          formatter = new AmfV3Formatter();
          break;

        case JSON:
          formatter = new JsonRPCFormatter();
          break;
#if(!UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
                case WOLF:
                    formatter = new WolfFormatter();
                    break;
#endif
        default:
          throw new Exception( "Unknown protocol type" );
      }

      MessageWriter.writeObject( obj, formatter );
      ProtocolBytes bytes = formatter.GetBytes();
      formatter.Cleanup();

      if( bytes.bytes.Length != bytes.length )
      {
        byte[] result = new byte[ bytes.length ];
        Array.Copy( bytes.bytes, result, bytes.length );
        return result;
      }
      else
      {
        return bytes.bytes;
      }
    }

    public static object FromBytes( byte[] bytes, int type, bool doNotAdapt )
    {
      switch( type )
      {
        case AMF0:
        case AMF3:
          using( MemoryStream stream = new MemoryStream( bytes ) )
          {
            using( FlashorbBinaryReader reader = new FlashorbBinaryReader( stream ) )
            {
              IAdaptingType adpatingType = Weborb.Protocols.Amf.RequestParser.readData( reader, type == AMF0 ? 0 : 3 );

              if( doNotAdapt )
                return adpatingType;
              else
                return adpatingType.defaultAdapt();
            }
          }
        case JSON:
          using( MemoryStream stream = new MemoryStream( bytes ) )
          {
            StreamReader streamReader = new StreamReader( stream, Encoding.UTF8 );

            using( JsonTextReader jsonReader = new JsonTextReader( streamReader ) )
            {
              jsonReader.Read();
              IAdaptingType jsonType = Weborb.Protocols.JsonRPC.RequestParser.Read( jsonReader );

              if( doNotAdapt )
                return jsonType;
              else
                return jsonType.defaultAdapt();
            }
          }
#if(!UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
          case WOLF:

          using( MemoryStream stream = new MemoryStream( bytes ) )
                {
                    Weborb.Protocols.Wolf.RequestParser parser = Weborb.Protocols.Wolf.RequestParser.GetInstance();
                    Request requestObj = parser.Parse( stream );
                    return requestObj.getRequestBodyData();
                }
          break;
#endif
        default:
          throw new Exception( "Unknown formatting type" );
      }
    }
  }
}
