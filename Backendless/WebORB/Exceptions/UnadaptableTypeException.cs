using System;

namespace Weborb.Exceptions
{
	public class UnadaptableTypeException : Exception
	{
		public UnadaptableTypeException( string errorMessage ) : base( errorMessage )
		{
		}
	}
}
