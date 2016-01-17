using System;

namespace Weborb.Protocols
{
	public class UnknownRequestFormatException : Exception
	{
		public UnknownRequestFormatException( string errorMessage ) : base( errorMessage )
		{
		}
	}
}
