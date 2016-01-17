using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Types.Generic
  {
  class WebORBArray<T> : List<T>, IWebORBArray
    {
    public WebORBArray()
      : base()
      {
      }

    public WebORBArray( IEnumerable<T> en )
      : base( en )
      {
      }

    public WebORBArray( int capacity )
      : base( capacity )
      {
      }
    }
  }
