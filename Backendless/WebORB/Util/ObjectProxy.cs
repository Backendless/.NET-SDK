using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Weborb.Reader;
using Weborb.Protocols.Amf;
using Weborb.Util.IO;

namespace Weborb.Util
{
    public class ObjectProxy : IExternalizable
    {
        #region IExternalizable Members

        public object readExternal( FlashorbBinaryReader reader, ParseContext context )
        {
            return RequestParser.readData( reader, context );
        }

        #endregion
    }
}
