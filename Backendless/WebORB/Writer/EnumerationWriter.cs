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
            MessageWriter.writeObject( Enum.GetName( obj.GetType(), Convert.ChangeType( enumeration, undertype, CultureInfo.InvariantCulture ) ), writer );;
        }

        #endregion
    }
}
