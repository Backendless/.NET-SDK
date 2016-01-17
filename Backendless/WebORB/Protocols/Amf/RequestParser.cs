using System;
using System.IO;
using System.Text;

using Weborb.Message;
using Weborb.Types;
using Weborb.Exceptions;
using Weborb.Util.IO;
using Weborb.Util.Logging;
using Weborb.Reader;
using Weborb.Protocols;
using Weborb.Writer;
using Weborb.Writer.Amf;

namespace Weborb.Protocols.Amf
{
  public class RequestParser : IMessageFactory
  {
    private static ITypeReader[] V1READERS;
    private static ITypeReader[] V3READERS;
    private static ITypeReader[][] READERS;
#if (FULL_BUILD)
    ASCIIEncoding encoding = new ASCIIEncoding();
#endif
    static RequestParser()
    {
      //TODO: review caching this or making it static
      V1READERS = new ITypeReader[Datatypes.TOTAL_V1TYPES];
      V1READERS[Datatypes.NUMBER_DATATYPE_V1] = new NumberReader();
      V1READERS[Datatypes.BOOLEAN_DATATYPE_V1] = new BooleanReader();
      V1READERS[Datatypes.UTFSTRING_DATATYPE_V1] = new UTFStringReader();
      V1READERS[Datatypes.OBJECT_DATATYPE_V1] = new AnonymousObjectReader();
      V1READERS[Datatypes.NULL_DATATYPE_V1] = new NullReader();
      V1READERS[Datatypes.POINTER_DATATYPE_V1] = new PointerReader();
      V1READERS[Datatypes.OBJECTARRAY_DATATYPE_V1] = new BoundPropertyBagReader();
      V1READERS[Datatypes.ENDOFOBJECT_DATATYPE_V1] = new NotAReader();
      V1READERS[Datatypes.UNKNOWN_DATATYPE_V1] = new UndefinedTypeReader();
      V1READERS[Datatypes.ARRAY_DATATYPE_V1] = new ArrayReader();
      V1READERS[Datatypes.DATE_DATATYPE_V1] = new DateReader();
      V1READERS[Datatypes.LONGUTFSTRING_DATATYPE_V1] = new LongUTFStringReader();
#if (FULL_BUILD)
      V1READERS[Datatypes.REMOTEREFERENCE_DATATYPE_V1] = new RemoteReferenceReader();
      V1READERS[Datatypes.PARSEDXML_DATATYPE_V1] = new XmlReader();
#endif
      V1READERS[Datatypes.RECORDSET_DATATYPE_V1] = null;
      V1READERS[Datatypes.NAMEDOBJECT_DATATYPE_V1] = new NamedObjectReader();
      V1READERS[Datatypes.V3_DATATYPE] = new V3Reader();

      V3READERS = new ITypeReader[Datatypes.TOTAL_V3TYPES];
      V3READERS[Datatypes.UNKNOWN_DATATYPE_V3] = new UndefinedTypeReader();
      V3READERS[Datatypes.NULL_DATATYPE_V3] = new NullReader();
      V3READERS[Datatypes.BOOLEAN_DATATYPE_FALSEV3] = new BooleanReader(false);
      V3READERS[Datatypes.BOOLEAN_DATATYPE_TRUEV3] = new BooleanReader(true);
      V3READERS[Datatypes.INTEGER_DATATYPE_V3] = new IntegerReader();
      V3READERS[Datatypes.DOUBLE_DATATYPE_V3] = new NumberReader();
      V3READERS[Datatypes.UTFSTRING_DATATYPE_V3] = new V3StringReader();
      V3READERS[Datatypes.DATE_DATATYPE_V3] = new V3DateReader();
      V3READERS[Datatypes.ARRAY_DATATYPE_V3] = new V3ArrayReader();
      V3READERS[Datatypes.OBJECT_DATATYPE_V3] = new V3ObjectReader();
#if (FULL_BUILD)
      V3READERS[Datatypes.LONGXML_DATATYPE_V3] = new V3XmlReader();
      V3READERS[Datatypes.XML_DATATYPE_V3] = new V3XmlReader();
#endif
      V3READERS[Datatypes.BYTEARRAY_DATATYPE_V3] = new V3ByteArrayReader();

      V3READERS[Datatypes.INT_VECTOR_V3] = new V3VectorReader<int>();
      V3READERS[Datatypes.UINT_VECTOR_V3] = new V3VectorReader<uint>();
      V3READERS[Datatypes.DOUBLE_VECTOR_V3] = new V3VectorReader<double>();
      V3READERS[Datatypes.OBJECT_VECTOR_V3] = new V3VectorReader<object>();
      V3READERS[Datatypes.V3_DATATYPE] = new V3Reader();

      READERS = new ITypeReader[4][];
      READERS[0] = V1READERS;
      READERS[1] = null;
      READERS[2] = null;
      READERS[3] = V3READERS;
    }

    public static void setAMF3Reader(int dataType, ITypeReader reader)
    {
      V3READERS[dataType] = reader;
    }

    public static ITypeReader getAMF3Reader(int dataType)
    {
      return V3READERS[dataType];
    }

    public string GetProtocolName(Request input)
    {
      return "amf" + (int)input.getVersion();
    }

    public string[] GetProtocolNames()
    {
      return new string[] { "amf0", "amf3" };
    }

    public Request readMessage(Stream input)
    {
      FlashorbBinaryReader reader = new FlashorbBinaryReader(input);

      try
      {
        if (Log.isLogging(LoggingConstants.DEBUG))
          Log.log(LoggingConstants.DEBUG, "MessageReader:: parsing stream");

        int version = reader.ReadUnsignedShort();
        int totalHeaders = reader.ReadUnsignedShort();

        if (Log.isLogging(LoggingConstants.DEBUG))
          Log.log(LoggingConstants.DEBUG, "MessageReader:: parsing message - version: " + version + " totalHeaders: " + totalHeaders);

        Header[] headers = new Header[totalHeaders];

        for (int i = 0; i < totalHeaders; i++)
          headers[i] = readHeader(reader);

        int totalBodyParts = reader.ReadUnsignedShort();

        if (Log.isLogging(LoggingConstants.DEBUG))
          Log.log(LoggingConstants.DEBUG, "MessageReader:: Total body parts: " + totalBodyParts);

        Body[] bodies = new Body[totalBodyParts];

        for (int i = 0; i < totalBodyParts; i++)
          bodies[i] = readBodyPart(reader);

        if (Log.isLogging(LoggingConstants.DEBUG))
          Log.log(LoggingConstants.DEBUG, "MessageReader:: returning AMFMessage");

        Request request = new Request(version, headers, bodies);
        request.SetFormatter(version == 3 ? (IProtocolFormatter)new AmfV3Formatter() : (IProtocolFormatter)new AmfFormatter());
        return request;
      }
      catch (Exception exception)
      {
        if (Log.isLogging(LoggingConstants.EXCEPTION))
          Log.log(LoggingConstants.EXCEPTION, "Exception: " + exception.Message + " StackTrace: " + exception.StackTrace);
        return null;
      }
    }

    private Header readHeader(FlashorbBinaryReader reader)
    {
      int nameLength = reader.ReadUnsignedShort();
      byte[] bytes = reader.ReadBytes(nameLength);
#if (FULL_BUILD)
      string headerName = encoding.GetString(bytes);
#else 
            string headerName = BitConverter.ToString( bytes );
#endif
      bool mustUnderstand = reader.ReadBoolean();
      //int length = reader.ReadInt32();
      int length = reader.ReadInteger();

      if (Log.isLogging(LoggingConstants.DEBUG))
        Log.log(LoggingConstants.DEBUG, "MessageReader::readHeader: name - " + headerName + " mustUnderstand - " + mustUnderstand + " length - " + length);

      return new Header(headerName, mustUnderstand, length, readData(reader));
    }

    private Body readBodyPart(FlashorbBinaryReader reader)
    {
      int serviceURILength = reader.ReadUnsignedShort();
#if (FULL_BUILD)
      string serviceURI = encoding.GetString(reader.ReadBytes(serviceURILength));
#else
            string serviceURI = BitConverter.ToString( reader.ReadBytes( serviceURILength ) );
#endif
      int responseURILength = reader.ReadUnsignedShort();
#if (FULL_BUILD)
      string responseURI = encoding.GetString(reader.ReadBytes(responseURILength));
#else
            string responseURI = BitConverter.ToString( reader.ReadBytes( responseURILength ) );
#endif
      int length = reader.ReadInteger();

      if (Log.isLogging(LoggingConstants.DEBUG))
        Log.log(LoggingConstants.DEBUG, "MessageReader::readBodyPart: serviceURI - " + serviceURI + " responseURI - " + responseURI + " length: " + length);

      return new Body(serviceURI, responseURI, length, readData(reader));
    }

    public static IAdaptingType readData(FlashorbBinaryReader reader)
    {
      return readData(reader, new ParseContext(0), V1READERS);
    }

    public static IAdaptingType readData(FlashorbBinaryReader reader, int version)
    {
      return readData(reader, new ParseContext(version), READERS[version]);
    }

    public static IAdaptingType readData(FlashorbBinaryReader reader, ParseContext parseContext)
    {
      return readData(reader, parseContext, READERS[parseContext.getVersion()]);
    }

    public static IAdaptingType readData(FlashorbBinaryReader reader, ParseContext parseContext, ITypeReader[] readers)
    {
      int type = reader.ReadByte();
      return readers[type].read(reader, parseContext);
    }

    public static IAdaptingType readData(int dataType, FlashorbBinaryReader reader, ParseContext parseContext)
    {
      return readData(dataType, reader, parseContext, READERS[parseContext.getVersion()]);
    }

    public static IAdaptingType readData(int dataType, FlashorbBinaryReader reader, ParseContext parseContext, ITypeReader[] readers)
    {
      return readers[dataType].read(reader, parseContext);
    }

    #region IMessageFactory Members

    public bool CanParse(String contentType)
    {
      return contentType.ToLower().Equals("application/x-amf");
    }

    public Request Parse(Stream requestStream)
    {
      return readMessage(requestStream);
    }

    #endregion
  }
}