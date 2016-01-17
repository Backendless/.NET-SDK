using System;
using Weborb.Client;
using Weborb.Util;
using Weborb.Reader;
using Weborb.Types;
using Weborb.Util.Logging;

namespace Weborb.V3Types
{
	public class BodyHolderFactory : IArgumentObjectFactory
	{
		#region IArgumentObjectFactory Members

		public object createObject( IAdaptingType argument )
		{
			BodyHolder bodyObj = new BodyHolder();
      Object arg = argument;

      if( argument is ArrayType )
        arg = ((ArrayType) argument).getArray();

      bodyObj.body = new Object[] { arg };
      return bodyObj;
		}
		#endregion
	}
}
