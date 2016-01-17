using System;
using System.Collections.Generic;
using System.Text;

namespace Weborb.V3Types.Core
{
    public class RemotingDestination : AbstractDestination
    {
        internal String serviceId;
        internal String destinationId;

        public RemotingDestination( String destinationId, String serviceId )
        {
            this.destinationId = destinationId;
            this.serviceId = serviceId;
        }
    }
}
