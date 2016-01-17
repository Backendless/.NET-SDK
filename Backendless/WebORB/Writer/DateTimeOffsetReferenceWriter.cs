using System;
namespace Weborb.Writer
{
  class DateTimeOffsetReferenceWriter : ITypeWriter
  {
    #region Implementation of ITypeWriter

    public void write(object obj, IProtocolFormatter formatter)
    {
      ReferenceCache referenceCache = formatter.GetReferenceCache();
      DateTime date = ((DateTimeOffset)obj).DateTime;
      int refId = referenceCache.GetObjectId( date.ToUniversalTime() );

      if ( refId != -1 )
      {
        formatter.WriteDateReference( refId );
      }
      else
      {
        referenceCache.AddObject( date.ToUniversalTime() );
        formatter.getContextWriter().write( obj, formatter );
      }
    }

    public ITypeWriter getReferenceWriter()
    {
      return null;
    }

    #endregion
  }
}
