using System;
using System.IO;
using System.Collections;

using Weborb;
using Weborb.Util;
using Weborb.Reader;

namespace Weborb.Writer
{
	public class PropertyBagWriter : AbstractReferenceableTypeWriter
	{
		#region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
		{
			IDictionary propertyBag = ((AnonymousObject) obj).Properties;
			writer.BeginWriteObject( propertyBag.Count );

			foreach( object key in propertyBag.Keys )
			{
                writer.WriteFieldName( key.ToString() );
                writer.BeginWriteFieldValue();
				MessageWriter.writeObject( propertyBag[ key ], writer );
                writer.EndWriteFieldValue();
			}

            writer.EndWriteObject();
		}

		#endregion
	}
}
