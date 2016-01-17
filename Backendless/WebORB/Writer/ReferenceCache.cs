using System;
using System.Collections;
using System.Collections.Generic;
using Weborb.Util;

namespace Weborb.Writer
{
	public class ReferenceCache
	{
		protected static IComparer identityComparer = new IdentityComparer();
		private Dictionary<Object, int> cache;

		public ReferenceCache()
		{
			//cache = new Hashtable( null, identityComparer );
            cache = new Dictionary<Object, int>();
		}

		public virtual void Reset()
		{
			cache.Clear();
		}

		public virtual void AddObject( object obj )
		{
			cache[ obj ] = cache.Count;
		}

        public virtual void AddString( object obj )
        {
            cache[ obj ] = cache.Count;
        }

        public virtual int GetStringId( object obj )
        {
            return GetId( obj );
        }

        public virtual int GetObjectId( object obj )
        {
            return GetId( obj );
        }

		private int GetId( object obj )
		{
            int id;

            if( cache.TryGetValue( obj, out id ) )
                return id;

            return -1;
		}
	}
}
