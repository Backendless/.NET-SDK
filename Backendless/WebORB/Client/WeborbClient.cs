using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.Security;

#if (!UNIVERSALW8 && !FULL_BUILD && !PURE_CLIENT_LIB)
using System.Windows.Controls;
#endif

#if( !UNIVERSALW8 && !FULL_BUILD && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
using System.Windows.Browser;
#endif


using System.Reflection;

using Weborb.Message;

#if !UNIVERSALW8 && !PURE_CLIENT_LIB && !WINDOWS_PHONE8
using Weborb.Messaging.Net.RTMP;
#endif

using Weborb.Reader;
using Weborb.V3Types;
using Weborb.Writer;
using Weborb.Writer.Amf;
using Weborb.Protocols.Amf;
using Weborb.Types;
using Weborb.Util;
using Weborb.Config;

#if !( UNIVERSALW8 || WINDOWS_PHONE || PURE_CLIENT_LIB || WINDOWS_PHONE8)
using Weborb.ProxyGen.DynamicProxy;
using Weborb.ProxyGen.Core.Interceptor;
#endif
namespace Weborb.Client
{
  public delegate void ResponseThreadConfigurator();
  public class AsyncStreamSetInfo<T>
  {
    public byte[] requestBytes;
    public HttpWebRequest request;
    public Responder<T> responder;
    public long messageSentTime;
    public ResponseThreadConfigurator responseThreadConfigurator;
  }

  /// <summary>
  /// WeborbClient is the main class for all remoting operations. The class 
  /// can be used for server-to-server invocations or by the Silverlight client
  /// to invoke server-side methods. The class supports two types of remoting: 
  /// <list type="number">
  /// <item><description>Remote invocation using general Invoke method</description></item>
  /// <item><description>Remote invocation via a dynamically generated proxy which implements specified target interface</description></item>
  /// </list>
  /// </summary>
  /// <example>
  /// The following examples demonstrates invocations using both of the approaches described above. 
  /// Consider the following server-side class:
  /// <code>
  /// namespace Weborb.Examples
  /// {
  ///   public class HelloWorldService
  ///   {
  ///     public string SayHello()
  ///     {
  ///         return "hi there";
  ///     }
  ///   }
  /// } 
  /// </code>
  /// The following code demonstrates invocation of the SayHello method using the Invoke approach:
  /// <code>
  /// WeborbClient client = new WeborbClient( "http://localhost/weborb4/weborb.aspx" );
  /// Responder&lt;string&gt; responder = new Responder&lt;string&gt;( gotResult, gotError );
  /// client.Invoke( "Weborb.Examples.HelloWorldService", "SayHello", null, responder );
  /// 
  /// public void gotResult( string result )
  /// {
  /// }
  /// 
  /// public function gotError( Fault fault )
  /// {
  /// }
  /// </code>
  /// An alternative approach is to bind to the remote class and perform invocations via a proxy object. The advantage of this approach
  /// is the proxy object implements the target interface. The target interface is not the same as implemented on the server-side (notice
  /// in this example, the server-side does not even have an interface). The interface simply enforces method signatures and uses special
  /// return types to enforce the asynchronous invocation result handling:
  /// <code>
  /// namespace Weborb.Examples
  /// {
  ///   public interface IHelloWorldService
  ///   {
  ///     AsyncToken&lt;string&gt; SayHello();
  ///   }
  /// } 
  /// </code>
  /// Create an instance of WeborbClient with the URL to a WebORB installation and generate a proxy using the target interface:
  /// <code>
  /// WeborbClient remotingService = new WeborbClient( "http://localhost/weborb4/weborb.aspx" );
  /// IHelloWorldService proxy = remotingService.Bind&lt;IHelloWorldService&gt;();
  /// </code>
  /// Now that the proxy is created, invoke the method on the proxy and add a listener to process the return value:
  /// <code>
  /// AsyncToken&lt;String&gt; asyncToken = proxy.SayHello();
  /// asyncToken.ResultListener += GotResult;
  /// 
  /// public void GotResult( String returnValue )
  /// {
  /// ...do something with the return value
  /// }
  /// </code></example>
  public class WeborbClient
  {
    private static Engine desiredEngine;
#if(SILVERLIGHT && !WINDOWS_PHONE && !PURE_CLIENT_LIB)
        public static Uri Uri = System.Windows.Browser.HtmlPage.Document.DocumentUri;
#endif

    public IdInfo IdInfo;
    private Engine _engine;
#if !UNIVERSALW8 && !PURE_CLIENT_LIB && !WINDOWS_PHONE8
    public BaseRTMPClient RTMP
    {
      get
      {
        if ( _engine != null && _engine.IsRTMP() )
          return ( (BaseRtmpEngine)_engine ).RTMP;
        return null;
      }
    }
#endif
#if !(UNIVERSALW8 || FULL_BUILD || PURE_CLIENT_LIB)
    internal UserControl uiControl;
#endif
    //public static ManualResetEvent allDone = new ManualResetEvent( false );

#if !(FULL_BUILD)
        static WeborbClient()
        {
          Types.Types.AddAbstractTypeMapping( typeof( IList<> ), typeof( List<> ) );
            Types.Types.AddClientClassMapping("flex.messaging.messages.AcknowledgeMessage", typeof(AckMessage));
            Types.Types.AddClientClassMapping( "flex.messaging.messages.AsyncMessage", typeof( AsyncMessage ) );
            Types.Types.AddClientClassMapping("flex.messaging.messages.RemotingMessage", typeof(ReqMessage));
            Types.Types.AddClientClassMapping("flex.messaging.messages.CommandMessage", typeof(CommandMessage));
            Types.Types.AddClientClassMapping( "flex.messaging.messages.ErrorMessage", typeof( ErrMessage ) );    
            Types.Types.AddClientClassMapping( "flex.messaging.io.ArrayCollection", typeof( ObjectProxy ) );
            ORBConfig.GetInstance().getObjectFactories().AddArgumentObjectFactory( "Weborb.V3Types.BodyHolder", new Weborb.V3Types.BodyHolderFactory() );
            Types.Types.AddAbstractTypeMapping( typeof( IDictionary ), typeof( Dictionary<object, object> ) );
        }
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="WeborbClient"/> class with the WebORB URL.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="gatewayURL">The gateway URL - must be a valid URL of a WebORB 
    /// installation. For instance, the gateway URL for the default installation 
    /// of WebORB for .NET is http://localhost/weborb4/weborb.aspx</param>
    public WeborbClient( String gatewayURL )
      : this( gatewayURL, "GenericDestination" )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeborbClient"/> class with the WebORB URL.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="gatewayURL">The gateway URL - must be a valid URL of a WebORB 
    /// installation. For instance, the gateway URL for the default installation 
    /// of WebORB for .NET is http://localhost/weborb4/weborb.aspx</param>
    /// <param name="timeout">Timeout is the number of milliseconds to wait before the request times out</param>
    public WeborbClient( String gatewayURL, int timeout )
      : this( gatewayURL, "GenericDestination", timeout )
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeborbClient"/> class with the WebORB URL and a destination name.
    /// </summary>
    /// <param name="gatewayURL">The gateway URL - must be a valid URL of a WebORB 
    /// installation. For instance, the gateway URL for the default installation 
    /// of WebORB for .NET is http://localhost/weborb4/weborb.aspx</param>
    /// <param name="destination">The destination name - must be a valid destination
    /// name configured in WEB-INF/flex/remoting-config.xml</param>
    /// <param name="timeout">Timeout is the number of milliseconds to wait before the request times out</param>
    /// <remarks>For the instances of WeborbClient created with this constructor, invocations using the Invoke method
    /// do not require class name, unless the destination argument is a generic destination.</remarks>
    public WeborbClient( String gatewayURL, String destination, int timeout )
    {
      IdInfo = new IdInfo();
      IdInfo.Destination = destination;

      if( desiredEngine != null )
        _engine = desiredEngine;
      else
        _engine = Engine.Create( gatewayURL, IdInfo );

      _engine.Timeout = timeout;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeborbClient"/> class with the WebORB URL and a destination name.
    /// </summary>
    /// <param name="gatewayURL">The gateway URL - must be a valid URL of a WebORB 
    /// installation. For instance, the gateway URL for the default installation 
    /// of WebORB for .NET is http://localhost/weborb4/weborb.aspx</param>
    /// <param name="destination">The destination name - must be a valid destination
    /// name configured in WEB-INF/flex/remoting-config.xml</param>
    /// <remarks>For the instances of WeborbClient created with this constructor, invocations using the Invoke method
    /// do not require class name, unless the destination argument is a generic destination.</remarks>
    public WeborbClient( String gatewayURL, String destination )
      : this( gatewayURL, destination, 0 )
    {
    }

#if !(FULL_BUILD || PURE_CLIENT_LIB || UNIVERSALW8)
    /// <summary>
        /// Initializes a new instance of the <see cref="WeborbClient"/> class with the WebORB URL and an instance of UserControl. The UserControl object is used to dispatch events into the UI thread for data binding purposes.
        /// </summary>
        /// <param name="gatewayURL">The gateway URL - must be a valid URL of a WebORB 
        /// installation. For instance, the gateway URL for the default installation 
        /// of WebORB for .NET is http://localhost/weborb30/weborb.aspx</param>
        /// <param name="uiControl">A UI control used by the WeborbClient instance to execute callbacks into the UI thread. This argument can be null.</param>
        public WeborbClient( String gatewayURL, UserControl uiControl )
            : this( gatewayURL, "GenericDestination", uiControl )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeborbClient"/> class with the WebORB URL, destination name and an instance of UserControl.
        /// </summary>
        /// <param name="gatewayURL">The gateway URL - must be a valid URL of a WebORB 
        /// installation. For instance, the gateway URL for the default installation 
        /// of WebORB for .NET is http://localhost/weborb30/weborb.aspx</param>
        /// <param name="destination">The destination name - must be a valid destination
        /// name configured in remoting-config.xml</param>
        /// <param name="uiControl">A UI control used by the <see cref="WeborbClient"/> instance to execute callbacks into the UI thread. The argument can be null.</param>
        /// <remarks>For the instances of WeborbClient created with this constructor, invocations using the Invoke method
        /// do not require class name, unless the destination argument is a generic destination.</remarks>
        public WeborbClient( String gatewayURL, String destination, UserControl uiControl )
        {
            IdInfo = new IdInfo();
            IdInfo.Destination = destination;
            this.uiControl = uiControl;

            if( desiredEngine != null )
              _engine = desiredEngine;
            else
              _engine = Engine.Create(gatewayURL, IdInfo, uiControl);
        }
#endif

    public static Engine Engine
        {
          set
          {
            desiredEngine = value;
          }
        }

    public int Timeout
    {
      get
      {
        return _engine.Timeout;
      }

      set
      {
        _engine.Timeout = value;
      }
    }

#if !PURE_CLIENT_LIB  && !WINDOWS_PHONE8 && !UNIVERSALW8
    /// <summary>
    /// Creates a proxy to a remote interface with the target interface defined as T. The interface must define the return types for the methods with the generic AsyncToken class.
    /// </summary>
    /// <typeparam name="T">Target interface</typeparam>
    /// <returns>A proxy implementing the methods from the target interface. The proxy can be used to perform remote method invocations.</returns>
    /// <remarks>The name of the remote class is derived from the target interface name. The interface must start with 'I' and be in the same namespace as the
    /// remote class. If this requirement cannot be met or is undesireable, use the Bind method with two arguments.</remarks>
    /// <example>Consider the following server-side class:
    /// <code>
    /// namespace Weborb.Examples
    /// {
    ///   public class DataBinding
    ///   {
    ///     public DataTable getCustomers()
    ///     {
    ///        //contains logic for data retrieval
    ///        return dataTable;
    ///     }
    ///     
    ///     public DateTime getServerTime()
    ///     {
    ///       return DateTime.Now;
    ///     }
    ///   }
    /// }
    /// </code>
    /// The class above can be represented through the following target interface used on the client:
    /// <code>
    /// namespace Weborb.Examples
    /// {
    ///   public class IDataBinding
    ///   {
    ///     AsyncToken&lt;List&lt;Customer&gt;&gt; getCustomers();
    ///     AsyncToken&lt;DateTime&gt; getServerTime();
    ///   }
    ///   
    ///   public class Customer
    ///   {
    ///      public string CustomerName {get;set;}
    ///      public string CustomerPhone {get;set;}
    ///   }
    /// }
    /// </code>
    /// Notice the class Customer above: the class declares two public properties corresponsing to the columns in the returned DataTable.
    /// The following code binds to the remote DataBinding class using the target interface:
    /// <code>
    /// WeborbClient client = new WeborbClient( "http://localhost/weborb4/weborb.aspx" );
    /// IDataBinding proxy = client.Bind&lt;IDataBiding&gt;();
    /// // now any call on the proxy object will result in a remote method invocation:
    /// AsyncToken&lt;List&lt;Customer&gt;&gt; asyncToken = proxy.getCustomers();
    /// </code>
    /// </example>
    public T Bind<T>()
    {
      return (T)Bind( typeof( T ) );
    }

    /// <summary>
    /// Creates a proxy to a remote interface with the target interface defined as targetInterface. 
    /// The interface must define the return types for the methods with the generic AsyncToken class.
    /// The target class name is derived from the interface name. For instance of the full name 
    /// for the interface is Com.Foo.IHelloWorldService, the method assumes the target class name is
    /// Com.Foo.HelloWorldService.
    /// </summary>
    /// <param name="targetInterface">The target interface.</param>
    /// <returns>A proxy implementing the methods from the target interface. The proxy can be used to perform remote method invocations.</returns>
    /// <remarks>The name of the remote class is derived from the target interface name. The interface must start with 'I' and be in the same namespace as the
    /// remote class. If this requirement cannot be met or is undesireable, use the Bind method with two arguments. The usage of this method is similar to the generic version of Bind with the only difference is the requirement for cast as shown in the example below.</remarks>
    /// <example>Consider the following server-side class:
    /// <code>
    /// namespace Weborb.Examples
    /// {
    ///   public class DataBinding
    ///   {
    ///     public DataTable getCustomers()
    ///     {
    ///        //contains logic for data retrieval
    ///        return dataTable;
    ///     }
    ///     
    ///     public DateTime getServerTime()
    ///     {
    ///       return DateTime.Now;
    ///     }
    ///   }
    /// }
    /// </code>
    /// The class above can be represented through the following target interface used on the client:
    /// <code>
    /// namespace Weborb.Examples
    /// {
    ///   public class IDataBinding
    ///   {
    ///     AsyncToken&lt;List&lt;Customer&gt;&gt; getCustomers();
    ///     AsyncToken&lt;DateTime&gt; getServerTime();
    ///   }
    ///   
    ///   public class Customer
    ///   {
    ///      public string CustomerName {get;set;}
    ///      public string CustomerPhone {get;set;}
    ///   }
    /// }
    /// </code>
    /// Notice the class Customer above: the class declares two public properties corresponsing to the columns in the returned DataTable.
    /// The following code binds to the remote DataBinding class using the target interface:
    /// <code>
    /// WeborbClient client = new WeborbClient( "http://localhost/weborb4/weborb.aspx" );
    /// IDataBinding proxy = (IDataBinding) client.Bind( typeof( IDataBinding) );
    /// // now any call on the proxy object will result in a remote method invocation:
    /// AsyncToken&lt;List&lt;Customer&gt;&gt; asyncToken = proxy.getCustomers();
    /// </code>
    /// </example>
    public object Bind( Type targetInterface )
    {
      String targetClassName = targetInterface.Namespace;
      String interfaceName = targetInterface.Name;

      if ( targetInterface.IsInterface )
      {
        if ( interfaceName.StartsWith( "I" ) )
          targetClassName += "." + interfaceName.Substring( 1 );
        else
          throw new Exception( "Interface name must start with 'I' or use the Bind method with two arguments" );
      }
      else
      {
        targetClassName += "." + interfaceName;
      }

      return Bind( targetInterface, targetClassName );
    }

    /// <summary>
    /// Creates a proxy to a remote interface with the target interface defined as targetInterface. 
    /// The interface must define the return types for the methods with the generic AsyncToken class.
    /// Works the same way as the generic/non-generic versions of the Bind, but provides a way to specify
    /// target class name.
    /// </summary>
    /// <param name="targetInterface">The target interface.</param>
    /// <param name="targetClassName">Full name of the target class.</param>
    /// <returns>A proxy implementing the methods from the target interface. The proxy can be used to perform remote method invocations.</returns>
    public object Bind( Type targetInterface, String targetClassName )
    {
      ProxyGenerator proxyGen = new ProxyGenerator();

      IInterceptor interceptor = new AMFInterceptor( targetClassName, this );

      if ( targetInterface.IsInterface )
        return proxyGen.CreateInterfaceProxyWithoutTarget( targetInterface, interceptor );
      else
        return proxyGen.CreateClassProxy( targetInterface, new IInterceptor[] { interceptor } );
    }
#endif
    /// <summary>
    /// Performs a remote method invocation for the instance of WeborbClient created with a non-generic destination name.
    /// </summary>
    /// <typeparam name="T">Identifies the return type of the invoked method</typeparam>
    /// <param name="methodName">Name of the method to invoke</param>
    /// <param name="args">Collection of arguments or null if none</param>
    /// <param name="responder">The responder containing references to the result and fault handling methods.</param>
    /// <remarks>The method does not have an argument for the targetted class name. As a result, 
    /// using this method will work with the instances of WeborbClient created with non-generic destinations. 
    /// <example>
    /// Consider the following server-side class deployed into a WebORB-enabled web application:
    /// <code>
    /// namespace Weborb.Examples
    /// {
    ///   public class OrderProcessor
    ///   {
    ///     public void ProcessOrder( Order orderObject )
    ///     {
    ///     .. do something with the order
    ///     }
    ///   }
    /// }
    /// 
    /// The class is defined in a destination in remoting-config.xml:
    /// 
    /// &lt;destination id="OrderProcessorDestination"&gt;
    ///   &lt;properties&gt;
    ///     &lt;source&gt;Weborb.Examples.OrderProcessor&lt;/source&gt;
    ///   &lt;/properties&gt;
    /// &lt;/destination&gt;
    /// &lt;/code&gt;
    /// </code>
    /// The ProcessOrder method can be invoked using the following code:
    /// <code>
    /// WeborbClient client = new WeborbClient( "http://localhost/weborb30/weborb.aspx", "OrderProcessorDestination" );
    /// Order orderInstance = new Order();
    /// Object[] args = new Object[] { orderInstance };
    /// //since the method returns void, use the special Result class as the designated generic type
    /// Responder&lt;Result&gt; responder = new Responder&lt;boolean&gt;( gotResult, gotFault );
    /// client.Invoke( "ProcessOrder", args, responder );
    /// 
    /// public void gotResult( Result resultValue )
    /// {
    /// }
    /// 
    /// public void gotFault( Fault fault )
    /// {
    /// }
    /// </code>
    /// </example>
    public void Invoke<T>( String methodName, Object[] args, Responder<T> responder )
    {
      Invoke( null, methodName, args, null, responder );
    }

    /// <summary>
    /// Performs a remote method invocation for the instance of WeborbClient created without a destination name.
    /// </summary>
    /// <typeparam name="T">Identifies the return type of the invoked method</typeparam>
    /// <param name="className">Name of the remote class to invoke method on</param>
    /// <param name="methodName">Name of the method to invoke</param>
    /// <param name="args">Collection of arguments or null if none</param>
    /// <param name="responder">Generic (types) async token with registered response/failure handlers. Type T defined the return type of the method invocation.</param>
    /// <example>
    /// Consider the following server-side class deployed into a WebORB-enabled web application:
    /// <code>
    /// namespace Weborb.Examples
    /// {
    ///   public class OrderProcessor
    ///   {
    ///     public void ProcessOrder( Order orderObject )
    ///     {
    ///     .. do something with the order
    ///     }
    ///   }
    /// }
    /// </code>
    /// The ProcessOrder method can be invoked using the following code:
    /// <code>
    /// WeborbClient client = new WeborbClient( "http://localhost/weborb30/weborb.aspx" );
    /// Order orderInstance = new Order();
    /// Object[] args = new Object[] { orderInstance };
    /// //since the method returns void, use the special Result class as the designated generic type
    /// Responder&lt;Result&gt; responder = new Responder&lt;boolean&gt;( gotResult, gotFault );
    /// client.Invoke( "Weborb.Examples.OrderProcessor", "ProcessOrder", args, responder );
    /// 
    /// public void gotResult( Result resultValue )
    /// {
    /// }
    /// 
    /// public void gotFault( Fault fault )
    /// {
    /// }
    /// </code>
    /// </example>
    public void Invoke<T>( String className, String methodName, Object[] args, Responder<T> responder )
    {
      Invoke( className, methodName, args, null, responder );
    }

    public void Invoke<T>( String className, String methodName, Object[] args, IDictionary messageHeaders, Responder<T> responder )
    {
      if ( responder == null )
        responder = new DefaultResponder<T>();
      Type methodReturnType = responder.GetType().GetGenericArguments()[0];
      Type asyncType = typeof( AsyncToken<> );
      Type[] argType = { methodReturnType };
      Type constructed = asyncType.MakeGenericType( argType );
      object asyncTokenObject = Activator.CreateInstance( constructed, new object[] { responder.ResponseHandler, responder.ErrorHandler } );

#if !(UNIVERSALW8 || FULL_BUILD || PURE_CLIENT_LIB || WINDOWS_PHONE8 )
      FieldInfo uiControlField = asyncTokenObject.GetType().GetField( "uiControl", BindingFlags.NonPublic | BindingFlags.Instance );
            uiControlField.SetValue( asyncTokenObject, this.uiControl );
#endif
#if( UNIVERSALW8 || WINDOWS_PHONE || PURE_CLIENT_LIB || WINDOWS_PHONE8)
      HandleInvocation( className, methodName, args, messageHeaders, asyncTokenObject );
#else
            HandleInvocation( null, className, methodName, args, messageHeaders, asyncTokenObject );
#endif
    }

    public void Publish( Object message )
    {
      this.Publish( message, null, null, null );
    }

    public void Publish( Object message, PublishingResponder responder )
    {
      this.Publish( message, null, null, responder );
    }

    public void Publish( Object message, Dictionary<object, object> headers )
    {
      this.Publish( message, headers, null, null );
    }

    public void Publish( Object message, Dictionary<object, object> headers, PublishingResponder responder )
    {
      this.Publish( message, headers, null, responder );
    }


    public void Publish( Object message, Dictionary<object, object> headers, String subtopic )
    {
      this.Publish( message, headers, subtopic, null );
    }

    public void Publish( Object message, Dictionary<object, object> headers, String subtopic, PublishingResponder responder )
    {
      Publish( message, headers, subtopic, responder, IdInfo.Destination );
    }

    public void Publish( Object message, Dictionary<object, object> headers, String subtopic, PublishingResponder responder, string destination )
    {
      AsyncMessage asyncMessage;
      if ( message is AsyncMessage )
      {
        asyncMessage = (AsyncMessage)message;
      }
      else
      {
        asyncMessage = new AsyncMessage();
        asyncMessage._body = new BodyHolder();
        asyncMessage._body.body = message;
      }

      asyncMessage.destination = destination;
      asyncMessage.headers = asyncMessage.headers ?? new Dictionary<object, object>();

      if ( headers != null )
        foreach ( KeyValuePair<object, object> keyValuePair in headers )
          if ( asyncMessage.headers.Contains( keyValuePair.Key ) )
            asyncMessage.headers[keyValuePair.Key] = keyValuePair.Value;
          else
            asyncMessage.headers.Add( keyValuePair.Key, keyValuePair.Value );

      asyncMessage.messageId = Guid.NewGuid().ToString();
      //
      //          asyncMessage.clientId = IdInfo.ClientId;

      if ( subtopic != null )
        asyncMessage.headers["DSSubtopic"] = subtopic;

      if ( IdInfo.DsId != null )
        asyncMessage.headers["DSId"] = IdInfo.DsId;

      if ( responder == null )
        responder = new PublishingResponder( PublishResponseHandler, PublishErrorHandler );

      _engine.SendRequest( asyncMessage, responder );
    }

    internal void PublishResponseHandler( AckMessage asyncMessage )
    {
      System.Console.WriteLine( "message published" );
    }

    internal void PublishErrorHandler( Fault fault )
    {
      System.Console.WriteLine( "got error - " + fault.Detail );
    }

    public Subscription Subscribe<T>( Responder<T> responder )
    {
      return Subscribe<T>( responder, null, null );
    }

    public Subscription Subscribe<T>( Responder<T> responder, String subTopic )
    {
      return Subscribe<T>( responder, subTopic, null );
    }

    public Subscription Subscribe<T>( Responder<T> responder, String subTopic, String selector )
    {
      Subscription token = new Subscription( subTopic, selector, _engine );

      token.SetResponder( responder );
      _engine.SetSubscription( token );

      if ( !token.IsSubscribed )
      {
        token.Subscribe<T>();
      }

      return token;
    }

    internal void MainInvoke<T>( String className, String methodName, Object[] args, IDictionary messageHeaders, Responder<T> responder )
    {
      Invoke( className, methodName, args, messageHeaders, responder, new AsyncStreamSetInfo<T>() );
    }

    internal void Invoke<T>( String className, String methodName, Object[] args, IDictionary messageHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      _engine.Invoke( className, methodName, args, null, messageHeaders, null, responder, asyncStreamSetInfo );
    }

    public void Invoke<T>( String className, String methodName, Object[] args, IDictionary httpRequestHeaders, IDictionary messageHeaders, Responder<T> responder )
    {
      Invoke( className, methodName, args, null, messageHeaders, httpRequestHeaders, responder );
    }

    public void Invoke<T>( String className, String methodName, Object[] args, IDictionary httpRequestHeaders, IDictionary messageHeaders, Responder<T> responder, ResponseThreadConfigurator responseThreadConfigurator )
    {
      AsyncStreamSetInfo<T> asyncStreamSetInfo = new AsyncStreamSetInfo<T>();
      asyncStreamSetInfo.responseThreadConfigurator = responseThreadConfigurator;
      _engine.Invoke( className, methodName, args, null, messageHeaders, httpRequestHeaders, responder, asyncStreamSetInfo );
    }

    internal void Invoke<T>(string className, string methodName, object[] args, IDictionary requestHeaders, IDictionary messageHeaders, IDictionary httpRequestHeader, Responder<T> responder)
    {
      _engine.Invoke( className, methodName, args, requestHeaders, messageHeaders, httpRequestHeader, responder, new AsyncStreamSetInfo<T>() );
    }

    internal void Invoke<T>( String className, String methodName, Object[] args, IDictionary requestHeaders, IDictionary messageHeaders, Responder<T> responder, AsyncStreamSetInfo<T> asyncStreamSetInfo )
    {
      _engine.Invoke( className, methodName, args, requestHeaders, messageHeaders, null, responder, asyncStreamSetInfo );
    }

    public void SendMessage<T>( V3Message v3Msg, Responder<T> responder )
    {
      _engine.SendRequest( v3Msg, null, null, responder, null );
    }

#if !(UNIVERSALW8 || PURE_CLIENT_LIB || WINDOWS_PHONE || WINDOWS_PHONE8)
    internal void HandleInvocation( IInvocation invocation, string className, string methodName, object[] arguments, IDictionary messageHeaders, object asyncTokenObject )
#else
        internal void HandleInvocation( string className, string methodName, object[] arguments, IDictionary messageHeaders, object asyncTokenObject )
#endif

    {
      Type methodReturnType = asyncTokenObject.GetType().GetGenericArguments()[0];
      Type[] argType = { methodReturnType };
      Type methodResponderType = typeof( MethodResponder<> );
      Type methodResponderConstructorType = methodResponderType.MakeGenericType( argType );
      object[] methodResponderArgs = { asyncTokenObject };
      object methodResponderObject = Activator.CreateInstance( methodResponderConstructorType, methodResponderArgs );

      //MethodInfo responseHandlerMethod = methodResponderObject.GetType().GetMethod( "ResponseHandler" );
      Type responseHandlerTypeDef = typeof( ResponseHandler<> );
      Type responseHandlerConstructed = responseHandlerTypeDef.MakeGenericType( argType );
      Delegate responseHandlerDelegate = Delegate.CreateDelegate( responseHandlerConstructed, methodResponderObject, "ResponseHandler" );

      //MethodInfo errorHandlerMethod = methodResponderObject.GetType().GetMethod( "ErrorHandler" );
      ErrorHandler errorHandler = (ErrorHandler)Delegate.CreateDelegate( typeof( ErrorHandler ), methodResponderObject, "ErrorHandler" );

      Type responderTypeDef = typeof( Responder<> );
      Type responderConstructed = responderTypeDef.MakeGenericType( argType );
      object responderObj = Activator.CreateInstance( responderConstructed, new object[] { responseHandlerDelegate, errorHandler } );

      Type stringType = typeof( string );
      Type[] invokeTypes = new Type[] { stringType, stringType, typeof( Object[] ), responderConstructed };

      MethodInfo invokeMethodDef = this.GetType().GetMethod( "MainInvoke", BindingFlags.NonPublic | BindingFlags.Instance );

      //MethodInfo invokeMethodDef = weborbClient.GetType().GetMethod( "Invoke", invokeTypes );
      MethodInfo invokeMethod = invokeMethodDef.MakeGenericMethod( argType );
      //weborbClient.Invoke( className, methodName, arguments, responder );
      object[] invokeArgs = new object[] { className, methodName, arguments, messageHeaders, responderObj };
      invokeMethod.Invoke( this, invokeArgs );

#if !( WINDOWS_PHONE || PURE_CLIENT_LIB || WINDOWS_PHONE8 )
      if ( invocation != null )
        invocation.ReturnValue = asyncTokenObject;
#endif
      // AsyncToken asyncToken = new AsyncToken( invocation );
      // MethodResponder methodResponder = new MethodResponder( asyncToken );
      // Responder responder = new Responder( methodResponder.ResponseHandler, methodResponder.ErrorHandler );
      // weborbClient.Invoke( className, methodName, arguments, responder );
      // invocation.ReturnValue = asyncToken;
    }

    public void SetHttpCokies( CookieContainer cookies )
    {
      _engine.Cookies = cookies;
    }

    public CookieContainer GetHttpCokies()
    {
      return _engine.Cookies;
    }
  }

  public class MethodResponder<T>
  {
    private AsyncToken<T> asyncToken;

    public MethodResponder( AsyncToken<T> asyncToken )
    {
      this.asyncToken = asyncToken;
    }

    public void ResponseHandler( T response )
    {
      asyncToken.Result = response;
    }

    public void ErrorHandler( Fault fault )
    {
      asyncToken.Fault = fault;
    }
  }

  public class IdInfo
  {
    //public String ClientId;
    public String DsId;
    public String Destination = "GenericDestination";

    public new IdInfo MemberwiseClone()
    {
      return (IdInfo)base.MemberwiseClone();
    }
  }
}
