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

    public BackendlessFault BackendlessFault => _backendlessFault;

    public string FaultCode => _backendlessFault.FaultCode;

    public override string Message => _backendlessFault.Message;

    public string Detail => _backendlessFault.Detail;

    public override string ToString()
    {
      return $"Error code: {_backendlessFault.FaultCode ?? "N/A"}, Message: {Message ?? "N/A"}";
    }
  }
}