using System;
using System.Collections;
using System.Text;
using Weborb.Util;
using Weborb.Util.IO;
using Weborb.Reader;
using Weborb.Protocols.Amf;

namespace Weborb.Types
{
    [Serializable]
    public class WebORBArrayCollection : ArrayList, IWebORBArrayCollection, IExternalizable
    {
        private ICollection internalCollection;

        public WebORBArrayCollection() 
            : base()
        {
        }

        public WebORBArrayCollection( ICollection collection )
            : base( collection )
        {
            this.internalCollection = collection;
        }

        public WebORBArrayCollection( int capacity )
            : base( capacity )
        {
        }

        public ICollection InternalCollection
        {
            get { return internalCollection; }
        }


        public object readExternal( FlashorbBinaryReader reader, ParseContext context )
        {
          ArrayType arrayType = (ArrayType) RequestParser.readData( reader, context );
          object array = arrayType.defaultAdapt();
          Object[] arrayCopy = new Object[ ((ICollection) array).Count ];
          ((ICollection) array).CopyTo( arrayCopy, 0 );
          return new ArrayCollectionType( arrayCopy, arrayType );
          //return new WebORBArrayCollection( (ICollection) array );
        }
    }
}
