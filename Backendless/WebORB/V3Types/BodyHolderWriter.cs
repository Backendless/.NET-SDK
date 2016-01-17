using System;
using System.Collections;
using Weborb.Util;
#if FULL_BUILD
using Weborb.Util.Cache;
#endif
using Weborb.Util.Logging;
using Weborb.Writer;

namespace Weborb.V3Types
  {
  public class BodyHolderWriter : AbstractUnreferenceableTypeWriter
    {
    #region ITypeWriter Members

    public override void write( object obj, IProtocolFormatter formatter )
      {
#if FULL_BUILD
      // write object and try to cache output if applicable
      Cache.WriteAndSave( ( (BodyHolder)obj ).body, formatter );
#else
      MessageWriter.writeObject( ((BodyHolder) obj).body, formatter );
#endif
      }

    #endregion
    }
  }
