using System;

namespace Weborb.V3Types
{
#if (FULL_BUILD)
    [Serializable()]
#endif
	public class BodyHolder
	{
		private object _body;

		public object body
		{
			get
			{
				return _body;
			}

			set
			{
				this._body = value;
			}
		}
	}
}
