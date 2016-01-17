using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Weborb.Data.Management;
using Weborb.Util;

namespace Weborb.Writer
{
    public class DomainObjectWriter : ObjectWriter
    {
        protected override void onWriteObject( Object obj, string className, IDictionary objectFields, IProtocolFormatter writer )
        {
            if( ((DomainObject) obj).getSerializeAsSimpleObject() )
                base.onWriteObject( obj, null, objectFields, writer );
            else
                base.onWriteObject( obj, className, objectFields, writer );
        }
    }
}
