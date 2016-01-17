using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Types;
using Weborb.Protocols.Amf;
using Weborb.Util.IO;

namespace Weborb.Reader
{
    public class SkipTypeReader : ITypeReader
    {
        #region ITypeReader Members

        public IAdaptingType read( FlashorbBinaryReader reader, ParseContext parseContext )
        {
            return RequestParser.readData( reader, parseContext );
        }

        #endregion
    }
}
