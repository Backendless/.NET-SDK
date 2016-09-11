using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Weborb.Writer;
using Weborb.Protocols.Amf;
#if( !UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
using Weborb.Protocols.Wolf;
using Weborb.Writer.Wolf;
#endif
using Weborb.Writer.Amf;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Message;

namespace Weborb.Util.IO
{
    public class Serializer
    {
        public const int AMF0 = 0;
        public const int AMF3 = 1;
        public const int WOLF = 2;

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
#if( !UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
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
#if( !UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
            if( type == AMF0 || type == AMF3 )
            {
#endif
            using ( MemoryStream stream = new MemoryStream( bytes ) )
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
#if( !UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
           }
            else
            {
                using( MemoryStream stream = new MemoryStream( bytes ) )
                {
                    Weborb.Protocols.Wolf.RequestParser parser = Weborb.Protocols.Wolf.RequestParser.GetInstance();
                    Request requestObj = parser.Parse( stream );
                    return requestObj.getRequestBodyData();
                }
            }
#endif
        }
    }
}
