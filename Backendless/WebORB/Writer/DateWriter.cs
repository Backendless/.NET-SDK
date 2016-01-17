using System;
using System.IO;
using Weborb;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
  public class DateWriter : ITypeWriter
  {
    private bool isReferenceable;
    protected ITypeWriter referenceWriter;

    public DateWriter( bool isReferenceable )
    {
      referenceWriter = new DateReferenceWriter();
      this.isReferenceable = isReferenceable;
    }

    #region ITypeWriter Members

    public virtual void write( object obj, IProtocolFormatter writer )
    {
      DateTime date = (DateTime)obj;
      writer.WriteDate( date );
    }

    public ITypeWriter getReferenceWriter()
    {
      return isReferenceable ? referenceWriter : null;
    }

    #endregion
  }
}
