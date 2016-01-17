using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

using Weborb;
using Weborb.Types;
using Weborb.Exceptions;
using Weborb.Util.Cache;
using Weborb.Util.Logging;
using Weborb.Util.License;
using LicenseManager = Weborb.Util.License.LicenseManager;

namespace Weborb.Util
  {
  public class Invocation
    {
    private static Hashtable methodInfoTable = new Hashtable();

    // contains a list of methods and classes WITHOUT pre- and post- attributes
    // this is done for performance optimization
    private static Hashtable attributesCache = new Hashtable();
    private static Type IWebORBAttributeType = typeof( IWebORBAttribute );

    public static object invoke( object obj, string function, object[] arguments )
      {
      MethodInfo method = null;

      try
        {
        method = MethodLookup.findMethod( obj.GetType(), function, arguments );
        }
      catch ( MissingMethodException exception )
        {
        string error = "unable to find method in class. method name " + function;

        if ( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, error, exception );

        return new ServiceException( error, exception );
        }

      if ( Log.isLogging( LoggingConstants.INFO ) )
        Log.log( LoggingConstants.INFO, "Resolved .net object. invoke - " + obj.GetType().FullName + ", method - " + function );

      return Invocation.invoke( obj, method, arguments );
      }

    public static object invoke( object obj, MethodInfo method, object[] arguments )
      {
      return invoke( obj, method, arguments, false );
      }

    private static object invoke( object obj, MethodInfo method, object[] arguments, bool isRIA )
      {
      LicenseManager licenseMgr = LicenseManager.GetInstance( LicenseManager.WEBORB_PRODUCT_ID );
      if ( !licenseMgr.IsValid( true ) )
        if ( !method.DeclaringType.Equals( typeof( LicensingService ) ) )
          return new ServiceException( LicenseManager.GetInstance( LicenseManager.WEBORB_PRODUCT_ID ).GetLicensingError(), 500 );
      object[] realArgs = null;

      try
        {
        realArgs = unwrapArguments( method, arguments );
        }
      catch ( Exception exception )
        {
        if ( Log.isLogging( LoggingConstants.ERROR ) )
          Log.log( LoggingConstants.ERROR, "unable to adapt to data type, ", exception );

        string error = "unable to adapt parameter to a method argument type. " + exception.Message;
        return new ServiceException( error, exception );
        }

      object response = null;
      long start = DateTime.Now.Ticks;
      IWebORBAttribute[] preAndPostAttributes = null;

      if ( !attributesCache.ContainsKey( method ) )
        {
        IWebORBAttribute[] classAttribs = (IWebORBAttribute[]) method.DeclaringType.GetCustomAttributes( IWebORBAttributeType, true );
        IWebORBAttribute[] methodAttribs = (IWebORBAttribute[]) method.GetCustomAttributes( IWebORBAttributeType, false );

        if ( classAttribs.Length == 0 && methodAttribs.Length == 0 )
          {
          attributesCache[ method ] = "";
          }
        else
          {
          ArrayList attribs = new ArrayList();
          attribs.AddRange( classAttribs );
          attribs.AddRange( methodAttribs );
          preAndPostAttributes = new IWebORBAttribute[ classAttribs.Length + methodAttribs.Length ];
          attribs.CopyTo( preAndPostAttributes );
          }
        }

      // clear runtime config for every invocation
      ThreadContext.getRuntimeConfig().Clear();

      try
        {
#if NET_RIA
        // in order to call EntityAction on the RIA DomainService side - we need to change the types 
        // of some arguments explicitly to avoid type case exceptions during runtime
        if ( isRIA && method.Name == "SubmitChanges" )                  
          AdoptTypesForRIA( method, realArgs );                                  
#endif
          object res = null;
          if( ( res = Cache.Cache.GetValue( obj, method, realArgs ) ) != null )
          {
            // don't invoke method, but return cached value instead
            // cached_response will be used in ITypeWriter implementations          
            ThreadContext.currentHttpContext().Items[ Cache.Cache.CACHED_RESPONSE ] = res;
            return null;
          }

        //if( !isStd )
          if ( preAndPostAttributes != null )
            try
              {            
                for( int i = 0; i < preAndPostAttributes.Length; i++ )
                  preAndPostAttributes[ i ].HandlePreInvoke( method, obj, realArgs );
              }
            catch(Exception exception)
              {
              // don't cache failure
              if ( ThreadContext.currentHttpContext() != null )
                ThreadContext.currentHttpContext().Items.Remove( "cacheAttribute" );

              return handleException( exception, obj, method, preAndPostAttributes, realArgs );
              }

        response = method.Invoke( obj, realArgs );

        if ( preAndPostAttributes != null )
          try
            {
            response = handlePostInvoke( method, preAndPostAttributes, obj, realArgs, response, false );
            }                    
          catch(Exception exception)
            {
            // don't cache failure
            if ( ThreadContext.currentHttpContext() != null )
              ThreadContext.currentHttpContext().Items.Remove( "cacheAttribute" );
            
            // postAndPreInvokeAttributes are null to avoid handlePostInvoke call
            response = handleException( exception, obj, method, null, realArgs );
            return response;
            }

        return response;
        }
      catch ( TargetInvocationException exception )
        {
        // don't cache failure
        if ( ThreadContext.currentHttpContext() != null )
          ThreadContext.currentHttpContext().Items.Remove( "cacheAttribute" );

        response = handleException( exception.InnerException, obj, method, preAndPostAttributes, realArgs );
        return response;
        }
      catch ( Exception exception )
        {
        // don't cache failure
        if ( ThreadContext.currentHttpContext() != null )
          ThreadContext.currentHttpContext().Items.Remove( "cacheAttribute" );

        if ( exception.GetType().FullName.StartsWith( "System.ServiceModel.FaultException" ) )
          {
          PropertyInfo propInfo = exception.GetType().GetProperty( "InnerException" );
          Exception innerException = (Exception)propInfo.GetValue( exception, null );
          response = handleException( innerException, obj, method, preAndPostAttributes, realArgs );
          return response;
          }
        else
          {
          throw exception;
          }
        }
      finally
        {
        long end = DateTime.Now.Ticks - start;
        long duration = end / 10000;

        if ( Log.isLogging( LoggingConstants.INSTRUMENTATION ) )
          Log.log( LoggingConstants.INSTRUMENTATION, "clr method invocation time (in ms) - " + duration + " method - " + method.Name );

        if ( ORBEvents.Instance.hasListeners )
          {
          InvocationInfo invocationInfo = new InvocationInfo( method, realArgs, response, duration );
          ThreadPool.QueueUserWorkItem( new WaitCallback( FireInvocationEvent ), invocationInfo );
          }
        }
      }

    private static ServiceException handleException( Exception innerException, object obj, MethodInfo method, IWebORBAttribute[] attribs, object[] realArgs )
      {
      String error = "exception during method invocation";

      if ( innerException.Message != null && innerException.Message.Length != 0 )
        error = innerException.Message;

      if ( Log.isLogging( LoggingConstants.ERROR ) )
        Log.log( LoggingConstants.ERROR, error, innerException );

      if( attribs != null )
        innerException = (Exception)handlePostInvoke( method, attribs, obj, realArgs, innerException, true );

      ServiceException serviceException = null;

      if ( !( innerException is ServiceException ) )
        serviceException = new ServiceException( error, innerException );
      else
        serviceException = (ServiceException)innerException;

      return serviceException;
      }

    private static object handlePostInvoke( MethodInfo method, IWebORBAttribute[] attribs, object obj, object[] realArgs, object response, bool isException )
      {
      Hashtable metadata = new Hashtable();

      for ( int i = 0; i < attribs.Length; i++ )
        {
        Hashtable attributeMetadata = null;

        attributeMetadata = attribs[ i ].HandlePostInvoke( method, obj, realArgs, ref response, isException );

        if ( attributeMetadata != null )
          for ( IDictionaryEnumerator e = attributeMetadata.GetEnumerator(); e.MoveNext(); )
            metadata.Add( e.Key, e.Value );
        }

      if ( metadata.Count != 0 )
        ThreadContext.getProperties()[ ORBConstants.RESPONSE_METADATA ] = metadata;

      return response;
      }

    public static void FireInvocationEvent( object state )
      {
      InvocationInfo invocationInfo = (InvocationInfo)state;
      ORBEvents.Instance.FireInvokeComplete( invocationInfo );
      }

    private static object[] unwrapArguments( MethodInfo method, object[] arguments )
      {
      ParameterInfo[] formalArgs = method.GetParameters();
      List<object> realArgs = new List<object>();
      bool logDebug = Log.isLogging( LoggingConstants.DEBUG );

      if ( logDebug )
        Log.log( LoggingConstants.DEBUG, "initiating argument adaptation for method " + method.Name );

      for ( int i = 0; i < formalArgs.Length; i++ )
        {
          if ( formalArgs[i].IsOptional && i >= arguments.Length )
          {
            realArgs.Add(formalArgs[i].DefaultValue);
            continue;
          }
        Type argType = formalArgs[ i ].ParameterType;

        if ( logDebug )
          {
          Log.log( LoggingConstants.DEBUG, "initial argument value " + arguments[ i ] );
          Log.log( LoggingConstants.DEBUG, "target type  " + formalArgs[ i ] );
          }

        object argument = null;

        if ( arguments[ i ] is IAdaptingType )
          argument = ObjectFactories.CreateArgumentObject( argType, (IAdaptingType)arguments[ i ] );
        else
          argument = arguments[ i ];

        if ( argument != null )
          realArgs.Add(argument);
        else
          realArgs.Add(arguments[ i ] != null ? ( (IAdaptingType)arguments[ i ] ).adapt( argType ) : null);

        if ( logDebug )
          Log.log( LoggingConstants.DEBUG, "adapted argument value " + arguments[ i ] );
        }

      return realArgs.ToArray();
      }

#if NET_RIA
    private static void AdoptTypesForRIA( MethodInfo method, object[] arguments )
      {
      foreach ( Handler.WCFDynamicProxy.ChangeSetEntry change in (object[])arguments[ 0 ] )
        if ( change.EntityActions != null )
          foreach( System.Collections.Generic.KeyValuePair<string,object[]> pair in change.EntityActions )
            {            
            var actionMethod = method.DeclaringType.GetMethod( pair.Key );
            for ( int j = 0; j < pair.Value.Length; j++ )
              {
              var parameterType = actionMethod.GetParameters()[ j + 1 ].ParameterType;

              if ( parameterType.IsSubclassOf( typeof( Enum ) ) )
                {                
                pair.Value[ j ] = Enum.Parse( parameterType, (string)pair.Value[ j ] );
                pair.Value[ j ] = Convert.ChangeType( pair.Value[ j ], parameterType );
                }

              if ( pair.Value[ j ].GetType() == typeof (double) )
                {                              
                pair.Value[ j ] = Convert.ChangeType( pair.Value[ j ], parameterType );
                }
              }
            }
      }

#endif
    }
  }
