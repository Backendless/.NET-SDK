using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Types.Generic
{
    public class WebORBArrayCollection<T> : List<T>, IWebORBArrayCollection
    {
        public WebORBArrayCollection() : base()
        {
        }

        public WebORBArrayCollection( IEnumerable<T> en )
            : base( en )
        {
        }

        public WebORBArrayCollection( int capacity )
            : base( capacity )
        {
        }
    }
}
