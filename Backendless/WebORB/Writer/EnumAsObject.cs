using System;
using System.Collections;
using System.Text;

using Weborb.Writer.Specialized;

namespace Weborb.Writer
{
    public class EnumAsObjectWriter : EnumeratorWriter
    {
        private ITypeWriter referenceWriter = new ObjectReferenceWriter();

        public override void write( object obj, IProtocolFormatter writer )
        {
            Enum enumeration = (Enum) obj;
            string numberValue = Enum.Format( obj.GetType(), obj, "d" );
            int enumNumber = int.Parse( numberValue );

            Type undertype = Enum.GetUnderlyingType( enumeration.GetType() );
            String stringValue = Enum.GetName( obj.GetType(), Convert.ChangeType( enumeration, undertype ) );

            EnumValue enumWrapper = new EnumValue();
            enumWrapper.name = stringValue;
            enumWrapper.code = enumNumber;

            MessageWriter.writeObject( enumWrapper, writer );
        }
    }
}
