using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Reader;
using Weborb.Util.IO;

namespace Weborb.Util
{
    public interface IExternalizable
    {
        object readExternal( FlashorbBinaryReader reader, ParseContext context );
    }
}
