using System;

namespace Weborb.Client
{
    /// <summary>
    /// Responsible for handling return value received from a remote method invocation
    /// </summary>
    public delegate void ResponseHandler<T>( T response );
    /// <summary>
    /// Responsible for handling errors/exceptions received from a remote method invocation
    /// </summary>
    public delegate void ErrorHandler( Fault fault );

    /// <summary>
    /// Responsible for maintaining references to a method receiving return value from a remote invocation
    /// and a method responsible for error handling from a remote invocation.
    /// </summary>
    /// <typeparam name="T">Return type from a remote method invocation</typeparam>
    public class Responder<T>
    {
        public ResponseHandler<T> ResponseHandler;
        public ErrorHandler ErrorHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Responder&lt;T&gt;"/> class with return value processing method
        /// and a fault handler method
        /// </summary>
        /// <param name="responseHandler">The response handler - invoked when the client received a response from a remote method invocation</param>
        /// <param name="errorHandler">The error handler - invoked if the remote method invocation throws an exception.</param>
        public Responder( ResponseHandler<T> responseHandler, ErrorHandler errorHandler )
        {
            this.ResponseHandler = responseHandler;
            this.ErrorHandler = errorHandler;
        }
    }
}
