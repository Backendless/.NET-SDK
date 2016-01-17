using System;
using System.Collections.Generic;
using System.Text;
using Weborb.Types;

namespace Weborb.Reader
{
    public class ReferenceCache
    {
        private Dictionary<IAdaptingType, Dictionary<Type, Object>> cache = new Dictionary<IAdaptingType, Dictionary<Type, object>>();

        public bool HasObject(IAdaptingType key)
        {
            return HasObject( key, key.getDefaultType() );
            //return cache.ContainsKey(key) && cache.Count > 0;
        }

        public bool HasObject(IAdaptingType key, Type type)
        {
            if( cache.ContainsKey(key) )
            {
                if (cache[key].ContainsKey(type))
                    return true;

                foreach (Type item in cache[key].Keys)
                    if (type.IsAssignableFrom(item))
                        return true;
            }

            return false;
        }

        public void AddObject(IAdaptingType adapter, Object value)
        {
            AddObject(adapter, adapter.getDefaultType(), value);
        }

        public void AddObject( IAdaptingType key, Type type, Object value )
        {
            if (!cache.ContainsKey(key))
                cache[key] = new Dictionary<Type, object>();

            cache[key][type] = value;
        }

        public Object GetObject( IAdaptingType key )
        {
            return GetObject( key, key.getDefaultType() );
            /*
            foreach (Object item in cache[key].Values)
                return item;

            throw new Exception("Object not exists in reference cache");
             */
        }

        public Object GetObject( IAdaptingType key, Type type )
        {
            if (cache[key].ContainsKey(type))
                return cache[key][type];

            foreach (Type item in cache[key].Keys)
                if (type.IsAssignableFrom(item))
                    return cache[key][item];

            throw new Exception("Object not exists in reference cache");
        }
    }
}
