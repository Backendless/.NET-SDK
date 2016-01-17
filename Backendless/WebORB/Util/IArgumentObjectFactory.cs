using System;
using System.Collections;
using Weborb.Types;

namespace Weborb.Util
{
	public interface IArgumentObjectFactory
	{
		object createObject( IAdaptingType argument );
	}
}
