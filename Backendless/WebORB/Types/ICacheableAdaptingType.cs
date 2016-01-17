using System;
using System.Collections;
using System.Text;

using Weborb.Reader;

namespace Weborb.Types
{
  public interface ICacheableAdaptingType : IAdaptingType
  {
    bool IsAdapting { get; set; }
    object defaultAdapt( ReferenceCache refCache );
    object adapt( Type type, ReferenceCache refCache );
    IAdaptingType getCacheKey();
  }
}
