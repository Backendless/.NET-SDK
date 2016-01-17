using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Weborb.Util
{
    public class ORBPerformanceCounter
    {
        private const string CATEGORY = "WebORB";

        public const String OPENED_CONNECTIONS = "Open connections";
        public const String LAST_INVOCATION = "Last invocation time (ms)";
        public const String AVERAGE_INVOCATION = "Average invocation time (ms)";
        public const String INVOCATION_COUNT = "Invocation count (times)";
        public const String SLOWEST_INVOCATION = "Slowest invocation time (ms)";
        public const String FASTEST_INVOCATION = "Fastest invocation time (ms)";
        public const String OPEN_RTMP_CONNECTIONS = "Open RTMP connections";

        public static PerformanceCounter getCounter( String counterName )
        {
            return getCounter( counterName, null );
        }

        public static PerformanceCounter getCounter( String counterName, String instanceName )
        {
            try
            {
                if( !PerformanceCounterCategory.CounterExists( counterName, CATEGORY ) )
                {
                    PerformanceCounterCategory.Create(
                        CATEGORY,
                        CATEGORY,
                        instanceName == null ? PerformanceCounterCategoryType.SingleInstance : PerformanceCounterCategoryType.MultiInstance,
                        counterName,
                        String.Empty );
                }

                if( instanceName == null )
                    return new PerformanceCounter( CATEGORY, counterName, false );
                else
                    return new PerformanceCounter( CATEGORY, counterName, instanceName, false );
            }
            catch( Exception )
            {
                return null;
            }
        }
    }
}
