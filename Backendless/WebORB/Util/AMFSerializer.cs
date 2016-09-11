using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Util.IO;
using Weborb.Writer;
using Weborb.Writer.Amf;
#if (!UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
using Weborb.Writer.Wolf;
#endif
using System.IO;
using Weborb.Reader;
using Weborb.Types;

namespace Weborb.Util
{
    public class AMFSerializer
    {
        public static byte[] SerializeToBytes(object obj)
        {
            return SerializeToBytes(obj, AMF3);
        }
        public static byte[] SerializeToBytes(object obj, int serializationType)
        {
            IProtocolFormatter formatter;
            switch (serializationType)
            {
                case AMF0:
                    formatter = new AmfFormatter();
                    break;

#if (!UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
                case WOLF:
                    formatter = new WolfFormatter();
                    break;
#endif
                case AMF3:
                default:
                    formatter = new AmfV3Formatter();
                    break;
            }
            
            MessageWriter.writeObject(obj, formatter);
            ProtocolBytes protocolBytes = formatter.GetBytes();
            formatter.Cleanup();

            return protocolBytes.bytes;
        }

        public static object DeserializeFromBytes(byte[] bytes)
        {
            return DeserializeFromBytes(bytes, false);
        }
        public static object DeserializeFromBytes(byte[] bytes, bool doNotAdapt)
        {
            return DeserializeFromBytes(bytes, doNotAdapt, AMF3);
        }
        public static object DeserializeFromBytes(byte[] bytes, bool doNotAdapt, int serializationType)
        {
            MemoryStream ms = new MemoryStream(bytes);
            FlashorbBinaryReader reader = new FlashorbBinaryReader(ms);

            try
            {
                IAdaptingType type;
                
                switch(serializationType)
                {
#if (!UNIVERSALW8 && !SILVERLIGHT && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
                    case WOLF:
                        type = (IAdaptingType)Weborb.Protocols.Wolf.RequestParser.GetInstance().Parse(ms).getRequestBodyData();
                        break;
#endif
                    case AMF3:
                    case AMF0:
                    default:
                        type = Weborb.Protocols.Amf.RequestParser.readData(reader, 3);
                        break;
                }


                if (type != null)
                {
                    if (!doNotAdapt)
                        return type.defaultAdapt();
                    else
                        return type;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public const int AMF0 = 1;
        public const int AMF3 = 2;
#if (!SILVERLIGHT)
        public const int WOLF = 3;
#endif
    }
}
