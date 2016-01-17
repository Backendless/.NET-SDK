using System;
using Weborb.Types;
using Weborb.Util.IO;
using Weborb.Util.Logging;

namespace Weborb.Reader
{
	public class V3DateReader : ITypeReader
	{
		#region ITypeReader Members

		public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
		{
			int refId = reader.ReadVarInteger();

			if( (refId & 0x1) == 0 )
				return (DateType) parseContext.getReference( refId >> 1 );

			double dateTime = reader.ReadDouble();

            DateTime sent = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

            try
            {
                sent = sent.AddMilliseconds(dateTime).ToLocalTime();
            }
            catch(Exception e)
            {
                if (Log.isLogging(LoggingConstants.EXCEPTION))
                    Log.log(LoggingConstants.EXCEPTION, e);

                sent = DateTime.MinValue;
            }

			DateType dateType = new DateType( sent );
			parseContext.addReference( dateType );
			return dateType;
		}

		#endregion
	}
}
