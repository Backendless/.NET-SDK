using System;

namespace Weborb.Exceptions
{
	public class InspectionException : Exception
	{
		public static string message = "Unable to inspect service ";

		public InspectionException( string service ) : base( message + service )
		{
		}
	}
}
