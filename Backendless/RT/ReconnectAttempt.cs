using System;
namespace BackendlessAPI.RT
{
  public class ReconnectAttempt
  {
    private int timeout;
    private int attempt;
    private String error;

    public ReconnectAttempt(int timeout, int attempt, String error) : this(timeout, attempt)
    {
      this.error = error;
    }

    public ReconnectAttempt(int timeout, int attempt)
    {
      this.timeout = timeout;
      this.attempt = attempt;
    }

    public int Timeout
    {
      get
      {
        return timeout;
      }
    }

    public int Attempt
    {
      get
      {
        return attempt;
      }
    }

    public String Error
    {
      get
      {
        return error;
      }
    }

    public override String ToString()
    {
      return "ReconnectAttempt{" + "timeout=" + timeout + ", attempt=" + attempt + ", error='" + error + '\'' + '}';
    }
  }
}
