using System;

namespace BackendlessAPI.Exception
{
  public class BackendlessException : System.Exception
  {
    private readonly BackendlessFault _backendlessFault;

    public BackendlessException( BackendlessFault backendlessFault )
    {
      _backendlessFault = backendlessFault;
    }

    public BackendlessException( String message )
    {
      _backendlessFault = new BackendlessFault( message );
    }

    public BackendlessFault BackendlessFault
    {
      get { return _backendlessFault; }
    }

    public string FaultCode
    {
      get { return _backendlessFault.FaultCode; }
    }

    public override string Message
    {
      get { return _backendlessFault.Message; }
    }

    public string Detail
    {
      get { return _backendlessFault.Detail; }
    }

    public override string ToString()
    {
      return String.Format( "Error code: {0}, Message: {1}", _backendlessFault.FaultCode ?? "N/A", Message ?? "N/A" );
    }
  }
}