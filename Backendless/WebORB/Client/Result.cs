using System;
using System.Collections;
using System.Net;
#if !PURE_CLIENT_LIB
using System.Windows;
#endif
using Weborb.Types;

namespace Weborb.Client
{
    /// <summary>
    /// Contains return value returned from remote method invocation. Must be referenced as the generic type
    /// of the Responder object passed into WeborbClient.Invoke methods. 
    /// </summary>
    /// <example>
    /// <code>
    /// namespace Weborb.Examples
    /// {
    ///   public class HelloWorldService
    ///   {
    ///     public string SayHello()
    ///     {
    ///       return "Hello World";
    ///     }
    ///   }
    /// }
    /// </code>
    /// The SayHello method can be invoked using the following code:
    /// <code>
    /// WeborbClient client = new WeborbClient( "http://localhost/weborb30/weborb.aspx" );
    /// Responder&lt;Result&gt; responder = new Responder&lt;boolean&gt;( gotResult, gotFault );
    /// client.Invoke( "Weborb.Examples.HelloWorldService", "SayHello", null, responder );
    /// 
    /// public void gotResult( Result resultValue )
    /// {
    ///    string result = (string) resultValue.ReturnValue;
    /// }
    /// 
    /// public void gotFault( Fault fault )
    /// {
    /// }
    /// </code>
    /// </example>
    public class Result
    {
        private IAdaptingType rawReturnValue;
        private object returnValue;
        private IDictionary headers;
        private long roundTripTime;
        private long clientParseTime;

        /// <summary>
        /// Returns unadapted return value received from the remote server.
        /// </summary>
        public IAdaptingType RawReturnValue { get { return rawReturnValue; } set { rawReturnValue = value; } }

        /// <summary>
        /// Returns adapted return value received from the remote server. Value created using default adaptation algorithm.
        /// </summary>
        public object ReturnValue { get { return returnValue; } set { returnValue = value; } }

        /// <summary>
        /// Returns message headers returned from the remote server.
        /// </summary>
        public IDictionary Headers { get { return headers; } set { headers = value; } }

        public long RoundTripTime { get { return roundTripTime; } set { roundTripTime = value; } }
        public long ClientParseTime { get { return clientParseTime; } set { clientParseTime = value; } }
    }
}
