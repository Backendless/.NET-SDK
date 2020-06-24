using System;
using System.Globalization;

namespace Weborb.Writer
{
	public class EnumerationWriter : AbstractUnreferenceableTypeWriter
	{
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
        {
            Enum enumeration = (Enum) obj;
            Type undertype = Enum.GetUnderlyingType( enumeration.GetType() );
            Type enumType = obj.GetType();
            Object convertedToType = Convert.ChangeType( enumeration, undertype, CultureInfo.InvariantCulture );
            String enumName = Enum.GetName( enumType, convertedToType );
            MessageWriter.writeObject( enumName, writer );
        }

        #endregion
    }
}
