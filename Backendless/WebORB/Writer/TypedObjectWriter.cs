using System;
using System.Collections;
using Weborb.Types;

namespace Weborb.Writer
{
	public class TypedObjectWriter : AbstractReferenceableTypeWriter
	{
		#region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter formatter )
		{
			TypedObject typedObject = (TypedObject) obj;
			formatter.GetObjectSerializer().WriteObject( typedObject.typeName, typedObject.objectData, formatter );
		}

		#endregion
	}
}
