using System;
using System.Globalization;
using System.IO;
using Weborb.Util.IO;
using Weborb.Util.Logging;
using Weborb.Types;

namespace Weborb.Reader
{
	public class DateReader : ITypeReader
	{
		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
            double dateTime = reader.ReadDouble();
            // ignore the stupid timezone
            reader.ReadUnsignedShort();

            DateTime sent = new DateTime( 1970, 1, 1 );

#if (FULL_BUILD || PURE_CLIENT_LIB )
            // get the offset of the time zone the server is in
            double localTimezoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset( sent ).TotalMilliseconds;
            // convert 1/1/1970 12AM to UTC
            sent = TimeZone.CurrentTimeZone.ToUniversalTime( sent );
#else
            double localTimezoneOffset = TimeZoneInfo.Local.GetUtcOffset( sent ).TotalMilliseconds;
            sent = TimeZoneInfo.ConvertTime( sent, TimeZoneInfo.Utc );
#endif
            
            // bring it back to 12AM
            sent = sent.AddMilliseconds( localTimezoneOffset );

            // now that the sent object is in UTC and it represents 1/1/1970 12AM
            // convert it to the time sent by the client. The result of the operation
            // is going to be client's datetime object in UTC
            sent = sent.AddMilliseconds( dateTime );

            return new DateType( sent );
		}	
	}
}
