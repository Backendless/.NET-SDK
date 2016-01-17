using System;
using Weborb;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
	public class NullWriter : AbstractUnreferenceableTypeWriter
	{
		#region ITypeWriter Members

        public override void write( object obj, IProtocolFormatter writer )
		{
            writer.WriteNull();
		}

		#endregion
	}
}
