using System;
using System.Collections;

namespace Weborb.Util
{
	public class IdentityComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			return object.ReferenceEquals( x, y ) ? 0 : 1;
		}

		#endregion
	}
}
