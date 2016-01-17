using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Weborb.Activation;
using Weborb.Config;
using Weborb.Handler;
using Weborb.Protocols;
using Weborb.Util.Logging;
using Weborb.Writer;
using Weborb.Service;

namespace Weborb.Util.Cache
  {
  class Cache
    {
    public const String CACHED_RESPONSE = "cached_response";
    public const String CURRENT_PROTOCOL = "current_protocol";

    // maps MethodInfo, object instance, protocol parser and arguments to actual response
    internal static Dictionary<MethodInfo, Dictionary<object, Dictionary<string, Dictionary<Arguments, CachedValue>>>> globalCache
                    = new Dictionary<MethodInfo, Dictionary<object, Dictionary<string, Dictionary<Arguments, CachedValue>>>>();

    private static Dictionary<object, bool> invalidators = new Dictionary<object, bool>();

    private static void EnsureSubscription( ICacheInvalidator obj )
      {
      lock ( invalidators )
        {
        if( !invalidators.ContainsKey( obj ) )
          {          
          obj.InvalidateCache += new CacheInvalidator( InvalidateCache );
          invalidators.Add( obj, true );
          }
        }
      }

    // if args is null - invalidate for any arguments
    private static void InvalidateCache( object target, MethodInfo method, object[] args )
      {
      if ( !globalCache.ContainsKey( method ) )
        return;

      WebORBCacheAttribute[] methodAttribs =
         (WebORBCacheAttribute[])method.GetCustomAttributes( typeof( WebORBCacheAttribute ), false );

      if ( methodAttribs == null || methodAttribs.Length == 0 )
        return;

      target = GetTarget( target, methodAttribs[ 0 ] );

      if ( !globalCache[ method ].ContainsKey( target ) )
        return;

      foreach ( IMessageFactory factory in ORBConfig.GetInstance().getProtocolRegistry().Factories )
        foreach( string protocolName in factory.GetProtocolNames() )
          {
          if ( !globalCache[ method ][ target ].ContainsKey( protocolName ) )
            continue;
          
          if ( args == null )
            {
            globalCache[ method ][ target ][ protocolName ].Clear();
            }
          else
            {
            Arguments arguments = new Arguments( args );

            if ( globalCache[ method ][ target ][ protocolName ].ContainsKey( arguments ) )
              globalCache[ method ][ target ][ protocolName ].Remove( arguments );
            }
          }              
      }

    internal static object GetValue( object obj, MethodInfo method, object[] args )
      {
      if ( obj != null && typeof( ICacheInvalidator ).IsAssignableFrom( obj.GetType() ) )
        EnsureSubscription( (ICacheInvalidator)obj );

      if ( ThreadContext.currentHttpContext() == null )
        return null;

      // context is used for communication between implemntation of ITypeWriter and Invocation/Cache classes
      IDictionary context = ThreadContext.currentHttpContext().Items;

      WebORBCacheAttribute[] methodAttribs =
          (WebORBCacheAttribute[])method.GetCustomAttributes( typeof( WebORBCacheAttribute ), false );

      if ( methodAttribs == null || methodAttribs.Length == 0 )
        return null;

      // if we have multi body AMF request - turn off caching
      if ( (int)context[ "request_bodies_count" ] > 1 )
        return null;

      WebORBCacheAttribute cacheAttr = methodAttribs[ 0 ];

      object target = GetTarget( obj, cacheAttr );

      // create cache if this is first cache request
      if ( !globalCache.ContainsKey( method ) )
        globalCache[ method ] = new Dictionary<object, Dictionary<string, Dictionary<Arguments, CachedValue>>>();

      if ( !globalCache[ method ].ContainsKey( target ) )
        globalCache[ method ][ target ] = new Dictionary<string, Dictionary<Arguments, CachedValue>>();

      string protocolParser = (string)ThreadContext.getProperties()[ CURRENT_PROTOCOL ];

      if ( !globalCache[ method ][ target ].ContainsKey( protocolParser ) )
        globalCache[ method ][ target ][ protocolParser ] = new Dictionary<Arguments, CachedValue>();

      Arguments arguments = new Arguments();
      arguments.arguments = args;
      Dictionary<Arguments, CachedValue> methodCache = globalCache[ method ][ target ][ protocolParser ];

      if ( methodCache.ContainsKey( arguments ) )
        {
        CachedValue value = methodCache[ arguments ];

        if ( value.ExpirationTime < DateTime.Now )
          {
          // value is expired - we should recache it there after invocation via SaveValue method
          CreateCacheRequest( context, cacheAttr, target, method, arguments );
          return null;
          }
        else if ( value.ExpirationTime != DateTime.MaxValue )
          {
          value.ExpirationTime = DateTime.Now + TimeSpan.FromMilliseconds( cacheAttr.ExpirationTimespan );
          }

        return value.Value;
        }

      // value isn't in cache - but we should place it there after invocation via SaveValue method
      CreateCacheRequest( context, cacheAttr, target, method, arguments );

      return null;
      }

    private static object GetTarget( object obj, WebORBCacheAttribute cacheAttr )
      {
      if ( cacheAttr.CacheScope == CacheScope.Instance )
        return obj;
      else if ( cacheAttr.CacheScope == CacheScope.Session )
        return ThreadContext.currentHttpContext().Session.SessionID;
      else
        return "default_object";

      }

    // this method is called from WriteAndSave method to cache value if neccessary
    internal static void SaveValue( object objectToCache )
      {
      IDictionary context = ThreadContext.currentHttpContext().Items;

      WebORBCacheAttribute cacheAttr = (WebORBCacheAttribute)context[ "cacheAttribute" ];
      MethodInfo method = (MethodInfo)context[ "cacheMethod" ];
      object target = context[ "cacheTarget" ];
      Arguments arguments = (Arguments)context[ "cacheArguments" ];

      string protocolParser = (string)ThreadContext.getProperties()[ CURRENT_PROTOCOL ];
      Dictionary<Arguments, CachedValue> methodCache = globalCache[ method ][ target ][ protocolParser ];

      DateTime expirationTime = DateTime.MaxValue;

      if ( cacheAttr.ExpirationTimespan != -1 )
        expirationTime = DateTime.Now + TimeSpan.FromMilliseconds( cacheAttr.ExpirationTimespan );

      methodCache[ arguments ] = new CachedValue( objectToCache, expirationTime );

      if ( Log.isLogging( LoggingConstants.DEBUG ) )
        Log.log( LoggingConstants.DEBUG, "Invocation response has been saved to cache" );

      }

    private static void CreateCacheRequest( IDictionary context, WebORBCacheAttribute cacheAttr, object target,
                                            MethodInfo method, Arguments arguments )
      {
      context[ "cacheAttribute" ] = cacheAttr;
      context[ "cacheTarget" ] = target;
      context[ "cacheMethod" ] = method;
      context[ "cacheArguments" ] = arguments;
      }

    internal static void WriteAndSave( object obj, IProtocolFormatter formatter )
      {
      // get cached value if applicable
      System.Web.HttpContext httpContext = ThreadContext.currentHttpContext();

      if ( httpContext != null && httpContext.Items[ "request_bodies_count" ] !=null && (int)httpContext.Items[ "request_bodies_count" ] == 1 && httpContext.Items.Contains( CACHED_RESPONSE ) )
        {
        IDictionary context = httpContext.Items;
        formatter.WriteCachedObject( context[ CACHED_RESPONSE ] );

        if ( Log.isLogging( LoggingConstants.DEBUG ) )
          Log.log( LoggingConstants.DEBUG, "Cached response is used" );
        }
      else
        {
        // if there is a value cache request - mark bytes
        bool saveToCache = httpContext != null && httpContext.Items.Contains( "cacheAttribute" );

        if ( saveToCache )
          formatter.BeginSelectCacheObject();

        MessageWriter.writeObject( obj, formatter );

        if ( saveToCache )
          SaveValue( formatter.EndSelectCacheObject() );
        }
      }
    }
  }
