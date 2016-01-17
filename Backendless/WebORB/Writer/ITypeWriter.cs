using System;

namespace Weborb.Writer
{
	public interface ITypeWriter
	{
		void write( object obj, IProtocolFormatter formatter );
        ITypeWriter getReferenceWriter();
	}
}
