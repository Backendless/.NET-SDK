using System;
using System.Collections;

namespace Weborb.Writer
{
	public interface IObjectSerializer
	{
		void WriteObject( string className, IDictionary objectFields, IProtocolFormatter writer );
	}
}
