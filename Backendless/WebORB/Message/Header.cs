using System;

using Weborb.Types;

namespace Weborb.Message
{
	public class Header
	{
		internal string headerName;
        internal bool mustUnderstand;
        internal IAdaptingType headerValue;

    public Header()
    {      
    }

		public Header( String headerName, bool mustUnderstand, int length, IAdaptingType dataObject )
		{
			this.headerName = headerName;
			this.mustUnderstand = mustUnderstand;
			this.headerValue = dataObject;
		}
	}
}
