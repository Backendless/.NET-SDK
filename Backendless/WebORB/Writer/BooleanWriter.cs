using System;
using System.IO;

using Weborb;

namespace Weborb.Writer
{
	public class BooleanWriter : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

		public override void write( object obj, IProtocolFormatter writer )
		{
            writer.WriteBoolean( (bool) obj );
		}

		#endregion
	}
}
