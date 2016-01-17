using System;
using Weborb.V3Types;

namespace Weborb.Client
  {
  /// <summary>
  /// Responsible for maintaining references to a method receiving return value from a remote invocation
  /// and a method responsible for error handling from a remote invocation.
  /// </summary>
  /// <typeparam name="T">Return type from a remote method invocation</typeparam>
  public class PublishingResponder : Responder<AckMessage>
    {
    /// <summary>
    /// Initializes a new instance of the <see cref="Responder&lt;T&gt;"/> class with return value processing method
    /// and a fault handler method
    /// </summary>
    /// <param name="responseHandler">The response handler - invoked when the client received a response from a remote method invocation</param>
    /// <param name="errorHandler">The error handler - invoked if the remote method invocation throws an exception.</param>
    public PublishingResponder( ResponseHandler<AckMessage> responseHandler, ErrorHandler errorHandler ) : base( responseHandler, errorHandler )
      { 
      }
    }
  }
