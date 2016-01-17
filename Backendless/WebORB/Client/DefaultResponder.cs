using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Client
{
    public class DefaultResponder<T> : Responder<T>
    {
        public DefaultResponder()
            : base(delegate(T adaptedObject) { },
                    delegate(Fault fault) { })
        { }
    }
}
