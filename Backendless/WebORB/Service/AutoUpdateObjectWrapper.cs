using System;

namespace Weborb.Service
{
	public class AutoUpdateObjectWrapper
	{
		internal IUpdateHandler handler;
		internal object obj;

		public AutoUpdateObjectWrapper( IUpdateHandler handler, object obj )
		{
			this.handler = handler;
			this.obj = obj;
		}
	}
}
