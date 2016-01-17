using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Weborb.Util;
using Weborb.Util.Logging;

namespace Weborb.Writer
{
  public class V3ReferenceCache : ReferenceCache
  {
    private Dictionary<object, int> objectCache;
    private Dictionary<object, int> stringCache;
    private Dictionary<string, int> traitsCache;

    public V3ReferenceCache()
    {
      //objectCache = new Hashtable( null, identityComparer );
      objectCache = new Dictionary<object, int>();
      stringCache = new Dictionary<object, int>();
      traitsCache = new Dictionary<string, int>();
    }

    public override void Reset()
    {
      objectCache.Clear();
      stringCache.Clear();
      traitsCache.Clear();

      //Log.startLogging( "REFCACHE" );
      //Log.log( "REFCACHE", "++++++++++++++++ RESETTING CACHE ++++++++++++++++" );
    }

    public void AddToTraitsCache( String className )
    {
      if( className != null && !traitsCache.ContainsKey( className ) )
      {
        //Log.startLogging( "TRAITSCACHE" );
        //Log.log( "TRAITSCACHE", "Adding " + traitsCache.Count + "\t" + className );

        traitsCache[ className ] = traitsCache.Count;
      }
    }

    public bool HasTraits( String className )
    {
      return traitsCache.ContainsKey( className );
    }

    public int GetTraitsId( String className )
    {
      int id = traitsCache[ className ];

      //Log.startLogging( "TRAITSCACHE" );
      //Log.log( "TRAITSCACHE", id + "\t" + className );

      return id;
    }

    public override void AddString( object obj )
    {
      if( StringUtil.IsEmptyString( obj ) )
        return;

      //Log.startLogging( "REFCACHE" );
      //Log.log( "REFCACHE", obj + "\t" + stringCache.Count );

      stringCache[ obj ] = stringCache.Count;
    }

    public override void AddObject( object obj )
    {
      //if( obj is DateTime )
      //    obj = ((DateTime) obj).ToUniversalTime();

      //Log.startLogging( "REFCACHE" );
      //Log.log( "REFCACHE", "GOING IN  " + objectCache.Count + "\t" + obj );
      objectCache[ obj ] = objectCache.Count;
    }

    public override int GetStringId( object obj )
    {
      int id;

      if( stringCache.TryGetValue( obj, out id ) )
      {
        // Log.startLogging( "REFCACHE" );
        // Log.log( "REFCACHE", id + "\t" + obj );

        return id;
      }
      else
        return -1;
    }

    public override int GetObjectId( object obj )
    {
      int id;

      //if( obj is DateTime )
      //    obj = ((DateTime) obj).ToUniversalTime();

      if( objectCache.TryGetValue( obj, out id ) )
      {
        // Log.startLogging( "REFCACHE" );
        // Log.log( "REFCACHE", "GOING OUT " +  id + "\t" + obj );

        return id;
      }
      else
        return -1;
    }
  }
}
