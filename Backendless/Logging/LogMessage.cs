using System;

namespace BackendlessAPI.Logging
{
  class LogMessage
  {
    internal LogMessage( DateTime timestamp, String message, String exception )
    {
      this.timestamp = timestamp;
      this.message = message;
      this.exception = exception;
    }

    public DateTime timestamp { get; set; }
    public String message { get; set; }
    public String exception { get; set; }
  }
}
