using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Types.Generic
  {
  public class WebORBVector<T> : List<T>, IWebORBVector<T>
    {
    public WebORBVector()
      : base()
      {
      }

    public WebORBVector( IEnumerable<T> en )
      : base( en )
      {
      }

    public WebORBVector( int capacity )
      : base( capacity )
      {
      }
    }
  }