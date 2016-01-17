using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    public class EnumAsNumberWriter : EnumerationWriter
    {
        public override void write( object obj, IProtocolFormatter writer )
        {
            Enum enumeration = (Enum) obj;
            string numberValue = Enum.Format( obj.GetType(), obj, "d");
            int enumNumber = int.Parse( numberValue );
            MessageWriter.writeObject( enumNumber, writer ); ;
        }
    }
}
