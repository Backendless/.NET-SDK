using System;
using System.Collections;
using Weborb.Util;
using Weborb.Config;

namespace Weborb.V3Types.Core
{
	public class DataServices
	{
		private Hashtable adapters = new Hashtable();
        private DestinationManager destinationManager = new DestinationManager();
		private Hashtable destinations = new Hashtable();
		private IAdapter defaultAdapter;

		public static void AddAdapter( string id, IAdapter adapter, bool isDefault )
		{
            //ThreadContext.getORBConfig().getDataServices()._AddAdapter( id, adapter, isDefault );
            ORBConfig.GetInstance().GetDataServices()._AddAdapter( id, adapter, isDefault );
		}

        public void _AddAdapter( string id, IAdapter adapter, bool isDefault )
		{
			if( isDefault )
                defaultAdapter = adapter;

            adapters[ id ] = adapter;
		}

		public static IAdapter GetAdapter( string id )
		{
			//return ThreadContext.getORBConfig().getDataServices()._GetAdapter( id );
            return ORBConfig.GetInstance().GetDataServices()._GetAdapter( id );
		}

		public IAdapter _GetAdapter( string id )
		{
            if( adapters.ContainsKey( id ) )
                return (IAdapter) adapters[ id ];

            return defaultAdapter;
		}

		public static IAdapter GetDefaultAdapter()
		{
			//return ThreadContext.getORBConfig().getDataServices()._GetDefaultAdapter();
            return ORBConfig.GetInstance().GetDataServices()._GetDefaultAdapter();
		}

		public IAdapter _GetDefaultAdapter()
		{
			return defaultAdapter;
		}

        internal DestinationManager GetDestinationManager()
        {
            return destinationManager;
        }
	}
}
