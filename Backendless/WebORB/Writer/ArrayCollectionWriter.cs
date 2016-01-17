using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Weborb.Types;

namespace Weborb.Writer
{
    public class ArrayCollectionWriter : AbstractReferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
        {
            IWebORBArrayCollection collection = (IWebORBArrayCollection) obj;
            List<Object> array = new List<Object>();
            IEnumerator en = collection.GetEnumerator();

            while( en.MoveNext() )
                array.Add( en.Current );

            Dictionary<String, object> fields = new Dictionary<String, object>();
            fields[ "source" ] = array.ToArray();
            writer.GetObjectSerializer().WriteObject( "flex.messaging.io.ArrayCollection", fields, writer );
        }

        #endregion
    }
}
