using System;
using System.Collections;
using System.Text;

namespace Weborb.Writer.Specialized
{
    internal class TypedDictionary
    {
        internal IDictionary dictionary;
        internal String clientType;

        public TypedDictionary( IDictionary dictionary, String clientType )
        {
            this.dictionary = dictionary;
            this.clientType = clientType;
        }
    }
}
