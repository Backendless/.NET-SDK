using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Weborb.Writer
{
    public class EnumeratorWriter : AbstractReferenceableTypeWriter
    {
        #region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
        {
            if( obj is IDictionary )
            {
                ITypeWriter typeWriter = MessageWriter.getWriter( typeof( IDictionary ), formatter, false );
                typeWriter.write( obj, formatter );
                return;
            }

            IEnumerator en = (IEnumerator)obj;
            List<Object> arrayList = new List<Object>();

            while( en.MoveNext() )
                arrayList.Add( en.Current );
            
          object[] array = arrayList.ToArray();
          ITypeWriter writer = MessageWriter.getWriter( array.GetType(), formatter, false );
          writer.write( array, formatter );
        }

        #endregion
    }
}
