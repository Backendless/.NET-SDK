using System;
using Weborb.Client;

namespace BackendlessAPI.Exception
{
  public class BackendlessFault
  {
    private readonly string _faultCode;
    private readonly string _message;
    private readonly string _detail;

    internal BackendlessFault( string message )
    {
      _message = message;
    }

    internal BackendlessFault( BackendlessException backendlessException )
    {
      _faultCode = backendlessException.FaultCode;
      _message = backendlessException.Message;
      _detail = backendlessException.Detail;
    }

    internal BackendlessFault( Weborb.Client.Fault fault )
    {
      _faultCode = fault.FaultCode;
      _message = fault.Message;
      _detail = fault.Detail;
    }

    internal BackendlessFault( System.Exception ex )
    {
      _faultCode = ex.GetType().Name;
      _message = ex.Message;
      _detail = ex.StackTrace;
    }

    public string Detail
    {
      get { return _detail; }
    }

    public string FaultCode
    {
      get { return _faultCode; }
    }

    public string Message
    {
      get { return _message; }
    }

    public override string ToString()
    {
      return String.Format( "Backendless BackendlessFault. Code: {0}, Message: {1}", FaultCode ?? "N/A",
                            Message ?? "N/A" );
    }
  }
}