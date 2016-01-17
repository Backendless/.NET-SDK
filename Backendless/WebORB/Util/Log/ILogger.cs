using System;

namespace Weborb.Util.Logging
{
	public interface ILogger
	{
		long getMask();

		void setMask( long mask );

		bool isLogging( long mask );

		bool isLogging( string category );

		void startLogging( string category );

		void stopLogging( string category );

        void setLogDateTime(string format);

        void setLogDateTime(bool isLogDateTime, string format);

        void setLogThreadName(bool logThreadName);

		void enable();

		void disable();

		bool isEnabled();

		void fireEvent( string category, object eventObj, DateTime timestamp );
	}
}
