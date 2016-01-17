using System;
using System.Collections;
using System.Text;

namespace Weborb.Writer.Specialized
{
    class TypedDictionaryWriter : AbstractReferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
        {
            TypedDictionary typedDictionary = (TypedDictionary) obj;
            IDictionary dict = typedDictionary.dictionary;
            Hashtable hashtable = dict is Hashtable ? (Hashtable) dict : new Hashtable( (IDictionary) dict );
            writer.GetObjectSerializer().WriteObject( typedDictionary.clientType, hashtable, writer );
        }

        #endregion
    }
}
