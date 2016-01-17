using System;

namespace Weborb.Client
{
  /// <summary>
  /// Encapsulates information about a remote error/exception.
  /// </summary>
  public class Fault
  {
    private string _message;
    private string _detail;
    private string _faultCode;

    internal Fault( string message, string detail )
    {
      _message = message;
      _detail = detail;
    }

    internal Fault( string message, string detail, string faultCode )
    {
      _message = message;
      _detail = detail;
      _faultCode = faultCode;
    }

    /// <summary>
    /// Returns a description of the received error/exception
    /// </summary>
    /// <value>Exception description or error message from the user application exception</value>
    public String Message
    {
      get { return _message; }
    }

    /// <summary>
    /// Contains detailed information about received exception. Typically a stack trace.
    /// </summary>
    /// <value>Exception stack trace and additional diagnostics information.</value>
    public String Detail
    {
      get { return _detail; }
    }

    public String FaultCode
    {
      get { return _faultCode; }

    }
  }
}
