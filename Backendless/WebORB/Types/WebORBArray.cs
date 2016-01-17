using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Types
  {
  [Serializable]
  class WebORBArray : ArrayList, IWebORBArray
    {
    private ICollection internalCollection;

    public WebORBArray()
      : base()
      {
      }

    public WebORBArray( ICollection collection )
      : base( collection )
      {
      this.internalCollection = collection;
      }

    public WebORBArray( int capacity )
      : base( capacity )
      {
      }

    public ICollection InternalCollection
      {
      get { return internalCollection; }
      }
    }
  }
