using System;

namespace Weborb.Writer
{
  public class DateTimeOffsetWriter : DateWriter
  {
    public DateTimeOffsetWriter(bool isReferenceable) : base(isReferenceable)
    {
      referenceWriter = new DateTimeOffsetReferenceWriter();
    }

    public override void write( object obj, IProtocolFormatter writer )
    {
      DateTime date = ((DateTimeOffset)obj).DateTime;
      writer.WriteDate( date );
    }
  }
}
